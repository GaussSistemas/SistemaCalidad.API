namespace SistemaDeCalidad.API.DTOs.Output
{
    public class PreguntaAdicionalOutput
    {
        public long Id { get; set; }
        public int EncuestaPreguntaId { get; set; }
        public int Orden { get; set; }
        public string Titulo { get; set; }
        public int TipoControlId { get; set; }
        public bool Obligatoria { get; set; }
        public List<PreguntaAdicionalOpcionOutput>? Opciones { get; set; }
    }
}
