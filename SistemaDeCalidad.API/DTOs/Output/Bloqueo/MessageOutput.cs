namespace SistemaDeCalidad.API.DTOs.Output.Bloqueo
{
    public class MessageOutput
    {
        public int Id { get; set; }
        public int MessageTypeId { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Immediately { get; set; }
        public int StepId { get; set; }
        public string Text { get; set; }

    }
}
