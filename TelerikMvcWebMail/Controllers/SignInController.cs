using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Controllers
{
    public class SignInController : Controller
    {
        //
        // GET: /SignIn/
        public ActionResult Index()
        {
            return View("SignIn");
        }
    }
}