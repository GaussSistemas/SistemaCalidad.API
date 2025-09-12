using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("MessagesLogs")]

    public class MessageLog : Base
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; }
        public int? StepFrom { get; set; }
        public int StepTo { get; set; }
    }
}
