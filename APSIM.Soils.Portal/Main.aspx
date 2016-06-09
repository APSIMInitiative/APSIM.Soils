<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="APSIM.Soils.Portal.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="BTNCleanOutDBTables" runat="server" Text="Clean Out Database Tables" OnClick="BTNCleanOutDBTables_Click" />
    </div>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <div>
    </div>
    <div>    
    <asp:Button ID="BTNRefreshDBTables" runat="server" Text="Refresh Database Tables from XML" OnClick="BTNRefreshDBTables_Click" />
    </div>
    <div>
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
