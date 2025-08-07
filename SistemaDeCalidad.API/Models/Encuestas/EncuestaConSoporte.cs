using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaConSoporte
    {

        public Soporte Soporte { get; set; }
        public Encuesta Encuesta { get; set; }
        public bool Contestada { get; set; }
    }
}
