<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 90%;">
<head runat="server">
    <title></title>
    <link href="Pub/Css/IndexStyles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery191.min.js"></script>
    <script>
        /* $('.divMenu').parents().each(function () {
        //alert($(this).html());
        $(this).css("height", "100%");
        });*/
    </script>
</head>
<body style="height: 90%; overflow:auto; background-image:url('<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/backgroundQC.jpg')">
    <form id="form1" runat="server" style="height: 100%;">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 100%;">
        <tr>
            <td style="vertical-align: middle; text-align:center">
                <asp:Literal ID="ltrLeftMenu" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
