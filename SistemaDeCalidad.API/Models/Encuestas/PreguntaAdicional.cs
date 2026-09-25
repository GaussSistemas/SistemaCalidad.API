namespace SistemaDeCalidad.API.Models.Encuestas
{
    // Pregunta que se muestra en la misma pantalla, debajo de una pregunta de la encuesta.
    // Se arma una instancia por cada relación en EncuestasPreguntasAdicionales.
    public class PreguntaAdicional
    {
        private string Tabla { get; } = "PreguntasAdicionales";
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public int EncuestaPreguntaId { get; set; }
        public int Orden { get; set; }
        public string Titulo { get; set; }
        public int TipoControlId { get; set; }
        public bool Obligatoria { get; set; }
        public List<PreguntaAdicionalOpcion>? Opciones { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
