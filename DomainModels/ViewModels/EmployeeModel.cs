using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities;
using System.Web.Mvc;
using System.Web;
using DomainModels.Resources;

namespace DomainModels.Models
{
    [NotMapped]
    public class EmployeeModel: UserModel
    {
        [ScaffoldColumn(true)]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpFullName_Display")]
        [StringLength(100)]
        [Required(ErrorMessageResourceType = (typeof(EmployeeValidations)), ErrorMessageResourceName = "EmpFullName_Required")]
        public string EmpFullName { get; set; }

        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpFatherName_Display")]
        [StringLength(100)]
        public string EmpFatherName { get; set; } //39700

        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpMotherName_Display")]
        [StringLength(100)]
        public string EmpMotherName { get; set; }

        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpEmailID_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "EmpEmailID_Invalid")]
        [UIHint("_Email")]
        [Required(ErrorMessageResourceType = (typeof(EmployeeValidations)), ErrorMessageResourceName = "EmpEmailID_Required")]
        [IsEmailExists]
        [Remote("IsAlreadySigned", "Employee", HttpMethod = "POST", ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "EmpEmailID_AlreadyExists")]
        [DataType(DataType.EmailAddress)]
        public string EmpEmailID { get; set; }

        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpPersonalEmailID_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "EmpEmailID_Invalid")]
        [UIHint("_Email")]
        [DataType(DataType.EmailAddress)]
        public string EmpPersonalEmailID { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "OfficialSkype_Display")]
        [StringLength(50)]
        public string OfficialSkypeID { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "PersonalSkype_Display")]
        [StringLength(50)]
        public string PersonalSkypeID { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name= "EmpDOB_Display")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Common.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? EmpDOB { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "PANNumber_Display")]
        [StringLength(15)]
        public string PANNumber { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "AadharNumber_Display")]
        [StringLength(50)]
        public string AadharNumber { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "TotalPrevExperience_Display")]
        [RegularExpression(Common.ValidIntegersValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "TotalPrevExperience_ValidInteger")]
        public double? TotalPrevExperience { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "PrevCompanySalary_Display")]
        [RegularExpression(Common.ValidIntegersValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "PrevCompanySalary_ValidInteger")]
        public double? PrevCompanySalary { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "SalaryHiredAt_Display")]
        [RegularExpression(Common.ValidIntegersValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "SalaryHiredAt_ValidInteger")]
        public double? SalaryHiredAt { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "LastDrawnSalary_Display")]
        [RegularExpression(Common.ValidIntegersValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "LastDrawnSalary_ValidInteger")]
        public double? LastDrawnSalary { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "JoiningDate_Display")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Common.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "ResignDate_Display")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Common.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? ResignDate { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "RelievingDate_Display")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Common.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? RelievingDate { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "ResignReason_Display")]
        [StringLength(500)]
        public string ResignReason { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "LocalAddress_Display")]
        [StringLength(500)]
        public string LocalAddress { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "PermanentAddress_Display")]
        [StringLength(500)]
        public string PermanentAddress { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "ContactNo_Display")]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "Reference1_Display")]
        [StringLength(50)]
        public string Reference1 { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "Reference1No_Display")]
        [StringLength(50)]
        public string Reference1No { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "Reference2_Display")]
        [StringLength(50)]
        public string Reference2 { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "Reference2No_Display")]
        [StringLength(50)]
        public string Reference2No { get; set; }

        [Display(ResourceType = (typeof(EmployeeValidations)), Name = "ProfileImage_Display")]
        [StringLength(500)]
        public string ProfileImage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ValidateImageAttribute]
        public HttpPostedFileBase file { get; set; }

        public ICollection<EmployeeDocument> EmployeeDocuments { get; set; }
    }
}
