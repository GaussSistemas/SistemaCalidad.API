using SistemaDeCalidad.API.Models;

namespace SistemaDeCalidad.API.DTOs.Output
{
    public class EncuestaConSoporteOutput
    {
        public SoporteOutput Soporte { get; set; }
        public EncuestaOutput Encuesta { get; set; }
        public bool Contestada { get; set; }
    }
}
