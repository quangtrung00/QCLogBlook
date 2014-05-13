<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupAddNew104.aspx.cs" Inherits="SysManager_GroupManage_GroupAddNew104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>群組維護</title>
</head>
<body>
    <form id="GroupAddNew1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:label id="lblMsg" runat="server" ForeColor="Red"></asp:label>
			<TABLE id="Table1" cellSpacing="5" cellPadding="3" width="379" border="0" style="WIDTH: 379px; HEIGHT: 79px">
				<TR>
					<TD style="WIDTH: 83px">
						<asp:Label id="lblGroupName" runat="server">GroupName:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtGroupName" runat="server" Width="138px"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvGroupName" runat="server" ErrorMessage="*" ControlToValidate="txtGroupName"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px">
						<asp:Label id="lblGroupType" runat="server" Width="75px">GroupType:</asp:Label></TD>
					<TD><FONT face="新細明體">
							<asp:DropDownList id="ddlGroupType" runat="server" Width="126px">
								<asp:ListItem Value="0" Selected="True">---------</asp:ListItem>
								<asp:ListItem Value="1">一般使用者</asp:ListItem>
								<asp:ListItem Value="2">管理者</asp:ListItem>
								<asp:ListItem Value="3">超級管理者</asp:ListItem>
							</asp:DropDownList></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px"></TD>
					<TD>
						<asp:Button CssClass="button" id="btnOK" runat="server" Text="OK" onclick="btnOK_Click"></asp:Button><FONT face="新細明體">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>
						<asp:Button CssClass="button" id="btnCancel" runat="server" Text="Cancel" CausesValidation="False" onclick="btnCancel_Click"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
</body>
</html>
