<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlineUser.aspx.cs" Inherits="OnlineUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OnlineUser</title>
</head>
<body>
		<form id="WinClose" method="post" runat="server">
			<asp:DataGrid id="DataGrid1" AutoGenerateColumns=False  runat="server">
				<Columns>
					<asp:BoundColumn HeaderText="Name" DataField="Key" />
					<asp:BoundColumn HeaderText="IP-Time" DataField="Value" />
				</Columns>
			</asp:DataGrid>
		</form>
	</body>
</html>
