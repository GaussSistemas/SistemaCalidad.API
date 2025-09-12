using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("MessagesViews")]

    public class MessageView : Base
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public string EiffelUserId { get; set; }
        public string EiffelUserName { get; set; }
        public bool DontShowAgain { get; set; }

    }
}
