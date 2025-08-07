namespace SistemaDeCalidad.API.DTOs.Input
{
    public class EncuestaRespuestaPreguntaInput
    {
        public int EncuestaPreguntaId { get; set; }
        public int EncuestaPreguntaOpcionId { get; set; }
        public string Valor { get; set; }
    }
}
