using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class MessagesUsersProfiles : Profile
    {
        public MessagesUsersProfiles()
        {
            CreateMap<DTOs.Input.Bloqueo.MessageUserInput, Persistence.Entities.MessageUser>();
        }
    }
}
