using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Helpers;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class RoleService : IRoleService
    {
        private readonly SistemaDeCalidadContext _context;
        private readonly int _loggedInUserId;

        public RoleService(SistemaDeCalidadContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            int.TryParse(httpContextAccessor.HttpContext.User.LoggedInUserId(), out var result);
            _loggedInUserId = result;
        }

        public async Task<Role> AssignStepsToRole(int roleId, List<int> stepsIds)
        {
            var role = await _context.Roles.Include(role => role.RoleSteps).FirstOrDefaultAsync(r => r.Id == roleId).ConfigureAwait(false);
            if (role == null)
                throw new BadHttpRequestException("El rol no existe en la base de datos.");

            if (stepsIds == null || (stepsIds != null && stepsIds.Count == 0))
                role.RoleSteps = new List<RoleStep>();
            else
                role.RoleSteps = stepsIds.Select(stepId => new RoleStep { RoleId = roleId, StepId = stepId }).ToList();

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

        public async Task<Role> GetUserRole()
        {
            var userRole = await _context.Users.AsNoTracking().Where(u => u.Id == _loggedInUserId).Select(u => u.RoleId).FirstOrDefaultAsync();
            var role = await _context.Roles.AsNoTracking().Where(r => r.Id == userRole).FirstOrDefaultAsync();

            if (role == null)
                throw new BadHttpRequestException("El rol no existe en la base de datos.");

            return role;
        }
    }
}
