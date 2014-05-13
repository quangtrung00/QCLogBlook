<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OpenWin.aspx.cs" Inherits="QCLogBlook_Report_OpenWin" EnableEventValidation="true" %>
<%@ Register src="~/Pub/Calendar/Calendar.ascx" tagname="Calendar" tagprefix="uc2" %>

<%@ Register src="~/Pub/Calendar/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">
        .ButtonStyle
        {
            border: thin solid #C6C3DE;
            background-color: #ECEBFA;
            margin-left: 5px;
            width: auto;
            height: 25px;
            padding-left: 5px;
            padding-right: 5px;
            font-size: 13px;
            font-weight: bold;
            padding-top: 3px;
            padding-bottom: 3px;
            text-align: center;
            color: #636363;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
        }
          </style>
</head>
<body>
    <form id="form1" runat="server">
    <div >
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
                        <asp:DropDownList ID="ddlDeptNo" 
                            runat="server" 
                            onselectedindexchanged="ddlDeptNo_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label3" runat="server" Text="Tổ"></asp:Label>
                    </td>
                    <td >                        
                        <asp:DropDownList ID="ddlSec"  
                              runat="server" 
                            onselectedindexchanged="ddlSec_SelectedIndexChanged" AutoPostBack="True"  >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label4" runat="server" Text="QC Cố định"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlQC"  runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td  align="right" style="width:14%">
                        <asp:Label ID="Label5" runat="server" Text="Ngày Tháng"></asp:Label>
                    </td>
                    <td>
                      <uc1:Calendar ID="CalendarSartDate" runat="server" FormatDate="yy/mm/dd" /> 
                            ~
                            <uc1:calendar ID="CalendarEndDate" runat="server" FormatDate="yy/mm/dd" />
                        
                        </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                  
                       
                            <asp:Button  id="btnSeacrh" runat="server" class="ButtonStyle"  
                            type="button" value="Search" Text="Search" onclick="btnSeacrh_Click" />
                            
                            </td>
                </tr>
            </table>
    </div>
    </form>
     <input id="hDept_No" type="hidden" name="hDept_No" runat="server" />
     <input id="hSec_no" type="hidden" name="hSec_no" runat="server" />
     <input id="hQC" type="hidden" name="hQC" runat="server" />

     <input id="hSecNo" type="hidden" name="hSecNo" runat="server" />
     <script type="text/javascript" language="javascript">

        


     
      
  
    </script>
</body>
</html>
