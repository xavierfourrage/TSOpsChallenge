using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TSOpsChallenge.Models;


namespace TSOpsChallenge.Controllers
{
    public class TSOpsController : Controller
    {
        // GET: TSOps
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostAction([FromBody] PIPointModel pipoint)
        {

            return new EmptyResult();
        }
    }
}