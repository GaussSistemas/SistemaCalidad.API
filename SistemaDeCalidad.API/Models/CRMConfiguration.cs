namespace SistemaDeCalidad.API.Models
{
    public class CRMConfiguration
    {
        public bool Habilitado { get; set; } = false;

        public string? IngestaURL { get; set; }

        public string? Token { get; set; }

        public int TimeoutSegundos { get; set; } = 8;

        public int OffsetHorasUTC { get; set; } = -3;

        public bool LoguearPayloadEnFallo { get; set; } = true;
    }
}
