using System.ComponentModel.DataAnnotations;

namespace TelerikMvcWebMail.Models
{
    public partial class Person
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
    }
}
