using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModels.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            CreatedDate = DateTime.Now;
        }

        [Key]
        public int CategoryId { get; set; }

        [StringLength(50)]
        [Column(TypeName ="varchar")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
