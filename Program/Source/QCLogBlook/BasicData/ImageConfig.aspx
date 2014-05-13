<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageConfig.aspx.cs" Inherits="QCLogBlook_BasicData_ImageConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Pub/Css/BodyStyles.css" rel="stylesheet" type="text/css" />
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function confirmDelete(id) {
            var re = confirm("Do You Want Delete This Item?");
            if (re == true) {
                document.getElementById("hdfDelete").value = id;
                document.getElementById("form1").submit();
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdfDelete" runat="server" />

    <div align="center" style="margin:30px;">
        <asp:Table ID="mTable" runat="server">
        </asp:Table>
    </div>
    </form>
</body>
</html>
