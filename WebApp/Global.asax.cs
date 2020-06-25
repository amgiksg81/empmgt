using DomainModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebApp.Security;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //if (Request.UserHostAddress != "192.168.1.1")
            //{
            //    FormsAuthentication.SignOut();
            //    Response.Redirect("~/Account/Login");
            //    //throw new PageNotFoundException();
            //}
            //else
            //{
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (!authTicket.Expired) //check for ticket expiration
                    {
                        UserModel serializeModel = JsonConvert.DeserializeObject<UserModel>(authTicket.UserData);

                        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                        newUser.Username = serializeModel.Username;
                        newUser.Name = serializeModel.Name;
                        newUser.Password = serializeModel.Password;
                        newUser.UserId = serializeModel.UserId;
                        newUser.Roles = serializeModel.Roles;

                        HttpContext.Current.User = newUser;
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        Response.Redirect("~/Account/Login");
                    }
                }
            //}
        }
    }
}
