<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PickFactToUser104.aspx.cs" Inherits="SysManager_UserFactManage_PickFactToUser104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>挑選廠別權限給使用者</title>
    <script language="javascript">
			function DataReset()
			{
				PickFactToUser104.txtfactno.value="";
				PickFactToUser104.txtfactnm.value="";
			}
			function SelectedAll()
			{
				count=<%= Session["chkcount"]%>;
				for(i=1;i<count;i++)
				{
					eval('PickFactToUser104.fact_id' + i ).checked=true;
				}
			}
			function CancelAll()
			{
				count=<%= Session["chkcount"]%>;
				for(i=1;i<count;i++)
				{
					eval('PickFactToUser104.fact_id' + i ).checked=false;
				}
			}
			
			function ReturnUserFact()
			{
				window.location.href = "UserFactManage104.aspx?ApID=<%=Request.QueryString["ApID"]%>"; 
			}
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<input id="txtReturn" type="hidden" name="txtReturn" runat="server"> <input id="txtuser" type="hidden" name="txtuser" runat="server">
			<asp:label id="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="Large"></asp:label>
			<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" width="100%" bgColor="#e7e7ff" border="0">
				<TR>
					<TD style="WIDTH: 20%">
						<asp:button id="btnQuery" runat="server" CssClass="button" Text="查詢" onclick="btnQuery_Click"></asp:button>
						<input type="button" class="button" value="清除" id="btnClear" onclick="DataReset()" name="btnClear">
					</TD>
					<TD style="WIDTH: 30%">
						<asp:label id="lblfactno" runat="server">廠別編號：</asp:label>
						<asp:textbox id="txtfactno" runat="server" MaxLength="20"></asp:textbox></TD>
					<TD style="WIDTH: 30%">
						<asp:label id="lblfactnm" runat="server">廠別名稱：</asp:label>
						<asp:textbox id="txtfactnm" runat="server" MaxLength="20"></asp:textbox></TD>
				</TR>
			</TABLE>
			<table width="95%">
				<tr>
					<td><FONT face="新細明體">
							<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></FONT></td>
					<td align="right"><asp:button id="Button1" CssClass="button" runat="server" Text="加入系統" onclick="Button1_Click"></asp:button><input id="goback" class="button" onclick="ReturnUserFact();" type="button" value="回使用者廠別管理" name="goback" runat="server">
						<input id="SelAll" class="button" onclick="SelectedAll()" type="button" value="全選" runat="server" NAME="SelAll">
						<input id="CanAll" class="button" onclick="CancelAll()" type="button" value="清除" runat="server" NAME="CanAll">
					</td>
				</tr>
			</table>
			<P>
				<asp:Table id="tab_fact" runat="server"></asp:Table></P>
		</form>
</body>
</html>
