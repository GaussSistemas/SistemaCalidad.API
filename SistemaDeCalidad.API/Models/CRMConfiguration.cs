namespace SistemaDeCalidad.API.Models
{
    /// <summary>
    /// Configuración del "dual record" de encuestas hacia el CRM. Cuando el
    /// cliente responde la encuesta, además de grabarla en la base legacy se
    /// postea al CRM para que genere la respuesta y, si es negativa, el caso.
    /// </summary>
    public class CRMConfiguration
    {
        /// <summary>Kill-switch: si está en false no se hace ninguna llamada saliente.</summary>
        public bool Habilitado { get; set; } = false;

        public string? IngestaURL { get; set; }

        /// <summary>Secreto compartido que viaja como Bearer. En producción va por variable de entorno.</summary>
        public string? Token { get; set; }

        public int TimeoutSegundos { get; set; } = 8;

        /// <summary>
        /// Offset fijo en vez de TimeZoneInfo.Local: Argentina no tiene horario
        /// de verano desde 2009, y así el payload no depende de que la zona
        /// horaria del server esté bien seteada.
        /// </summary>
        public int OffsetHorasUTC { get; set; } = -3;

        /// <summary>
        /// Sin cola ni reintentos, el log del payload es la única forma de
        /// recuperar a mano una encuesta que el CRM no llegó a recibir.
        /// </summary>
        public bool LoguearPayloadEnFallo { get; set; } = true;

        // El mapeo de preguntas a campos del CRM no se configura acá: vive en
        // la tabla encuesta_mapeo_preguntas del CRM y se lee por
        // GET /api/encuestas/mapeo (misma base que IngestaURL) en cada publicación.
    }
}
