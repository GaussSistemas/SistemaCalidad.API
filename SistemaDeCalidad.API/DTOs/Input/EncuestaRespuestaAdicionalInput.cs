namespace SistemaDeCalidad.API.DTOs.Input
{
    public class EncuestaRespuestaAdicionalInput
    {
        public int EncuestaPreguntaId { get; set; }
        public int PreguntaAdicionalId { get; set; }
        public int PreguntaAdicionalOpcionId { get; set; }
        public string Valor { get; set; }
    }
}
