using AutoMapper;
using SistemaDeCalidad.API.DTOs.Output;
using SistemaDeCalidad.API.Models.Soportes;

namespace SistemaDeCalidad.API.Mapping
{
    public class SoportesProfiles : Profile
    {
        public SoportesProfiles()
        {
            CreateMap<Soporte, SoporteOutput>();
        }
    }
}
