using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSIsoft.AF.Time;

namespace TSOps.Models
{
    public class TagModel
    {
        public string tagname { get; set; }
        public string snapshot { get; set; }
        public string archivedvalue { get; set; }
        public string newtag{ get; set; }
        public string timestamp { get; set; }
    }
}