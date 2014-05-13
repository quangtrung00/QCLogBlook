<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadImgCondition.aspx.cs" Inherits="QCLogBlook_BasicData_uploadImgCondition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Pub/Css/BodyStyles.css" rel="stylesheet" type="text/css" />
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
   <div align="center" style="margin:30px;">
        <table style="width:400px" class="cssDocTable" cellpadding="2" cellspacing="0" border="1">
            <tr>
                <td align="right" style="width:130px" class="cssDocTD">
                    <asp:Label ID="lbType" runat="server" Text="Hình Ảnh:"></asp:Label>
                </td>
                <td align="left" >
                    <asp:Image ID="imgType" runat="server" /></td>
            </tr>
       
            
             <tr>
                <td align="right" class="cssDocTD">
                    <asp:Label ID="lbPath" runat="server" Text="Đường Dẩn:"></asp:Label>
                </td>
                <td align="left" >
                    <asp:FileUpload ID="fulImage" runat="server" />

                    </td>
            </tr>
            <tr>
                <td align="center"  class="ActDocTD3" colspan="2">
                    <asp:Button ID="btnOK" CssClass="cssDocButton" runat="server" Text="OK" 
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
