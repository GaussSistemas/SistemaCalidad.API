using Newtonsoft.Json;

namespace SistemaDeCalidad.API.DTOs.Input.Bloqueo
{
    public class UserInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
