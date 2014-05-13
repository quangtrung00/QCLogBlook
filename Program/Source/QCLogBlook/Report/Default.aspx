<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="QCLogBlook_Report_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    </form>
    <table class="style1">
        <tr>
            <td rowspan="2" class="style2" style="width: 5%; text-align: left" >
                日期<br />
                Date</td>
            <td rowspan="2" class="col_tieude" style="width: 5%;">
                檢驗雙數<br />
                Total</td>
            <td colspan="2" class="col_tieude" style="width: 5%;">
                檢驗結果<br />
                Result</td>
            <td rowspan="2">
                通過率<br />
                Reate</td>
            <td rowspan="2" style="text-align: left">
                問題點備注<br />
                Note (Note)</td>
            <td rowspan="2" class="col_tieude" style=" width: 5%; ">
                檢查者簽名<br />
                Sign (Sign)</td>
        </tr>
        <tr>
            <td>
                通過數量<br />
                Qty (Qty)</td>
            <td>
                不通過數量<br />
                QtyOKitem (QtyOKitem)</td>
        </tr>
    </table>
</body>
</html>
