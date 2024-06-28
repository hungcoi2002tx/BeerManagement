using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.CustomAttribute
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }
        }
    }
}
