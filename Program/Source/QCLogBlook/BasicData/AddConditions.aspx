<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddConditions.aspx.cs" Inherits="QCLogBlook_BasicData_AddConditions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Pub/Css/BodyStyles.css" rel="stylesheet" type="text/css" />
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
    <script src="../../Pub/Js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= txtCount.ClientID%>").keydown(function (e) {
                if (e.shiftKey)
                    e.preventDefault();
                else {
                    var nKeyCode = e.keyCode;
                    //Ignore Backspace and Tab keys
                    if (nKeyCode == 8 || nKeyCode == 9)
                        return;
                    if (nKeyCode < 95) {
                        if (nKeyCode < 48 || nKeyCode > 57)
                            e.preventDefault();
                    }
                    else {
                        if (nKeyCode < 96 || nKeyCode > 105)
                            e.preventDefault();
                    }
                }
            });
        });
        function checkValid() {
            var flag = true;
            if (document.getElementById("<%= txtCount.ClientID %>").value == "") {
                alert("不良次數 not null!!");
                flag = false;
            }

            return flag;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="margin:30px;">
        <table style="width:400px" class="cssDocTable" cellpadding="2" cellspacing="0" border="1">
            <tr>
                <td align="right" style="width:130px" class="cssDocTD">
                    <asp:Label ID="lbType" runat="server" Text="Loại điều kiện:"></asp:Label></td>
                <td align="left"  >
                    <asp:Image ID="imgType" runat="server" /></td>
            </tr>
            <tr>
                <td align="right" class="cssDocTD">
                    <asp:Label ID="lbCondition" runat="server" Text="條件(Điều kiện):"></asp:Label>
                    </td>
                <td align="left"  ><asp:DropDownList ID="ddlConditions" runat="server"></asp:DropDownList></td>
            </tr>
            
             <tr>
                <td align="right" class="cssDocTD">
                    <asp:Label ID="lbCount" runat="server" Text="不良次數(Số lần):"></asp:Label></td>
                <td align="left" class="ActDocTD3">
                    <asp:TextBox ID="txtCount" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center"   colspan="2">
                    <asp:Button ID="btnOK" CssClass="cssDocButton" runat="server" Text="OK" OnClientClick="return checkValid();"
                        onclick="btnOK_Click" />
                    <asp:Button ID="btnCancel" CssClass="cssDocButton" runat="server" Text="Cancel" 
                        onclick="btnCancel_Click" />
                </td>

            </tr>
        </table>
        
    </div>
     <div>
        <asp:Label ID="lbAlert" runat="server"  visible ="false" ForeColor="Blue" ></asp:Label>
    </div>
    </form>
</body>
</html>
