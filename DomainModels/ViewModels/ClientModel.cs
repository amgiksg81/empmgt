using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using DomainModels.Resources;

namespace DomainModels.Models
{
    [NotMapped]
    public class ClientModel
    {
        [ScaffoldColumn(true)]
        public int ClientId { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ClientName_Display")]
        [StringLength(100)]
        [Required(ErrorMessageResourceType = (typeof(ClientValidations)), ErrorMessageResourceName = "ClientName_Required")]
        public string ClientName { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ClientCountry_Display")]
        [StringLength(100)]
        public string ClientCountry { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ClientAddress_Display")]
        [StringLength(500)]
        public string ClientAddress { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ClientSkypeID_Display")]
        [StringLength(100)]
        public string ClientSkypeID { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ClientEmailID_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(ClientValidations), ErrorMessageResourceName = "ClientEmailID_Invalid")]
        [UIHint("_Email")]
        [DataType(DataType.EmailAddress)]
        public string ClientEmailID { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ContactNo_Display")]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "WhatsappNo_Display")]
        [StringLength(50)]
        public string WhatsappNo { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ClientCompanyName_Display")]
        [StringLength(200)]
        public string ClientCompanyName { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ClientCompanyEmailID_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(ClientValidations), ErrorMessageResourceName = "ClientEmailID_Invalid")]
        [UIHint("_Email")]
        [DataType(DataType.EmailAddress)]
        public string ClientCompanyEmailID { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ClientCompanyWebsite_Display")]
        [StringLength(100)]
        public string ClientCompanyWebsite { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "TaxNo_Display")]
        [StringLength(100)]
        public string TaxNo { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "GSTNo_Display")]
        [StringLength(100)]
        public string GSTNo { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "PANNo_Display")]
        [StringLength(100)]
        public string PANNo { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "PaypalAddress_Display")]
        [StringLength(100)]
        public string PaypalAddress { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "TimeZone_Display")]
        [StringLength(100)]
        public string TimeZone { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "FirstHiredUpworkID_Display")]
        [StringLength(100)]
        public string FirstHiredUpworkID { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name= "HiringDate_Display")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Common.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? HiringDate { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "HiredBy_Display")]
        [StringLength(100)]
        public string HiredBy { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "AddedOnSkype_Display")]
        [StringLength(100)]
        public string AddedOnSkype { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "AddedOnEmail_Display")]
        [StringLength(100)]
        public string AddedOnEmail { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "Description_Display")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ReferenceByName_Display")]
        [StringLength(50)]
        public string ReferenceByName { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ReferenceByContactNo_Display")]
        [StringLength(50)]
        public string ReferenceByContactNo { get; set; }

        [Display(ResourceType = typeof(ClientValidations), Name = "ReferenceByEmailSkype_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(ClientValidations), ErrorMessageResourceName = "ReferenceByEmailSkype")]
        [UIHint("_Email")]
        [DataType(DataType.EmailAddress)]
        public string ReferenceByEmailSkype { get; set; }

        [Display(ResourceType = (typeof(ClientValidations)), Name = "ProfileImage_Display")]
        [StringLength(500)]
        public string ProfileImage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ValidateImageAttribute]
        public HttpPostedFileBase file { get; set; }
    }
}
