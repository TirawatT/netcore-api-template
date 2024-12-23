using Microsoft.IdentityModel.Tokens;
using NetcoreApiTemplate.Models.Dtos.Authorize;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetcoreApiTemplate.Services.Authen
{
    public class AuthorizeService(IConfiguration _config)
    {
        public LoginResponseDto? Login(LoginRequestDto item)
        {
            if (item == null) return null;
            var user = GetUser(item);
            var roles = GetRolse(user);
            var token = CreateToken(user, roles);
            var res = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Roles = roles,
            };
            return res;
        }
        private UserDto GetUser(LoginRequestDto item)
        {
            var res = new UserDto()
            {
                Username = "name",
                Email = "email",
            };
            return res;
        }
        private string[] GetRolse(UserDto item)
        {
            var res = new string[] { "VIEW", "ADMIN" };
            return res;
        }

        private string CreateToken(UserDto user, string[] roles)
        {
            var jwtWebSiteDomain = _config.GetValue<string>("JwtConfig:JwtWebSiteDomain");
            var jwtSecretKey = _config.GetValue<string>("JwtConfig:JwtSecretKey");
            var max = DateTime.MaxValue;
            var claims = GetClaims(user, roles);
            if (string.IsNullOrEmpty(jwtSecretKey))
                return string.Empty;
            var jwtSec = new JwtSecurityToken
            (
                issuer: jwtWebSiteDomain,
                audience: jwtWebSiteDomain,
                claims: claims,
                //expires: new DateTimeOffset(DateTime.Now.AddHours(8)).DateTime,
                expires: DateTime.MaxValue,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSec);
            return token;
        }
        private IEnumerable<Claim> GetClaims(UserDto user, string[] roles)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };
            roles.ToList().ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));
            return claims.AsEnumerable();
        }

    }
}
