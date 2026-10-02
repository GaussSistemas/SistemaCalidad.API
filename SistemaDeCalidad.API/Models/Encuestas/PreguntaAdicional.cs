namespace SistemaDeCalidad.API.Models.Encuestas
{
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
