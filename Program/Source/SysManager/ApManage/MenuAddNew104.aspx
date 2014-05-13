<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuAddNew104.aspx.cs" Inherits="SysManager_ApManage_MenuAddNew104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>選單維護</title>
</head>
<body>
    <form id="MenuAddNew1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:label id="lblMsg" runat="server" ForeColor="Red" Font-Size="Larger"></asp:label>
			<TABLE id="Table1" cellSpacing="5" cellPadding="3" width="435" border="0" style="WIDTH: 435px; HEIGHT: 261px">
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblMenuNo" runat="server">MenuNo:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtMenuNo" runat="server" Width="97px" MaxLength="20"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvMenuNo" runat="server" ErrorMessage="*" ControlToValidate="txtMenuNo"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblMenuName" runat="server">MenuName:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtMenuName" runat="server" Width="176px" MaxLength="25"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvMenuName" runat="server" ErrorMessage="*" ControlToValidate="txtMenuName"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblMenuLink" runat="server">MenuLink:</asp:Label></TD>
					<TD>
						<asp:TextBox id="txtMenuLink" runat="server" Width="284px" MaxLength="100"></asp:TextBox>
						<asp:RequiredFieldValidator id="rfvMenuLink" runat="server" ErrorMessage="*" ControlToValidate="txtMenuLink"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px">
						<asp:Label id="lblApName" runat="server" Width="99px">ApName:</asp:Label></TD>
					<TD><FONT face="新細明體">
							<asp:DropDownList id="ddlApName" runat="server" Width="126px"></asp:DropDownList></FONT>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px"><asp:label id="lblCheckMK" runat="server" Width="101">CheckMK：</asp:label></TD>
					<TD>
						<asp:CheckBox id="chkCheckMK" runat="server" Checked="True"></asp:CheckBox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 109px"><asp:label id="lblManageMK" runat="server" Width="101">ManageMK：</asp:label></TD>
					<TD>
						<asp:TextBox id="txtManageMK" runat="server" MaxLength="1" Width="22px"></asp:TextBox></TD>
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
