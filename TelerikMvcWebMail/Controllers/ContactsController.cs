using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelerikMvcWebMail.Models;

namespace TelerikMvcWebMail.Controllers
{
    public class ContactsController : Controller
    {
		//
		// GET: /Contacts/
		public ActionResult Contacts()
		{
			return View();
		}

		public ActionResult NewContact()
		{
			return View();
		}

        public ActionResult Contacts_Read([DataSourceRequest] DataSourceRequest request)
        {
            var contacts = new List<ContactViewModel>();

            for (int i = 0; i < 20; i++)
            {
                var currentContact = new ContactViewModel()
                {
                    EmployeeID = i,
                    Address = "Some random adress, " + i,
                    City = "Sofia",
                    Country = "Bulgaria",
                    FirstName = "Pesho",
                    LastName = "Goshev",
                    HomePhone = "+356 676 616 71"
                };

                contacts.Add(currentContact);
            }

            return Json(contacts.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Create([DataSourceRequest] DataSourceRequest request, ContactViewModel product)
        {
            var results = new List<ContactViewModel>();

            //if (product != null && ModelState.IsValid)
            //{
            //    productService.Create(product);
            //    results.Add(product);
            //}

            //return Json(results.ToDataSourceResult(request, ModelState));

            return Json(results);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Update([DataSourceRequest] DataSourceRequest request, ContactViewModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                //productService.Update(product);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Destroy([DataSourceRequest] DataSourceRequest request, ContactViewModel product)
        {
            if (product != null)
            {
                //productService.Destroy(product);
            }

            return Json(ModelState.ToDataSourceResult());
        }
    }
}