using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APSIM.Shared.Soils;
using APSIM.Soils.Service.Utilities;


namespace APSIM.Soils.Service.Controllers
{
    public class ApsoilDBController : ApiController
    {
        // GET: api/ApsoilDB
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApsoilDB/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApsoilDB
        public void Post([FromBody]string value)
        {
            List<string> eachSoilsXML = DatabaseManager.GetEachSoilsXMLfromDB();
            List<Soil> soilObjects = new List<Soil>();
            Soil soil;

            foreach (string xml in eachSoilsXML)
            {
                soil = SoilUtilities.FromXML(xml);
                if (SoilInAustralia(soil))
                {
                    soilObjects.Add(soil);
                }
            }
            AllSoilsInDataTables soilsInDataTables = new AllSoilsInDataTables();
            soilsInDataTables.AddAllSoilsIntoDataTables(soilObjects);
            DatabaseManager.CleanOutDBTables();
            DatabaseManager.InsertDataTablesIntoDB(soilsInDataTables);
        }

        private bool SoilInAustralia(Soil soil)
        {
            if (soil.Country == "Australia")
            {
                switch (soil.State)
                {
                    case "Queensland":
                        return true;
                    case "New South Wales":
                        return true;
                    case "Australian Capital Territory":
                        return true;
                    case "Victoria":
                        return true;
                    case "Tasmania":
                        return true;
                    case "South Australia":
                        return true;
                    case "Western Australia":
                        return true;
                    default:
                        return false;  //Don't want Generic Australian soils or Northern Territory soils
                }
            }
            else
            {
                return false;
            }
        }

        // PUT: api/ApsoilDB/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApsoilDB/5
        public void Delete(int id)
        {
            DatabaseManager.CleanOutDBTables();
        }

 
    }


}
