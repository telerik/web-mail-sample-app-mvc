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
        
        [Required, StringLength(50)]
        public string Name
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Title
        {
            get;
            set;
        }

        [Required, StringLength(50)]
        public string Email
        {
            get;
            set;
        }

        [StringLength(25)]
        public string Country
        {
            get;
            set;
        }

        [StringLength(30)]
        public string City
        {
            get;
            set;
        }

        [StringLength(40)]
        public string Company
        {
            get;
            set;
        }

        [StringLength(30)]
        public string Phone
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Category { get; set; }

        public Person ToEntity()
        {
            return new Person
            {
                City = City,
                Company = Company,
                Country = Country,
                Email = Email,
                Id = EmployeeID,
                Name = Name,
                Phone = Phone,
                Category = Category,
                Title = Title
            };
        }
    }
}