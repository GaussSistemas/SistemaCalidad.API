using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class MessagesProfiles : Profile
    {
        public MessagesProfiles()
        {
            CreateMap<Persistence.Entities.Message, DTOs.Output.Bloqueo.MessageOutput>();

            CreateMap<DTOs.Input.Bloqueo.MessageInput, Persistence.Entities.Message>()
                .ForMember(opt => opt.MessageUsers, src => src.MapFrom(opt => opt.Users)); 
            CreateMap<DTOs.Input.Bloqueo.MessageUserInput, Persistence.Entities.MessageUser>();
        }
    }
}
