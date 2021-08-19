using OSIsoft.AF.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOps.Models;
using TSOps.Services;

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

            if (tagn.snapshot == null || tagn.snapshot.Length == 0)
            {
               
                AFValue afval = pipoint.Snapshot(pipoint.findPiPoint(tagn.tagname));
                tag.tagname = tagn.tagname;
                tag.snapshot = afval.Value.ToString();
                ViewBag.Message3 = "Value: "+tag.snapshot + " ,timestamp: " + afval.Timestamp;
            }

            else
            {
                tag.tagname = tagn.tagname;
                tag.snapshot = tagn.snapshot;
                pipoint.SendSnapshotValue(pipoint.findPiPoint(tagn.tagname), tagn.snapshot); //this sends the snapshot value in the specified PI Point to the default PI server
                ViewBag.Message3 = "Entered value is now set as the new snapshot";
            }

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