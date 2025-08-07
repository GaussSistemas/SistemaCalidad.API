namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class EncuestaPreguntaOpcion
    {
        private string Tabla { get; } = "EncuestasPreguntasOpciones";
        public int Id { get; set; }
        public int EncuestaPreguntaId { get; set; }
        public string Opcion { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
