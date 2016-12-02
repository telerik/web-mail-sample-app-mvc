using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class MailViewModel
    {
        public bool CheckBoxCheked { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }
    }
}