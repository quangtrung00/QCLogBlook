<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FactGroupDetail104.aspx.cs" Inherits="SysManager_FactGroupManage_FactGroupDetail104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>廠群組明細</title>
    <LINK href="../../Pub/css/PccStyles.css" rel="stylesheet">
    <script language="JavaScript" src="../../Pub/js/common.js"></script>
    <script language="javascript">
		
		function CheckData()
		{
			if(FactGroupDetail104.TxtFgrpNm.value =="")
			{
				alert("請輸入廠群組名稱");
				return;
			}
			FactGroupDetail104.TxtReturn.value = "AddNewFgrp";
			FactGroupDetail104.submit();
		}
		function DataReset()
		{
			FactGroupDetail104.TxtQueryFgrpNm.value = "";
		}
		
		function ChangeView_Click()
		{
			window.parent.location.href= "UserFgrpView104.aspx?ApID=<%=Request.QueryString["ApID"]%>";
		}
	</script>
</head>
<body>
    <form id="FactGroupDetail104" method="post" runat="server">
	<input id="TxtReturn" type="hidden" name="TxtReturn" runat="server">
				<br>
				<asp:panel id="PalFgrp" Runat="server">
					<TABLE class="ActDocTB" cellSpacing="1" cellPadding="2" width="100%">
						<TR>
							<TD class="ActDocTD3" style="HEIGHT: 30px" vAlign=middle align=center width="20%" height="29">
								<asp:Button id="BtnQuery" Runat="server" Text="查詢" CssClass="button" name="BtnQuery" onclick="BtnQuery_Click"></asp:Button>
								<INPUT class="button" id="BtnClear" style="FONT-SIZE: 10pt" onclick="DataReset();" type="button" value="清除" name="BtnClear" runat="server">
							</TD>
							<TD class="ActDocTD2" vAlign=middle align="left" width="20%" bgColor="#aaaadd">
								<asp:label id="LblFgrp_Nm" runat="server">廠群組名稱：</asp:label></TD>
							<TD class="ActDocTD3" width="60%">
								<asp:TextBox id="TxtQueryFgrpNm" runat="server"></asp:TextBox></TD>
						</TR>
					</TABLE>
					<TABLE width="100%">
						<TR>
							<TD colSpan="3">
								<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></TD>
							<TD align="right">
								<a href="Space104.aspx" id="ChangeView" onclick="ChangeView_Click()">更換檢視角度</a>
								<asp:LinkButton id="LbtnAddNewFgrp" Runat="server" Visible="False" onclick="LbtnAddNewFgrp_Click">新增廠群組</asp:LinkButton></TD>
						</TR>
						<TR>
							<TD colSpan="4"><FONT face="新細明體"></FONT><FONT face="新細明體">
									<asp:Table id="TblDs_Fgrp" runat="server"></asp:Table></FONT></TD>
						</TR>
					</TABLE>
				</asp:panel><asp:panel id="PalAddFgrp" Visible="False" Runat="server">
					<TABLE id="Table1" width="50%" border="0">
						<TR>
							<TD colSpan="3">
								<asp:label id="LblErrMsg" runat="server" ForeColor="blue" Font-Size="12" Font-Bold="true"></asp:label></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD align="right">
								<asp:label id="LblFgrpNm" Runat="server">廠群組名稱：</asp:label></TD>
							<TD>
								<asp:TextBox id="TxtFgrpNm" Runat="server" MaxLength="20"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD></TD>
							<TD></TD>
							<TD>
								<INPUT class="button" id="BtnAddNewOK" onclick="CheckData();" type="button" value="確定" name="BtnAddNewOK" runat="server" onserverclick="BtnAddNewOK_ServerClick">
								<asp:button id="BtnAddNewCancel" runat="server" Text="取消" CssClass="button" CausesValidation="False" onclick="BtnAddNewCancel_Click"></asp:button></TD>
						</TR>
					</TABLE>
				</asp:panel>
				<br>
				<asp:panel id="PalDelFgrp" Runat="server" Width="60%" Visible="False">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Label id="LblDelMsg" runat="server" Width="728px">Label</asp:Label><BR><BR>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Button id="BtnDelOK" runat="server" Text="確定" CssClass="button" CausesValidation="False" onclick="BtnDelOK_Click"></asp:Button>
<asp:Button id="BtnDelCancel" runat="server" Text="取消" CssClass="button" CausesValidation="False" onclick="BtnDelCancel_Click"></asp:Button>
				</asp:panel>
				<asp:panel id="PalDelFact" Runat="server" Width="60%" Visible="False">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Label id="LblDelFactMsg" runat="server" Width="728px">Label</asp:Label><BR><BR>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Button id="BtnDelFactOK" runat="server" Text="確定" CssClass="button" CausesValidation="False" onclick="BtnDelFactOK_Click"></asp:Button>
<asp:Button id="BtnDelFactCancel" runat="server" Text="取消" CssClass="button" CausesValidation="False" onclick="BtnDelFactCancel_Click"></asp:Button>
				</asp:panel>
				<asp:panel id="PalDelUser" Runat="server" Width="60%" Visible="False">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Label id="LblDelUserMsg" runat="server" Width="728px">Label</asp:Label><BR><BR>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:Button id="BtnDelUserOK" runat="server" Text="確定" CssClass="button" CausesValidation="False" onclick="BtnDelUserOK_Click"></asp:Button>
<asp:Button id="BtnDelUserCancel" runat="server" Text="取消" CssClass="button" CausesValidation="False" onclick="BtnDelUserCancel_Click"></asp:Button>
				</asp:panel>
        <asp:TreeView ID="TreeView1" runat="server" Visible="False">
        </asp:TreeView>
		</form>
</body>
</html>
