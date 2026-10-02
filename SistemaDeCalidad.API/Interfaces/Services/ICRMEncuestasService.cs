using SistemaDeCalidad.API.Models.Encuestas;
using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Interfaces.Services
{
    public interface ICRMEncuestasService
    {
        /// <summary>
        /// Publica en el CRM una encuesta ya grabada en la base legacy, pero
        /// solo si esta grabación contestó la última pregunta (la encuesta se
        /// graba en partes y el CRM ignora un origenHash repetido, así que se
        /// manda una sola vez y completa, leída de la base).
        /// Best-effort: NUNCA lanza — si el CRM está caído se loguea y listo,
        /// la respuesta del cliente no se puede perder por esto.
        /// Devuelve true solo si el CRM confirmó con un 2xx.
        /// </summary>
        Task<bool> PublicarSiFinalizo(EncuestaRespuesta respuesta, Soporte soporte, Encuesta encuesta);
    }
}
