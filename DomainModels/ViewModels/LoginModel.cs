using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModels.Resources;
using System.ComponentModel;

namespace DomainModels.Models
{
    public class LoginModel
    {
        [Display(ResourceType = typeof(EmployeeValidations), Name = "EmpEmailID_Display")]
        [StringLength(100)]
        [RegularExpression(Common.ValidEmailValidation, ErrorMessageResourceType = typeof(EmployeeValidations), ErrorMessageResourceName = "EmpEmailID_Invalid")]
        [Required(ErrorMessageResourceType = (typeof(EmployeeValidations)), ErrorMessageResourceName = "EmpEmailID_Required")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [DisplayName("Password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: <br/>Upper case (A-Z), Lower case (a-z), Number (0-9) and Special character (e.g., !@#$%^&*)")]
        [Required(ErrorMessage = "Enter Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
