using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.DTOs.Input
{
    public class EncuestaInput
    {
        public string Titulo { get; set; }
        public bool Activa { get; set; }
        public List<EncuestaPreguntaInput> Preguntas { get; set; }
    }
}
