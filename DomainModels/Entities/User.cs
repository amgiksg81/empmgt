using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities
{
    public class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Roles = new HashSet<Role>();
            UserRoles = new HashSet<UserRole>();
            Employees = new HashSet<Employee>();
            UpdatedDate = DateTime.Now; //default value
        }

        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Username { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

    }
}
