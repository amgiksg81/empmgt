using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using DomainModels.Resources;

namespace DomainModels.Models
{
    public class IsEmailExists : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string str = (string)value;
            if (str != null && str.Contains(Common.ValidDomain))
                return ValidationResult.Success;
            else
                return new ValidationResult(EmployeeValidations.DomainEmailCheck_Invalid);
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateImageAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 20; //20 MB
            string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return true;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
            {
                ErrorMessage = EmployeeValidations.UploadPhotoType_Invalid + " " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = EmployeeValidations.UploadPhotoLength_Invalid + " " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateDocumentAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 20; //20 MB
            string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".doc", ".docx", ".odt" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
            {
                ErrorMessage = EmployeeValidations.UploadPhotoType_Invalid + " " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = EmployeeValidations.UploadPhotoLength_Invalid + " " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }


}