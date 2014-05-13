<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCAddFactDept.aspx.cs" Inherits="QCLogBlook_BasicData_QCAddFactDept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Pub/Css/BodyStyles.css" rel="stylesheet" type="text/css" />
    <link href="../../Pub/Css/ControlStyles.css" rel="stylesheet" type="text/css" />
    <script src="../../Pub/Js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        function InsertType(id) {
            var type = CheckType(id);

            var sec_name = $("#" + id + "1").val();// value của checkbox1 là sec_name
            var dept_name = $("#" + id + "2").val();// value của checkbox2 là dept_name

            $.ajax({
                type: "POST",
                url: "QCAddFactDept.aspx",
                data: 'EventName=CHECKTYPE&id=' + id + '&type=' + type + '&sec_name=' + sec_name + '&dept_name=' + dept_name,
                dataType: "text",
                beforeSend: function () {

                },
                error: function (err) { alert(err); },
                success: function (msg) {
                    if (msg != "") {
                        alert("Erro: " + msg);
                    }
                }
            });
        }

        function CheckType(id) {
            var id1 = id + "1";
            var id2 = id + "2";

            var type = "D";
            var chk1 = $("#" + id1).is(':checked');
            var chk2 = $("#" + id2).is(':checked');

            if (chk1 == true && chk2 == true) {
                type = "0";
            }
            else {
                if (chk1 == true) {
                    type = "1";
                }

                if (chk2 == true) {
                    type = "2";
                }

            }

            //alert(type);
            return type;

        }
        function ClearControl() {
            document.getElementById("<%=ddlDept.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ddlBuild.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ddlFloor.ClientID %>").selectedIndex = 0;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%;font-size:11pt;font-weight:bold;" class="cssDocTable" cellpadding="5" cellspacing="0" border="1">
            <tr>
                <td class="cssDocTD" align="right" style="width:80px;" >
                    <asp:Label ID="lbDesc_title" runat="server" Text="User Desc" ></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbUserDesc" runat="server" ></asp:Label>
                </td>
          
                <td class="cssDocTD" align="right" style="width:100px;" >
                    <asp:Label ID="lbFact_title" runat="server" Text="Fact" ></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbFact_no" runat="server" ></asp:Label>
                </td>
     
                <td class="cssDocTD" align="right" style="width:100px;" >
                    Email:
                </td>
                <td>
                    <asp:Label ID="lbEmail" runat="server" ></asp:Label>
                </td>

                 <td class="cssDocTD" align="right" style="width:100px;" >
                    <asp:Label ID="lbEmp_no_title" runat="server" ></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbEmp_no" runat="server" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top:10px;" id="divSearch" runat="server">
            <table style="width:100%;" class="cssDocTable" cellpadding="2" cellspacing="0" border="1">
                <tr>
                   
                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbDept" runat="server" Text="部門(Bộ Phận):"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlDept" runat="server">
                        </asp:DropDownList>
                    </td>

                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbBuild" runat="server" Text="Toà:"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlBuild" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="cssDocTD" style="width:70px;" >
                        <asp:Label ID="lbFloor" runat="server" Text="Tầng Lầu:"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlFloor" runat="server">
                        </asp:DropDownList>
                    </td>

                    
                    <td style="width:150px;" >
                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="cssDocButton"
                            onclick="btnSearch_Click" />
                        <input type="button" runat="server" id="btnClear" class="cssDocButton"  onclick="ClearControl();" value="Clearn" />
                        
                    </td>
                    <td align="right" style="width:100px;" >
                        <a href="QCConfig.aspx">
                            <asp:Label ID="lbReturn" runat="server"  Text="Return"></asp:Label>
                            <img src="../../Images/Icon/go_previous.png" alt="" />
                        </a>
                    </td>
                </tr>
                
                   
                   

                    
                   
                
            </table>
        </div>

        <div style="margin-top:10px;">
            <asp:GridView ID="gvData" Width="100%" runat="server" 
                AutoGenerateColumns="False" EnableModelValidation="True" 
                HorizontalAlign="Center" onrowdatabound="gvData_RowDataBound" 
                AllowPaging="True" onpageindexchanging="gvData_PageIndexChanging" 
                GridLines="None"
                    CssClass="mGrid"
                    PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt"

                    EnableViewState="false"
                PageSize="15">
                <Columns>
                    <asp:BoundField DataField="fact_no" HeaderText="fact_no" />
                    <asp:BoundField DataField="dept_name" HeaderText="dept_name" />
                    <asp:BoundField DataField="sec_no" HeaderText="sec_no" />
                    <asp:BoundField DataField="sec_name" HeaderText="sec_name" />
                    <asp:BoundField DataField="build_no" HeaderText="build_no" />
                    <asp:BoundField DataField="floor" HeaderText="floor" />
                    <asp:BoundField DataField="remark" HeaderText="remark" />
                    
                    <asp:TemplateField HeaderText="Loại">
                        <ItemTemplate>
                            <asp:Label ID="lbType_Check" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="180px" />
                    </asp:TemplateField>
                    
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
        </div>
  


        <div align="center" >
            <asp:Label runat="server" ID="lbAlert" Font-Bold="true" Font-Size="Large" ForeColor="Blue" Visible="false"></asp:Label>
        </div>
     
    </form>

</body>
</html>
