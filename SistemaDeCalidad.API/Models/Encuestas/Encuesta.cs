namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class Encuesta
    {
        private string Tabla { get; } = "Encuestas";
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool Activa { get; set; }
        public string Introduccion { get; set; }
        public string Cierre { get; set; }
        public List<EncuestaPregunta> Preguntas { get; set; }

        public string NombreTabla() { return Tabla; }

    }
}
