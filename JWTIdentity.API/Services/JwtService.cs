using JWTIdentity.API.Entities;
using JwtOptions = JWTIdentity.API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace JWTIdentity.API.Services
{
    public class JwtService(JwtOptions.TokenOptions _tokenOptions,
                            UserManager<AppUser> _userManager) : IJwtService
    {

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            if (string.IsNullOrEmpty(_tokenOptions.Key))
                throw new InvalidOperationException("JWT signing key is not configured.");

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.Key));

            var userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? string.Empty),
                new Claim("fullName", String.Join("", user.Name, user.Surname)),
            };

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_tokenOptions.ExpireInMinutes),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

            JwtSecurityTokenHandler handler = new();

            var token = handler.WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
