using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Interfaces.Repositories
{
    public interface ISoportesRepository
    {
        public Soporte RecuperarSoporte(string id);
        public bool FinalizarSoporte(ClaveSoporte claveSoporte); 
    }
}
