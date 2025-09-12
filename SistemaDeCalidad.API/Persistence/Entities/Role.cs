using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("Roles")]

    public class Role : Base
    {
        public string Name { get; set; }
        public bool Authorize { get; set; }
        public List<User> Users { get; set; }
        public List<RoleStep> RoleSteps { get; set; }
    }
}
