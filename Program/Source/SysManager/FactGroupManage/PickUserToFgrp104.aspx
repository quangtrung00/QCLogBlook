﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PickUserToFgrp104.aspx.cs" Inherits="SysManager_FactGroupManage_PickUserToFgrp104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>加入使用者到廠群組</title>
    <script language="javascript">
			function DataReset()
			{
				PickUserToFgrp104.TxtUserName.value="";
				PickUserToFgrp104.TxtFactNo.value="";
			}
			function SelectedAll()
			{
				count=<%= Session["chkcount"]%>;
				for(i=1;i<count;i++)
				{
					eval('PickUserToFgrp104.user_id' + i ).checked=true;
				}
			}
			function CancelAll()
			{
				count=<%= Session["chkcount"]%>;
				for(i=1;i<count;i++)
				{
					eval('PickUserToFgrp104.user_id' + i ).checked=false;
				}
			}
			
			function ReturnUserFact()
			{
				window.location.href = "FactGroupDetail104.aspx?ApID=<%=Request.QueryString["ApID"]%>&SrcUp_Id=<%=Request.QueryString["SrcUp_Id"]%>&Up_Id=<%=Request.QueryString["Up_Id"]%>"; 
			}
	</script>
</head>
<body>
    <form id="PickUserToFgrp104" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<input id="txtReturn" type="hidden" name="txtReturn" runat="server">
			<asp:label id="lblMsg" runat="server" Font-Size="Large" ForeColor="Red" Font-Bold="True"></asp:label>
			<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" width="100%" bgColor="#e7e7ff" border="0">
				<TR>
					<TD style="WIDTH: 20%"><asp:button id="btnQuery" runat="server" Text="查詢" CssClass="button" onclick="btnQuery_Click"></asp:button><input class="button" id="btnClear" onclick="DataReset()" type="button" value="清除" name="btnClear">
					</TD>
					<TD style="WIDTH: 30%"><asp:label id="LblUserName" runat="server">中文姓名：</asp:label><asp:textbox id="TxtUserName" runat="server" MaxLength="20"></asp:textbox></TD>
					<TD style="WIDTH: 30%"><asp:label id="LblFactNo" runat="server">歸屬廠別：</asp:label><asp:textbox id="TxtFactNo" runat="server" MaxLength="20"></asp:textbox></TD>
				</TR>
			</TABLE>
			<table width="95%">
				<tr>
					<td style="WIDTH: 307px"><FONT face="新細明體">
							<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></FONT></td>
					<td align="right"><asp:button id="Button1" runat="server" Text="加入" CssClass="button" onclick="Button1_Click"></asp:button>&nbsp;<input class="button" id="goback" onclick="ReturnUserFact();" type="button" value="回管理頁" name="goback" runat="server">
						<input class="button" id="SelAll" onclick="SelectedAll()" type="button" value="全選" name="SelAll" runat="server">
						<input class="button" id="CanAll" onclick="CancelAll()" type="button" value="清除" name="CanAll" runat="server">
					</td>
				</tr>
			</table>
			<P><asp:table id="tab_user" runat="server"></asp:table></P>
		</form>
</body>
</html>
