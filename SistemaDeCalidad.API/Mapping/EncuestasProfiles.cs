using AutoMapper;
using SistemaDeCalidad.API.DTOs.Input;
using SistemaDeCalidad.API.DTOs.Output;
using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.Mapping
{
    public class EncuestasProfiles : Profile
    {
        public EncuestasProfiles()
        {
            CreateMap<Encuesta, EncuestaOutput>(); 
            CreateMap<EncuestaTipoControl, EncuestaTipoControlOutput>();
            CreateMap<EncuestaPregunta,  EncuestaPreguntaOutput>();
            CreateMap<EncuestaPreguntaOpcion, EncuestaPreguntaOpcionOutput>();
            CreateMap<EncuestaConSoporte, EncuestaConSoporteOutput>();

            CreateMap<EncuestaRespuestaInput, EncuestaRespuesta>();
            CreateMap<EncuestaRespuestaPreguntaInput, EncuestaRespuestaPregunta>();
            CreateMap<EncuestaInput, Encuesta>(); 
            CreateMap<EncuestaPreguntaInput, EncuestaPregunta>();
            CreateMap<EncuestaPreguntaOpcionInput, EncuestaPreguntaOpcion>(); 
        }
    }
}
