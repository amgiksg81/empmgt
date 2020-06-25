using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class Employee
    {
        public Employee()
        {
            UpdatedDate = DateTime.Now; //default value
        }

        [Key]
        public int EmployeeId { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string EmpFullName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string EmpFatherName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string EmpMotherName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string EmpEmailID { get; set; }        

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string EmpPersonalEmailID { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string OfficialSkypeID { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string PersonalSkypeID { get; set; }

        public DateTime? EmpDOB { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(15)]
        public string PANNumber { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string AadharNumber { get; set; }

        public double? TotalPrevExperience { get; set; }

        public double? PrevCompanySalary { get; set; }

        public double? SalaryHiredAt { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? ResignDate { get; set; }

        public DateTime? RelievingDate { get; set; }

        public double? LastDrawnSalary { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string ResignReason { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string LocalAddress { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string PermanentAddress { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Reference1 { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Reference1No { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Reference2 { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Reference2No { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500)]
        public string ProfileImage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; }
    }
}
