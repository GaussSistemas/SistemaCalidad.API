using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class RoleService : IRoleService
    {
        private readonly SistemaDeCalidadContext _context;

        public RoleService(SistemaDeCalidadContext context)
        {
            _context = context;
        }

        public async Task<Role> AssignStepsToRole(int roleId, List<int> stepsIds)
        {
            var role = await _context.Roles.Include(role => role.RoleSteps).FirstOrDefaultAsync(r => r.Id == roleId).ConfigureAwait(false);
            if(role == null)
                throw new BadHttpRequestException("El rol no existe en la base de datos.");

            if(stepsIds == null || (stepsIds != null && stepsIds.Count == 0))
                role.RoleSteps = new List<RoleStep>();
            else
                role.RoleSteps = stepsIds.Select(stepId => new RoleStep { RoleId = roleId, StepId = stepId } ).ToList();

            await _context.SaveChangesAsync().ConfigureAwait(false);
            
            return role; 
        }

        public async Task<Role> CreateRole(Role newRole)
        {
            var existingRole = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(u => u.Name == newRole.Name).ConfigureAwait(false);
            if (existingRole != null)
                throw new BadHttpRequestException("El rol ya existe en la base de datos.");

            var createdRole = _context.Roles.Add(newRole);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return createdRole.Entity;
        }
    }
}
