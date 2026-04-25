using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalcTarifa.BusinessApplication.Interfaces.Services;
using CalcTarifa.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CalcTarifa.Infrastructure.Services
{
    // implementación de jwt
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<(string Token, DateTime Expiracion)> GenerarTokenAsync(Usuario usuario, IEnumerable<string> roles)
        {
            var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds      = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expMinutes = int.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60");
            var expiracion = DateTime.UtcNow.AddMinutes(expMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,   usuario.Id),
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new("Nombre", usuario.NombreCompleto)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenObj = new JwtSecurityToken(
                issuer:             _config["Jwt:Issuer"],
                audience:           _config["Jwt:Audience"],
                claims:             claims,
                expires:            expiracion,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenObj);
            return Task.FromResult((token, expiracion));
        }
    }
}
