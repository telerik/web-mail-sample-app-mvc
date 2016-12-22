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
    public class TasksController : Controller
    {
        private NotesService notesService;

        public TasksController()
        {
            notesService = new NotesService(new WebMailEntities1());
        }

        public ActionResult Tasks()
        {
            return View();
        }

        public ActionResult NewTask()
        {
            return View();
        }

        public ActionResult Tasks_Read([DataSourceRequest] DataSourceRequest request, string search)
        {
            return Json(notesService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tasks_Create([DataSourceRequest] DataSourceRequest request, NoteViewModel note)
        {
            var results = new List<NoteViewModel>();

            if (note != null && ModelState.IsValid)
            {
                notesService.Insert(note);
            }

            return Json(new[] { note }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tasks_Update([DataSourceRequest] DataSourceRequest request, NoteViewModel note)
        {
            if (note != null && ModelState.IsValid)
            {
                notesService.Update(note);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tasks_Destroy([DataSourceRequest] DataSourceRequest request, NoteViewModel note)
        {
            if (note != null)
            {
                notesService.Delete(note);
            }

            return Json(ModelState.ToDataSourceResult());
        }
    }
}