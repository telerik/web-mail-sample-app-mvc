using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class ContactViewModel
    {
        public string EmployeeID
        {
            get;
            set;
        }

        [Required]
        public string Name
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Company
        {
            get;
            set;
        }

        public string HomePhone
        {
            get;
            set;
        }

        public string Folder { get; set; }

        public Contact ToEntity()
        {
            return new Contact
            {
                City = City,
                Company = Company,
                Country = Country,
                Email = Email,
                Id = EmployeeID,
                Name = Name,
                Phone = HomePhone,
                Folder = Folder,
                Title = Title
            };
        }
    }
}