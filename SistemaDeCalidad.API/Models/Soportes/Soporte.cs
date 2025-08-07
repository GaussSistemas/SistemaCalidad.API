namespace SistemaDeCalidad.API.Models.Soportes
{
    public class Soporte
    {
        public string TipoId { get; set; }
        public double Numero { get; set; }
        public string Tarea { get; set; }
        public string ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string UsuarioCliente { get; set; }
        public string Responsable { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaResolucion { get; set; }
    }
}
