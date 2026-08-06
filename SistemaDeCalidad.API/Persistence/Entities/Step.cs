using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCalidad.API.Persistence.Entities
{
    [Table("Steps")]

    public class Step : Base
    {
        public string Name { get; set; }
        public bool ForUser { get; set; }
        public bool SistemBlocked { get; set; }
        public List<RoleStep> RoleSteps { get; set; }
        public List<MessageComment> MessageComments { get; set; }
    }
}
