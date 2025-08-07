using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Interfaces.Services
{
    public interface ISoportesService
    {
        Task<Soporte> GetSoporte(string id);
        Task<bool> FinalizarSoporte(string id);     
    }
}
