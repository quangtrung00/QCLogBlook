<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DialogContentPage.aspx.cs" Inherits="QCLogBlook_Report_DialogContentPage" EnableViewState="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainContentDiv">
      
          
        <br /><br />
        <div>
            <asp:Label ID="ResultLabel" runat="server" Height="60px" Width="400px" Style="display: none"></asp:Label>
        </div>

        <table class="style1">
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label1" runat="server" Text="Xưởng"></asp:Label>
                    </td>
                    <td   style="width:14%">
                        <asp:Label ID="lblFactNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label2" runat="server" Text="Bộ Phận"></asp:Label>
                    </td>
                   <td>
                        <asp:DropDownList ID="ddlDeptNo"   runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label3" runat="server" Text="Tổ"></asp:Label>
                    </td>
                    <td >                        
                        <asp:DropDownList ID="ddlSec"  onchange="OnchangeDeptVlue(this)" runat="server"  >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label4" runat="server" Text="QC Cố định"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlQC" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label5" runat="server" Text="Ngày Tháng"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                  
                        </td>
                </tr>
            </table>
    </div>
    </form>

    <input id="hDept_No" type="hidden" name="hDept_No" runat="server" />

     <script type='text/javascript' language="javascript">

         function OnchangeDept(obj) {
             alert('s');
             var dept_No = obj.value;
            
             QCLogBlook_Report_OpenWin.Getsec_noByFactAjax(dept_No, FactChange_CallBack);
         }

         function FactChange_CallBack(res) {
             
             if (res != undefined) {
                 var ds = res.value;
                 var ddlSec = document.getElementById("ddlSec");
                 if (ds != null && typeof (ds) == "object" && ds != null) {
                     ddlSec.options.length = 0;
                     ddlSec.options[0] = new Option("--All--", "");
                     var len = ds.Rows.length;
                     var Sec_No;
                     // var dept_Nm

                     for (var i = 0; i < len; i++) {
                         document.getElementById("hDept_No").value = ""; //ds.Rows[0].dept_no;
                         Sec_No = ds.Rows[i].sec_no;
                         // dept_Nm = ds.Rows[i].dept_nm;

                         ddlSec.options[i + 1] = new Option(Sec_No);
                     }
                 }

             }
         }
         function OnchangeDeptVlue(obj) {
             document.getElementById("hDept_No").value = obj.value;
         }
  
    </script>
</body>
</html>
