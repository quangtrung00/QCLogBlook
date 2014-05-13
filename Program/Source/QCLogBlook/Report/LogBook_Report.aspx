<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogBook_Report.aspx.cs" Inherits="QCLogBlook_Report_LogBook_Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
     <link rel="Stylesheet" type="text/css" href="../../Pub/Css/Copy of LookBogs.css" />
    <link rel="Stylesheet" type="text/css" href="jquery-ui-1.10.4/development-bundle/themes/base/jquery.ui.all.css" />
    <link rel="Stylesheet" type="text/css" href="jquery-ui-1.10.4/development-bundle/themes/demos/demos.css" />
    <script type="text/javascript" src="jquery-ui-1.10.4/js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="jquery-ui-1.10.4/js/jquery-ui-1.10.4.js"></script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 12px;
            padding: 0 0 0 0;
        }
        label, input
        {
            display: block;
        }
        input.text
        {
            margin-bottom: 12px;
            width: 95%;
            padding: .4em;
        }
        fieldset
        {
            padding: 0;
            border: 0;
            margin-top: 25px;
        }
        h1
        {
            font-size: 1.2em;
            margin: .6em 0;
        }
        .ui-dialog .ui-state-error
        {
            padding: .3em;
        }
        .validateTips
        {
            border: 1px solid transparent;
            padding: 0.3em;
        }
        .style1
        {
            width: 100%;
        }
    </style>
    <title></title>
    <script language="javascript" type="text/javascript">
        function OpenDialog() {
            window.open('OpenWin.aspx?Type=New&', null, 'status=no,scrollbars=yes,toolbar=no,width=500px,Left=200px,Top=200px');
        }
    </script>
    
</head>
<body>
    <form id="LogBook_Repor" runat="server">
    <div class="cssQCTitle" style="width:100%;height:50px;float:left;font-weight:bold;text-align:center;line-height:2;font-size:21px;">
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        </div>
    <button id="search">
        查詢</button>
    <div style="width: 100%; float: left; position: relative">
       
        <asp:Literal ID="ltrMaster" runat="server"></asp:Literal>
        <asp:Panel ID="pnlMaster" runat="server" Width="100%" Height="100%">
        </asp:Panel>
    </div>
    <div>
        <div id="divMain">
            <table id="tblData" class="table" cellspacing="0">
            </table>
        </div>
    </div>
    <div id="dialog-form" title="log book">
        <p class="validateTips">
           </p>
        <fieldset>

            <table  >
                <tr>
                    <td>
                        <label for="Xuong">
                         廠別Xưởng:</label>
                        </td>
                    <td>
             <label for="Xuong" id="lblFact" runat="server">
            </label>
            </td>
                </tr>
                <tr>
                    <td>
                        <label for="BoPhan" id="lblBophan">
                          部門 Bộ Phận:
                         </label>
                        </td>
                    <td>
                        <asp:DropDownList ID="ddlDept" runat="server">
                        </asp:DropDownList>
                        
                        </td>
                </tr>
                <tr>
                    <td>
                         <label for="xuong">
                組別 Tổ :</label>
                        </td>
                    <td>
                       <asp:DropDownList ID="ddlsec" runat="server">
            </asp:DropDownList>
                        
                        </td>
                </tr>
                <tr>
                    <td>
                       <label for="QC">
               定點 QC：</label>                        
                        </td>
                    <td>
                       <asp:DropDownList ID="ddlQC" runat="server">
            </asp:DropDownList>
                        </td>
                </tr>
                <tr>
                    <td>
                        <label for="ddmm">
                日期 Ngày tháng:</label>
            <label for="from">
            </label>                       
                        </td>
                    <td>
                           <input type="text" id="from" name="from" class="text ui-widget-content ui-corner-all" />
            <label for="to">
                ~
            </label><input type="text" id="to" name="to" class="text ui-widget-content ui-corner-all" />
                        </td>
                </tr>
                <tr>
                    <td>
                        
                        </td>
                    
                </tr>
               
            </table>

        <br />
           
            
            
            
           
            
            
            
            
        
            

        </fieldset>
    </div>
    </form>
</body>
<script language="javascript" type="text/javascript">
    var user_id = '<%= Session["UserID"]%>';
    var UserName = '<%= Session["UserName"]%>';
    $(window).load(function () {
        //phan tim kiem

        $('#search').button().click(function () {

            //bind lblFact
            $.ajax({

                type: "POST",
                url: "LogBook_Report.aspx/GetFactAjax", //LogBook_Statistics.aspx/GetFactAjax
                data: '{"user_id":"' + user_id + '"}',

                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $.each(data.d, function (index, value) {

                        //                               
                        $('#lblFact').html(data.d[0].FactNo.toString());

                        //                                for (var i = 0; i < data.d.length; i++) {
                        //                                    value.append("<option value=" +  + ">" + data.d[i].FactNo.toString() + "</option>");
                        //                                }


                    });

                }
            });
            //bind ddlDept                  
            $.ajax({
                type: "POST",
                url: "LogBook_Report.aspx/GetDeptAjax",
                data: '{"user_id":"' + user_id + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $.each(data.d, function (index, value) {

                        var Dropdown = $('#<%=ddlDept.ClientID%>');
                        //clear
                        Dropdown.html("");
                        Dropdown.append("<option value=''>--Chọn--</option>");


                        for (var i = 0; i < data.d.length; i++) {
                            Dropdown.append("<option value=" + data.d[i].Dept_no.toString() + ">" + data.d[i].Dept_name.toString() + "</option>");
                        }

                    });

                }
            });
            //change deptBysec
            $('#<%=ddlDept.ClientID%>')
                  .change(function () {

                      //    alert('aa');
                      var ddlDeptItem = $('#ddlDept').val();
                      //
                      $.ajax({
                          type: "POST",
                          url: "LogBook_Report.aspx/GetSecByDeptAjax",
                          data: '{"user_id":"' + user_id + '" , "dept_no":"' + ddlDeptItem + '"}',
                          // data: '{"pfact_no":"' + fact + '","start_date":"' + s_date + '","end_date":"' + e_date + '"}',
                          contentType: "application/json;charset=utf-8",
                          dataType: "json",
                          success: function (data) {

                              $.each(data.d, function (index, value) {

                                  var Dropdown = $('#<%=ddlsec.ClientID%>');
                                  //clear
                                  Dropdown.html("");
                                  Dropdown.append("<option value=''>--Chọn--</option>");


                                  for (var i = 0; i < data.d.length; i++) {
                                      Dropdown.append("<option value=" + data.d[i].Sec_no.toString() + ">" + data.d[i].Sec_name.toString() + "</option>");
                                  }

                              });

                          }
                      });


                  })//end
                  .change();
            //changeQCbysec

            $('#<%=ddlsec.ClientID%>')
                  .change(function () {

                      //    alert('aa');
                      var ddlDeptItem = $('#ddlDept').val();
                      var ddlsecItem = $('#ddlsec').val();
                      //
                      $.ajax({
                          type: "POST",
                          url: "LogBook_Report.aspx/GetQCBySecAjax",
                          data: '{"user_id":"' + user_id + '" , "dept_no":"' + ddlDeptItem + '","sec_no":"' + ddlsecItem + '"}',
                          // data: '{"pfact_no":"' + fact + '","start_date":"' + s_date + '","end_date":"' + e_date + '"}',
                          contentType: "application/json;charset=utf-8",
                          dataType: "json",
                          success: function (data) {

                              $.each(data.d, function (index, value) {

                                  var Dropdown = $('#<%=ddlQC.ClientID%>');
                                  //clear
                                  Dropdown.html("");
                                  Dropdown.append("<option value=''>--Chọn--</option>");


                                  for (var i = 0; i < data.d.length; i++) {
                                      Dropdown.append("<option value=" + data.d[i].User_id.toString() + ">" + data.d[i].User_name.toString() + "</option>");
                                  }

                              });

                          }
                      });


                  })//end
                  .change();

            //
            $("#dialog-form").dialog("open");
            return false;
        });

        $("#dialog-form").dialog({
            autoOpen: false,
            height: 430,
            width: 500,
            modal: true,
            buttons: {
                "查詢": function () {

                    var fact = $('#lblFact').html();
                    var Dept = $("#ddlDept").val();
                    var Sec = $("#ddlsec").val();
                    var QC = $("#ddlQC").val(); //user_id;
                    var s_date = $("#from").datepicker({ dateFormat: "yymmdd" }).val();
                    var e_date = $("#to").datepicker({ dateFormat: "yymmdd" }).val();

                    loaddata(fact, Dept, Sec, QC, s_date, e_date);

                    $(this).dialog("close");
                }
                    ,
                "取消": function () {
                    $(this).dialog("close");
                }
            }
        });
        //

        //datepicker
        $("#from").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "yymmdd"
        });

        $("#from").datepicker("setDate", new Date());


        $("#to").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "yymmdd"
        });

        $("#to").datepicker("setDate", new Date());



        var fact = ""; //default
        var Dept = "";
        var Sec = "";
        var QC = "";
        var s_date = $("#from").datepicker({ dateFormat: "yymmdd" }).val();
        var e_date = $("#to").datepicker({ dateFormat: "yymmdd" }).val();

        // loaddata();
        loaddata(fact, Dept, Sec, QC, s_date, e_date);

    });

    function formatToMMYY(yyyymm) {
        var s = "";
        if (yyyymm.toString().length == 6) {

            s = yyyymm.toString().substring(4, 6) + "/" + yyyymm.toString().substring(2, 4);
        }
        return s;
    }
    function formatdate(yyyymmdd) {
        var s = "";
        if (yyyymmdd.toString().length == 8) {

            s = yyyymmdd.toString().substring(0, 4) + "/" + yyyymmdd.toString().substring(4, 6) + "/" + yyyymmdd.toString().substring(6, 8);
        }
        return s;
    }
    function getThang(yyyymm) {
        var s = "";
        if (yyyymm.toString().length == 6) {

            s = yyyymm.toString().substring(4, 6);
        }
        return s;
    }
    // Load DaTa 
    function loaddata(fact_no, dept_no, sec_no, user_id, start_date, end_date) {
        $(document).ready(function () {

            $.ajax({
                type: "POST",
                url: "LogBook_Report.aspx/GetSumQtyAjax",
                data: '{"pfact_no":"' + fact_no + '","pdept_no":"' + dept_no + '","psec_no":"' + sec_no + '","pQC":"' + user_id + '","start_date":"' + start_date + '","end_date":"' + end_date + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $tbl = $("#tblData");

                    //reset
                    $tbl.html("");

                    //var st = "<table id='main_data' class='table' cellspacing='0' cellpadding='2'>";
                    var st = "<thead>";
                    //first header
                    st += "<tr>";
                    st += "<td colspan='7' class='col_tieude_name' style='width:5%;'>";
                    st += "姓名(Name): ";
                    st += UserName;
                    st += "</td>";
                    st += "</tr>";
                    st += "<tr>";
                    st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                    st += "日期<br/>Date";
                    st += "</td>";
                    st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                    st += "檢驗雙數<br/>Total";
                    st += "</td>";
                    st += "<td colspan='2' class='col_tieude' style='width:5%;'>";
                    st += "檢驗結果<br/>Result";
                    st += "</td>";
                    st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                    st += "通過率<br/>Reate";
                    st += "</td>";
                    st += "<td rowspan='2' class='col_tieude' style='width:30%;'>";
                    st += "問題點備注<br/>Note ";
                    st += "</td>";
                    st += "<td rowspan='2' class='col_tieude' style='width:10%;'>";
                    st += "檢查者簽名<br/>Sign ";
                    st += "</td>";
                    st += "</tr>";
                    st += "<tr>";
                    st += "<td  class='col_tieude' style='width:5%;'>";
                    st += "通過數量<br/>Qty ";
                    st += "</td>";
                    st += "<td   class='col_tieude' style='width:5%;'>";
                    st += "不通過數量<br/>QtyOKitem ";
                    st += "</td>";

                    st += "</tr>";
                    st += "</thead>";

                    //Bind data
                    if (data.d.length > 0) {
                        /* var oYM;

                        if (data.d.length > 0) {
                        oYM = data.d[0].AL;

                        $.each(oYM, function (index, value) {

                        st += "<td colspan='3' class='col_tieude'>";
                        st += formatToMMYY(value.ColumnName);
                        st += "</td>";


                        });
                        }*/
                        /////////

                        //row data



                        var result = [];
                        $.each(data.d, function (i, e) {
                            if ($.inArray(e.Date, result) == -1) result.push(e.Date);
                        });


                        var i = 0, date = "", total = "", qty = "", qtyOK = "", rate = "", bad = "", sign = "", yyyymm = "", rate1 = "";

                        var date_old, date_old_2;
                        var t = 0, sum_rate = 0, Qty_agvr = 0, m = 0, total_number = 0, QtyOKitem_number = 0;
                        $.each(data.d, function (index, value) {//lap trong co so du lieu
                            // i = index + 1;
                            date = value.Date.toString();
                            total = value.Total;
                            total_number = Number(total);
                            qty = value.Qty;
                            qtyOK = value.QtyOKitem;
                            QtyOKitem_number = Number(qtyOK);
                           
                            m++;
                            rate = ((QtyOKitem_number / total_number) * 100); //value.Rate.toFixed(2)//cot rate

                            rate1 = (QtyOKitem_number / total_number) * 100
                            sum_rate += Number(rate);
                            bad = value.Bad 
                            sign = value.Sign
                            var k = 0;


                            if (date != date_old_2) {
                                //date_old = date;
                                date_old = data.d[index].Date;
                                for (var j = t; j <= data.d.length; j++) {
                                    if (date == date_old) {
                                        k++;
                                    }
                                    else {
                                        date_old_2 = date; // data.d[j].Date;
                                        t = j - 1;
                                        break;
                                    }

                                    if (j < data.d.length) {
                                        date_old = data.d[j].Date;

                                    }


                                }
                                k = k - 1;
                                date_old_2 = date;
                            }

                            st = st + "<tr>";

                            if (k > 0) {
                                st = st + "<td class='col_chan' rowspan='" + k + "'>" + formatdate(date) + "</td>";

                                st = st + "<td class='col_chan_right'>" + total + "</td>";
                                st = st + "<td class='col_chan_right'>" + qty + "</td>";
                                st = st + "<td class='col_chan_right'>" + qtyOK + "</td>";
                                st = st + "<td class='col_chan_right'>" + rate.toFixed(2) + "%</td>"; //.toFixed(2)
                                st = st + "<td class='col_chan'>" + bad + "</td>";
                                st = st + "<td class='col_chan'><img alt='Image Fact' src='SignImage.aspx?id=" + user_id + "'>" + sign + "</td>";
                            }
                            else {

                                st = st + "<td class='col_chan_right'>" + total + "</td>";
                                st = st + "<td class='col_chan_right'>" + qty + "</td>";
                                st = st + "<td class='col_chan_right'>" + qtyOK + "</td>";
                                st = st + "<td class='col_chan_right'>" + rate.toFixed(2) + "%</td>";
                                st = st + "<td class='col_chan'>" + bad + "</td>";
                                st = st + "<td class='col_chan'><img alt='Image Fact' src='SignImage.aspx?id=" + user_id + "'>" + sign + "</td>";
                            }
                            st = st + "</tr>";



                        }); //end each data.d

                        //});
                        var tb = 0;
                        tb = (sum_rate / m) * 100; //sum_qty.toFixed(2)                        

                        st = st + "<tr>";
                        st = st + "<td class='col_quy_total' colspan='7' >" + tb.toFixed(2);
                        st = st + "</td>";
                        st = st + "</tr>";
                        //
                    }
                   if (data.d.length <= 0) {//khong co du lieu

                        //reset<IMG alt="Image Fact" src="SignImage.aspx?id=524.00">
                        $tbl.html("");

                        var st = "<thead>";
                        //first header
                        st += "<tr>";
                        st += "<td colspan='7' class='col_tieude_name' style='width:5%;'>";
                        st += "姓名<br/>Name: ";
                        st += UserName;
                        st += "</td>";
                        st += "</tr>";
                        st += "<tr>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "日期<br/>Date";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "檢驗雙數<br/>Total";
                        st += "</td>";
                        st += "<td colspan='2' class='col_tieude' style='width:20%;'>";
                        st += "檢驗結果<br/>Result";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "通過率<br/>Reate";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:15%;'>";
                        st += "問題點備注<br/>Note (Note)";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:15%;'>";
                        st += "檢查者簽名<br/>Sign (Sign)";
                        st += "</td>";
                        st += "</tr>";
                        st += "<tr>";
                        st += "<td  class='col_tieude' style='width:15%;'>";
                        st += "通過數量<br/>Qty (Qty)";
                        st += "</td>";
                        st += "<td   class='col_tieude' style='width:15%;'>";
                        st += "不通過數量<br/>QtyOKitem (QtyOKitem)";
                        st += "</td>";

                        st += "</tr>";
                        st += "</thead>";

                        st += "<tbody>";
                        st += "<tr>";
                        st += "<td colspan='7' class='col_nodata'>查無資料 Tìm không thấy dữ liệu</td>";
                        st += "</tr>";
                        st += "</tbody>";

                        // $tbl.append(st);

                    }
                    //
                   if   (total == "0" & qty == "0" & qtyOK == "0" ) {//khong co du lieu

                        //reset<IMG alt="Image Fact" src="SignImage.aspx?id=524.00">
                        $tbl.html("");

                        var st = "<thead>";
                        //first header
                        st += "<tr>";
                        st += "<td colspan='7' class='col_tieude_name' style='width:5%;'>";
                        st += "姓名<br/>Name: ";
                        st += UserName;
                        st += "</td>";
                        st += "</tr>";
                        st += "<tr>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "日期<br/>Date";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "檢驗雙數<br/>Total";
                        st += "</td>";
                        st += "<td colspan='2' class='col_tieude' style='width:20%;'>";
                        st += "檢驗結果<br/>Result";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "通過率<br/>Reate";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:15%;'>";
                        st += "問題點備注<br/>Note (Note)";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:15%;'>";
                        st += "檢查者簽名<br/>Sign (Sign)";
                        st += "</td>";
                        st += "</tr>";
                        st += "<tr>";
                        st += "<td  class='col_tieude' style='width:15%;'>";
                        st += "通過數量<br/>Qty (Qty)";
                        st += "</td>";
                        st += "<td   class='col_tieude' style='width:15%;'>";
                        st += "不通過數量<br/>QtyOKitem (QtyOKitem)";
                        st += "</td>";

                        st += "</tr>";
                        st += "</thead>";

                        st += "<tbody>";
                        st += "<tr>";
                        st += "<td colspan='7' class='col_nodata'>查無資料 Tìm không thấy dữ liệu</td>";
                        st += "</tr>";
                        st += "</tbody>";

                        // $tbl.append(st);

                    }
                    //
                    $tbl.append(st);

                }, //end success
                error: function (xhr, ajaxOptions, thrownError) {
                    // alert("failed to load data");
                    alert(xhr.status);
                    alert(thrownError);
                    alert(xhr.responseText);
                }


            });

        });

    }

    //
    //  }
    //  );

</script>
</html>
