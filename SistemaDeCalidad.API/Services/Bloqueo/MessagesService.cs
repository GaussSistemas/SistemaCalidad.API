using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Helpers;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class MessagesService : IMessagesService
    {
        private readonly SistemaDeCalidadContext _context;
        private readonly int _loggedInUserId;
        private readonly int rejectedStepId = 6;
        private readonly int blockedStepId = 3;
        private readonly int blockedMessageTypeId = 3;
        private readonly int warningMessageTypeId = 1;
        private readonly int communicationMessageTypeId = 2;
        private readonly int authorizedStepId = 2;
        public MessagesService(SistemaDeCalidadContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            int.TryParse(httpContextAccessor.HttpContext.User.LoggedInUserId(), out var result);
            _loggedInUserId = result;
        }

        public async Task<MessageComment> CreateComment(int messageId, string comment)
        {
            var message = await _context.Messages
                .AsNoTracking()
                .Where(m => m.Id == messageId)
                .FirstOrDefaultAsync();

            var newComment = new MessageComment()
            {
                Comment = comment,
                MessageId = messageId,
                StepId = message.StepId,
                UserId = _loggedInUserId
            };

            var createdComment = _context.MessagesComments.Add(newComment);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return createdComment.Entity;
        }

        public async Task<MessageComment> UpdateComment(int messageId, int commentId, string comment)
        {
            var messageComment = await _context.MessagesComments.Where(mc => mc.MessageId == messageId && mc.Id == commentId).FirstOrDefaultAsync();
            if (messageComment == null)
                throw new BadHttpRequestException("El comentario no existe para el mensaje indicado.");

            messageComment.Comment = comment;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return messageComment;
        }

        public async Task<List<MessageComment>> GetMessageComments(int messageId)
        {
            var messageComments = await _context.MessagesComments.AsNoTracking().Where(mc => mc.MessageId == messageId).ToListAsync();
            return messageComments;
        }

        public async Task<Message> CreateMessage(Message newMessage)
        {
            var messagesForPeriod = await _context.Messages
                .Include(m => m.Step)
                .AsNoTracking()
                .Where(m => m.CustomerId == newMessage.CustomerId && m.CompanyId == newMessage.CompanyId &&
                        m.MessageTypeId == newMessage.MessageTypeId && m.Step.Id != rejectedStepId &&
                        (
                        (m.Immediately && !m.EndDate.HasValue) ||
                        (m.Immediately && newMessage.EndDate.HasValue && m.EndDate.Value.Date >= newMessage.EndDate.Value.Date) ||
                        (m.StartDate.HasValue && newMessage.StartDate.HasValue && m.StartDate.Value.Date <= newMessage.StartDate.Value.Date) &&
                        m.EndDate.HasValue && newMessage.EndDate.HasValue && m.EndDate.Value >= newMessage.EndDate.Value.Date)
                      )
                .ToListAsync()
                .ConfigureAwait(false);

            if (messagesForPeriod.Any())
                throw new BadHttpRequestException("El cliente y empresa indicada, ya cuenta con un mensaje para el mismo período.");

            var log = new MessageLog()
            {
                UserId = _loggedInUserId,
                Action = "Creación de mensaje",
                StepTo = newMessage.StepId
            };
            newMessage.MessageLogs = new List<MessageLog> { log };
            var createdMessage = _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return createdMessage.Entity;
        }

        public async Task<List<Message>> GetMessages(DateTime? since, DateTime? to)
        {
            var messages = await _context.Messages
                .Include(x => x.MessageType)
                .Include(x => x.Step)
                .Include(x => x.MessageUsers)
                .Where(x => 
                    (since.HasValue ? (DateTime)x.EndDate.Value.Date >= since.Value.Date : true) &&
                    (to.HasValue ? (DateTime)x.EndDate.Value.Date <= to.Value.Date : true))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
            return messages;
        }

        public async Task<List<Message>> GetMessagesForCustomerCompany(string customerId, string companyId, DateTime? since, DateTime? to)
        {
            var messages = await _context.Messages
                .AsNoTracking()
                .Include(msg => msg.MessageType)
                .Include(msg => msg.Step)
                .Include(msg => msg.MessageUsers)
                .Where(m => m.CustomerId == customerId && m.CompanyId == companyId &&
                    (since.HasValue ? (DateTime)m.EndDate.Value.Date >= since.Value.Date : true) &&
                    (to.HasValue ? (DateTime)m.EndDate.Value.Date <= to.Value.Date : true)
                )
                .ToListAsync()
                .ConfigureAwait(false);

            return messages;
        }

        public async Task<Message> GetMessageForEiffelUser(string customerId, string companyId, string eiffelUserId)
        {
            var comparissonDate = DateTime.Now.Date;

            var message = await _context.Messages
                .AsNoTracking()
                .Include(m => m.MessageViews)
                .Include(m => m.MessageUsers)
                .Include(m => m.Step)
                .Include(m => m.MessageType)
                .Where(m => m.CustomerId == customerId && m.CompanyId.ToUpper() == companyId.ToUpper() && m.StepId == blockedStepId).
                FirstOrDefaultAsync();

            if (message == null)
            {
                message = await _context.Messages
                .AsNoTracking()
                .Include(m => m.MessageViews)
                .Include(m => m.MessageUsers)
                .Include(m => m.Step)
                .Include(m => m.MessageType)
                .Where
                (m =>
                    ((m.CustomerId == customerId && m.CompanyId.ToUpper() == companyId.ToUpper() && m.StepId == blockedStepId) ||
                     (m.CustomerId == customerId && m.CompanyId.ToUpper() == companyId.ToUpper() && m.Step.ForUser && m.StartDate.HasValue && m.StartDate.Value < comparissonDate &&
                      m.EndDate.HasValue && m.EndDate.Value < comparissonDate && m.MessageTypeId == warningMessageTypeId && m.StepId == authorizedStepId))
                ||
                (
                    m.CustomerId == customerId && m.CompanyId == companyId && m.Step.ForUser &&
                    (m.Immediately || (m.StartDate.HasValue && m.StartDate.Value <= comparissonDate && m.EndDate.HasValue && m.EndDate.Value > comparissonDate)) &&
                    m.MessageUsers.Any(mu => mu.EiffelUserId == eiffelUserId) 
                )
                )
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            }

            if (message != null)
            {
                if ((message.MessageTypeId == warningMessageTypeId && message.MessageViews.Any(mv => mv.EiffelUserId == eiffelUserId && mv.DontShowAgain && mv.Created.Date == comparissonDate)) ||
                    (message.MessageTypeId == communicationMessageTypeId && message.MessageViews.Any(mv => mv.EiffelUserId == eiffelUserId && mv.DontShowAgain)))
                    message = null;

            }

            var messageExpired = false;
            if(message != null && message.EndDate.HasValue)
                messageExpired = message.EndDate.Value.Date < comparissonDate;
            
            if (message != null && messageExpired && message.MessageTypeId == warningMessageTypeId)
            {
                var blockedMessageType = await _context.MessagesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(mt => mt.Id == blockedMessageTypeId)
                .ConfigureAwait(false);

                var blockedStep = await _context.Steps.FirstOrDefaultAsync(step => step.Id == blockedStepId).ConfigureAwait(false);

                // Actualizar estado a bloqueado
                var updatedMessage = await UpdateMessage(message.Id, blockedStepId).ConfigureAwait(false);

                // Poner la información del mensaje de bloqueo
                message.Step = blockedStep ?? message.Step;
                message.Text = blockedMessageType?.Template ?? message.Text;
                message.MessageType = blockedMessageType ?? message.MessageType;

            }

            return message;
        }

        public async Task<Message> MarkMessageAsRead(int messageId, string customerId, string companyId, string eiffelUserId, string eiffelUserName, bool dontShowAgain)
        {
            var message = await _context.Messages
                .Include(m => m.MessageUsers)
                .AsNoTracking()
                .Where(m => m.Id == messageId && m.CustomerId == customerId && m.CompanyId == companyId && m.MessageUsers.Any(mu => mu.EiffelUserId == eiffelUserId))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (message == null)
                throw new BadHttpRequestException("El mensaje no existe para el cliente, empresa y usuario indicado.");

            var newMessageRead = new MessageView()
            {
                DontShowAgain = dontShowAgain,
                EiffelUserId = eiffelUserId,
                EiffelUserName = eiffelUserName,
                MessageId = messageId
            };

            var createdMessageRead = _context.MessagesViews.Add(newMessageRead);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return message;
        }

        public async Task<Message> UpdateMessage(int messageId, int stepId)
        {
            var message = await _context.Messages
                .Include(m => m.MessageLogs)
                .Include(m => m.MessageViews)
                .Include(m => m.MessageUsers)
                .Where(m => m.Id == messageId).FirstOrDefaultAsync();
            if (message == null)
                throw new BadHttpRequestException("El mensaje no existe.");

            var originalStep = message.StepId;
            var log = new MessageLog()
            {
                UserId = _loggedInUserId != 0 ? _loggedInUserId : 3,
                Action = "Actualización de mensaje",
                StepFrom = originalStep,
                StepTo = stepId
            };
            if (stepId == blockedStepId)
                message.MessageTypeId = blockedMessageTypeId;
            message.StepId = stepId;
            message.MessageLogs.Add(log);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return message;
        }

        public async Task<List<MessageView>> GetMessageViews(int messageId)
        {
            var messageViews = await _context.MessagesViews.AsNoTracking().Where(mv => mv.MessageId == messageId).ToListAsync();
            return messageViews;
        }
    }
}
