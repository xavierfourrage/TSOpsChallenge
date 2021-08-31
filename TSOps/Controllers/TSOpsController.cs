using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSOps.Controllers
{
    public class tsopsController : Controller
    {
        // GET: tsops
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DescriptionReader()
        {
            return View();
        }

        public ActionResult PIPointRenamer()
        {
            return View();
        }
    }
}