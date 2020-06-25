using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DomainModels.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: <br/>Upper case (A-Z), Lower case (a-z), Number (0-9) and Special character (e.g., !@#$%^&*)")]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Enter Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Enter Confirm Password.")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: <br/>Upper case (A-Z), Lower case (a-z), Number (0-9) and Special character (e.g., !@#$%^&*)")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string EditPassword { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [DisplayName("Confirm Password")]
        [System.Web.Mvc.Compare("EditPassword", ErrorMessage = "Password and Confirm Password must match.")]
        public string EditConfirmPassword { get; set; }

        public string Name { get; set; } 

        public string[] Roles { get; set; }
    }
}
