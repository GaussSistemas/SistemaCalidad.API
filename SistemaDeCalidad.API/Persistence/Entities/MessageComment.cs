using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("MessagesComments")]
    public class MessageComment : Base
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
    }
}
