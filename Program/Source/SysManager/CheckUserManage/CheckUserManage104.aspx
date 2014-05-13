<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckUserManage104.aspx.cs" Inherits="SysManager_CheckUserManage_CheckUserManage104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>審核使用者</title>
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
		function CheckChange(objthis)
		{
			if (objthis.checked && objthis.parentElement.parentElement.children(5).children(0).value == "0")
			{
				alert("請先選擇群組！");
				objthis.checked = false;
			}
		}
		
		function GroupChange(objthis)
		{
			if (objthis.value == "0")
			{
				objthis.parentElement.parentElement.children(6).children(0).checked = false;	
			}
		}
		
		
		function Hello()
		{
			var i = 0;
			var b = false;
			for (i = 1; i < tblCheckUser.children(0).children.length ; i++)
			{
				if (tblCheckUser.children(0).children(i).children(6).children(0).checked)
				{
					b = true;
					break;
				}
			}
			
			if (!b)
			{
				alert("請選擇核准的使用者！");
			}
			else
			{
				CheckUserManage104.txtReturn.value = "<PccMsg><Method>AddCheckUser</Method></PccMsg>"
				CheckUserManage104.submit(); 
			}
			
		}
		
		</script>
</head>
<body>
    <form id="CheckUserManage104" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header><input id="txtReturn" type="hidden" name="txtReturn" runat="server">
			<asp:label id="lblMsg" runat="server" ForeColor="Red" Font-Size="Larger"></asp:label>
			<asp:panel id="plMain" runat="server" Width="100%">
				<TABLE class="ActDocTB" id="Table1" style="BORDER-TOP-WIDTH: 1pt; BORDER-LEFT-WIDTH: 1pt; BORDER-LEFT-COLOR: royalblue; BORDER-BOTTOM-WIDTH: 1pt; BORDER-BOTTOM-COLOR: royalblue; WIDTH: 100%; BORDER-TOP-COLOR: royalblue; HEIGHT: 31px; BORDER-RIGHT-WIDTH: 1pt; BORDER-RIGHT-COLOR: royalblue" borderColor="#aaaadd" cellSpacing="1" cellPadding="0" width="100%" border="0">
					<TR>
						<TD class="ActDocTD3" style="HEIGHT: 29px" vAlign=middle align=center width="10%" height="29">
							<P style="VERTICAL-ALIGN: baseline; TEXT-ALIGN: center" align="center">
								<asp:button CssClass="button" id="btnQuery" runat="server" Font-Size="10pt" ForeColor="Black" Text="Query" onclick="btnQuery_Click"></asp:button>&nbsp;
							</P>
						</TD>
						<TD class="cssDocTD" style="HEIGHT: 29px" vAlign=middle align=center width="15%" bgColor="#aaaadd" colSpan="1" height="29" rowSpan="1">
							<asp:Label id="lbluser_desc" runat="server">UserName</asp:Label></TD>
						<TD class="ActDocTD3" style="HEIGHT: 29px" width="30%" height="29">
							<P>
								<asp:textbox id="user_desc" runat="server" Width="200px" Columns="50" MaxLength="25"></asp:textbox></P>
						</TD>
						<TD class="ActDocTD3" style="HEIGHT: 29px" width="45%" height="29"><FONT face="新細明體"></FONT></TD>
					</TR>
				</TABLE>
				<BR>
				<TABLE id="Table2" style="WIDTH: 100%; HEIGHT: 66px" cellSpacing="1" cellPadding="2" border="0">
					<TR>
						<TD style="WIDTH: 80%"><FONT face="新細明體">
								<cc1:PageControl id="PageControl1" runat="server" onpageclick="OnPageClick"></cc1:PageControl></FONT></TD>
						<TD align="right">
							<INPUT class="button" id="btnAddUser" onclick="Hello()" type="button" value="Add User" name="btnAddUser" runat="server">
						</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 26px" colSpan="2">
							<TABLE id="Table6" style="WIDTH: 100%; HEIGHT: 31px" cellSpacing="1" cellPadding="2" align="left" border="0">
								<TR>
									<TD>
										<asp:table id="tblCheckUser" runat="server"></asp:table></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</asp:panel>
			<P></P>
			<P></P>
			<P><asp:panel id="plDelete" Width="60%" Visible="False" Runat="server">
					<asp:Label id="lblDelMsg" runat="server" Width="728px">Label</asp:Label>
					<BR>
					<asp:Button CssClass="button" id="btnDelOK" runat="server" Text="DelOK" onclick="btnDelOK_Click"></asp:Button>
					<asp:Button CssClass="button" id="btnDelCancel" runat="server" Text="DelCancel" onclick="btnDelCancel_Click"></asp:Button>
				</asp:panel></P>
		</form>

</body>
</html>
