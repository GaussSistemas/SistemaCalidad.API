namespace SistemaDeCalidad.API.DTOs.Input
{
    /// <summary>
    /// Fila del mapeo que devuelve GET /api/encuestas/mapeo del CRM (tabla
    /// encuesta_mapeo_preguntas). Indica de qué pregunta de Eiffel sale cada
    /// campo del payload de ingesta.
    /// </summary>
    public class CRMMapeoPreguntaInput
    {
        /// <summary>Nombre del campo en EncuestaCRMOutput, en camelCase (ej: "tiempoRespuesta").</summary>
        public string? CampoCrm { get; set; }

        /// <summary>encuestasPreguntas.id</summary>
        public int EncuestaPreguntaId { get; set; }

        /// <summary>
        /// preguntasAdicionales.id cuando el campo es una repregunta
        /// (encuestasRespuestasAdicionales). Null para preguntas comunes.
        /// </summary>
        public int? PreguntaAdicionalId { get; set; }
    }
}
