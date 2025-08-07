namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaLog
    {
        private string Tabla { get; } = "Log_Envio_Encuesta";
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Observacion { get; set; }
        public string EstadoId { get; set; }
        public string UsuarioId { get; set; }
        public string SoporteId { get; set; }
        public long Numero { get; set; }
        public string NombreTabla() { return Tabla; }
    }
}
