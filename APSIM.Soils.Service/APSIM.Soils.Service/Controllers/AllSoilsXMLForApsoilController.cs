using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APSIM.Soils.Service.Utilities;
using System.Data.SqlClient;
using System.Data;


namespace APSIM.Soils.Service.Controllers
{
    public class AllSoilsXMLForApsoilController : ApiController
    {




        // GET: api/AllSoilsXMLForApsoil
        //nb. Web API serialises the List of strings into JSON not xml by default.
        //    It is ok though because the strings themselves are still xml 
        //    it is only the list itself that is turned into JSON
        public List<string> Get()
        {
            return DatabaseManager.GetEachSoilsXMLfromDB();
        }



        // GET: api/AllSoilsXMLForApsoil/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AllSoilsXMLForApsoil
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AllSoilsXMLForApsoil/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AllSoilsXMLForApsoil/5
        public void Delete(int id)
        {
        }


    }
}
