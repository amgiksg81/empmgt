using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class EmployeeDocument
    {
        public EmployeeDocument()
        {
            CreatedDate = DateTime.Now; //default value
        }
        [Key]
        public int EmployeeDocId { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string ImageName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string ImagePath { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string FileType { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
