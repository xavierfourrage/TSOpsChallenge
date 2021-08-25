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
        public bool SendValue(PIPoint pipoint, string snap, AFTime aftime)
        {
            AFValue myValue = new AFValue(snap);            
            myValue.PIPoint = pipoint;
            myValue.Timestamp = aftime;

            // checking if the input value fits the point type
            if (TryParseAll(pipoint.PointType, snap))
            {
                myValue.PIPoint.UpdateValue(myValue, AFUpdateOption.Insert);
                return true;
            }
            else { return false; } // return false if input does not match PIPoint type
            
        }

        public bool CreatePIPoint(string newpipoint)
        {
            PIServer piServer = PIServers.GetPIServers().DefaultPIServer;

            if (findPiPoint(newpipoint)== null) // checking whether PIPoint already exists or not.
            {
                piServer.CreatePIPoint(newpipoint);
                return true;
            }
            else { return false; }
        }
        public AFTime ConvertToAFTime(string datetime)
        {
            AFTime aftime = new AFTime();
            if (isAFTime(datetime))
            {
                bool isDate = AFTime.TryParse(datetime, out aftime);
                return aftime;
            }
            return aftime;
        }
        static bool isAFTime(string stringtime)
        {
            bool isDate = new bool();
            AFTime aftime = new AFTime();
            if (string.IsNullOrEmpty(stringtime))
            {
                return false;
            }
            isDate = AFTime.TryParse(stringtime, out aftime);
            return isDate;
        }
        public bool CheckingConnectionToPI() 
        {
            PIServer piServer = PIServers.GetPIServers().DefaultPIServer;
            
            try
            {
                piServer.Connect();
                piServer.Disconnect();
                return true;               
            }
            catch
            {
                return false;
            };
            
        }

        public static bool TryParseAll(PIPointType typeToConvert, object valueToConvert)
        {

            bool succeed = false;

            switch (typeToConvert.ToString().ToUpper())
            {
                case "DOUBLE":
                    double d;
                    succeed = double.TryParse(valueToConvert.ToString(), out d);
                    break;
                case "FLOAT32":
                    float f32;
                    succeed = float.TryParse(valueToConvert.ToString(), out f32);
                    break;
                case "FLOAT64":
                    float f64;
                    succeed = float.TryParse(valueToConvert.ToString(), out f64);
                    break;
                case "DATETIME":
                    DateTime dt;
                    succeed = DateTime.TryParse(valueToConvert.ToString(), out dt);
                    break;
                case "INT16":
                    Int16 i16;
                    succeed = Int16.TryParse(valueToConvert.ToString(), out i16);
                    break;
                case "INT":
                    Int32 i32;
                    succeed = Int32.TryParse(valueToConvert.ToString(), out i32);
                    break;
                case "INT32":
                    Int32 i322;
                    succeed = Int32.TryParse(valueToConvert.ToString(), out i322);
                    break;
                case "INT64":
                    Int64 i64;
                    succeed = Int64.TryParse(valueToConvert.ToString(), out i64);
                    break;
                case "BOOLEAN":
                    bool b;
                    succeed = Boolean.TryParse(valueToConvert.ToString(), out b);
                    break;
                case "BOOL":
                    bool b1;
                    succeed = bool.TryParse(valueToConvert.ToString(), out b1);
                    break;
            }

            return succeed;
        }
    }
}