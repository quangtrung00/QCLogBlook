<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FactGroupManage104.aspx.cs" Inherits="SysManager_FactGroupManage_FactGroupManage104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>廠別群組管理</title>    
</head>
<body>
    <form id="Form1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<TABLE class="border" id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD vAlign="top" width="25%">
                        <asp:TreeView ID="TreeView1" runat="server" EnableClientScript="true" EnableTheming="False" ShowLines="True"></asp:TreeView>
						
					</TD>
					<TD vAlign="top" align="left" width="75%"><IFRAME id="doc" name="doc" frameBorder="0" src="Space104.aspx" width="100%" scrolling="auto" height="450" runat="server">
						</IFRAME>
					</TD>
				</TR>
			</TABLE>
			
		</form>
</body>
</html>
