using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Services
{
    public class SoportesService : ISoportesService
    {
        private readonly ISoportesRepository _soporteRepository;
        public SoportesService(ISoportesRepository soportesRepository)
        {
            _soporteRepository = soportesRepository;
        }

        public async Task<Soporte> GetSoporte(string id)
        {
            var soporte = _soporteRepository.RecuperarSoporte(id);
            return soporte;
        }

        public async Task<bool> FinalizarSoporte(string id)
        {
            var claveSoporte = new ClaveSoporte(){ EstadoId = "FIN", Hash = id };
            var finalizado = _soporteRepository.FinalizarSoporte(claveSoporte); 

            return finalizado;
        }
    }
}
