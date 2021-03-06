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
            
            if (tagn.tagname == null)
            {
                ViewBag.Message0 = "Bad";
                ViewBag.Message1 = "Tagname cannot be null";
            }

            else if (!pipoint.CheckingConnectionToPI())
            {
                ViewBag.Message0 = "Bad";
                ViewBag.Message1 = "Could not connect to your default PI DA";
            }

            else if (pipoint.findPiPoint(tagn.tagname) == null)
            {
                ViewBag.Message0 = "Bad";
                ViewBag.Message1 = tagn.tagname+" does not exist";
            }

            else 
            {
                PIPoint tagname = pipoint.findPiPoint(tagn.tagname);
                tagname.LoadAttributes(PICommonPointAttributes.Descriptor);
                object drAttrValue;
                drAttrValue = tagname.GetAttribute(PICommonPointAttributes.Descriptor);
                ViewBag.Message0 = "Good";
                ViewBag.Message1 = drAttrValue;
                tagname.UnloadAllAttributes(PICommonPointAttributes.Descriptor);
            }

            return View("Index", tagn);

        }

        public ActionResult RenamePiPoint(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();

            if (tagn.newtagname == null || tagn.oldtagname == null)
            {
                if(tagn.newtagname == null) 
                {   ViewBag.Message2 = "Bad";
                    ViewBag.Message3 = "New tagname cannot be null"; }
                else {
                    ViewBag.Message2 = "Bad";
                    ViewBag.Message3 = "Tagname cannot be null"; }
            }

            else if (tagn.newtagname== tagn.oldtagname)
            {
                ViewBag.Message2 = "Bad";
                ViewBag.Message3 = "Name and NewName are equal";
            }

            else if (!pipoint.CheckingConnectionToPI()) //checking connection to PI DA
            {
                ViewBag.Message2 = "Bad";
                ViewBag.Message2 = "Could not connect to your default PI DA";
            }

            else if (pipoint.findPiPoint(tagn.oldtagname)==null)
            {
                ViewBag.Message2 = "Bad";
                ViewBag.Message3 = tagn.oldtagname+" does not exist";
            }

            else if (pipoint.findPiPoint(tagn.newtagname) != null) 
            {
                ViewBag.Message2 = "Bad";
                ViewBag.Message3 = tagn.newtagname + " already exists";
            }

            else // renaming PI Point
            {
                PIPoint oldpipoint = pipoint.findPiPoint(tagn.oldtagname);
                oldpipoint.LoadAttributes(PICommonPointAttributes.Tag);
                oldpipoint.SetAttribute(PICommonPointAttributes.Tag, tagn.newtagname);
                oldpipoint.SaveAttributes(PICommonPointAttributes.Tag);
                oldpipoint.UnloadAttributes(PICommonPointAttributes.Tag);

/*                string test = pipoint.findPiPoint(tagn.newtagname).Name;*/

                ViewBag.Message2 = "Good";
                ViewBag.Message3 = tagn.newtagname;

            }

            return View("Index", tagn);
        }
    }
}