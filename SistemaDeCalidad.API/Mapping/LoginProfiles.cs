using AutoMapper;

namespace SistemaDeCalidad.API.Mapping
{
    public class LoginProfiles : Profile
    {
        public LoginProfiles()
        {
            CreateMap<DTOs.Input.LoginInput, Models.Login>();
        }
    }
}
