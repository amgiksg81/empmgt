using BAL;
using System.Web.Mvc;
using DomainModels.Models;
using System;

namespace WebApp.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "SuperAdmin, Admin, PM")]
    [HandleError]
    public class ClientController : BaseController
    {
        #region [Default Constructor - IUnitOfWork]
        /// <summary>
        /// Default Constructor - IUnitOfWork
        /// </summary>
        /// <param name="_uof"></param>
        public ClientController(IUnitOfWork _uof) : base(_uof)
        {
        }
        #endregion 

        #region [CLIENT: All Client's List]
        /// <summary>
        /// All Client's List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(uof.ClientRepo.GetAll());
        }
        #endregion

        #region [CLIENT: Create/Save New Client Details]
        public ActionResult Create()
        {
            ViewBag.SampleImage = Common.SampleImagePath;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(ClientModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", model);
                }
                else
                {
                    //Employee data = UpdateEmployeeDetailsData(model);
                    //data.CreatedDate = DateTime.Now;
                    //DomainModels.Models.Common.EncryptedPassword = model.Password;

                    //DomainModels.Entities.User user = new DomainModels.Entities.User();
                    //user.Username = model.EmpEmailID;
                    //user.Password = DomainModels.Models.Common.EncryptedPassword;
                    //user.CreatedDate = DateTime.Now;
                    //Boolean boolImagePath = false;

                    ///* Getting Uploaded Profile Image */
                    //if (model.file != null)
                    //{
                    //    boolImagePath = true;
                    //    data.ProfileImage = model.ProfileImage = Path.GetFileName(model.file.FileName);
                    //}

                    //int _userID = uof.EmployeeRepo.SaveUser(user);

                    //data.UserId = model.UserId = _userID;

                    //if (model.Roles != null)
                    //{
                    //    foreach (var item in model.Roles)
                    //    {
                    //        UserRole userRole = new UserRole { UserId = data.UserId, RoleId = Convert.ToInt16(item) };
                    //        uof.UserRoleRepo.Add(userRole);
                    //    }
                    //}
                    //else
                    //{
                    //    int roleID = ((IEnumerable<Role>)TempData["AllRoles"]).Where(r => r.Name == UserInRoles.DEV).Select(d => d.RoleId).FirstOrDefault();
                    //    UserRole userRole = new UserRole { UserId = data.UserId, RoleId = roleID };
                    //    uof.UserRoleRepo.Add(userRole);
                    //}

                    //uof.EmployeeRepo.Add(data);
                    //uof.SaveChanges();


                    ///* Upload Profile Image */
                    //if (boolImagePath)
                    //{
                    //    string folderPath = Common.ProfileImagePath + data.EmployeeId + "/";
                    //    bool exists = Directory.Exists(Server.MapPath(folderPath));
                    //    if (!exists)
                    //        Directory.CreateDirectory(Server.MapPath(folderPath));

                    //    var path = Path.Combine(Server.MapPath(folderPath), data.ProfileImage);
                    //    model.file.SaveAs(path);
                    //}
                    ///* Upload Profile Image */

                    //FlashMessage.Confirmation(EmployeeResource.Employee_RecordAdded);
                    //return RedirectToAction("Index");
                    ////return View("Thankyou");
                }
            }
            catch (Exception ex)
            {
                //if (model.UserId != 0)
                //{
                //    if (model.EmployeeId != 0)
                //        DeleteUserFiles(model.EmployeeId, model.ProfileImage);

                //    uof.AuthenticateRepo.DeleteById(model.UserId);
                //    uof.SaveChanges();
                //}
            }
            return View();
        }
        #endregion        
    }
}