using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;

namespace Api.CustomAttribute
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Kiểm tra xác thực
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            // Kiểm tra quyền
            if (_roles.Any())
            {
                var userRoles = context.HttpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

                if (!userRoles.Any(r => _roles.Contains(r)))
                {
                    context.Result = new JsonResult(new { message = "Forbidden" })
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                    return;
                }
            }
        }
    }
}
