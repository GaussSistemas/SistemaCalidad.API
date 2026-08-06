using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class MessagesProfiles : Profile
    {
        public MessagesProfiles()
        {
            CreateMap<Persistence.Entities.Message, DTOs.Output.Bloqueo.MessageOutput>()
                .ForMember(dest => dest.MessageUsers, src => src.MapFrom(opt => opt.MessageUsers.Select(mu => mu.EiffelUserId).ToList()))
                .ForMember(dest => dest.SistemBlocked, src => src.MapFrom(opt => opt.Step != null ? opt.Step.SistemBlocked : false))
                .ForMember(dest => dest.Theme, src => src.MapFrom(opt => opt.MessageType != null ? opt.MessageType.Theme : ""))
                .ForMember(dest => dest.Title, src => src.MapFrom(opt => opt.MessageType != null ? opt.MessageType.Title : ""));
            CreateMap<Persistence.Entities.MessageComment, DTOs.Output.Bloqueo.MessageCommentOutput>();

            CreateMap<DTOs.Input.Bloqueo.MessageInput, Persistence.Entities.Message>()
                .ForMember(dest => dest.StartDate, src => src.MapFrom(opt => opt.StartDate.HasValue ? DateTime.SpecifyKind(opt.StartDate.Value, DateTimeKind.Unspecified) :(DateTime?) null))
                .ForMember(dest => dest.EndDate, src => src.MapFrom(opt => opt.EndDate.HasValue ? DateTime.SpecifyKind(opt.EndDate.Value, DateTimeKind.Unspecified) : (DateTime?) null))
                .ForMember(dest => dest.MessageUsers, src => src.MapFrom(opt => opt.Users)); 
            CreateMap<DTOs.Input.Bloqueo.MessageUserInput, Persistence.Entities.MessageUser>();
        }
    }
}
