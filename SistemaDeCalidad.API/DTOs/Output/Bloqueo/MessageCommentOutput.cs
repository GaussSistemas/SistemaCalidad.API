namespace SistemaDeCalidad.API.DTOs.Output.Bloqueo
{
    public class MessageCommentOutput
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int StepId { get; set; }

    }
}
