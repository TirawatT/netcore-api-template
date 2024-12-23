using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using System.Security.Principal;

namespace NetcoreApiTemplate.Utilities
{
    public static class UserProfile
    {
        private static ActionContextAccessor _actionContextAccessor = new ActionContextAccessor();
        private static ClaimsPrincipal? _user => _actionContextAccessor?.ActionContext?.HttpContext.User;
        private static IEnumerable<Claim> Claims => _user?.Claims ?? new List<Claim>();
        public static object User
        {
            get
            {
                var user = new
                {
                    EmployeeNo = EmployeeNo,
                    FullName = FullName,
                    Roles = Roles,
                    IsAuthen = IsAuthen,
                };
                return user;
            }
        }
        public static string EmployeeNo
        {
            get
            {
                var result = Claims?.FirstOrDefault(x => x.Type.Equals("En", StringComparison.OrdinalIgnoreCase))?.Value;
                return result ?? string.Empty;
            }
        }
        public static string FullName
        {
            get
            {
                var result = Claims?.FirstOrDefault(x => x.Type.Equals("FullName", StringComparison.OrdinalIgnoreCase))?.Value;
                return result ?? string.Empty;
            }
        }
        public static List<string> Roles
        {
            get
            {
                var result = Claims?.Where(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase)).Select(s => s.Value).ToList();
                return result ?? new List<string>();
            }
        }
        public static IIdentity? UserIdentity
        {
            get
            {
                var user = _actionContextAccessor?.ActionContext?.HttpContext.User.Identity;
                return user;
            }
        }
        public static bool IsAuthen
        {
            get
            {
                return _user?.Identity?.IsAuthenticated ?? false;
            }
        }
        public static string GetUserClaim(string claimType)
        {
            var res = Claims?.FirstOrDefault(x => x.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;
            return res ?? string.Empty;
        }

    }
}
