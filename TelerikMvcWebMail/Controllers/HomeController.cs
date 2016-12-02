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
            var mailsData = new List<MailViewModel>();
            for (int i = 0; i < 20; i++)
            {
                if (i % 5 == 0)
                {
                    Thread.Sleep(1000);
                }

                var mail = new MailViewModel()
                {
                    CheckBoxCheked = false,
                    From = "Veselin Tsvetanov",
                    Subject = i + "Check this mail!",
                    Date = DateTime.Now,
                    Text = "This is a simple mail with a simple content"
                };

                mailsData.Add(mail);
            }

            return View(mailsData);
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
    }
}
