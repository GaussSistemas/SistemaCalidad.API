using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IMessagesService
    {
        Task<List<Message>> GetAllMessages();
        Task<List<Message>> GetAllMessagesForCustomerCompany(string customerId, string companyId);
        Task<Message> GetMessageForEiffelUser(string customerId, string companyId, string eiffelUserId);
        Task<Message> CreateMessage(Message newMessage);
        Task<Message> UpdateMessage(int messageId, int stepId);
        Task<Message> MarkMessageAsRead(int messageId, string customerId, string companyId, string eiffelUserId, string eiffelUserName, bool dontShowAgain);
        Task<MessageComment> CreateComment(int messageId, string comment);
        Task<MessageComment> UpdateComment(int messageId, int commentId, string comment);
    }
}
