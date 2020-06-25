using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class Client
    {
        public Client()
        {
            UpdatedDate = DateTime.Now; //default value
        }

        [Key]
        public int ClientId { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientCountry { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string ClientAddress { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientSkypeID { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientEmailID { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string WhatsappNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string ClientCompanyName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientCompanyEmailID { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ClientCompanyWebsite { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string TaxNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string GSTNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string PANNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string PaypalAddress { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string TimeZone { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string FirstHiredUpworkID { get; set; }

        public DateTime? HiringDate { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string HiredBy { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string AddedOnSkype { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string AddedOnEmail { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string ReferenceByName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string ReferenceByContactNo { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string ReferenceByEmailSkype { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string ProfileImage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}