using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        public bool CheckBoxCheked { get; set; }

        public string Subject { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        internal Note ToEntity()
        {
            return new Note
            {
                Category = Category,
                DateCreated = CreatedOn,
                Subject = Subject,
                NoteContent = Content,
                Id = Id
            };
        }
    }
}