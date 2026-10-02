namespace SistemaDeCalidad.API.DTOs.Input
{
    public class CRMMapeoPreguntaInput
    {
        public string? CampoCrm { get; set; }

        public int EncuestaPreguntaId { get; set; }

        public int? PreguntaAdicionalId { get; set; }
    }
}
