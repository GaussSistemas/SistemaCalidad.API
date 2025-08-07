using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.DTOs.Input
{
    public class EncuestaPreguntaInput
    {
        public string Titulo { get; set; }
        public int TipoControlId { get; set; }
        public List<EncuestaPreguntaOpcionInput>? Opciones { get; set; }
    }
}
