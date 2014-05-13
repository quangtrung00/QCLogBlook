<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupManage104.aspx.cs" Inherits="SysManager_GroupManage_GroupManage104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>群組管理</title>
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
		function Return_Click(objThis)
		{
			GroupManage1.txtReturn.value = "<PccMsg><btnID>" + objThis.id + "</btnID><Method>DeleteUserByGroup</Method></PccMsg>";
			GroupManage1.submit(); 
		}
		
		function Return_ClickM(objThis)
		{
			GroupManage1.txtReturn.value = "<PccMsg><btnID>" + objThis.id + "</btnID><Method>UpdateMenuByGroup</Method></PccMsg>";
			GroupManage1.submit(); 
		}
	</script>
</head>
<body>
    <form id="GroupManage1" method="post" runat="server">
				<uc1:Header id="Header1" runat="server"></uc1:Header>
				<input type="hidden" id="txtReturn" runat="server" NAME="txtReturn"><asp:panel id="plMain" runat="server" Width="100%">&nbsp; 
<TABLE class="ActDocTB" id="Table1" style="BORDER-TOP-WIDTH: 1pt; BORDER-LEFT-WIDTH: 1pt; BORDER-LEFT-COLOR: royalblue; BORDER-BOTTOM-WIDTH: 1pt; BORDER-BOTTOM-COLOR: royalblue; WIDTH: 100%; BORDER-TOP-COLOR: royalblue; HEIGHT: 31px; BORDER-RIGHT-WIDTH: 1pt; BORDER-RIGHT-COLOR: royalblue" borderColor="#aaaadd" cellSpacing="1" cellPadding="0" width="100%" border="0">
						<TR>
							<TD class="ActDocTD3" style="HEIGHT: 29px" valign=middle align=center width="25%" height="29">
								<P style="VERTICAL-ALIGN: baseline; TEXT-ALIGN: center" align="center">
									<asp:button CssClass="button" id="btnQuery" runat="server" Text="Query" ForeColor="Black" Font-Size="10pt" onclick="btnQuery_Click"></asp:button>&nbsp;
									<asp:button CssClass="button" id="btnClear" runat="server" Text="Clear" Font-Size="10pt" onclick="btnClear_Click"></asp:button></P>
							</TD>
							<TD class="cssDocTD" style="HEIGHT: 29px" valign=middle align=center width="25%" bgColor="#aaaadd" colSpan="1" height="29" rowSpan="1">
								<asp:Label id="lblGroupName" runat="server">GroupName</asp:Label></TD>
							<TD class="ActDocTD3" style="HEIGHT: 29px" width="50%" height="29">
								<P>
									<asp:textbox id="txtGroupName" runat="server" MaxLength="20" Columns="50"></asp:textbox></P>
							</TD>
						</TR>
					</TABLE><BR>
<TABLE id="Table2" cellSpacing="1" cellPadding="2" width="100%" border="0">
						<TR>
							<TD style="WIDTH: 60%">
								<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></TD>
							<TD align="right">
								<asp:LinkButton id="lbtnAddGroup" runat="server" Visible="False" onclick="lbtnAddGroup_Click">Add New Group</asp:LinkButton></TD>
						</TR>
						<TR>
							<TD style="HEIGHT: 26px" colSpan="2">
								<TABLE id="Table6" style="WIDTH: 100%; HEIGHT: 31px" cellSpacing="1" cellPadding="2" width="100%" align="left" border="0">
									<TR>
										<TD>
											<asp:table id="tblGroup" runat="server"></asp:table></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</asp:panel>
			<P><asp:panel id="plDelete" Width="100%" Visible="False" Runat="server">
					<asp:Label id="lblDelMsg" runat="server" Width="100%">Label</asp:Label>
					<BR>
					<asp:Button CssClass="button" id="btnDelOK" runat="server" Text="DelOK" onclick="btnDelOK_Click"></asp:Button>
					<asp:Button CssClass="button" id="btnDelCancel" runat="server" Text="DelCancel" onclick="btnDelCancel_Click"></asp:Button>
				</asp:panel></P>
			<P>&nbsp;</P>
		</form>
</body>
</html>
