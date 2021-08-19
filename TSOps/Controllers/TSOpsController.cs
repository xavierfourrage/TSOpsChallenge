using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOps.Models;
using TSOps.Services;
using OSIsoft.AF.Asset;

namespace TSOps.Controllers
{
    public class TSOpsController : Controller
    {
        // GET: TSOps
        public ActionResult Index()
        {

            return View(new TagModel());
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

        [HttpPost]
        public ActionResult SendData(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();

            tag.snapshot = tagn.snapshot;
            if (tagn.newtag!=null)
            {
                pipoint.SendSnapshotValue(pipoint.findPiPoint(tagn.newtag), tagn.snapshot); //this sends the snapshot value in the specified PI Point to the default PI server

                ViewBag.Message2 = "Value was sent successfully";
            }
            else { ViewBag.Message2 = "You need to create a tag first"; }
           
            return View("Index", tag);
        }

        [HttpPost]
        public ActionResult GetTagInfo(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();
            if (tagn.newtag != null)
            {
                AFValue afval = pipoint.Snapshot(pipoint.findPiPoint(tagn.newtag));
                tag.tagname = tagn.tagname;
                tag.snapshot = afval.Value.ToString();

                ViewBag.Message3 = tag.snapshot + " ,timestamp: " + afval.Timestamp;
            }

            else
            {
                ViewBag.Message3 = "You need to create a tag first";
            }
              
            return View("Index",tag);
        }
    }
}