using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOps.Models;
using TSOps.Services;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

namespace TSOps.Controllers
{
    public class TagCreatorController : Controller
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
            tag.newtagname = tagn.newtagname;
            

            if (tagn.newtagname==null)
            {
                ViewBag.Message = "Tagname cannot be null";
            }
            else if (!pipoint.CheckingConnectionToPI())
            {
                ViewBag.Message = "Could not connect to your default PI DA";
            }
            else // if we are able to connect to Default PI DA
            {
                bool created = pipoint.CreatePIPoint(tagn.newtagname); // this creates the pipoint AND returns true, else false if the tag already exists.
                if (!created)
                {
                    ViewBag.Message = tagn.newtagname + " already exists, but you can still update and check its snapshot";
                }

                else
                {
                    ViewBag.Message = tagn.newtagname + " was created successfully";
                }
            }

            return View("Index", tag);
        }

        [HttpPost]
        public ActionResult SendData(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();
            AFTime aftime = new AFTime("*");

            if (tagn.newtagname!=null) // checking whether newtag is empty and whether the tagname already exists
            {
                tag.snapshot = tagn.snapshot;
                bool valuesent = pipoint.SendValue(pipoint.findPiPoint(tagn.newtagname), tagn.snapshot,aftime); //this sends the snapshot value in the specified PI Point to the default PI server
                if (valuesent)
                {
                    ViewBag.Message2 = "Value was sent successfully";
                }
                else { ViewBag.Message2 = "Entered value does not match point type"; }
                
            }
            else 
            { ViewBag.Message2 = "You need to create a new tag first";
                tag.snapshot = null;
                tag.newtagname = null;
            }          
            return View("Index", tag);
        }

        [HttpPost]
        public ActionResult GetTagInfo(TagModel tagn)
        {
            PIPointDataService pipoint = new PIPointDataService();
            TagModel tag = new TagModel();
            if (tagn.newtagname != null)
            {
                AFValue afval = pipoint.Snapshot(pipoint.findPiPoint(tagn.newtagname));
                tag.tagname = tagn.tagname;
                tag.snapshot = afval.Value.ToString();

                ViewBag.Message3 = "Value: "+tag.snapshot + " ,timestamp: " + afval.Timestamp;
            }

            else
            {
                ViewBag.Message3 = "You need to create a tag first";
            }
              
            return View("Index",tag);
        }
    }
}