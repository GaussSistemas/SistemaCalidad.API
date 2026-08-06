namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IStepsService
    {
        Task<Persistence.Entities.Step> CreateStep(Persistence.Entities.Step newStep);

    }
}
