using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daily_Services.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategory_Id { get; set; }
        public string SubCategory_Type { get; set; }
        public int? Category_Id { get; set; }

        [ForeignKey("Category_Id")]
        public virtual Category Category { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}
