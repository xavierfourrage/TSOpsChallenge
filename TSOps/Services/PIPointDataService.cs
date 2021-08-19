using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSIsoft.AF.PI;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;

namespace TSOps.Services
{
    public class PIPointDataService
    {
        public PIPoint findPiPoint(string pipoint)
        {           
            PIServer piServer = PIServers.GetPIServers().DefaultPIServer;
           
            try
            {
                PIPoint myPIPoint = PIPoint.FindPIPoint(piServer, pipoint);
                return myPIPoint;
            }
            catch (Exception ex) // if PIPoint does not exist, return NULL
            {
                Console.WriteLine(ex.Message);
                return null;
            }            
        }

        public AFValue Snapshot(PIPoint pipoint)
        {
            if (pipoint != null)
            {
                AFValue afvalue = pipoint.CurrentValue();
                return afvalue;
            }

            else // if PIPoint is null, return NULL
            {
                return null;
            }
        }

        public string SnapshotTimeStamp (PIPoint pipoint)
        {
           if (pipoint!=null)
            {
                string timestamp = pipoint.CurrentValue().Timestamp.ToString();
                return timestamp;
            }
            
            else // if PIPoint is null, return NULL
            {            
                return null;
            }
        }

        public string SnapshotValue(PIPoint pipoint)
        {
            if (pipoint != null)
            {
                string snapshot = pipoint.CurrentValue().ToString();
                return snapshot;
            }

            else // if PIPoint is null, return NULL
            {
                return null;
            }
        }
        public void SendSnapshotValue(PIPoint pipoint, string snap)
        {
            AFValue myValue = new AFValue(snap);
            AFTime aftime = new AFTime("*");
            myValue.PIPoint = pipoint;
            myValue.Timestamp = aftime;
            myValue.PIPoint.UpdateValue(myValue,AFUpdateOption.Insert);
        }

        public bool CreatePIPoint(string newpipoint)
        {
            PIServer piServer = PIServers.GetPIServers().DefaultPIServer;

            if (findPiPoint(newpipoint)== null) // checking whether PIPoint already exists or not
            {
                piServer.CreatePIPoint(newpipoint);
                return true;
            }
            else { return false; }
        }
    }
}