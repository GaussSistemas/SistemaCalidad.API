using SistemaDeCalidad.API.Models.Encuestas;
using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Interfaces.Services
{
    public interface ICRMEncuestasService
    {
        /// <summary>Publishes the completed survey to the CRM. Best-effort: never throws; returns true on 2xx.</summary>
        Task<bool> PublicarSiFinalizo(EncuestaRespuesta respuesta, Soporte soporte, Encuesta encuesta);
    }
}
