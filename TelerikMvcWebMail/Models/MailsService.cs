using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class MailsService
    {
        private WebMailEntities1 entities;

        public MailsService(WebMailEntities1 entities)
        {
            this.entities = entities;
        }

        public IEnumerable<MailViewModel> Read()
        {
            return entities.Messages.Select(message => new MailViewModel
            {
                MailID = message.MessageID,
                CheckBoxCheked = message.IsRead,
                From = message.From,
                Subject = message.Subject,
                Date = message.Received,
                Text = message.Body,
                Folder = message.Folder
            });
        }

        public void Create(MailViewModel mail)
        {
            var entity = new Message();

            entity.Body = mail.Text;
            entity.From = mail.From;
            entity.Subject = mail.Subject;
            entity.Received = mail.Date;
            entity.IsRead = mail.CheckBoxCheked;
            //entity.To = mail.To;
            entity.Folder = mail.Folder;
            entity.MessageID = mail.MailID;

            entities.Messages.Add(entity);
            entities.SaveChanges();

            mail.MailID = entity.MessageID;
        }

        public void Update(MailViewModel mail)
        {
            var entity = mail.ToEntity();
            entities.Messages.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }
    }
}