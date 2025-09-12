using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class RoleProfiles : Profile
    {
        public RoleProfiles()
        {
            CreateMap<Persistence.Entities.Role, DTOs.Output.Bloqueo.RoleOutput>();
            CreateMap<DTOs.Input.Bloqueo.RoleInput, Persistence.Entities.Role>();
        }
    }
}
