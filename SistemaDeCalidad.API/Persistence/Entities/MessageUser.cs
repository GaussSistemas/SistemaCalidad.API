using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("MessagesUsers")]

    public class MessageUser : Base
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public string EiffelUserId { get; set; }
    }
}
