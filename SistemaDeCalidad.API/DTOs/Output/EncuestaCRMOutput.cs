namespace SistemaDeCalidad.API.DTOs.Output
{
    /// <summary>
    /// Payload que consume POST /api/encuestas/ingesta del CRM. Se serializa
    /// con JsonNamingPolicy.CamelCase (OrigenHash -> origenHash).
    ///
    /// Las calificaciones y el "resuelto" van como string crudo, tal cual los
    /// tiene cargados la encuesta ("1".."5", "Si"/"No"): el CRM ya sabe
    /// parsearlos porque es lo mismo que venía en el Excel.
    ///
    /// Todo es nullable: si una pregunta no fue respondida, el campo viaja en
    /// null y el CRM lo guarda como tal.
    /// </summary>
    public class EncuestaCRMOutput
    {
        /// <summary>Hash del soporte. Es la clave de idempotencia del lado del CRM.</summary>
        public string? OrigenHash { get; set; }
        public string? OrigenTicket { get; set; }

        /// <summary>Título del soporte (cabecera.nombre).</summary>
        public string? Titulo { get; set; }

        /// <summary>Código Eiffel del cliente (cabecera.id_cliente).</summary>
        public string? IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? Respondente { get; set; }

        /// <summary>Mail del usuario del soporte (cabecera.mail_usuario).</summary>
        public string? MailRespondente { get; set; }

        /// <summary>ISO 8601 con offset, ej: "2026-09-22T14:35:00-03:00".</summary>
        public string? FechaHora { get; set; }

        public string? Responsable { get; set; }
        public string? Atencion { get; set; }
        public string? TiempoRespuesta { get; set; }
        public string? RespuestaClara { get; set; }
        public string? RespuestaClaraComentario { get; set; }
        public string? ContactoPrevio { get; set; }
        public string? Resuelto { get; set; }
        public string? Comentario { get; set; }
        public string? TipoEncuesta { get; set; }
    }
}
