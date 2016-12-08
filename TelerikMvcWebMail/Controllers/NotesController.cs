using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TelerikMvcWebMail.Models;

namespace TelerikMvcWebMail.Controllers
{
    public class NotesController : Controller
    {
        // GET: /Notes/
        public ActionResult Notes()
        {
            return View();
        }

        public ActionResult NewNote()
        {
            return View();
        }

        public ActionResult Notes_Read([DataSourceRequest] DataSourceRequest request, string search)
        {
            var notesData = new List<NoteViewModel>();
            for (int i = 0; i < 20; i++)
            {
                if (i % 5 == 0)
                {
                    Thread.Sleep(1000);
                }

                var note = new NoteViewModel()
                {
                    CheckBoxCheked = false,
                    Subject = i + "Check this mail!",
                    CreatedOn = DateTime.Now,
                    Category = "Work"
                };

                notesData.Add(note);
            }

            if (search != null && search != String.Empty)
            {
                notesData = notesData.FindAll(n => n.Subject.Contains(search));
            }

            return Json(notesData.ToDataSourceResult(request));
        }
    }
}