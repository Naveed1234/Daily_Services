using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Daily_Services.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        public string Category_Type { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }

    }
}
