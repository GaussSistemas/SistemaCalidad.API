namespace SistemaDeCalidad.API.DTOs.Output.Bloqueo
{
    public class MessageViewOutput
    {
        public int Id { get; set; }
        public string EiffelId { get; set; }
        public string UserName { get; set; }
        public DateTime Visualized { get; set; }
        public bool DontShowAgain { get; set; }
    }
}
