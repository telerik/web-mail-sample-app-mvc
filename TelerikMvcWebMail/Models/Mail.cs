using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelerikMvcWebMail.Models
{
    public partial class Mail
    {
        [Key]
        public int MessageID { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public DateTime? Received { get; set; }
        public bool? IsRead { get; set; }
        public string To { get; set; }
        public string Category { get; set; }
    }
}
