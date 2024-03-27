using Microsoft.IdentityModel.Tokens;
using PMFWikipedia.Common;
using PMFWikipedia.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PMFWikipedia.ImplementationsBL.Helpers
{
    public class AuthService
    {
        public static string GetJWT(UserViewModel user)
        {
            var key = Encoding.UTF8.GetBytes(ConfigProvider.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Program", user.Program)
                }),
                Expires = DateTime.UtcNow.AddDays(ConfigProvider.TokenLifetimeHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}