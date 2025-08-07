using SistemaDeCalidad.API.Models;

namespace SistemaDeCalidad.API.DTOs.Output
{
    public class EncuestaOutput
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public bool Activa { get; set; }
        public string Introduccion { get; set; }
        public string Cierre { get; set; }
        public List<EncuestaPreguntaOutput> Preguntas { get; set; }
    }
}
