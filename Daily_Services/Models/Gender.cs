using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Daily_Services.Models
{
    public class Gender
    {

        [Key]
        public int Gender_Id { get; set; }
        public string Gender_Type { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
