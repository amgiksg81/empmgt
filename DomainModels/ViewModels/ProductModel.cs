using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DomainModels.Models
{
    [NotMapped]
    public class ProductModel : Product
    {
        public HttpPostedFileBase file { get; set; }
    }
}
