using OSIsoft.AF.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOps.Models;
using TSOps.Services;

// THIS CONTROLLER CAN BE IGNORED
// IT WAS CREATED FOR TESTING PURPOSES

namespace TSOps.Controllers
{
    public class DataEntryController : Controller
    {
        // GET: TSOps
        public ActionResult Index()
        {

            return View(new TagModel());
        }

        [HttpPost]
        public ActionResult GetTagInfo(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();

            if (tagn.tagname != null)
            {
               
                AFValue afval = pipoint.Snapshot(pipoint.findPiPoint(tagn.tagname));
                tag.tagname = tagn.tagname;
                tag.snapshot = afval.Value.ToString();
                ViewBag.Message3 = "Value: "+tag.snapshot + " ,timestamp: " + afval.Timestamp;
            }

            else
            {             
                ViewBag.Message3 = "Invalid tag name";
            }

            return View("Index", tag);
        }

        [HttpPost]
        public ActionResult SendData(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();

            tag.snapshot = tagn.snapshot;
            if (tagn.tagname != null)
            {
                pipoint.SendSnapshotValue(pipoint.findPiPoint(tagn.tagname), tagn.snapshot); //this sends the snapshot value in the specified PI Point to the default PI server

                ViewBag.Message2 = "Value was sent successfully";
            }
            else { ViewBag.Message2 = "You need to enter a tagname first"; }

            return View("Index", tag);
        }

        [HttpPost]
        public ActionResult CreateNewTag(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();
            tag.newtag = tagn.newtag;
            bool created = pipoint.CreatePIPoint(tagn.newtag);

            if (!created)
            {
                ViewBag.Message = tagn.newtag + " already exists";
            }

            else
            {
                ViewBag.Message = tagn.newtag + " was created successfully";
            }

            return View("Index", tag);
        }
    }
}