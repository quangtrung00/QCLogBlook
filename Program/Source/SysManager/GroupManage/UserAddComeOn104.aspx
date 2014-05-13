<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAddComeOn104.aspx.cs" Inherits="SysManager_GroupManage_UserAddComeOn104" %>
<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>加入使用者到群組中</title>
</head>
<body>
    <form id="UserAddComeOn1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header><asp:label id="lblMsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:label>
			<asp:panel id="PanelGrid" runat="server" Width="100%">
				<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffcc99" border="0">
					<TR>
						<TD style="WIDTH: 20%">
							<asp:Button CssClass="button" id="btnQuery" runat="server" Width="45px" Height="25px" Text="Query" onclick="btnQuery_Click"></asp:Button>
							<asp:Button CssClass="button" id="btnCancel" runat="server" Width="45px" Height="25px" Text="Cancel" onclick="btnCancel_Click"></asp:Button>
							<asp:Button CssClass="button" id="Button1" runat="server" Width="85px" Height="25px" Text="TransExcel" Visible="False" onclick="Button1_Click"></asp:Button></TD>
						<TD style="WIDTH: 40%" align="left">
							<asp:Label id="lblFact" runat="server">FactID:</asp:Label>
							<asp:DropDownList id="ddlfact_id" runat="server" Width="173px"></asp:DropDownList></TD>
						<TD style="WIDTH: 40%" align="left">
							<asp:Label id="lblUserDesc" runat="server">UserDesc:</asp:Label>
							<asp:TextBox id="txtUserDesc" runat="server" MaxLength="20"></asp:TextBox></TD>
						<TD>
							<asp:button CssClass="button" id="btnAddComeOn" runat="server" Text="加入使用者" onclick="btnAddComeOn_Click"></asp:button></TD>
					</TR>
				</TABLE>
				<BR>
				<asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="#CCFFCC" BorderWidth="2px" BorderColor="#FF6666" AlternatingItemStyle-BackColor="#ccccff" AutoGenerateColumns="False" ToolTip="A List of Menu" AllowSorting="True">
					<SelectedItemStyle BackColor="#FFFFCC"></SelectedItemStyle>
					<EditItemStyle BackColor="#FFFFCC"></EditItemStyle>
					<AlternatingItemStyle BackColor="#CCCCFF"></AlternatingItemStyle>
					<HeaderStyle BackColor="#FFCCCC"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="編號">
							<ItemTemplate>
								<%# GrideCount %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn Visible="False" DataField="user_id" ReadOnly="True" HeaderText="使用者ID"></asp:BoundColumn>
						<asp:BoundColumn DataField="email" HeaderText="電子郵件帳號"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="user_nm" HeaderText="帳號"></asp:BoundColumn>
						<asp:BoundColumn DataField="user_desc" HeaderText="使用者名稱"></asp:BoundColumn>
						<asp:BoundColumn DataField="fact_nm" HeaderText="廠別"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="選取">
							<ItemTemplate>
								<asp:CheckBox id="chkUser" runat="server"></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid>
			</asp:panel><br>
		</form>
</body>
</html>
