namespace SistemaDeCalidad.API.Models.Soportes
{
    public class ClaveSoporte
    {
        private string Tabla { get; } = "ClavesSoportes";
        public string Hash { get; set; }
        public string EstadoId { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
