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
    public class CalendarController : Controller
    {
        private EventsService eventsService;

        public CalendarController()
        {
            eventsService = new EventsService(new WebMailEntities());
        }

        public ActionResult Index()
        {
            return View("Calendar");
        }

        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(eventsService.GetAll().ToDataSourceResult(request));
        }

        public virtual JsonResult Destroy([DataSourceRequest] DataSourceRequest request, EventViewModel appointment)
        {
            if (ModelState.IsValid)
            {
                eventsService.Delete(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request, EventViewModel appointment)
        {
            if (ModelState.IsValid)
            {
                eventsService.Insert(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request, EventViewModel appointment)
        {
            if (ModelState.IsValid)
            {
                eventsService.Update(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            eventsService.Dispose();
            base.Dispose(disposing);
        }
    }
}