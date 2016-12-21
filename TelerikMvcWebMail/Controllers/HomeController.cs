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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewMail()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetMails().ToDataSourceResult(request));
        }

        private static IEnumerable<MailViewModel> GetMails()
        {
            var northwind = new WebMailEntities1();

            return northwind.Messages.Select(message => new MailViewModel
            {
                MailID = message.MessageID,
                CheckBoxCheked = message.IsRead,
                From = message.From,
                Subject = message.Subject,
                Date = message.Received,
                Text = message.Body
            });
        }
    }
}
