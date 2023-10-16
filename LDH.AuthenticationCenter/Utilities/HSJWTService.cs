using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LDH.AuthenticationCenter.Utilities
{
    public class HSJWTService : IJWTService
    {
        private readonly JWTTokenOptions _jwtOptions;
        public HSJWTService(IOptions<JWTTokenOptions> options)
        {
            _jwtOptions = options.Value;
        }
        public string GetToken(string name)
        {
            //payload
            var claims = new Claim[]
                {

                    new Claim("Id","11"),
                    new Claim("Name",name),
                    //为授权添加
                    new Claim(ClaimTypes.Name, "luo"),
                    new Claim("EMail", "1152417278@qq.com"),
                    new Claim(ClaimTypes.Email, "luo@ldh.net"),
                    new Claim("Account", "admin"),
                    new Claim("Age", "35"),
                    new Claim("Model","15565586570"),
                    new Claim(ClaimTypes.Role,"admin"),
                    new Claim("Role","User"),
                    new Claim("Sex","1"),
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtOptions.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               issuer: _jwtOptions.Issuer,
               audience: _jwtOptions.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(10),
               signingCredentials: creds
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
