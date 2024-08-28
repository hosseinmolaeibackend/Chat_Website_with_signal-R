using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities
{
    public static class UserUtil
    {
        public static int GetUserId(this ClaimsPrincipal? principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(userId);
        }

        public static string GetUserName(this ClaimsPrincipal? principal)
        {
            var userName = principal.FindFirst(ClaimTypes.Name).Value;
            return userName;
        }
    }
}
