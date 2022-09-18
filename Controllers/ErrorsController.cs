using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nakazadde.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Error
        public ActionResult Error()
        {
            return RedirectToAction("PageNotFound");
        }
        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult UnderDevelopment()
        {
            return View();
        }
    }
}