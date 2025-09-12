using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class StepsService : IStepsService
    {
        private readonly SistemaDeCalidadContext _context;

        public StepsService(SistemaDeCalidadContext context)
        {
            _context = context;
        }

        public async Task<Step> CreateStep(Step newStep)
        {
            var existingStep = await _context.Steps
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == newStep.Name)
                .ConfigureAwait(false);

            if (existingStep != null)
                throw new BadHttpRequestException("El estado ya existe en la base de datos.");

            var createdStep = _context.Steps.Add(newStep);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return createdStep.Entity;
        }
    }
}
