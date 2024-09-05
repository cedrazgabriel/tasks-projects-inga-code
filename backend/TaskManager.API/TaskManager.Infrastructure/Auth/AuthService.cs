using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Application.Services
{
    public class AuthService(IConfiguration configuration): IAuthService
    {
        
        public async Task<string> GenerateJwtToken(string username)
        {
          
            var secretKey = configuration["Jwt:SecretKey"];

            if (secretKey is null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Cria o token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],   
                audience: configuration["Jwt:Audience"], 
                claims: claims,
                expires: DateTime.Now.AddHours(1),      
                signingCredentials: credentials);

            // Gera o token em formato string
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return await Task.FromResult(jwtToken); 
        }
    }
}
