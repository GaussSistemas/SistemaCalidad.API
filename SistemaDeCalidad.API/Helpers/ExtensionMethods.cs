using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemaDeCalidad.API.Helpers
{
    public static class ExtensionMethods
    {
        public static string LoggedInUserId(this ClaimsPrincipal claims)
        {
            if(claims == null)
                throw new ArgumentNullException(nameof(claims));

            return claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }
    }
}
