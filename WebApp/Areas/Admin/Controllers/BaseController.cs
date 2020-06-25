using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Security;

namespace WebApp.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
       protected IUnitOfWork uof;
        public BaseController(IUnitOfWork _uof)
        {
            uof = _uof;
        }

        public CustomPrincipal CurrentUser
        {
            get
            {
                return HttpContext.User as CustomPrincipal;
            }
        }
    }
}