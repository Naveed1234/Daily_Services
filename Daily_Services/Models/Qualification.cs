using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Daily_Services.Models
{
    public class Qualification
    {
        [Key]
        public int Qualification_Id { get; set; }
        public string Qualification_Type { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
