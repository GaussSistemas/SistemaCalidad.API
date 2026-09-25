namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaRespuestaAdicional
    {
        private string Tabla { get; } = "encuestasRespuestasAdicionales";
        public int Id { get; set; }
        public string Hash { get; set; }
        public int EncuestaPreguntaId { get; set; }
        public int PreguntaAdicionalId { get; set; }
        public int PreguntaAdicionalOpcionId { get; set; }
        public string Valor { get; set; }

        public string NombreTabla()
        {
            return Tabla;
        }
    }
}
