using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace APSIM.Soils.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IApsoilManage
    {

        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);


        //See APSIM.Registrations for what I used as a template for the WebGet

        /// <summary>
        /// Cleans Out (Removes) all the data from the Apsoil Database Tables.
        /// (Usually in preparation for the Tables to be Refreshed from the XML)
        /// 
        /// RefreshDBTablesFromXML will do this anyway, so you don't need to do it manually.
        /// However, if you wish to be sure RefreshDBTablesFromXML is working correctly:
        /// You can manually Clean Out the data using this method. 
        /// Then check to see the database tables are empty.
        /// Then run RefreshDBTablesFromXML.
        /// You can then be sure all the new data in the tables is new.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/CleanOutDBTables", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string CleanOutDBTables();



        //See APSIM.Registrations for what I used as a template for the WebGet

        /// <summary>
        /// Refreshes all the Apsoil Database Tables using the XML stored in the AllSoils table.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/RefreshDBTablesFromXML", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RefreshDBTablesFromXML();


    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
