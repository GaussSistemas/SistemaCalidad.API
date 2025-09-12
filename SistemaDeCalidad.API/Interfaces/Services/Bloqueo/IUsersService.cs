using SistemaDeCalidad.API.Models;

namespace SistemaDeCalidad.API.Interfaces.Services.Bloqueo
{
    public interface IUsersService
    {
        Task<Persistence.Entities.User> CreateUser(Persistence.Entities.User newUser);
        Task<AccessToken> CreateToken(Login credentials);

    }
}
