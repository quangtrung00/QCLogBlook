<%@ Register TagPrefix="uc1" TagName="Header" Src="../../Pub/Module/Header.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserJoinAp104.aspx.cs" Inherits="SysManager_UserManage_UserJoinAp104" %>
<%@ Register src="../../control/PageControl.ascx" tagname="PageControl" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserJoinAp1</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="UserJoinAp1" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header><asp:label id="lblMsg" runat="server" Font-Size="Large" Font-Bold="True" ForeColor="Red"></asp:label>
			<asp:panel id="PanelGrid" runat="server" Width="100%">
				<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 29px" cellSpacing="1" cellPadding="1" bgColor="#ffcc99"
					border="0">
					<TR>
						<TD style="WIDTH: 20%">
							<asp:Button class="button" id="btnQuery" runat="server" Width="45px" Text="Query" Height="25px" onclick="btnQuery_Click"></asp:Button>
							<asp:Button class="button" id="btnCancel" runat="server" Width="45px" Text="Cancel" Height="25px" onclick="btnCancel_Click"></asp:Button></TD>
						<TD style="WIDTH: 30%">
							<asp:Label id="lblFact" runat="server">FactID:</asp:Label>
							<asp:DropDownList id="ddlfact_id" runat="server" Width="173px" Height="23px"></asp:DropDownList></TD>
						<TD style="WIDTH: 30%">
							<asp:DropDownList id="ddlQuerySelect" runat="server" Width="79px">
								<asp:ListItem Value="1" Selected="True">姓名查詢</asp:ListItem>
								<asp:ListItem Value="2">帳號查詢</asp:ListItem>
							</asp:DropDownList>
							<asp:TextBox id="txtUserDesc" runat="server" MaxLength="20"></asp:TextBox></TD>
						<TD>
							<asp:button class="button" id="btnAddComeOn" runat="server" Text="加入使用者" onclick="btnAddComeOn_Click"></asp:button></TD>
					</TR>
				</TABLE>
				<uc2:PageControl ID="PageControl1" runat="server"  OnPageIndexChanged="PageControl1_PageIndexChanged"
													EnableViewState="True" />
                <br>
                    <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" 
                        AlternatingItemStyle-BackColor="#ccccff" AutoGenerateColumns="False" 
                        BackColor="#CCFFCC" BorderColor="#FF6666" BorderWidth="2px" 
                        ToolTip="A List of Menu" Width="100%" AllowPaging="True">
                        <PagerStyle Visible="False" BorderStyle="None"></PagerStyle>
                        <SelectedItemStyle BackColor="#FFFFCC" />
                        <EditItemStyle BackColor="#FFFFCC" />
                        <AlternatingItemStyle BackColor="#CCCCFF" />
                        <HeaderStyle BackColor="#FFCCCC" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="編號">
                                <ItemTemplate>
                                    <%# GrideCount %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="user_id" HeaderText="使用者ID" ReadOnly="True" 
                                Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="email" HeaderText="電子郵件帳號"></asp:BoundColumn>
                            <asp:BoundColumn DataField="user_nm" HeaderText="帳號" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="user_desc" HeaderText="使用者名稱"></asp:BoundColumn>
                            <asp:BoundColumn DataField="fact_nm" HeaderText="廠別"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="選取">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkUser" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                <br>
                <br></br>
                <br></br>
                </br>
                </br>
            </asp:panel><br>
		</form>
	</body>
</HTML>
