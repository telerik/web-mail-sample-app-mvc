using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Models
{
    public class MailViewModel
    {
        public int MailID { get; set; }

        public bool? CheckBoxCheked { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public DateTime? Date { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public string Folder { get; set; }

        internal Message ToEntity()
        {
            return new Message
            {
                Body = Text,
                From = From,
                Subject = Subject,
                Received = Date,
                IsRead = CheckBoxCheked,
                //To = To;
                Folder = Folder,
                MessageID = MailID
            };
        }
    }
}