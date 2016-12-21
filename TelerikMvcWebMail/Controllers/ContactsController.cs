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
        private ContactsService contactsService;

        public ContactsController()
        {
            contactsService = new ContactsService(new WebMailEntities1());
        }

        public ActionResult Contacts()
		{
			return View(contactsService.Read());
		}

		public ActionResult NewContact()
		{
			return View();
		}

        public ActionResult Contacts_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(contactsService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Create([DataSourceRequest] DataSourceRequest request, ContactViewModel contact)
        {
            var results = new List<ContactViewModel>();

            if (contact != null && ModelState.IsValid)
            {
                contactsService.Insert(contact);
            }

            return Json(new[] { contact }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Update([DataSourceRequest] DataSourceRequest request, ContactViewModel contact)
        {
            if (contact != null && ModelState.IsValid)
            {
                contactsService.Update(contact);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Destroy([DataSourceRequest] DataSourceRequest request, ContactViewModel contact)
        {
            if (contact != null)
            {
                contactsService.Delete(contact);
            }

            return Json(ModelState.ToDataSourceResult());
        }
    }
}