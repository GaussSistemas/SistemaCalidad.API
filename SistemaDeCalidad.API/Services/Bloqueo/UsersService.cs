using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Persistence.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaDeCalidad.API.Services.Bloqueo
{
    public class UsersService : IUsersService
    {
        private readonly SistemaDeCalidadContext _context;
        private readonly PasswordHasher<User> _hasher;
        private readonly JWTConfiguration _jwtConfiguration;

        public UsersService(SistemaDeCalidadContext context, IOptions<JWTConfiguration> jwtConfiguration)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<AccessToken> CreateToken(Login credentials)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToUpper() == credentials.Email.ToUpper()).ConfigureAwait(false);
            if (user == null)
                throw new BadHttpRequestException("Usuario o contraseña incorrectos."); 
            
            if (PasswordVerificationResult.Success != _hasher.VerifyHashedPassword(user, user.Password, credentials.Password))
                throw new BadHttpRequestException("Usuario o contraseña incorrectos.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.DurationInMinutes),
                signingCredentials: creds);
            return new AccessToken() { ExpiresIn = (int)TimeSpan.FromMinutes(_jwtConfiguration.DurationInMinutes).TotalSeconds, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }

        public async Task<User> CreateUser(User newUser)
        {
            var existingUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == newUser.Email)
                .ConfigureAwait(false);

            if (existingUser != null)
                throw new BadHttpRequestException("El usuario ya existe en la base de datos.");

            newUser.Password = _hasher.HashPassword(newUser, newUser.Password);
            var createdUser = _context.Users.Add(newUser); 
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return createdUser.Entity;
        }
    }
}
