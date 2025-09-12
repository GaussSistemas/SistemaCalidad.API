using AutoMapper;

namespace SistemaDeCalidad.API.Mapping.Bloqueo
{
    public class StepsProfiles : Profile
    {
        public StepsProfiles()
        {
            CreateMap<Persistence.Entities.Step, DTOs.Output.Bloqueo.StepOutput>();
            CreateMap<DTOs.Input.Bloqueo.StepInput, Persistence.Entities.Step>();
        }
    }
}
