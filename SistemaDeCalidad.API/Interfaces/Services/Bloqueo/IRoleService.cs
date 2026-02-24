
namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IRoleService
    {
        Task<Persistence.Entities.Role> CreateRole(Persistence.Entities.Role newRole);

        Task<Persistence.Entities.Role> AssignStepsToRole(int roleId, List<int> stepsIds);

        Task<Persistence.Entities.Role> GetUserRole();
    }
}
