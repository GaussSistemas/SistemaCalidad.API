using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("MessagesTypes")]

    public class MessageType : Base
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public List<Message> Messages { get; set; }
    }
}
