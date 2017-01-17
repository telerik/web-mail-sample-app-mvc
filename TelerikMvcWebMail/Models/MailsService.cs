using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class MailsService
    {
        private WebMailEntities entities;

        private static bool UpdateDatabase = false;

        public MailsService(WebMailEntities entities)
        {
            this.entities = entities;
        }

        public IList<MailViewModel> Read()
        {
            IList<MailViewModel> result = HttpContext.Current.Session["Mails"] as IList<MailViewModel>;

            if (result == null || UpdateDatabase)
            {
                result = entities.Messages.Select(message => new MailViewModel
                {
                    ID = message.MessageID,
                    IsRead = message.IsRead,
                    From = message.From,
                    To = message.To,
                    Subject = message.Subject,
                    Date = message.Received,
                    Text = message.Body,
                    Folder = message.Folder,
                    Email = message.Email
                }).ToList();

                if (!UpdateDatabase)
                {
                    HttpContext.Current.Session["Mails"] = result;
                }
            }

            return result;
        }

        public void Create(MailViewModel mail)
        {
            if (!UpdateDatabase)
            {
                var first = Read().OrderByDescending(e => e.ID).FirstOrDefault();
                var id = (first != null) ? first.ID : 0;

                mail.ID = id + 1;

                Read().Insert(0, mail);
            }
            else
            {
                var entity = mail.ToEntity();

                entities.Messages.Add(entity);
                entities.SaveChanges();

                mail.ID = entity.MessageID;
            }
        }

        public void Update(MailViewModel mail)
        {
            if (!UpdateDatabase)
            {
                var target = One(e => e.ID == mail.ID);

                if (target != null)
                {
                    target.Text = mail.Text;
                    target.From = mail.From;
                    target.Subject = mail.Subject;
                    target.Date = mail.Date;
                    target.IsRead = mail.IsRead;
                    target.To = mail.To;
                    target.Folder = mail.Folder;
                    target.ID = mail.ID;
                    target.Email = mail.Email;
                }
            }
            else
            {
                var entity = mail.ToEntity();
                entities.Messages.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public MailViewModel One(Func<MailViewModel, bool> predicate)
        {
            return Read().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}