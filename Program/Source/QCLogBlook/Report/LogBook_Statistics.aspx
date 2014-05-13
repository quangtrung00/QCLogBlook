<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogBook_Statistics.aspx.cs"
    Inherits="QCLogBlook_Report_LogBook_Statistics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>QC Log Book Review</title>
    <link rel="Stylesheet" type="text/css" href="../../Pub/Css/Statistics.css" />
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
            padding: 0.5em;
        }
        select
        {
            padding: 0.5em;
        }
        input.text
        {
            margin-bottom: 12px;
            width: 95%;
            padding: .4em;
        }
        fieldset
        {
            padding: 1em;
            border: 0;
            margin-top: 25px;
        }
        /*
        label {
          float:left;
          width:25%;
          margin-right:0.5em;
          padding-top:0.2em;
          text-align:right;
          font-weight:bold;
          }
       */
        .ui-dialog .ui-state-error
        {
            padding: .3em;
        }
        .validateTips
        {
            border: 1px solid transparent;
            padding: 0.3em;
        }
    </style>
    <script language="javascript" type="text/javascript">

        $(window).load(function () {

            //phan tim kiem
            $('#search').button().click(function () {

                //bind ddlFact
                $.ajax({
                    type: "POST",
                    url: "LogBook_Statistics.aspx/GetFactAjax",
                    data: "{}",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $.each(data.d, function (index, value) {

                            var Dropdown = $('#<%=ddlFact.ClientID%>');
                            //clear
                            Dropdown.html("");
                            Dropdown.append("<option value=''>--<%=sStaMsgSelect %>--</option>");


                            for (var i = 0; i < data.d.length; i++) {
                                Dropdown.append("<option value=" + data.d[i].FactNo.toString() + ">" + data.d[i].FactNo.toString() + "</option>");
                            }

                        });

                    }
                });

                $("#dialog-form").dialog("open");
                return false;
            });

            $("#dialog-form").dialog({
                autoOpen: false,
                height: 430,
                width: 450,
                modal: true,
                buttons: {
                    "<%=sStaOk %>": function () {

                        var fact = $("#ddlFact").val();
                        var s_date = $("#from").datepicker({ dateFormat: "yymmdd" }).val();
                        var e_date = $("#to").datepicker({ dateFormat: "yymmdd" }).val();

                        loaddata(fact, s_date, e_date);

                        $(this).dialog("close");
                    }
                    ,
                    "<%=sStaCancel %>": function () {
                        $(this).dialog("close");
                    }
                }
            });

            //datepicker
            /*  */
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
            var s_date = $("#from").datepicker({ dateFormat: "yymmdd" }).val();
            var e_date = $("#to").datepicker({ dateFormat: "yymmdd" }).val();

            loaddata(fact, s_date, e_date);

        });

        function formatToEnglishMonthName(mm) {
            var s = "";

            switch (mm) {
                case "01":
                    s = "Jan";
                    break;
                case "02":
                    s = "Feb";
                    break;
                case "03":
                    s = "Mar";
                    break;
                case "04":
                    s = "Apr";
                    break;
                case "05":
                    s = "May";
                    break;
                case "06":
                    s = "Jun";
                    break;
                case "07":
                    s = "Jul";
                    break;
                case "08":
                    s = "Aug";
                    break;
                case "09":
                    s = "Sep";
                    break;
                case "10":
                    s = "Oct";
                    break;
                case "11":
                    s = "Nov";
                    break;
                case "12":
                    s = "Dec";
                    break;
            }

            return s;
        }

        function formatToMMYY(yyyymm) {
            var s = "";
            if (yyyymm.toString().length == 6) {

                s = formatToEnglishMonthName(yyyymm.toString().substring(4, 6)) + "/" + yyyymm.toString().substring(2, 4);
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

        function getYear(yyyymm) {
            var s = "";
            if (yyyymm.toString().length == 6) {

                s = yyyymm.toString().substring(0, 4);
            }
            return s;
        }


        function fnGetQuy(co) {

            var s = "";

            switch (co) {
                case "01":
                case "02":
                case "03":
                    s = "AVG Jan – Mar";
                    break;
                case "04":
                case "05":
                case "06":
                    s = "AVG Apr – Jun";
                    break;
                case "07":
                case "08":
                case "09":
                    s = "AVG Jul – Sep";
                    break;
                case "10":
                case "11":
                case "12":
                    s = "AVG Nov – Dec";
                    break;
            } //end switch
            return s;
        }

        function fnCalTotalQuy(totalQuy1, totalQuy2, totalQuy3, totalQuy4, flag1, flag2, flag3, flag4, co) {
            var st = 0;

            switch (co) {
                case "01":
                case "02":
                case "03":
                    if (flag1 > 0) {
                        st = (totalQuy1 / flag1);
                    }
                    break;
                case "04":
                case "05":
                case "06":
                    if (flag2 > 0) {
                        st = (totalQuy2 / flag2);
                    }
                    break;
                case "07":
                case "08":
                case "09":
                    if (flag3 > 0) {
                        st = (totalQuy3 / flag3);
                    }
                    break;
                case "10":
                case "11":
                case "12":
                    if (flag4 > 0) {
                        st = (totalQuy4 / flag4);
                    }
                    break;
            } //end switch

            if (st > 0) st = st.toFixed(2);

            return st;
        }


        function fnGetQuy(co,nam) {

            var s = "";

            switch (co) {
                case "01":
                case "02":
                case "03":
                    s = "AVG Jan – Mar/"+nam;
                    break;
                case "04":
                case "05":
                case "06":
                    s = "AVG Apr – Jun/" + nam;
                    break;
                case "07":
                case "08":
                case "09":
                    s = "AVG Jul – Sep/" + nam;
                    break;
                case "10":
                case "11":
                case "12":
                    s = "AVG Nov – Dec/" + nam;
                    break;
            } //end switch
            return s;
        }

        //        function fnCalTotalQuy(totalQuy1, totalQuy2, totalQuy3, totalQuy4, flag1, flag2, flag3, flag4, co) {
        //            var st = 0;

        //            switch (co) {
        //                case "01":
        //                case "02":
        //                case "03":
        //                    st = (totalQuy1 / flag1);
        //                    break;
        //                case "04":
        //                case "05":
        //                case "06":
        //                    st = (totalQuy2 / flag2);
        //                    break;
        //                case "07":
        //                case "08":
        //                case "09":
        //                    st = (totalQuy3 / flag3);
        //                    break;
        //                case "10":
        //                case "11":
        //                case "12":
        //                    st = (totalQuy4 / flag4);
        //                    break;
        //            } //end switch

        //            if (st > 0) st = st.toFixed(2);

        //            return st;
        //        }


        function loaddata(fact, s_date, e_date) {
            $(document).ready(function () {
                // alert(fact + " " + s_date + " " + e_date);

                $.ajax({
                    type: "POST",
                    url: "LogBook_Statistics.aspx/GetStatisticsAjaxNew",
                    data: '{"pfact_no":"' + fact + '","start_date":"' + s_date + '","end_date":"' + e_date + '"}',
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
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        st += "NO.";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        //st += "廠別<br/>Xưởng";
                        st += "<% =sStaFactName%>";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        //st += "樓別/樓層<br/>Tầng lầu";
                        st += "<% =sStaSecNo%>";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:5%;'>";
                        //st += "工號<br/>Số thẻ";
                        st += "<% =sStaPnlNo%>";
                        st += "</td>";
                        st += "<td rowspan='2' class='col_tieude' style='width:15%;'>";
                        //st += "Name(定點QC)<br/>Tên (QC cố định)";
                        st += "<% =sStaQCCD%>";
                        st += "</td>";

                        //create dynamic columns   
                        //chi lay dong dau tien lam cot
                        /*
                        dinh dang du lieu
                        fact_no	floor	emp_no	user_desc	201401	201403	201404	201406	201407	201409	201411	201412
                        172 	4	005687	黎氏蓉(Le Nhung)	1000	NULL	500	NULL	NULL	NULL	NULL	NULL
                        */
                        //                          var oYM = [
                        //                            '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'
                        //                          ];

                        var oYM, thang, nam;
                        var co = ["", "", "", ""]; //co: ghi nhan quy nao co du lieu

                        if (data.d.length > 0) {
                            oYM = data.d[0].AL;

                            $.each(oYM, function (index, value) {

                                //them cot quy
                                /* */
                                thang = getThang(value.ColumnName);

                                switch (thang) {
                                    case "01":
                                        co[0] = "01";
                                        break;
                                    case "02":
                                        co[0] = "02";
                                        break;
                                    case "03":

                                        co[0] = "03";
                                        break;
                                    case "04":
                                        co[1] = "04";
                                        break;
                                    case "05":
                                        co[1] = "05";
                                        break;
                                    case "06":

                                        co[1] = "06";
                                        break;
                                    case "07":
                                        co[2] = "07";
                                        break;
                                    case "08":
                                        co[2] = "08";
                                        break;
                                    case "09":

                                        co[2] = "09";
                                        break;
                                    case "10":
                                        co[3] = "10";
                                        break;
                                    case "11":
                                        co[3] = "11";
                                        break;
                                    case "12":
                                        co[3] = "12";
                                        break;

                                } //end switch

                            });

                            $.each(oYM, function (index, value) {

                                st += "<td colspan='3' class='col_tieude'>";
                                st += formatToMMYY(value.ColumnName);
                                st += "</td>";

                                //them cot quy
                                /* */
                                thang = getThang(value.ColumnName);
                                nam = getYear(value.ColumnName);

                                $.each(co, function (index, value) {
                                    //if (value != null && value!="" && value != "03" && value != "06" && value != "09" && value != "12") {
                                    if (thang == value) {

                                        st += "<td rowspan=2 class='col_tieude_quy'>";
                                        st += fnGetQuy(value, nam); //lay tieu de quy
                                        st += "</td>";
                                    }
                                });
                            });
                            /////////


                            st = st + "</tr>";


                            //  $tbl.append(st);

                            //header 2
                            st += "<tr>";
                            $.each(oYM, function (index, value) {

                                st += "<td  class='col_tieude'>INSP</td>";
                                st += "<td  class='col_tieude'>ACC</td>";
                                st += "<td  class='col_tieude'>%</td>";
                            });
                            st += "</tr>";

                            st += "</thead>";
                            st += "<tbody>";
                            //row data --------------------------------------------------------------------------------------------
                            var i = 0, fact_no = "", floor = "", emp_no = "", user_desc = "", insp = "", yyyymm = "", acc = "";
                            var percent = 0, totalQuy1 = 0, totalQuy2 = 0, totalQuy3 = 0, totalQuy4 = 0;
                            var flag1 = 0, flag2 = 0, flag3 = 0, flag4 = 0; //ghi nhan thang tinh de tinh trung binh cong
                            var totalInsp = [oYM.length], totalAcc = [oYM.length]; //tong insp theo dong
                            var totalQuyP1 = 0, totalQuyP2 = 0, totalQuyP3 = 0, totalQuyP4 = 0; //tong % theo quy
                            //init
                            $.each(oYM, function (index, value) {
                                totalInsp[index] = 0;
                                totalAcc[index] = 0;
                            });

                            $.each(data.d, function (index, value) {
                                i = index + 1;
                                fact_no = value.FactNo.toString();
                                floor = value.Floor;
                                emp_no = value.EmpNo;
                                user_desc = value.UserDesc;


                                st = st + "<tr>";

                                st = st + "<td class='col_chan_center'>" + i + "</td>";
                                st = st + "<td class='col_chan_center'>" + fact_no + "</td>";
                                st = st + "<td class='col_chan_center'>" + floor + "</td>";
                                st = st + "<td class='col_chan_center'>" + emp_no + "</td>";
                                st = st + "<td class='col_chan'>" + user_desc + "</td>";

                                //gan gia tri cho cac cot yyyymm
                                //reset co
                                co = ["", "", "", ""];
                                coTotal = [0, 0, 0, 0];
                                flagTotal = [0, 0, 0, 0];

                                $.each(value.AL, function (i, v) {

                                    insp = Number(v.INSP);

                                    acc = Number(v.ACC);

                                    if (insp > 0) {
                                        totalInsp[i] += insp;
                                    }
                                    else {
                                        totalInsp[i] += 0;
                                    }

                                    if (acc > 0) {

                                        totalAcc[i] += acc;
                                    }
                                    else {
                                        totalAcc[i] += 0;
                                    }

                                    if ($.isNumeric(insp) && $.isNumeric(acc) && insp > 0 && acc > 0) {
                                        percent = acc / insp * 100;

                                    } else {
                                        percent = 0;
                                    }

                                    switch (getThang(v.ColumnName)) {
                                        case "01":
                                            totalQuy1 += percent;
                                            if (percent > 0) {
                                                flag1 += 1;
                                            }
                                            co[0] = "01";
                                            coTotal[0] += totalQuy1;
                                            flagTotal[0] += flag1;
                                            break;
                                        case "02":
                                            totalQuy1 += percent;
                                            if (percent > 0) {
                                                flag1 += 1;
                                            }
                                            co[0] = "02";
                                            coTotal[0] += totalQuy1;
                                            flagTotal[0] += flag1;
                                            break;
                                        case "03":
                                            if (percent > 0) {
                                                flag1 += 1;
                                            }
                                            totalQuy1 += percent;

                                            co[0] = "03";
                                            coTotal[0] += totalQuy1;
                                            flagTotal[0] += flag1;
                                            break;
                                        case "04":
                                            if (percent > 0) {
                                                flag2 += 1;
                                            }
                                            totalQuy2 += percent;
                                            co[1] = "04";
                                            coTotal[1] += totalQuy2;
                                            flagTotal[1] += flag2;
                                            break;
                                        case "05":
                                            if (percent > 0) {
                                                flag2 += 1;
                                            }
                                            totalQuy2 += percent;
                                            co[1] = "05";
                                            coTotal[1] += totalQuy2;
                                            flagTotal[1] += flag2;
                                            break;
                                        case "06":
                                            if (percent > 0) {
                                                flag2 += 1;
                                            }
                                            totalQuy2 += percent;

                                            co[1] = "06";
                                            coTotal[1] += totalQuy2;
                                            flagTotal[1] += flag2;
                                            break;
                                        case "07":
                                            if (percent > 0) {
                                                flag3 += 1;
                                            }
                                            totalQuy3 += percent;
                                            co[2] = "07";
                                            coTotal[2] += totalQuy3;
                                            flagTotal[2] += flag3;
                                            break;
                                        case "08":
                                            if (percent > 0) {
                                                flag3 += 1;
                                            }
                                            totalQuy3 += percent;
                                            co[2] = "08";
                                            coTotal[2] += totalQuy3;
                                            flagTotal[2] += flag3;
                                            break;
                                        case "09":
                                            if (percent > 0) {
                                                flag3 += 1;
                                            }
                                            totalQuy3 += percent;

                                            co[2] = "09";
                                            coTotal[2] += totalQuy3;
                                            flagTotal[2] += flag3;
                                            break;
                                        case "10":
                                            if (percent > 0) {
                                                flag4 += 1;
                                            }
                                            totalQuy4 += percent;
                                            co[3] = "10";
                                            coTotal[3] += totalQuy4;
                                            flagTotal[3] += flag4;
                                            break;
                                        case "11":
                                            if (percent > 0) {
                                                flag4 += 1;
                                            }
                                            totalQuy4 += percent;
                                            co[3] = "11";
                                            coTotal[3] += totalQuy4;
                                            flagTotal[3] += flag4;
                                            break;
                                        case "12":
                                            if (percent > 0) {
                                                flag4 += 1;
                                            }
                                            totalQuy4 += percent;

                                            co[3] = "12";
                                            coTotal[3] += totalQuy4;
                                            flagTotal[3] += flag4;
                                            break;
                                    } //end switch

                                    //reset
                                    totalQuy1 = 0;
                                    totalQuy2 = 0;
                                    totalQuy3 = 0;
                                    totalQuy4 = 0;

                                    flag1 = 0;
                                    flag2 = 0;
                                    flag3 = 0;
                                    flag4 = 0;

                                }); //end each value.AL
                                //-------------------------------------------------------
                                $.each(value.AL, function (i, v) {

                                    insp = Number(v.INSP);

                                    acc = Number(v.ACC);

                                    if (insp > 0) {
                                        st += "<td  class='col_chan_center'>" + insp + "</td>";

                                    }
                                    else
                                        st += "<td  class='col_chan_center'></td>";

                                    if (acc > 0) {
                                        st += "<td  class='col_chan_center'>" + acc + "</td>";

                                    }
                                    else
                                        st += "<td  class='col_chan_center'></td>";

                                    if ($.isNumeric(insp) && $.isNumeric(acc) && insp > 0 && acc > 0) {
                                        percent = acc / insp * 100;

                                        if (percent >= 90)
                                            st = st + "<td class='col_xanh'>" + percent.toFixed(2) + " %</td>";
                                        else
                                            st = st + "<td class='col_vang'>" + percent.toFixed(2) + " %</td>";

                                    } else {
                                        percent = 0;
                                        st = st + "<td  class='col_do'></td>";

                                    }

                                    thang = getThang(v.ColumnName);

                                    $.each(co, function (coi, covalue) {
                                        if (thang == covalue) {
                                            var iTotalQuy = fnCalTotalQuy(coTotal[0], coTotal[1], coTotal[2], coTotal[3], flagTotal[0], flagTotal[1], flagTotal[2], flagTotal[3], covalue);
                                            st += "<td  class='col_quy_total'>";
                                            if (iTotalQuy > 0)
                                                st += iTotalQuy + " %</td>";
                                            else
                                                st += "</td>";

                                        }
                                    });

                                }); //end each value.AL
                                //-------------------------------------------------------



                                //reset
                                //                                totalQuy1 = 0;
                                //                                totalQuy2 = 0;
                                //                                totalQuy3 = 0;
                                //                                totalQuy4 = 0;

                                //                                flag1 = 0;
                                //                                flag2 = 0;
                                //                                flag3 = 0;
                                //                                flag4 = 0;

                                st = st + "</tr>";

                            }); //end each data.d

                            //total row------------------------------------------
                            //reset
                            coTotal = [0, 0, 0, 0];
                            flagTotal = [0, 0, 0, 0];


                            st += "</tbody>";
                            st += "<tfoot>";
                            st += "<tr>";
                            st += "<td class='col_total'>";
                            st += "&nbsp;</td>";
                            st += "<td class='col_total'>";
                            st += "&nbsp;</td>";
                            st += "<td class='col_total'>";
                            st += "&nbsp;</td>";
                            st += "<td class='col_total'>";
                            st += "&nbsp;</td>";
                            st += "<td class='col_total'>";
                            st += "&nbsp;</td>";
                            //tao cac cot yyyymm
                            var totalPercent = 0;

                            $.each(oYM, function (index, value) {

                                if ($.isNumeric(totalInsp[index]) && $.isNumeric(totalAcc[index]) && totalInsp[index] > 0) {
                                    totalPercent = totalAcc[index] / totalInsp[index] * 100;
                                }

                                switch (getThang(value.ColumnName)) {
                                    case "01":
                                        //totalQuyP1 += totalPercent;
                                        flag1 += 1;
                                        co[0] = "01";
                                        coTotal[0] += totalPercent;
                                        flagTotal[0] += flag1;
                                        break;
                                    case "02":
                                        //totalQuyP1 += totalPercent;
                                        flag1 += 1;
                                        co[0] = "02";
                                        coTotal[0] += totalPercent;
                                        flagTotal[0] += flag1;
                                        break;
                                    case "03":
                                        //totalQuyP1 += totalPercent;
                                        flag1 += 1;

                                        co[0] = "03";
                                        coTotal[0] += totalPercent;
                                        flagTotal[0] += flag1;
                                        break;
                                    case "04":
                                        //totalQuyP2 += totalPercent;
                                        flag2 += 1;
                                        co[1] = "04";
                                        coTotal[1] += totalPercent;
                                        flagTotal[1] += flag2;
                                        break;
                                    case "05":
                                        // totalQuyP2 += totalPercent;
                                        flag2 += 1;
                                        co[1] = "05";
                                        coTotal[1] += totalPercent;
                                        flagTotal[1] += flag2;
                                        break;
                                    case "06":
                                        // totalQuyP2 += totalPercent;
                                        flag2 += 1;

                                        co[1] = "06";
                                        coTotal[1] += totalPercent;
                                        flagTotal[1] += flag2;
                                        break;
                                    case "07":
                                        // totalQuyP3 += totalPercent;
                                        flag3 += 1;
                                        co[2] = "07";
                                        coTotal[2] += totalPercent;
                                        flagTotal[2] += flag3;
                                        break;
                                    case "08":
                                        //  totalQuyP3 += totalPercent;
                                        flag3 += 1;
                                        co[2] = "08";
                                        coTotal[2] += totalPercent;
                                        flagTotal[2] += flag3;
                                        break;
                                    case "09":
                                        //totalQuyP3 += totalPercent;
                                        flag3 += 1;
                                        co[2] = "09";
                                        coTotal[2] += totalPercent;
                                        flagTotal[2] += flag3;
                                        break;
                                    case "10":
                                        // totalQuyP4 += totalPercent;
                                        flag4 += 1;
                                        co[3] = "10";
                                        coTotal[3] += totalPercent;
                                        flagTotal[3] += flag4;
                                        break;
                                    case "11":
                                        // totalQuyP4 += totalPercent;
                                        flag4 += 1;
                                        co[3] = "11";
                                        coTotal[3] += totalPercent;
                                        flagTotal[3] += flag4;
                                        break;
                                    case "12":
                                        // totalQuyP4 += totalPercent;
                                        flag4 += 1;
                                        co[3] = "12";
                                        coTotal[3] += totalPercent;
                                        flagTotal[3] += flag4;
                                        break;
                                } //end switch               


                                //reset
                                //totalQuyP1 = 0;
                                //totalQuyP2 = 0;
                                // totalQuyP3 = 0;
                                //totalQuyP4 = 0;
                                totalPercent = 0;
                                flag1 = 0;
                                flag2 = 0;
                                flag3 = 0;
                                flag4 = 0;

                            }); //end each oYM

                            $.each(oYM, function (index, value) {


                                st += "<td  class='col_total'>" + totalInsp[index] + "</td>";
                                st += "<td  class='col_total'>" + totalAcc[index] + "</td>";

                                if ($.isNumeric(totalInsp[index]) && $.isNumeric(totalAcc[index]) && totalInsp[index] > 0) {
                                    totalPercent = totalAcc[index] / totalInsp[index] * 100;
                                }

                                if (totalPercent >= 90)
                                    st = st + "<td class='col_xanh'>" + totalPercent.toFixed(2) + " %</td>";
                                else
                                    st = st + "<td class='col_vang'>" + totalPercent.toFixed(2) + " %</td>";


                                thang = getThang(value.ColumnName);

                                $.each(co, function (ccoi, ccovalue) {
                                    if (thang == ccovalue) {

                                        st += "<td   class='col_quy_total'>";
                                        st += fnCalTotalQuy(coTotal[0], coTotal[1], coTotal[2], coTotal[3], flagTotal[0], flagTotal[1], flagTotal[2], flagTotal[3], ccovalue)
                                        st += " %</td>";
                                    }
                                });

                            }); //end each oYM


                            //--------------------------------------------------------------------------------------

                            st += "</tr>";
                            st += "</tfoot>";

                            $tbl.append(st);

                        } //end data.d.length
                        else {//khong co du lieu

                            //reset
                            $tbl.html("");

                            //var st = "<table id='main_data' class='table' cellspacing='0' cellpadding='2'>";
                            var st = "<thead>";
                            //first header
                            st += "<tr>";
                            st += "<td  class='col_tieude'>";
                            st += "NO.";
                            st += "</td>";
                            st += "<td  class='col_tieude'>";
                            //st += "廠別<br/>Xưởng";
                            st += "<% =sStaFactName%>";
                            st += "</td>";
                            st += "<td  class='col_tieude'>";
                            //st += "樓別/樓層<br/>Tầng lầu";
                            st += "<% =sStaSecNo%>";
                            st += "</td>";
                            st += "<td class='col_tieude'>";
                            //st += "工號<br/>Số thẻ";
                            st += "<% =sStaPnlNo%>";
                            st += "</td>";
                            st += "<td  class='col_tieude'>";
                            //st += "Name(定點QC)<br/>Tên (QC cố định)";
                            st += "<% =sStaQCCD%>";
                            st += "</td>";

                            st += "</tr>";
                            st += "</thead>";

                            st += "<tbody>";
                            st += "<tr>";
                            //st += "<td colspan='5' class='col_nodata'>查無資料 Tìm không thấy dữ liệu</td>";
                            st += "<td colspan='5' class='col_nodata'>";
                            st += "<% =sStaMsgNoData%>";
                            st += "</td>";
                            st += "</tr>";
                            st += "</tbody>";

                            $tbl.append(st);

                        }
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

     

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <h1>
        <asp:Label ID="Label1" runat="server" Text="QC Log Book  Review "></asp:Label>
    </h1>
    <div id="searchbuttondiv">
        <button id="search">
            <%=sStaSearch%></button>
    </div>
    <br />
    <div>
        <div id="divMain">
            <table id="tblData" class="table" cellspacing="0">
            </table>
        </div>
    </div>
    <div id="dialog-form" title="<%=sStaSearchForm%>">
        <fieldset>
            <label for="xuong">
                <%=sStaFactName%>:</label>
            <asp:DropDownList ID="ddlFact" runat="server" >
            </asp:DropDownList>
            
            <label for="ddmm">
                <%=sDDMM%>:</label>
            <label for="from">
            </label>
            <input type="text" id="from" name="from" class="text ui-widget-content ui-corner-all" />
            <label for="to">
                ~
            </label>
            <input type="text" id="to" name="to" class="text ui-widget-content ui-corner-all"  />
        </fieldset>
    </div>
    </form>
</body>
</html>
