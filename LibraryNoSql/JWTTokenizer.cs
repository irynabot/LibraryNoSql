using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryNoSql
{
    public class JWTTokenizer
    {
        public static string GetEncodedJWT(ClaimsIdentity identity, TimeSpan lifetime)
        {
           DateTime now = DateTime.UtcNow;
           var jwt = new JwtSecurityToken(
           issuer: AuthOptions.Issuer,
           audience: AuthOptions.Audience,
           notBefore: now,
           claims: identity.Claims,
           expires: now.Add(lifetime),
           signingCredentials: new
           SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
           SecurityAlgorithms.HmacSha256));
           return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
