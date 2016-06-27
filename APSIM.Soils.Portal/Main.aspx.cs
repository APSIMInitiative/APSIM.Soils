using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using APSIM.Shared.Utilities;


namespace APSIM.Soils.Portal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "";
            Label2.Text = "";
        }

        protected void BTNCleanOutDBTables_Click(object sender, EventArgs e)
        {
            Label1.Text = "";
            string url = "http://www.apsim.info/APSIM.Soils.Service/ApsoilManage.svc/CleanOutDBTables";
            Label1.Text = WebUtilities.CallRESTService<string>(url);
        }

        protected void BTNRefreshDBTables_Click(object sender, EventArgs e)
        {
            Label2.Text = "";
            string url = "http://www.apsim.info/APSIM.Soils.Service/ApsoilManage.svc//RefreshDBTablesFromXML";
            Label2.Text = WebUtilities.CallRESTService<string>(url);
        }
    }
}