using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("RolesSteps")]

    public class RoleStep : Base
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int StepId { get; set; }
        public Step Step { get; set; }
    }
}
