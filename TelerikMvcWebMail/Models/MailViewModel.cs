using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Models
{
    public class MailViewModel
    {
        public int ID { get; set; }

        public bool? IsRead { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public DateTime? Date { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public string Folder { get; set; }

        public string To { get; set; }

        internal Message ToEntity()
        {
            return new Message
            {
                Body = Text,
                From = From,
                Subject = Subject,
                Received = Date,
                IsRead = IsRead,
                To = To,
                Folder = Folder,
                MessageID = ID
            };
        }
    }
}