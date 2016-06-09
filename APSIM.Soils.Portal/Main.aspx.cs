using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APSIM.Soils.Portal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BTNCleanOutDBTables_Click(object sender, EventArgs e)
        {
            //https://msdn.microsoft.com/en-us/library/bb386386%28v=vs.100%29.aspx

            Label1.Text = "";
            ApsoilManageServiceReference.ApsoilManageClient client = new ApsoilManageServiceReference.ApsoilManageClient();
            Label1.Text = client.CleanOutDBTables(); 
        }

        protected void BTNRefreshDBTables_Click(object sender, EventArgs e)
        {
            Label2.Text = "";
            ApsoilManageServiceReference.ApsoilManageClient client = new ApsoilManageServiceReference.ApsoilManageClient();
            Label2.Text = client.RefreshDBTablesFromXML();
        }
    }
}