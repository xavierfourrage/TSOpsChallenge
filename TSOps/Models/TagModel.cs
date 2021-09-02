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
        public string newtagname{ get; set; }
        public string timestamp { get; set; }
        public string decription { get; set; }
        public string oldtagname { get; set; }
    }
}