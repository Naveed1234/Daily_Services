using System.ComponentModel.DataAnnotations;

namespace Daily_Services.Models
{
    public partial class ContactUs
    {
        [Key]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
