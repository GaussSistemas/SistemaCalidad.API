namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaTipoControl
    {
        public string Tabla { get; } = "EncuestasTiposControles";
        public int Id { get; set; }
        public string Control { get; set; }
        public string Tipo { get; set; }
        public bool Opciones { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
