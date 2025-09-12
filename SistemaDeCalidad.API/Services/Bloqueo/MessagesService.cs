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
        public MessagesService(SistemaDeCalidadContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _loggedInUserId = int.Parse(httpContextAccessor.HttpContext.User.LoggedInUserId());
        }

        public async Task<Message> CreateMessage(Message newMessage)
        {
            var messagesForPeriod = await _context.Messages
                .Include(m => m.Step)
                .AsNoTracking()
                .Where(m => m.CustomerId == newMessage.CustomerId && m.CompanyId == newMessage.CompanyId && 
                        m.MessageTypeId == newMessage.MessageTypeId && m.Step.Id != rejectedStepId && 
                        ((m.Immediately || 
                        (m.StartDate.HasValue && newMessage.StartDate.HasValue && m.StartDate.Value.Date <= newMessage.StartDate.Value.Date && 
                        m.EndDate.HasValue && newMessage.EndDate.HasValue && m.EndDate.Value >= newMessage.EndDate)) ||
                        (newMessage.Immediately ||
                        (m.StartDate.HasValue && newMessage.StartDate.HasValue && m.StartDate.Value.Date <= newMessage.StartDate.Value.Date &&
                        m.EndDate.HasValue && newMessage.EndDate.HasValue && m.EndDate.Value >= newMessage.EndDate))))
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

        public async Task<List<Message>> GetAllMessages()
        {
            var messages = await _context.Messages.AsNoTracking().ToListAsync().ConfigureAwait(false);
            return messages;
        }

        public async Task<List<Message>> GetAllMessagesForCustomerCompany(string customerId, string companyId)
        {
            var messages = await _context.Messages
                .AsNoTracking()
                .Where(m => m.CustomerId == customerId && m.CompanyId == companyId)
                .ToListAsync()
                .ConfigureAwait(false);

            return messages;
        }

        public async Task<Message> GetMessageForEiffelUser(string customerId, string companyId, string eiffelUserId)
        {
            var message = await _context.Messages.AsNoTracking()
                .Include(m => m.MessageViews)
                .Include(m => m.MessageUsers)
                .Include(m => m.Step)
                .Where(m => m.CustomerId == customerId && m.CompanyId == companyId && m.Step.ForUser &&
                ((m.StartDate.HasValue && m.StartDate.Value.Date <= DateTime.UtcNow.Date && m.EndDate.HasValue && m.EndDate.Value.Date >= DateTime.UtcNow.Date) || m.Immediately) &&
                m.MessageUsers.Any(mu => mu.EiffelUserId == eiffelUserId) &&
                !m.MessageViews.Any(mv => mv.EiffelUserId == eiffelUserId))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

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
                UserId = _loggedInUserId,
                Action = "Actualización de mensaje",
                StepFrom = originalStep,
                StepTo = stepId
            };
            message.StepId = stepId;
            message.MessageLogs.Add(log); 

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return message;
        }
    }
}
