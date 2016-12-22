using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Models
{
    public class ContactsService
    {
        //private static bool UpdateDatabase = false;
        private WebMailEntities1 entities;

        public ContactsService(WebMailEntities1 entities)
        {
            this.entities = entities;
        }

        public IList<ContactViewModel> GetAll()
        {
            IList<ContactViewModel> result = new List<ContactViewModel>();

            result = entities.Contacts.Select(e => new ContactViewModel
            {
                EmployeeID = e.Id,
                Name = e.Name,
                HomePhone = e.Phone,
                Country = e.Country,
                City = e.City,
                Company = e.Company,
                Folder = e.Folder,
                Title = e.Title
            }).ToList();

            return result;
        }

        public IEnumerable<ContactViewModel> Read()
        {
            return GetAll();
        }

        public void Insert(ContactViewModel contact)
        {
            if (string.IsNullOrEmpty(contact.Name))
            {
                contact.Name = "New contact";
            }

            var entity = contact.ToEntity();

            entities.Contacts.Add(entity);
            entities.SaveChanges();

            contact.EmployeeID = entity.Id;
        }

        public void Update(ContactViewModel contact)
        {
            if (string.IsNullOrEmpty(contact.Name))
            {
                contact.Name = "";
            }

            var entity = contact.ToEntity();
            entities.Contacts.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Delete(ContactViewModel contact)
        {
            var entity = contact.ToEntity();
            entities.Contacts.Attach(entity);
            entities.Contacts.Remove(entity);
            entities.SaveChanges();
        }

        public ContactViewModel One(Func<ContactViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}