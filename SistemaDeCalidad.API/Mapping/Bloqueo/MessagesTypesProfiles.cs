using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class MessagesTypesProfiles : Profile
    {
        public MessagesTypesProfiles()
        {
            CreateMap<Persistence.Entities.MessageType, DTOs.Output.Bloqueo.MessageTypeOutput>();
            CreateMap<DTOs.Input.Bloqueo.MessageTypeInput, Persistence.Entities.MessageType>();
        }
    }
}
