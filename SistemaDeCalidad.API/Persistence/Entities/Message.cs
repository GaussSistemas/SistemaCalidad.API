using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("Messages")]
    public class Message : Base
    {
        public int MessageTypeId { get; set; }
        public MessageType MessageType { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Immediately { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
        public string Text { get; set; }
        public List<MessageComment> MessageComments { get; set; }
        public List<MessageUser> MessageUsers { get; set; }
        public List<MessageLog> MessageLogs { get; set; }
        public List<MessageView> MessageViews { get; set; }
    }
}
