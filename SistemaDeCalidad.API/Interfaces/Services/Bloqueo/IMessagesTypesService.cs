namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IMessagesTypesService
    {
        Task<List<Persistence.Entities.MessageType>> GetAllMessageTypes();
        Task<Persistence.Entities.MessageType> CreateMessageType(Persistence.Entities.MessageType newMessageType);
        Task<Persistence.Entities.MessageType> ModifyTemplate(int messageTypeId, string newTemplate);
    }
}
