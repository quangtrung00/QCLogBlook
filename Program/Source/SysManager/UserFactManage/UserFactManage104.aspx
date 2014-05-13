<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserFactManage104.aspx.cs" Inherits="SysManager_UserFactManage_UserFactManage104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>使用者廠別權限管理</title>
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
			function DataReset()
			{
				UserFactManage104.txtfactno.value="";
				UserFactManage104.txtfactnm.value="";
			
			}
	</script>
</head>
<body>
    <form id="UserFactManage104" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<input id="txtReturn" type="hidden" name="txtReturn" runat="server"> <input id="txtfact" type="hidden" name="txtfact" runat="server">
			<asp:label id="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="Large"></asp:label>
			<asp:panel id="PanelGrid" runat="server" Width="100%">
				<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" width="100%" class="cssDocTable" bgColor="#e7e7ff" border="0">
					<TR>
						<TD style="WIDTH: 20%">
							<asp:button id="btnQuery" runat="server" CssClass="button" Text="查詢" onclick="btnQuery_Click"></asp:button>
							<INPUT class="button" id="btnClear" onclick="DataReset();" type="button" value="清除" name="btnClear" runat="server" onserverclick="btnClear_ServerClick"></TD>
						<TD style="WIDTH: 30%" >
							<asp:label id="lblfactno" runat="server">廠別編號：</asp:label>
							<asp:textbox id="txtfactno" runat="server" MaxLength="4"></asp:textbox></TD>
						<TD style="WIDTH: 30%">
							<asp:label id="lblfactnm" runat="server">廠別名稱：</asp:label>
							<asp:textbox id="txtfactnm" runat="server" MaxLength="20"></asp:textbox></TD>
					</TR>
				</TABLE>
				<TABLE width="90%">
					<TR>
						<TD><FONT face="新細明體">
								<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></FONT></TD>
						<TD align="right">
							<asp:linkbutton id="changeView" Runat="server" Visible="True" onclick="changeView_Click">更換檢視角度</asp:linkbutton>&nbsp;&nbsp;
							<asp:linkbutton id="pickfact" Runat="server" Visible="False" onclick="pickfact_Click">挑選廠別</asp:linkbutton></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 26px" colSpan="2">
							<asp:table id="tblFact" runat="server"></asp:table>
						</TD>
					</TR>
				</TABLE>
				<BR>
			</asp:panel><asp:panel id="panelDel" Runat="server" width="100%" Visible="False">
				<asp:Label id="lblDelMsg" runat="server" Width="426px">Label</asp:Label>
				<BR>
				<asp:Button id="btnDelOK" runat="server" CssClass="button" Text="確定" onclick="btnDelOK_Click"></asp:Button>
				<asp:Button id="btnDelCancel" runat="server" CssClass="button" Text="取消" onclick="btnDelCancel_Click"></asp:Button>
			</asp:panel><asp:panel id="panelDelUser" Runat="server" width="100%" Visible="False">
				<asp:Label id="lblDelMsg1" runat="server" Width="426px">Label</asp:Label>
				<BR>
				<asp:Button id="btnDelOK1" runat="server" CssClass="button" Text="確定" onclick="btnDelOK1_Click"></asp:Button>
				<asp:Button id="btnDelCancel1" runat="server" CssClass="button" Text="取消" onclick="btnDelCancel1_Click"></asp:Button>
			</asp:panel>
		</form>

</body>
</html>
