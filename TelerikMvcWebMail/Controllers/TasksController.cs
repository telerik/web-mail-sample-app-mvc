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
        private TasksService tasksService;

        public TasksController()
        {
            tasksService = new TasksService(new WebMailEntities());
        }

        public ActionResult Index()
        {
            return View("Tasks");
        }

        public ActionResult NewTask()
        {
            return PartialView("NewTask");
        }

        public ActionResult Tasks_Read([DataSourceRequest] DataSourceRequest request, string search)
        {
            return Json(tasksService.Read().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }      

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tasks_Destroy([DataSourceRequest] DataSourceRequest request, TaskViewModel task)
        {
            if (task != null)
            {
                tasksService.Delete(task);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tasks_Create(TaskViewModel task)
        {
            task.CreatedOn = DateTime.Now;

            if (task != null && ModelState.IsValid)
            {
                tasksService.Insert(task);
            }

            return View("Tasks");
        }
    }
}