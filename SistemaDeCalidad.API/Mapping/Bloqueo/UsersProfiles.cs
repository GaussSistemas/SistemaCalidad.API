using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class UsersProfiles : Profile
    {
        public UsersProfiles()
        {
            CreateMap<Persistence.Entities.User, DTOs.Output.Bloqueo.UserOutput>();
            CreateMap<DTOs.Input.Bloqueo.UserInput, Persistence.Entities.User>();
        }
    }
}
