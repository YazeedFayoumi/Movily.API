using Microsoft.AspNetCore.Authorization;

namespace test1.Controllers
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }

}
