<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserFgrpView104.aspx.cs" Inherits="SysManager_FactGroupManage_UserFgrpView104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>使用者廠群組管理</title>
    <script language="javascript">
			function DataReset()
			{
				UserFgrpView104.txtusernm.value="";
				UserFgrpView104.txtfactno.value="";
			
			}
			function Return_ClickM(objThis)
			{
				UserFgrpView104.txtReturn.value = "<PccMsg><btnID>" + objThis.id + "</btnID><Method>UnPickFgrp</Method></PccMsg>";
				UserFgrpView104.submit(); 
			}
			function ChangeView_Click()
			{
				alert("aaa");
				window.location.href= "FactGroupManage104.aspx?ApID=<%=Request.QueryString["ApID"]%>";
				alert("bbb");
			}
	</script>
</head>
<body>
    <form id="UserFgrpView104" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<input id="txtReturn" type="hidden" name="txtReturn" runat="server"> <input id="txtuser" type="hidden" name="txtuser" runat="server">
			<asp:label id="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="Large"></asp:label>
			<asp:panel id="PanelGrid" runat="server" Width="100%">
				<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" width="100%" bgColor="#e7e7ff" border="0">
					<TR>
						<TD style="WIDTH: 20%">
							<asp:button id="btnQuery" runat="server" CssClass="button" Text="查詢" onclick="btnQuery_Click"></asp:button>
							<INPUT class="button" id="btnClear" onclick="DataReset()" type="button" value="清除" name="btnClear">
						</TD>
						<TD style="WIDTH: 30%">
							<asp:label id="lblusernm" runat="server">使用者姓名：</asp:label>
							<asp:textbox id="txtusernm" runat="server" MaxLength="20"></asp:textbox></TD>
						<TD style="WIDTH: 30%">
							<asp:label id="lblfactno" runat="server">廠別編號：</asp:label>
							<asp:textbox id="txtfactno" runat="server" MaxLength="20"></asp:textbox></TD>
					</TR>
				</TABLE>
				<TABLE width="90%">
					<TR>
						<TD><FONT face="新細明體">
								<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></FONT></TD>
						<TD align="right"><A id="ChangeView" href="FactGroupManage104.aspx?ApID=<%=Request.QueryString["ApID"]%>">更換檢視角度</A>&nbsp;&nbsp;
						</TD>
					<TR>
						<TD style="HEIGHT: 26px" colSpan="2">
							<asp:table id="tblUser" runat="server"></asp:table>
						</TD></TR>
				</TABLE>
				<BR>
			</asp:panel><asp:panel id="panelDel" Runat="server" width="100%" Visible="False">
				<asp:Label id="lblDelMsg" runat="server" Width="426px">Label</asp:Label>
				<BR>
				<asp:Button id="btnDelOK" runat="server" CssClass="button" Text="確定"></asp:Button>
				<asp:Button id="btnDelCancel" runat="server" CssClass="button" Text="取消"></asp:Button>
			</asp:panel><asp:panel id="panelDelUser" Runat="server" width="100%" Visible="False">
				<asp:Label id="lblDelMsg1" runat="server" Width="426px">Label</asp:Label>
				<BR>
				<asp:Button id="btnDelOK1" runat="server" CssClass="button" Text="確定" onclick="btnDelOK1_Click"></asp:Button>
				<INPUT class="button" onclick="window.history.go(-1);" type="button" value="取消">
			</asp:panel>
		</form>
</body>
</html>
