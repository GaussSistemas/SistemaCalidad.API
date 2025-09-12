using AutoMapper;

namespace SistemaDeCalidad.API.Mapping
{
    public class TokenProfiles : Profile
    {
        public TokenProfiles()
        {
            CreateMap<Models.AccessToken, DTOs.Output.AccessTokenOutput>();
        }
    }
}
