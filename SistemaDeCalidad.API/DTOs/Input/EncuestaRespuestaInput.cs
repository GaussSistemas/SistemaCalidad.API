namespace SistemaDeCalidad.API.DTOs.Input
{
    public class EncuestaRespuestaInput
    {
        public string Hash { get; set; }
        public int EncuestaId { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public List<EncuestaRespuestaPreguntaInput> RespuestasPreguntas { get; set; }
    }
}
