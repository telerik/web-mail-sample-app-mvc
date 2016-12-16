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

		//
		// GET: /Calendar/
		public ActionResult Calendar()
		{
			return View();
		}

        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(eventsService.GetAll().ToDataSourceResult(request));
        }

        public virtual JsonResult Destroy([DataSourceRequest] DataSourceRequest request, EventViewModel task)
        {
            if (ModelState.IsValid)
            {
                eventsService.Delete(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request, EventViewModel task)
        {
            if (ModelState.IsValid)
            {
                eventsService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request, EventViewModel task)
        {
            if (ModelState.IsValid)
            {
                eventsService.Update(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            eventsService.Dispose();
            base.Dispose(disposing);
        }
    }
}