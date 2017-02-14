using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
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
            contactsService = new ContactsService(new WebMailEntities());
        }

        public ActionResult Index()
        {
            return View("Contacts");
        }

        public ActionResult NewContact()
        {
            return View();
        }

        public ActionResult Contacts_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(contactsService.Read().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Picture(string id)
        {
            if (id != null)
            {
                return base.File("../../Content/contacts/" + id + ".jpg", "image/jpeg");
            }
            else
            {
                return base.File("../../Content/contacts/" + "default-contact.png", "image/jpeg");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contacts_Create([DataSourceRequest] DataSourceRequest request, ContactViewModel contact)
        {
            var results = new List<ContactViewModel>();

            if (contact != null && ModelState.IsValid)
            {
                contactsService.Insert(contact);

                return Json(new[] { contact }.ToDataSourceResult(request, ModelState));
            }
            else
            {
                return Json(ModelState.ToDataSourceResult());
            }
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

        public ActionResult UploadPhoto(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // The files are not actually saved in this demo
                    // file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemovePhoto(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public JsonResult ContactsMails()
        {
            var result = contactsService.Read().Select(c => new
            {
                Value = c.Email,
                Text = c.Email
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}