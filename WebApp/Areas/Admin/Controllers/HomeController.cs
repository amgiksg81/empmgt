using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork _uof) : base(_uof)
        {
        }

        public ActionResult Index()
        {
            string name = CurrentUser.Name;
            return View();
        }
    }
}