using AutoMapper;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class MessagesViewsProfiles : Profile
    {
        public MessagesViewsProfiles()
        {
            CreateMap<MessageView, MessageViewOutput>()
                   .ForMember(dest => dest.EiffelId, src => src.MapFrom(opt => opt.EiffelUserId))
                   .ForMember(dest => dest.UserName, src => src.MapFrom(opt => opt.EiffelUserName))
                   .ForMember(dest => dest.Visualized, src => src.MapFrom(opt => opt.Created));
        }
    }
}
