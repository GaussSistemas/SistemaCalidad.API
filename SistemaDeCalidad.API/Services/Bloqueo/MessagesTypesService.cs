using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class MessagesTypesService : IMessagesTypesService
    {
        private readonly SistemaDeCalidadContext _context;
        public MessagesTypesService(SistemaDeCalidadContext context)
        {
            _context = context;
        }

        public async Task<MessageType> CreateMessageType(MessageType newMessageType)
        {
            var existingMessageType = await _context.MessagesTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == newMessageType.Name)
                .ConfigureAwait(false);
            
            if (existingMessageType != null)
                throw new BadHttpRequestException("El tipo de mensaje ya existe en la base de datos.");

            var createdMessageType = _context.MessagesTypes.Add(newMessageType);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return createdMessageType.Entity;
        }

        public async Task<List<MessageType>> GetAllMessageTypes()
        {
            var messagesTypes = await _context.MessagesTypes
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return messagesTypes;
        }

        public async Task<MessageType> ModifyTemplate(int messageTypeId, string newTemplate)
        {
            var existingMessageType = await _context.MessagesTypes
                            .FirstOrDefaultAsync(u => u.Id == messageTypeId)
                            .ConfigureAwait(false);

            if (existingMessageType == null)
                throw new BadHttpRequestException("El tipo de mensaje no existe en la base de datos.");

            existingMessageType.Template = newTemplate;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            
            return existingMessageType;
        }
    }
}
