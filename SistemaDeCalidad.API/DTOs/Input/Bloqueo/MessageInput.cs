namespace SistemaDeCalidad.API.DTOs.Input.Bloqueo
{
    public class MessageInput
    {
        public int MessageTypeId { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Immediately { get; set; }
        public int StepId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public List<MessageUserInput> Users { get; set; }
    }
}
