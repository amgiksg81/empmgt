using BAL;
using DomainModels.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DomainModels.Entities;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork _uow) : base(_uow)
        {

        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Login(string returnUrl)
        {
            //string _URL = ToTinyURLS();

            //Uri address = new Uri("http://tinyurl.com/api-create.php?url=" + "http://localhost:57415/Uploads/ProfileImages/1/Amrik Singh.jpg");
            //WebClient client = new WebClient();
            //string tinyUrl = client.DownloadString(address);

            //return tinyUrl;

            //ForgotPassword();

            if (CurrentUser != null)
            {
                if (CurrentUser.Roles.Contains(UserInRoles.SUPERADMIN) || CurrentUser.Roles.Contains(UserInRoles.ADMIN) || CurrentUser.Roles.Contains(UserInRoles.HR))
                {
                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "App");
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        protected string ToTinyURLS(string txt)
        {

            string tURL = MakeTinyUrl(txt);
            txt = txt.Replace(txt, tURL);

            //Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

            //MatchCollection mactches = regx.Matches(txt);

            //foreach (Match match in mactches)
            //{
            //    string tURL = MakeTinyUrl(match.Value);
            //    txt = txt.Replace(match.Value, tURL);
            //}

            return txt;
        }

        public static string MakeTinyUrl(string Url)
        {
            try
            {
                if (Url.Length <= 12)
                {
                    return Url;
                }
                if (!Url.ToLower().StartsWith("http") && !Url.ToLower().StartsWith("ftp"))
                {
                    Url = "http://" + Url;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + Url);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return Url;
            }
        }

        public void ForgotPassword()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //SmtpClient client = new SmtpClient("mail.ostsolutions.net");  //("smtp.ostsolutions.net");                    
                    //client.Credentials = new NetworkCredential("puriamrik@ostsolutions.net", "&venl)9Z?)Z]");
                    //client.Port = 465;

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);  //("smtp.ostsolutions.net");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    //client.Timeout = 20000;
                    client.Credentials = new NetworkCredential("puriamrik2204@gmail.com", "Alberta99");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("puriamrik2204@gmail.com");
                    mailMessage.To.Add("puriamrik@ostsolutions.net");
                    mailMessage.Subject = "Hello There";
                    mailMessage.Body = "Hello my friend!";
                    mailMessage.Priority = System.Net.Mail.MailPriority.High;

                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            UserModel user = uow.AuthenticateRepo.ValidateUser(model.Username, model.Password);
            if (user != null)
            {
                string data = JsonConvert.SerializeObject(user);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(20), false, data);
                string encTicket = FormsAuthentication.Encrypt(ticket);

                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(cookie);

                if (user.Roles.Contains(UserInRoles.SUPERADMIN) || user.Roles.Contains(UserInRoles.ADMIN) || user.Roles.Contains(UserInRoles.HR))
                {
                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Employee", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "App");
                }
            }
            ViewBag.Message = "Oops! Username/Password is incorrect.. Please provide valid credentials to prevent your login from being disabled.";
            // Code pending for 3 time attempts and deactivation... 
            return View();
        }
    }
}