<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManage104.aspx.cs" Inherits="SysManager_UserManage_UserManage104" %>

<%@ Register Src="../../Pub/Module/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="PccServerControls" Namespace="Pcc.tw.ServiceControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>使用者維護</title>
    <link href="<%= ResolveClientUrl("~/")%>Pub/Css/ControlStyles.css" rel="stylesheet"
        type="text/css" />
    <script language="javascript">
        function Return_ClickM(objThis) {
            Form1.txtReturn.value = "<PccMsg><btnID>" + objThis.id + "</btnID><Method>UpdateMenuByUser</Method></PccMsg>";
            var a = "";
            a = objThis.id.toString();
            if (eval(a.split("-")[1]).rows.length > 1) {
                Form1.submit();
            }
            else {
                alert("請先加入群組！");
            }
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <uc1:Header ID="Header1" runat="server"></uc1:Header>
    <input id="txtReturn" type="hidden" name="txtReturn" runat="server">
    <asp:Panel ID="plMain" runat="server" Width="100%">
        <table class="cssDocTable" id="Table1" width="100%" border="1" cellpadding="2">
            <tr>
                <td valign="middle" align="center" width="10%">
                    <p style="vertical-align: baseline; text-align: center" align="center">
                        <asp:Button CssClass="cssDocButton" ID="btnQuery" runat="server" Text="Query" ForeColor="Black"
                            Font-Size="10pt" OnClick="btnQuery_Click"></asp:Button>&nbsp;
                    </p>
                </td>
                <td class="cssDocTD" valign="middle" align="center" width="10%" colspan="1" rowspan="1">
                    <asp:Label ID="lblFact" runat="server">廠別</asp:Label>
                </td>
                <td width="25%">
                    <asp:DropDownList ID="ddlFact" runat="server" Width="208px">
                    </asp:DropDownList>
                </td>
                <td class="cssDocTD" valign="middle" align="center" width="10%" bgcolor="#aaaadd"
                    colspan="1" rowspan="1">
                    <asp:Label ID="lblUserType" runat="server">UserType</asp:Label>
                </td>
                <td style="width: 110px;" width="110">
                    <p>
                        <font face="新細明體">
                            <asp:DropDownList ID="ddlUserType" runat="server" Width="103px">
                                <asp:ListItem Value="All" Selected="True">全部搜尋</asp:ListItem>
                                <asp:ListItem Value="N">一般使用者</asp:ListItem>
                                <asp:ListItem Value="Y">管理者</asp:ListItem>
                            </asp:DropDownList>
                        </font>
                    </p>
                </td>
                <td class="cssDocTD" style="width: 52px;" valign="middle" align="center" width="52"
                    bgcolor="#aaaadd" colspan="1" rowspan="1">
                    <asp:DropDownList ID="ddlQuerySelect" runat="server" Width="79px">
                        <asp:ListItem Value="1" Selected="True">姓名查詢</asp:ListItem>
                        <asp:ListItem Value="2">帳號查詢</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="20%">
                    <p>
                        <asp:TextBox ID="txtUserName" runat="server" Width="128px" MaxLength="20" Columns="50"></asp:TextBox></p>
                </td>
            </tr>
        </table>
        <br>
        <table id="Table2" style="width: 100%; height: 66px" cellspacing="1" cellpadding="2"
            width="100%" border="0">
            <tr>
                <td>
                    <font face="新細明體">
                        <cc1:PageControl ID="PageControl1" runat="server" OnPageClick="OnPageClick">
                        </cc1:PageControl>
                    </font>
                </td>
                <td align="right">
                    <asp:LinkButton ID="lbtnAddNewUser" runat="server" Visible="False" OnClick="lbtnAddNewUser_Click">Add New User</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lbtnJoinUser" runat="server" Visible="False" OnClick="lbtnJoinUser_Click">Join User</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="height: 26px" colspan="2">
                    <table id="Table6" cellspacing="1" cellpadding="2" width="100%" align="left" border="0">
                        <tr>
                            <td>
                                <asp:Table ID="tblApUser" runat="server">
                                </asp:Table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
        <asp:Panel ID="plDelete" Width="100%" runat="server" Visible="False">
            <asp:Label ID="lblDelMsg" runat="server" Width="100%">Label</asp:Label>
            <br>
            <asp:Button CssClass="button" ID="btnDelOK" runat="server" Text="DelOK" OnClick="btnDelOK_Click">
            </asp:Button>
            <asp:Button CssClass="button" ID="btnDelCancel" runat="server" Text="DelCancel" OnClick="btnDelCancel_Click">
            </asp:Button>
        </asp:Panel>
    </p>
    <p>
        <asp:Panel ID="plReturnGroup" Width="100%" runat="server" Visible="False">
            <asp:Label ID="lblReturnMsg" runat="server" Width="100%">Label</asp:Label>
            <br>
            <asp:Button CssClass="button" ID="btnReturnOK" runat="server" Text="ReturnOK" OnClick="btnReturnOK_Click">
            </asp:Button>
            <asp:Button CssClass="button" ID="btnReturnCancel" runat="server" Text="ReturnCancel"
                OnClick="btnReturnCancel_Click"></asp:Button>
        </asp:Panel>
    </p>
    <p>
        <font face="新細明體"></font>&nbsp;</p>
    </form>
</body>
</html>
