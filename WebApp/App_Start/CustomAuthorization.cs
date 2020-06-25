using System.Linq;
using System.Web.Mvc;

namespace WebApp
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                string viewResult = "";
                if (filterContext.HttpContext.User.IsInRole("Admin") || filterContext.HttpContext.User.IsInRole("SuperAdmin") || filterContext.HttpContext.User.IsInRole("HR"))
                    viewResult = "~/Areas/Admin/Views/Employee/UnAuthorized.cshtml";
                else
                    viewResult = "~/Views/Error/UnAuthorized.cshtml";

                filterContext.Result = new ViewResult
                {
                    ViewName = viewResult
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}