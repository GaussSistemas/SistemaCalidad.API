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
            CreateMap<PreguntaAdicional, PreguntaAdicionalOutput>();
            CreateMap<PreguntaAdicionalOpcion, PreguntaAdicionalOpcionOutput>();
            CreateMap<EncuestaConSoporte, EncuestaConSoporteOutput>();

            CreateMap<EncuestaRespuestaInput, EncuestaRespuesta>();
            CreateMap<EncuestaRespuestaPreguntaInput, EncuestaRespuestaPregunta>();
            CreateMap<EncuestaRespuestaAdicionalInput, EncuestaRespuestaAdicional>();
            CreateMap<EncuestaInput, Encuesta>(); 
            CreateMap<EncuestaPreguntaInput, EncuestaPregunta>();
            CreateMap<EncuestaPreguntaOpcionInput, EncuestaPreguntaOpcion>(); 
        }
    }
}
