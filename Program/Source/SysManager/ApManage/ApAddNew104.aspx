<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApAddNew104.aspx.cs" Inherits="SysManager_ApManage_ApAddNew104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>應用程式維護</title>
</head>
<body>
    <form id="ApAddNew1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:label id="lblMsg" runat="server" ForeColor="Red" Font-Size="Larger"></asp:label>
			<TABLE id="Table1" cellSpacing="5" cellPadding="3" width="435" border="0" style="WIDTH: 435px; HEIGHT: 171px">
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblApNo" runat="server">ApNo:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtApNo" runat="server" Width="97px" MaxLength="20"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblApName" runat="server">ApName:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtApName" runat="server" Width="176px" MaxLength="25"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvApName" runat="server" ErrorMessage="*" ControlToValidate="txtApName"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblApLink" runat="server">ApLink:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtApLink" runat="server" Width="284px" MaxLength="100"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvApLink" runat="server" ErrorMessage="*" ControlToValidate="txtApLink"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblApVpath" runat="server">ApVpath:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtVpath" runat="server" Width="284px" MaxLength="100"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvApVpath" runat="server" ErrorMessage="*" ControlToValidate="txtVpath"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px"></TD>
					<TD>
						<asp:Button CssClass=button id="btnOK" runat="server" Text="OK" onclick="btnOK_Click"></asp:Button><FONT face="新細明體">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>
						<asp:Button CssClass=button id="btnCancel" runat="server" Text="Cancel" CausesValidation="False" onclick="btnCancel_Click"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
</body>
</html>
