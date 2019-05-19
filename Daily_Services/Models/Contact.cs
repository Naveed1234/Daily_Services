using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Daily_Services.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string Message { get; set; }

    }
}