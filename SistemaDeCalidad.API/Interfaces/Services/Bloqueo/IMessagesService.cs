using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IMessagesService
    {
        Task<List<Message>> GetMessages(DateTime? since, DateTime? to);
        Task<List<Message>> GetMessagesForCustomerCompany(string customerId, string companyId, DateTime? since, DateTime? to);
        Task<Message> GetMessageForEiffelUser(string customerId, string companyId, string eiffelUserId);
        Task<Message> CreateMessage(Message newMessage);
        Task<Message> UpdateMessage(int messageId, int stepId);
        Task<Message> MarkMessageAsRead(int messageId, string customerId, string companyId, string eiffelUserId, string eiffelUserName, bool dontShowAgain);
        Task<MessageComment> CreateComment(int messageId, string comment);
        Task<MessageComment> UpdateComment(int messageId, int commentId, string comment);
        Task<List<MessageComment>> GetMessageComments(int messageId);
        Task<List<MessageView>> GetMessageViews(int messageId); 

    }
}
