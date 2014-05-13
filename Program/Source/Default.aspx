<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html style="height: 100%">
<head style="height: 100%" id="Head1">
    <title>QCLogBlook</title>
    <link rel="stylesheet" type="text/css" href="Pub/Css/LoginStyle.css">
</head>
<body scroll="no">
    <form style="height: 100%" id="form1" runat="server">
    <table class="tblBody">
        <tr style="height: 100%">
            <td style="height: 100%">
                <table style="margin: 0px auto; vertical-align: middle; width: 759px" border="0"
                    cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td colspan="2" class="tblTop">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table class="tblLogin" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="text-align: left; vertical-align: top; width: 450px">
                                                </td>
                                                <td style="text-align: right; vertical-align: top;">
                                                    <table class="tblLoginC" border="0" cellspacing="0" cellpadding="2" width="100%"
                                                        id="tblLogin" runat="server" visible="false">
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <table border="0" cellspacing="0" cellpadding="0" width="80%">
                                                                    <tr>
                                                                        <td class="lblText">
                                                                            帳號：
                                                                        </td>
                                                                        <td class="tdLogin">
                                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="txtLogin"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style='height: 2px'>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="lblText">
                                                                            密碼：
                                                                        </td>
                                                                        <td class="tdLogin">
                                                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="txtLogin" TextMode="Password"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 50px">
                                                                        <td>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:ImageButton ID="cmdLogin" runat="server" ImageUrl="Images/LoginPage/login.jpg"
                                                                                OnClick="cmdLogin_Click1"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="cmdClear"
                                                                                    runat="server" ImageUrl="Images/LoginPage/reset.jpg" OnClick="cmdClear_Click1">
                                                                            </asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="cmdAddAccount" runat="server"
                                                                                ImageUrl="Images/LoginPage/regis.jpg" OnClick="cmdAddAccount_Click"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblOutput" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tblBottom" style="padding-left: 30px; padding-top: 5px; text-align: left;
                            vertical-align: top">
                            <asp:DataList ID="dtlContact" runat="server" Width="333px" RepeatDirection="Vertical"
                                RepeatColumns="2" OnItemDataBound="dtlContact_ItemDataBound1">
                                <ItemStyle Font-Size="9pt" Font-Names="Arial" HorizontalAlign="Left" CssClass="lblContact">
                                </ItemStyle>
                                <ItemTemplate>
                                    <li>
                                        <asp:Label ID="lblContact" runat="server" Text="" Style="color: #484846"></asp:Label></li>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; vertical-align:top">
                            <div style="text-align: center; font-size: 12px; color: #889399; vertical-align: bottom">
                                <br>
                                建議使用的瀏覽器為 <a href="http://chrome.google.com/" target="_blank">Chrome</a>， 解析度 1024
                                * 768 以上
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
