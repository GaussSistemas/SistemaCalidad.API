namespace SistemaDeCalidad.API.Models.Encuestas
{
    public class PreguntaAdicionalOpcion
    {
        private string Tabla { get; } = "PreguntasAdicionalesOpciones";
        public int Id { get; set; }
        public int PreguntaAdicionalId { get; set; }
        public string Opcion { get; set; }
        public string NombreTabla() { return Tabla; }

    }
}
