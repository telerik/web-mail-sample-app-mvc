using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        public bool CheckBoxCheked { get; set; }

        public string Subject { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Category { get; set; }
    }
}