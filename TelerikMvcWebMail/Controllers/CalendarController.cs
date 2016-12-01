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
		//
		// GET: /Calendar/
		public ActionResult Calendar()
		{
			var data = new List<TaskViewModel>();

			return View(data);
		}
	}
}