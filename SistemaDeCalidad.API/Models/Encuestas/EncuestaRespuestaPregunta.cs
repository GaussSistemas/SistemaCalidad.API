namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaRespuestaPregunta
    {
        private string Tabla { get; } = "encuestasRespuestasPreguntas";
        public int Id { get; set; }
        public string Hash { get; set; }
        public int EncuestaPreguntaId { get; set; }
        public int EncuestaPreguntaOpcionId { get; set; }
        public string Valor { get; set; }

        public string NombreTabla()
        {
            return Tabla;
        }
    }
}
