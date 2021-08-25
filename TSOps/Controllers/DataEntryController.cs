using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
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
            AFTime aftime = new AFTime("*");

            tag.snapshot = tagn.snapshot;
            if (tagn.tagname != null)
            {
                bool valuesent = pipoint.SendValue(pipoint.findPiPoint(tagn.tagname), tagn.snapshot,aftime); //this sends the snapshot value in the specified PI Point to the default PI server
                if (valuesent)
                {
                    ViewBag.Message2 = "Value was sent successfully";
                }
                else { ViewBag.Message2 = "Entered value does not match point type"; }
                
            }
            else { ViewBag.Message2 = "You need to select a tagname first"; }

            return View("Index", tag);
        }
        
        [HttpPost]
        public ActionResult SendArchivedData(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();

            tag.archivedvalue = tagn.archivedvalue;
            AFTime aftime = pipoint.ConvertToAFTime(tagn.timestamp);
            AFTime firstjan1970 = new AFTime();

            if(aftime== firstjan1970)
            {
                ViewBag.Message4 = "Invalid timestamp";
            }
           
           else if (tagn.tagname != null)
            {
                bool valuesent = pipoint.SendValue(pipoint.findPiPoint(tagn.tagname), tagn.archivedvalue, aftime); //this sends the snapshot value in the specified PI Point to the default PI server
                if (valuesent)
                {
                    ViewBag.Message4 = String.Format("value: {0}, timestamp: {1}, was sent successfully", tagn.archivedvalue, aftime.ToString());
                }
                else { ViewBag.Message4 = "Entered value does not match point type"; }
                
            }
            else { ViewBag.Message4 = "You need to select a tagname first"; }

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