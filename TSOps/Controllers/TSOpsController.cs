using OSIsoft.AF.Asset;
using OSIsoft.AF.PI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOps.Models;
using TSOps.Services;

namespace TSOps.Controllers
{
    public class tsopsController : Controller
    {
        // GET: tsops
        public ActionResult Index()
        {
            return View(new TagModel());
        }

        public ActionResult ReadDescription(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();

            PIPoint tagname = pipoint.findPiPoint(tagn.tagname);
            

            if (tagn.tagname != null)
            {
                tagname.LoadAttributes(PICommonPointAttributes.Descriptor);
                object drAttrValue;
                drAttrValue = tagname.GetAttribute(PICommonPointAttributes.Descriptor);
                ViewBag.Message0 = "Good";
                ViewBag.Message1 = drAttrValue;
            }

            else
            {
                ViewBag.Message0 = "Bad";
                ViewBag.Message1 = "Could not get the description";
            }

            return View("Index", tag);

        }

        public ActionResult RenamePiPoint(TagModel tagn)
        {
            return View();
        }
    }
}