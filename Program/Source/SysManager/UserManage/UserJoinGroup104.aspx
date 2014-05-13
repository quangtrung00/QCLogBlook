<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserJoinGroup104.aspx.cs" Inherits="SysManager_UserManage_UserJoinGroup104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>使用者加入群組</title>
</head>
<body>
    <form id="UserJoinGroup1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:label id="lblMsg" runat="server" Font-Size="Large" Font-Bold="True" ForeColor="Blue"></asp:label>
			<br>
			<asp:table id="tblGroup" runat="server" Width="720px"></asp:table><br>
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="730" border="0">
				<TR>
					<TD align=center><asp:button CssClass=button id="btnOK" runat="server" Text="OK" onclick="btnOK_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button CssClass=button id="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"></asp:button></TD>
				</TR>
			</TABLE>
	</form>
</body>
</html>
