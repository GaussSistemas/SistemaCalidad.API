namespace SistemaDeCalidad.API.Models
{
    public class SistemaDeCalidadConfiguration
    {
        public List<string> SistemaDeCalidadDSNs { get; set; }
        public string SistemaDeCalidadQueryExecutorURL { get; set; }
        // Preguntas que se muestran junto a otra en el front y no deben tomarse como la última de la encuesta
        public List<int> PreguntasExcluidasDeCierre { get; set; } = new List<int>();
    }
}
