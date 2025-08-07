namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaPregunta
    {
        private string Tabla { get; } = "EncuestasPreguntas";
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public string Titulo { get; set; }
        public int TipoControlId { get; set; }
        public List<EncuestaPreguntaOpcion>? Opciones { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
