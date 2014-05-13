<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCConfig.aspx.cs" Inherits="QCLogBlook_BasicData_QCConfig" %>
<%@ Register Namespace="Pcc" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Pub/Css/BodyStyles.css" rel="stylesheet" type="text/css" />
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />

    <script src="../../Pub/Js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ShowDetail(rowID, user_id) {

            if ($("#" + rowID).css("display") == "none") {
                var fact_no = document.getElementById("<%=ddlFact.ClientID %>").value;
                var dept_no = document.getElementById("<%=ddlDept.ClientID %>").value;

                $("#" + rowID).show();
                $("#" + rowID).children().html("Loading data, please wait ....<br /><img src='../../Images/loading.gif'>");
                $.ajax({
                    type: "POST",
                    url: "QCConfig.aspx",
                    data: 'EventName=GetQCFactDept&user_id=' + user_id + '&fact_no=' + fact_no + '&dept_no=' + dept_no,
                    dataType: "text",
                    beforeSend: function () {

                    },
                    error: function (err) { alert(err); },
                    success: function (msg) {
                        $("#" + rowID).children().html(msg);
                        //alert(msg);
                    }
                });
            }
            else {
                $("#" + rowID).hide();
            }
        }

        function DeleteQCFactDept(id) {
            var result = confirm("Do you want Delete it?");
            if (result == true) {
                
                var rowID = "R" + id.split("-")[0];
               

                $.ajax({
                    type: "POST",
                    url: "QCConfig.aspx",
                    data: 'EventName=DeleteQCFactDept&id=' + id,
                    dataType: "text",
                    beforeSend: function () {
                        //$("#" + rowID).children().html("Loading data, please wait ....<br><img src='../../Images/loading.gif'>");
                    },
                    error: function (err) { alert(err); },
                    success: function (msg) {
                        $("#" + rowID).children().html(msg);
                        //alert(msg);
                    }
                });

            }
        }

        function LoadDeptNo() {
            var ddlFact = document.getElementById("<%=ddlFact.ClientID %>");

            $.ajax({
                type: "POST",
                url: "QCConfig.aspx",
                data: 'EventName=GetDept&fact_no=' + ddlFact.value,
                dataType: "text",
                beforeSend: function () {

                },
                error: function (err) { alert(err); },
                success: function (msg) {
                    show_dept(msg);
                }
            });
        }

        function show_dept(msg) {
            var ddlDept = document.getElementById("<%=ddlDept.ClientID %>");
            $(ddlDept).html("");

            var allOpt = document.createElement("OPTION");
            allOpt.text = "--ALL--";
            allOpt.value = "";
            ddlDept.options.add(allOpt);


            if (msg != "") {
                var result = msg.split(";");

                for (var i = 0; i < result.length; i++) {
                    var item = result[i].split(":");

                    var value = item[0];
                    var text = item[1];

                    var newopt = document.createElement("OPTION");
                    newopt.text = text;
                    newopt.value = value;
                    ddlDept.options.add(newopt);
                }
            }

        }

        function ClearControl() {
            document.getElementById("<%=txtUserDesc.ClientID %>").value = "";
            document.getElementById("<%=txtEmail.ClientID %>").value = "";
            document.getElementById("<%=ddlDept.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ddlFact.ClientID %>").selectedIndex = 0;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:10px;">
            <table style="width:100%;" class="cssDocTable" cellpadding="2" border="1">
                <tr>
                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbFact_no" runat="server" Text="廠別(Xưởng):"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlFact" AutoPostBack="true"    runat="server" 
                            onselectedindexchanged="ddlFact_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbDept_no" runat="server" Text="部門(Bộ Phận):"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlDept" runat="server" >
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbUserDesc" runat="server" Text="姓名(Họ tên):"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox ID="txtUserDesc" runat="server"></asp:TextBox>
                    </td>
                     <td align="right" class="cssDocTD" style="width:70px;" >Email:</td>
                    <td >
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    </td>
                    
                    <td >
                        <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="cssDocButton"
                            onclick="btnSearch_Click" />
                        <input type="button" id="btnClear"  class="cssDocButton" runat="server" onclick="ClearControl();" value="Clear" />
                    </td>
                </tr>
            </table>
        </div>

        <div></div>
    
        <div style="margin-top:10px;">
            <div>
                <cc1:pagecontrol id="PageControl1" runat="server" onpageclick="PageControl1_PageClick"></cc1:pagecontrol>
            </div>
            <div style="margin-top:5px;" >
                <asp:Table ID="mTable" runat="server"></asp:Table>
            </div>
        </div>
        <script type="text/javascript">
            $("#ddlFact").change(function () {
                //LoadDeptNo();
                
            });
            $("#ddlDept").change(function () {
                 $("[id^=R]").hide();
            });
            
        </script>

    </form>
    
</body>
</html>
