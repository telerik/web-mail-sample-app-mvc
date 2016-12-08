using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class ContactViewModel
    {
        public int EmployeeID
        {
            get;
            set;
        }

        [Required]
        public string FirstName
        {
            get;
            set;
        }

        public string EmployeeName
        {
            get;
            set;
        }

        [Required]
        public string LastName
        {
            get;
            set;
        }

        public string Title
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

        public string Address
        {
            get;
            set;
        }

        public string HomePhone
        {
            get;
            set;
        }
    }
}