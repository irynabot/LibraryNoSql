using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryNoSql
{
    public static class HttpContextExtensions
    {
        public static string GetNameFromToken(this HttpContext context)
        {
            Claim userClaim = context?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            return userClaim?.Value;
        }

    }
}
