using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DomainModels.Models
{
    [NotMapped]
    public class EmployeeDocumentModel
    {
        public EmployeeDocumentModel()
        {
            CreatedDate = DateTime.Now; //default value
        }

        [Required(ErrorMessage = "Upload File.")]
        [ValidateDocumentAttribute]
        public HttpPostedFileBase file { get; set; }
                
        public int EmployeeDocId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Enter Document Name")]
        public string ImageName { get; set; }

        [StringLength(250)]
        public string ImagePath { get; set; }

        [StringLength(50)]
        public string FileType { get; set; }

        public DateTime CreatedDate { get; set; }

        public int EmployeeId { get; set; }

    }
}
