using SistemaDeCalidad.API.Models;

namespace SistemaDeCalidad.API.DTOs.Output
{
    public class EncuestaPreguntaOutput
    {
        public long Id { get; set; }
        public int EncuestaId { get; set; }
        public string Titulo { get; set; }
        public int TipoControlId { get; set; }
        public List<EncuestaPreguntaOpcionOutput>? Opciones { get; set; }
        public EncuestaTipoControlOutput TipoControl { get; set; }
    }
}
