using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using DomainModels.Entities;
using DomainModels.Models;
using System.IO;
using Vereyon.Web;
using WebApp.App_GlobalResources;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using GemBox.Document;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Collections.Generic;

namespace WebApp.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "SuperAdmin, Admin, HR")]
    [HandleError]
    public class EmployeeController : BaseController
    {
        #region [Default Constructor - IUnitOfWork]
        /// <summary>
        /// Default Constructor - IUnitOfWork
        /// </summary>
        /// <param name="_uof"></param>
        public EmployeeController(IUnitOfWork _uof) : base(_uof)
        {
        }
        #endregion        

        #region [EMPLOYEE: All Employee's List]
        /// <summary>
        /// All Employees List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            bool exists = Directory.Exists(Server.MapPath(Common.TempEmployeeDocPath));
            if (exists)
                Directory.Delete(Server.MapPath(Common.TempEmployeeDocPath), true);

            return View(uof.EmployeeRepo.GetAll());
        }
        #endregion

        #region [EMPLOYEE: Get Selected Employee Details]
        /// <summary>
        /// Details of the Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(id, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                EmployeeModel model = EmpDetails(data);
                TempData["CurrentAction"] = "Details";
                ViewBag.ShowActions = true;
                ViewBag.ShowDelete = true;

                //Check if Role is HR and Current accessed employee is Admin
                if (CurrentUser.Roles.Contains(UserInRoles.HR))
                {
                    ViewBag.ShowDelete = false;

                    if (data.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.ADMIN))
                        ViewBag.ShowActions = false;
                }
                return View(model);
            }
        }
        #endregion

        #region [EMPLOYEE: Create/Save New Employee/User Details]
        public ActionResult Create()
        {
            ViewBag.SampleImage = Common.SampleImagePath;
            ViewBag.UserRoles = TempData["AllRoles"] = GetAllRoles();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.UserRoles = GetAllRoles();
                    ViewBag.Password = model.Password;
                    return View("Create", model);
                }
                else
                {
                    Employee data = UpdateEmployeeDetailsData(model);
                    data.CreatedDate = DateTime.Now;
                    DomainModels.Models.Common.EncryptedPassword = model.Password;

                    DomainModels.Entities.User user = new DomainModels.Entities.User();
                    user.Username = model.EmpEmailID;
                    user.Password = DomainModels.Models.Common.EncryptedPassword; 
                    user.CreatedDate = DateTime.Now;
                    Boolean boolImagePath = false;

                    /* Getting Uploaded Profile Image */
                    if (model.file != null)
                    {
                        boolImagePath = true;
                        data.ProfileImage = model.ProfileImage = Path.GetFileName(model.file.FileName);
                    }

                    int _userID = uof.EmployeeRepo.SaveUser(user);

                    data.UserId = model.UserId = _userID;

                    if (model.Roles != null)
                    {
                        foreach (var item in model.Roles)
                        {
                            UserRole userRole = new UserRole { UserId = data.UserId, RoleId = Convert.ToInt16(item) };
                            uof.UserRoleRepo.Add(userRole);
                        }
                    }
                    else
                    {
                        int roleID = ((IEnumerable<Role>)TempData["AllRoles"]).Where(r => r.Name == UserInRoles.DEV).Select(d => d.RoleId).FirstOrDefault();
                        UserRole userRole = new UserRole { UserId = data.UserId, RoleId = roleID };
                        uof.UserRoleRepo.Add(userRole);
                    }

                    uof.EmployeeRepo.Add(data);
                    uof.SaveChanges();


                    /* Upload Profile Image */
                    if (boolImagePath)
                    {
                        string folderPath = Common.ProfileImagePath + data.EmployeeId + "/";
                        bool exists = Directory.Exists(Server.MapPath(folderPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(folderPath));

                        var path = Path.Combine(Server.MapPath(folderPath), data.ProfileImage);
                        model.file.SaveAs(path);
                    }
                    /* Upload Profile Image */

                    FlashMessage.Confirmation(EmployeeResource.Employee_RecordAdded);
                    return RedirectToAction("Index");
                    //return View("Thankyou");
                }
            }
            catch (Exception ex)
            {
                if (model.UserId != 0)
                {
                    if (model.EmployeeId != 0)
                        DeleteUserFiles(model.EmployeeId, model.ProfileImage);

                    uof.AuthenticateRepo.DeleteById(model.UserId);
                    uof.SaveChanges();
                }
            }
            return View();
        }
        #endregion        

        #region [EMPLOYEE: Edit Fill/Update Details]
        /// <summary>
        /// Edit Record Fill Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(id, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                EmployeeModel model = EmpDetails(data);
                TempData["CurrentAction"] = "Edit";
                ViewBag.ShowActions = true;
                ViewBag.ShowDelete = true;

                //Check if Role is HR and Current accessed employee is Admin
                if (CurrentUser.Roles.Contains(UserInRoles.HR))
                {
                    ViewBag.ShowDelete = false;

                    if (data.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.ADMIN))
                        ViewBag.ShowActions = false;
                }

                return View(model);
            }
        }

        /// <summary>
        /// Edit Record Update Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.UserRoles = GetAllRoles();
                    ViewBag.EditPassword = model.EditPassword;
                    return View("Edit", model);
                }
                else
                {
                    Employee data = UpdateEmployeeDetailsData(model);

                    data.UserId = model.UserId;
                    data.CreatedDate = model.CreatedDate;
                    data.UpdatedDate = DateTime.Now;

                    if (model.file != null)
                    {
                        string folderPath = "~/" + Common.ProfileImagePath + model.EmployeeId + "/";

                        //deleting previous one
                        var filePath = Server.MapPath(folderPath + model.ProfileImage);
                        if (System.IO.File.Exists(filePath))
                        {
                            if (filePath.IndexOf("sample.jpg") == -1)
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        /* Upload Profile Image */
                        bool exists = Directory.Exists(Server.MapPath(folderPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(folderPath));

                        //saving file
                        var fileName = Path.GetFileName(model.file.FileName);
                        var path = Path.Combine(Server.MapPath(folderPath), fileName);
                        model.file.SaveAs(path);
                        model.ProfileImage = fileName;
                        /* Upload Profile Image */
                    }

                    data.ProfileImage = model.ProfileImage;

                    if (model.EditPassword != null)
                    {
                        DomainModels.Models.Common.EncryptedPassword = model.EditPassword;

                        if (!string.IsNullOrEmpty(model.EditPassword.Trim()) && model.EditPassword != model.Password)
                        {
                            DomainModels.Entities.User user = new DomainModels.Entities.User();
                            user.UserId = data.UserId;
                            user.Username = data.EmpEmailID;
                            user.UpdatedDate = DateTime.Now;
                            user.CreatedDate = model.CreatedDate;
                            user.Password = DomainModels.Models.Common.EncryptedPassword;  

                            uof.AuthenticateRepo.Modify(user);
                            uof.SaveChanges();
                        }
                    }

                    if (model.Roles != null)
                    {
                        string[] tempRoles = uof.UserRoleRepo.GetUserRoles(data.UserId).Select(d => d.RoleId.ToString()).ToArray();

                        // Delete Roles
                        for (int iCounter = 0; iCounter < tempRoles.Count(); iCounter++)
                        {
                            if (!model.Roles.Contains(tempRoles[iCounter]))
                            {
                                UserRole userRole = uof.UserRoleRepo.GetUserRoles(model.UserId).Where(d => d.RoleId == Convert.ToInt32(tempRoles[iCounter])).FirstOrDefault();
                                uof.UserRoleRepo.Delete(userRole);
                            }
                        }

                        // Add New Roles
                        for (int iCounter = 0; iCounter < model.Roles.Count(); iCounter++)
                        {
                            if (!tempRoles.Contains(model.Roles[iCounter]))
                            {
                                uof.UserRoleRepo.Add(new UserRole { UserId = model.UserId, RoleId = Convert.ToInt32(model.Roles[iCounter]) });
                            }
                        }
                    }
                    else
                    {
                        uof.UserRoleRepo.DeleteList(uof.UserRoleRepo.GetUserRoles(model.UserId));

                        int roleID = ((IEnumerable<Role>)TempData["AllRoles"]).Where(r => r.Name == UserInRoles.DEV).Select(d => d.RoleId).FirstOrDefault();
                        UserRole userRole = new UserRole { UserId = data.UserId, RoleId = roleID };
                        uof.UserRoleRepo.Add(userRole);
                    }

                    uof.EmployeeRepo.Modify(data);
                    uof.SaveChanges();

                    FlashMessage.Confirmation(EmployeeResource.Employee_RecordUpdated);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                TempData.Remove("UserRoles");
                TempData.Remove("AllRoles");
            }
            return View();
        }
        #endregion

        #region [EMPLOYEE: Get Employee Details By ID]
        /// <summary>
        /// Get Employee Details By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private EmployeeModel EmpDetails(Employee data)
        {
            EmployeeModel model = new EmployeeModel();

            if (data != null)
            {
                model.EmployeeId = data.EmployeeId;
                model.EmpFullName = data.EmpFullName;
                model.EmpFatherName = data.EmpFatherName;
                model.EmpMotherName = data.EmpMotherName;
                model.EmpEmailID = data.EmpEmailID;
                model.EmpPersonalEmailID = data.EmpPersonalEmailID;
                model.OfficialSkypeID = data.OfficialSkypeID;
                model.PersonalSkypeID = data.PersonalSkypeID;
                model.EmpDOB = data.EmpDOB;
                model.PANNumber = data.PANNumber;
                model.AadharNumber = data.AadharNumber;
                model.TotalPrevExperience = data.TotalPrevExperience;
                model.PrevCompanySalary = data.PrevCompanySalary;
                model.SalaryHiredAt = data.SalaryHiredAt;
                model.LastDrawnSalary = data.LastDrawnSalary;
                model.JoiningDate = data.JoiningDate;
                model.ResignDate = data.ResignDate;
                model.RelievingDate = data.RelievingDate;
                model.ResignReason = data.ResignReason;
                model.LocalAddress = data.LocalAddress;
                model.PermanentAddress = data.PermanentAddress;
                model.ContactNo = data.ContactNo;
                model.Reference1 = data.Reference1;
                model.Reference1No = data.Reference1No;
                model.Reference2 = data.Reference2;
                model.Reference2No = data.Reference2No;
                model.CreatedDate = data.CreatedDate;

                model.EmployeeDocuments = data.EmployeeDocuments;
                ViewBag.DocumentPath = Common.ProfileImagePath + data.EmployeeId + "/" + @Common.EmployeeDocFolderName;

                // Hide Admin Fields/Controls on the page
                ViewBag.HideAdminFields = HideAdminFieldDetails();

                if (data.ProfileImage != null)
                {
                    if (!string.IsNullOrEmpty(data.ProfileImage.Trim()))
                    {
                        model.ProfileImage = data.ProfileImage;
                        string folderPath = Common.ProfileImagePath + model.EmployeeId + "/";
                        ViewBag.ProfileImage = folderPath + model.ProfileImage;
                    }
                }
                else
                    ViewBag.ProfileImage = Common.SampleImagePath;

                model.UserId = data.UserId;

                DomainModels.Models.Common.DecryptedPassword = data.User.Password;
                model.Password = model.ConfirmPassword = model.EditConfirmPassword = ViewBag.EditPassword = DomainModels.Models.Common.DecryptedPassword;

                ViewBag.UserRoles = TempData["AllRoles"] = GetAllRoles();
                model.Roles = uof.UserRoleRepo.GetUserRoles(data.UserId).Select(d => d.RoleId.ToString()).ToArray();
            }
            return model;
        }
        #endregion

        #region [EMPLOYEE: Check If User Already Exists]
        /// <summary>
        /// Check if user already exists with this Email Id.
        /// </summary>
        /// <param name="EmpEmailID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsAlreadySigned(string EmpEmailID)
        {
            return Json(IsUserAvailable(EmpEmailID));
        }

        private bool IsUserAvailable(string EmpEmailID)
        {
            var RegEmailId = (from e in uof.EmployeeRepo.GetAll()
                              where e.EmpEmailID.ToUpper() == EmpEmailID.ToUpper()
                              select new { EmpEmailID }).FirstOrDefault();

            bool status;
            if (RegEmailId != null)
            {
                //Already registered
                status = false;
            }
            else
            {
                //Available to use
                status = true;
            }
            return status;
        }
        #endregion

        #region [EMPLOYEE: Update Employee Details for Create/Edit Record]
        /// <summary>
        /// Update Employee Details for Create/Edit Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Employee UpdateEmployeeDetailsData(EmployeeModel model)
        {
            Employee data = new Employee();

            data.EmployeeId = model.EmployeeId;
            data.EmpFullName = model.EmpFullName;
            data.EmpFatherName = model.EmpFatherName;
            data.EmpMotherName = model.EmpMotherName;
            data.EmpEmailID = model.EmpEmailID;
            data.EmpPersonalEmailID = model.EmpPersonalEmailID;
            data.OfficialSkypeID = model.OfficialSkypeID;
            data.PersonalSkypeID = model.PersonalSkypeID;
            data.EmpDOB = model.EmpDOB;
            data.PANNumber = model.PANNumber;
            data.AadharNumber = model.AadharNumber;
            data.TotalPrevExperience = model.TotalPrevExperience;
            data.PrevCompanySalary = model.PrevCompanySalary;

            data.SalaryHiredAt = model.SalaryHiredAt;
            data.LastDrawnSalary = model.LastDrawnSalary;
            data.JoiningDate = model.JoiningDate;
            data.ResignDate = model.ResignDate;
            data.RelievingDate = model.RelievingDate;
            data.ResignReason = model.ResignReason;
            data.LocalAddress = model.LocalAddress;
            data.PermanentAddress = model.PermanentAddress;
            data.ContactNo = model.ContactNo;
            data.Reference1 = model.Reference1;

            data.Reference1No = model.Reference1No;
            data.Reference2 = model.Reference2;
            data.Reference2No = model.Reference2No;
            return data;
        }
        #endregion               

        #region [EMPLOYEE: Delete User/Employee Record Permanently]
        /// <summary>
        /// Delete Employee/User Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [CustomAuthorization(Roles = "SuperAdmin, Admin")]
        public ActionResult Delete(int id, int UserId, string file)
        {
            Employee data = uof.EmployeeRepo.GetById(id);

            if (CurrentUser.UserId == UserId || (CurrentUser.Roles.Contains(UserInRoles.ADMIN) && data.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.ADMIN)))
                return RedirectToAction("UnAuthorized");

            uof.EmployeeDocsRepo.DeleteAllDocuments(id);
            uof.SaveChanges();

            uof.UserRoleRepo.DeleteList(uof.UserRoleRepo.GetUserRoles(UserId));
            uof.SaveChanges();

            uof.EmployeeRepo.DeleteById(id);
            uof.SaveChanges();

            uof.AuthenticateRepo.DeleteById(UserId);
            uof.SaveChanges();

            DeleteUserFiles(id, file);

            //TempData["Deleted"] = "Record has been deleted successfully.";
            FlashMessage.Danger(EmployeeResource.Employee_RecordDeleted);
            return RedirectToAction("Index");
        }
        #endregion

        #region [EMPLOYEE: Delete User Files for given UserId]
        /// <summary>
        /// Delete User Files for given UserId
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="file"></param>
        private void DeleteUserFiles(int EmployeeId, string file)
        {
            string folderPath = "~/" + Common.ProfileImagePath + EmployeeId + "/";
            bool exists = Directory.Exists(Server.MapPath(folderPath));
            if (exists)
                Directory.Delete(Server.MapPath(folderPath), true);
        }
        #endregion

        #region [DOCUMENT: Employee's Documents List]
        /// <summary>
        /// All Employees List
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentList(int id)
        {
            ViewBag.EmployeeID = id;
            TempData.Remove("CurrentAction");

            Employee data = null;
            string returnResult = RoleBasedAuthorizations(id, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                ViewBag.EmpFullName = data.EmpFullName;
                return View();
            }
        }
        #endregion

        #region [DOCUMENT: Employee's Documents Partial List]
        /// <summary>
        /// All Employees List
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentPartialList(int id)
        {
            ViewBag.EmployeeID = id;
            ViewBag.DocumentPath = Common.ProfileImagePath + id + "/" + @Common.EmployeeDocFolderName;
            return PartialView("_DocumentList", uof.EmployeeDocsRepo.GetAll().Where(e => e.EmployeeId == id));
        }
        #endregion

        #region [DOCUMENT: Add New Document]
        /// <summary>
        /// All Employees List
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentAddNew(int id)
        {
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(id, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                ViewBag.EmpFullName = data.EmpFullName;
                EmployeeDocumentModel empDocument = new EmployeeDocumentModel();
                empDocument.EmployeeId = id;
                return View(empDocument);
            }
        }
        #endregion        

        #region [DOCUMENT: Upload New Document]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DocumentAddNew(EmployeeDocumentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("DocumentAddNew", model);
                }
                else
                {
                    EmployeeDocument data = new EmployeeDocument();
                    data.CreatedDate = DateTime.Now;
                    data.EmployeeId = model.EmployeeId;
                    data.ImageName = model.ImageName;
                    data.ImagePath = Path.GetFileName(model.file.FileName);

                    Boolean FileUploaded = false;
                    if (model.file != null)
                    {
                        FileUploaded = true;
                        data.FileType = Path.GetExtension(model.file.FileName).Replace(".", "");
                    }

                    uof.EmployeeDocsRepo.Add(data);
                    uof.SaveChanges();

                    int _empDocumentID = data.EmployeeDocId;

                    ///* Upload Document */
                    if (_empDocumentID != 0 && FileUploaded)
                    {
                        string folderPath = Common.ProfileImagePath + data.EmployeeId + "/" + Common.EmployeeDocFolderName + "/";
                        bool exists = Directory.Exists(Server.MapPath(folderPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(folderPath));

                        var path = Path.Combine(Server.MapPath(folderPath), _empDocumentID + data.ImagePath);
                        model.file.SaveAs(path);
                    }
                    ///* Upload Profile Image */

                    string redirectPage = DocumentReturnAction();

                    FlashMessage.Confirmation(EmployeeResource.Employee_DocumentUploaded);

                    if (redirectPage.ToLower().Contains("documentlist"))
                        return Redirect(Url.Action(redirectPage, new { id = model.EmployeeId }));
                    else
                        return Redirect(Url.Action(redirectPage, new { id = model.EmployeeId }) + "#List");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        #endregion

        #region [DOCUMENT: Delete Document by EmployeeDocumentId]
        /// <summary>
        /// Delete Document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult DeleteDocument(int id)
        {
            EmployeeDocument empDocument = uof.EmployeeDocsRepo.GetById(id);
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(empDocument.EmployeeId, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                uof.EmployeeDocsRepo.DeleteById(id);
                uof.SaveChanges();

                string folderPath = Common.ProfileImagePath + empDocument.EmployeeId + "/" + Common.EmployeeDocFolderName + "/";
                var filePath = Server.MapPath(folderPath + empDocument.EmployeeDocId + empDocument.ImagePath);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                string redirectPage = DocumentReturnAction();

                FlashMessage.Danger(EmployeeResource.Employee_DocumentDeleted);

                if (redirectPage.ToLower().Contains("documentlist"))
                    return Redirect(Url.Action(redirectPage, new { id = empDocument.EmployeeId }));
                else
                    return Redirect(Url.Action(redirectPage, new { id = empDocument.EmployeeId }) + "#List");
            }
        }
        #endregion

        #region [DOCUMENT: Return to Action Page After Document Upload/Delete]
        /// <summary>
        /// Return to Action Page After Document Upload/Delete
        /// </summary>
        /// <returns></returns>
        private string DocumentReturnAction()
        {
            String redirectPage = "DocumentList";

            if (TempData["CurrentAction"] != null)
            {
                String currentAction = TempData["CurrentAction"].ToString().ToLower();
                TempData["CurrentAction"] = null;

                switch (currentAction)
                {
                    case "edit":
                        redirectPage = "Edit";
                        break;
                    case "details":
                        redirectPage = "Details";
                        break;
                    default:
                        redirectPage = "DocumentList";
                        break;
                }
            }

            return redirectPage;
        }
        #endregion                

        #region [DOCUMENT: Document View - Using GemBox Library]
        /// <summary>
        /// Document View - Using GemBox Library
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentView(int id)
        {
            EmployeeDocument empDocument = uof.EmployeeDocsRepo.GetById(id);
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(empDocument.EmployeeId, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                string folderPath = Common.ProfileImagePath + empDocument.EmployeeId + "/" + Common.EmployeeDocFolderName + "/";
                var filePath = Server.MapPath(folderPath + empDocument.EmployeeDocId + empDocument.ImagePath);

                empDocument.FileType = empDocument.FileType.ToLower();
                if (empDocument.FileType == "docx" || empDocument.FileType == "doc")
                {
                    // If using Professional version, put your serial key below.
                    ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                    ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
                    DocumentModel document = DocumentModel.Load(filePath);

                    // Images will be embedded directly in HTML img src attribute.
                    string randomName = DateTime.Now.Ticks.ToString();
                    object htmlFilePath = Server.MapPath(Common.TempEmployeeDocPath) + randomName + ".htm";
                    string directoryPath = Common.TempEmployeeDocPath + randomName + "_files/";

                    CreateTempFolder();
                    document.Save(htmlFilePath.ToString(), new HtmlSaveOptions() { EmbedImages = true });

                    //Read the saved Html File.
                    string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString()).Replace("�", "");

                    //Loop and replace the Image Path.
                    foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
                    {
                        wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, Common.TempEmployeeDocPath + match.Groups[1].Value);
                    }

                    wordHTML = wordHTML.Replace("src=\"" + randomName + "_files/", "src=\"" + Common.SiteURL + directoryPath);
                    wordHTML = wordHTML.Replace("src=\"" + Common.TempEmployeeDocPath + randomName + "_files/", "src=\"" + Common.SiteURL + directoryPath);

                    ViewBag.Content = wordHTML;

                    //Delete the Html File
                    System.IO.File.Delete(htmlFilePath.ToString());
                }
                else if (empDocument.FileType == "pdf")
                {
                    filePath = folderPath + empDocument.EmployeeDocId + empDocument.ImagePath;

                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"500px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ViewBag.Content = string.Format(embed, VirtualPathUtility.ToAbsolute(filePath));
                }
                else
                {
                    filePath = folderPath + empDocument.EmployeeDocId + empDocument.ImagePath;
                    ViewBag.Content = "<img src='" + filePath + "' style='max-width:1120px;'/>";
                }
                ViewBag.EmployeeId = empDocument.EmployeeId;
                ViewBag.EmployeeDocId = empDocument.EmployeeDocId;
                ViewBag.Print = "false";
                ViewBag.EmpFullName = data.EmpFullName;
                return View();
            }
        }
        #endregion

        #region [DOCUMENT: Document View Using Microsoft.Office.Interop.Word - Not In Use]
        /// <summary>
        /// Document View Using Microsoft.Office.Interop.Word - Not In Use
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentViewUsingInteropWord(int id)
        {
            EmployeeDocument empDocument = uof.EmployeeDocsRepo.GetById(id);
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(empDocument.EmployeeId, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                string folderPath = Common.ProfileImagePath + empDocument.EmployeeId + "/" + Common.EmployeeDocFolderName + "/";
                var filePath = Server.MapPath(folderPath + empDocument.EmployeeDocId + empDocument.ImagePath);

                empDocument.FileType = empDocument.FileType.ToLower();
                if (empDocument.FileType == "docx" || empDocument.FileType == "doc")
                {
                    object documentFormat = 8;
                    string randomName = DateTime.Now.Ticks.ToString();
                    object htmlFilePath = Server.MapPath(Common.TempEmployeeDocPath) + randomName + ".htm";
                    string directoryPath = Common.TempEmployeeDocPath + randomName + "_files/";
                    object fileSavePath = Server.MapPath(Common.TempEmployeeDocPath) + randomName + empDocument.ImagePath;

                    //If Directory not present, create it.
                    if (!Directory.Exists(Server.MapPath(Common.TempEmployeeDocPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Common.TempEmployeeDocPath));
                    }

                    try
                    {
                        //Upload the word document and save to Temp folder.
                        System.IO.File.Copy(filePath, fileSavePath.ToString(), false);

                        //Open the word document in background.
                        Application applicationclass = new Application();
                        applicationclass.Documents.Open(ref fileSavePath);
                        applicationclass.Visible = false;
                        Document document = applicationclass.ActiveDocument;

                        //Save the word document as HTML file.
                        document.SaveAs(ref htmlFilePath, ref documentFormat);

                        //Close the word document.
                        document.Close();

                        //Read the saved Html File.
                        string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString()).Replace("�", "");

                        //Loop and replace the Image Path.
                        foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
                        {
                            wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, Common.TempEmployeeDocPath + match.Groups[1].Value);
                        }

                        wordHTML = wordHTML.Replace("src=\"" + randomName + "_files/", "src=\"" + Common.SiteURL + directoryPath);
                        wordHTML = wordHTML.Replace("src=\"" + Common.TempEmployeeDocPath + randomName + "_files/", "src=\"" + Common.SiteURL + directoryPath);

                        ViewBag.Content = wordHTML;

                        //Delete the Uploaded Word File.
                        System.IO.File.Delete(fileSavePath.ToString());

                        //Delete the Html File
                        System.IO.File.Delete(htmlFilePath.ToString());
                        directoryPath = Server.MapPath(Common.TempEmployeeDocPath + randomName + "_files/");
                        if (Directory.GetFiles(directoryPath) != null)
                        {
                            foreach (string file in Directory.GetFiles(directoryPath))
                            {
                                System.IO.File.Delete(file);
                            }
                            Directory.Delete(directoryPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Content = "File not available.";
                    }
                }
                else if (empDocument.FileType == "pdf")
                {
                    filePath = folderPath + empDocument.EmployeeDocId + empDocument.ImagePath;

                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"500px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ViewBag.Content = string.Format(embed, VirtualPathUtility.ToAbsolute(filePath));
                }
                else
                {
                    filePath = folderPath + empDocument.EmployeeDocId + empDocument.ImagePath;
                    ViewBag.Content = "<img src='" + filePath + "' style='max-width:1120px;'/>";
                }
                ViewBag.EmployeeId = empDocument.EmployeeId;
                ViewBag.EmployeeDocId = empDocument.EmployeeDocId;

                return View();
            }
        }
        #endregion

        #region [DOCUMENT: Download File]
        /// <summary>
        /// Download File 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DownloadDocument(int id)
        {
            EmployeeDocument empDocument = uof.EmployeeDocsRepo.GetById(id);
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(empDocument.EmployeeId, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                string fileName = empDocument.EmployeeDocId + empDocument.ImagePath;
                string FullFilePath = Server.MapPath(Common.ProfileImagePath + empDocument.EmployeeId + "/" + Common.EmployeeDocFolderName + "/" + fileName);
                return File(FullFilePath, MimeMapping.GetMimeMapping(FullFilePath), empDocument.ImagePath);
            }
        }
        #endregion

        #region [DOCUMENT: Document Print]
        /// <summary>
        /// Document Print
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DocumentPrint(int id)
        {
            EmployeeDocument empDocument = uof.EmployeeDocsRepo.GetById(id);
            Employee data = null;
            string returnResult = RoleBasedAuthorizations(empDocument.EmployeeId, ref data);
            if (returnResult == "UnAuthorized")
                return View("UnAuthorized");
            else if (returnResult == "RedirectToAction")
                return RedirectToAction("Index");
            else
            {
                string folderPath = Common.ProfileImagePath + empDocument.EmployeeId + "/" + Common.EmployeeDocFolderName + "/";
                var source_FilePath = Server.MapPath(folderPath + empDocument.EmployeeDocId + empDocument.ImagePath);

                string randomName = DateTime.Now.Ticks.ToString();
                string target_FilePath = "";

                empDocument.FileType = empDocument.FileType.ToLower();
                if (empDocument.FileType == "docx" || empDocument.FileType == "doc")
                {
                    target_FilePath = Path.ChangeExtension((Server.MapPath(Common.TempEmployeeDocPath) + randomName + empDocument.ImagePath), ".pdf");

                    // If using Professional version, put your serial key below.
                    ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                    ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
                    DocumentModel.Load(source_FilePath).Save(target_FilePath);

                    target_FilePath = Common.TempEmployeeDocPath + randomName + Path.ChangeExtension(empDocument.ImagePath, ".pdf");
                }
                else if (empDocument.FileType == "pdf")
                {
                    target_FilePath = folderPath + empDocument.EmployeeDocId + empDocument.ImagePath;
                }
                else
                {
                    CreateTempFolder();
                    target_FilePath = Path.ChangeExtension((Server.MapPath(Common.TempEmployeeDocPath) + randomName + empDocument.ImagePath), ".pdf");
                    SaveImageAsPdf(source_FilePath, target_FilePath);
                    target_FilePath = Common.TempEmployeeDocPath + randomName + Path.ChangeExtension(empDocument.ImagePath, ".pdf");
                }

                ViewBag.EmployeeId = empDocument.EmployeeId;
                ViewBag.EmployeeDocId = empDocument.EmployeeDocId;

                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"500px\">";
                embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                embed += "</object>";

                ViewBag.Content = string.Format(embed, VirtualPathUtility.ToAbsolute(target_FilePath));
                ViewBag.Print = "true";
                ViewBag.EmpFullName = data.EmpFullName;
                return View("DocumentView");
            }
        }
        #endregion

        #region [DOCUMENT: Temp Folder for Temp Files]
        /// <summary>
        /// Create Temp Folder for Temp Files
        /// </summary>
        private void CreateTempFolder()
        {
            bool exists = Directory.Exists(Server.MapPath(Common.TempEmployeeDocPath));
            if (!exists)
                Directory.CreateDirectory(Server.MapPath(Common.TempEmployeeDocPath));
        }
        #endregion        

        #region [DOCUMENT: Save Image to PDF File]
        /// <summary>
        /// Save Image to PDF File
        /// </summary>
        /// <param name="imageFileName"></param>
        /// <param name="pdfFileName"></param>
        /// <param name="width"></param>
        /// <param name="deleteImage"></param>
        internal void SaveImageAsPdf(string imageFileName, string pdfFileName, int width = 600, bool deleteImage = false)
        {
            using (var document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                using (XImage img = XImage.FromFile(imageFileName))
                {
                    // Calculate new height to keep image ratio
                    var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                    // Change PDF Page size to match image
                    page.Width = width;
                    page.Height = height;

                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(img, 0, 0, width, height);
                }
                document.Save(pdfFileName);
            }

            if (deleteImage)
                System.IO.File.Delete(imageFileName);
        }
        #endregion

        #region [ROLE: Set User Actions based on Role (Documents / Edit / Details / Delete)]
        /// <summary>
        /// Set User Actions based on Role [Documents / Edit / Details / Delete ]
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public ActionResult UserActions(Employee emp)
        {
            ViewBag.id = emp.EmployeeId;
            ViewBag.UserId = emp.UserId;
            ViewBag.file = emp.ProfileImage;
            ViewBag.DeleteActions = true;

            //Check if self details
            if (CurrentUser.UserId != emp.UserId)
                ViewBag.ShowActions = true;

            //Check if Role is HR and Current accessed employee is Admin
            if (CurrentUser.Roles.Contains(UserInRoles.HR) || CurrentUser.Roles.Contains(UserInRoles.ADMIN))
            {
                if(CurrentUser.Roles.Contains(UserInRoles.ADMIN))
                    ViewBag.DeleteActions = true;
                else
                    ViewBag.DeleteActions = false;

                if (emp.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.ADMIN))
                {
                    ViewBag.ShowActions = false;
                    ViewBag.DeleteActions = false;
                }
            }
            return PartialView("_UserActions");
        }
        #endregion

        #region [ROLE: Hide Admin Field Details]
        /// <summary>
        /// Hide Admin Field Details
        /// </summary>
        /// <returns></returns>
        private bool HideAdminFieldDetails()
        {
            // To Show/Hide Super Admin Data/Fields from editing...
            if (CurrentUser.Roles.Contains(UserInRoles.SUPERADMIN))
                return false;
            else
                return true;
        }
        #endregion

        #region [ROLE: Get All User Roles Except Admin]
        /// <summary>
        /// Get All User Roles Except Admin
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Role> GetAllRoles()
        {
            if (CurrentUser.Roles.Contains(UserInRoles.SUPERADMIN))
                return uof.RoleRepo.GetAll().Where(d => d.Name != "SuperAdmin");
            else
                return uof.RoleRepo.GetAll().Where(d => d.Name != "SuperAdmin" && d.Name != "Admin");

        }
        #endregion

        #region [ROLE: Show User Roles By User ID]
        /// <summary>
        /// Show User Roles By User ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowUserRoles(int id)
        {
            IEnumerable<UserRole> roles = uof.UserRoleRepo.GetUserRoles(id);
            if (roles.Count() > 0)
                ViewBag.RoleNames = roles.Select(a => a.Role.Name).Aggregate((i, j) => i + ", " + j);
            else
                ViewBag.RoleNames = "No Role";
            return PartialView("_UserRoles");
        }
        #endregion

        #region [ROLE: Role Based Authorizations for Edit/Delete Actions]
        /// <summary>
        /// Role Based Authorizations for Edit/Delete Actions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string RoleBasedAuthorizations(int id, ref Employee data)
        {
            data = uof.EmployeeRepo.GetById(id);
            string returnResult = "";
            if (data != null)
            {

                if (data.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.SUPERADMIN) || CurrentUser.UserId == data.UserId)
                {
                    returnResult = "UnAuthorized";
                }
                else if (data.User.UserRoles.Select(d => d.Role.Name).Contains(UserInRoles.ADMIN) && CurrentUser.UserId != data.UserId
                    && !CurrentUser.Roles.Contains(UserInRoles.SUPERADMIN))
                {
                    returnResult = "UnAuthorized";
                }
            }
            else
                returnResult = "RedirectToAction";
            return returnResult;
        }
        #endregion

        #region [ROLE: Un-Authorized Access]
        /// <summary>
        /// ROLE: Un-Authorized Access
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAuthorized()
        {
            return View();
        }
        #endregion        
    }
}