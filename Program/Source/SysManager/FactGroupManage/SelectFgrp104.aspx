<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectFgrp104.aspx.cs" Inherits="SysManager_FactGroupManage_SelectFgrp104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>選擇廠群組</title>    
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header id="Header1" runat="server"></uc1:Header>
    <TABLE class="border" id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD vAlign="top" width="25%">
                        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" EnableClientScript="True" Enabled="False">
                        </asp:TreeView>
					</TD>
					<TD vAlign="top" align="left" width="75%"><FONT face="新細明體">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>
						<asp:Button id="BtnOK" runat="server" Text="確定" CssClass="button" onclick="BtnOK_Click"></asp:Button>
						<asp:Button id="BtnCancel" runat="server" Text="取消" CssClass="button" onclick="BtnCancel_Click"></asp:Button>
						<asp:Label id="lblMsg" runat="server" Visible="False" ForeColor="Red"></asp:Label>
						<br>
						<asp:Table id="TblDs_Fgrp" runat="server"></asp:Table>
					</TD>
				</TR>
			</TABLE>
    </form>
</body>
</html>
