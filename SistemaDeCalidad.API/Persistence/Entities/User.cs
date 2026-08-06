using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("Users")]

    public class User : Base
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Inactive { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<MessageComment> MessageComments { get; set; }
        public List<MessageLog> MessageLogs { get; set; }
    }
}
