namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaRespuesta
    {
        private string Tabla { get; } = "EncuestasRespuestas";
        public int Id { get; set; }
        public string Hash { get; set; }
        public int EncuestaId { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public List<EncuestaRespuestaPregunta> RespuestasPreguntas { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; } = DateTime.Now.TimeOfDay;
        public string NombreTabla() { return Tabla; }
    }
}
