$(function () {

    $("#LoadDialogButton").off("click");
    $(document).on("click", "#LoadDialogButton", function () {


        var url = "DialogContentPage.aspx";
        var divId = " #MainContentDiv";

        var q1 = "?inp1=" + $("#Input1").val();
        var q2 = "&inp2=" + $("#Input2").val();

        url = url + q1 + q2 + divId; //url in the form 'DialogContentPage.aspx?inp1=xx&inp2=yy #MainContentDiv'

        $('<div id=DialogDiv>').dialog("destroy");

        $('<div id=DialogDiv>').dialog({
            dialogClass: 'DynamicDialogStyle',
            modal: true,
            open: function () {
                $(this).load(url);
            },
            close: function (e) {
                $(this).empty();
                $(this).dialog('destroy');
            },
            height: 350,
            width: 540,
            title: 'Dynamic Dialog'

        });
    });



    //---------------------------------------------

    $("#EvaluateButton").off("click");
    $(document).on("click", "#EvaluateButton", function () {

        var inputArray = new Array();
        $("#DynamicTable").find("input").each(function (index) {
            //alert(this.id);
            inputArray[index] = $(this).val();
        });

        var inputArrayList = "{ inputArray: " + JSON.stringify(inputArray) + "}";

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "DialogContentPage.aspx/Evaluate_Click",
            data: inputArrayList,
            success: function (data) {
                //debugger;

                if (data.d.indexOf("Error") != -1) {
                }
                else {
                    $("#ResultLabel").show();
                    $("#ResultLabel").text("Sum of all the contents is: " + data.d);
                }
            },
            error: function (e, ts, et) {
                //debugger;
                alert(ts);
            }
        }); //ajax func end
    });

    $("#LocalDialogModal").dialog('destroy');
    $("#LocalDialogModal").dialog({
        dialogClass: 'DynamicDialogStyle',
        autoOpen: false,
        resizable: false,
        draggable: false,
        modal: true,

        open: function (type, data) {
            $(this).parent().appendTo("form");
        },

        width: 500,
        height: 238,
        title: "Save Employee"
    });


    $("#LoadLocalDialog").off("click");
    $(document).on("click", "#LoadLocalDialog", function () {
        //debugger;
        $("#LocalDialogModal").dialog("open");
    });

});    //end of main jQuery Ready method

/*function OnchangeDept(obj) {
    //alert('s');
    var dept_No = obj.value;
    url: "LogBook_Report.aspx/Getsec_noByFactAjax";
    url.Getsec_noByFactAjax(dept_No, FactChange_CallBack);

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
}*/
function loaddata() {
    $(document).ready(function () {

        //bind ddlFact
        $.ajax({
            type: "POST",
            url: "DialogContentPage.aspx/Getsec_noByFactAjax",
            data: "{}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {

                $.each(data.d, function (index, value) {

                    var Dropdown = $('#<%=ddlDeptNo.ClientID%>');
                    //clear
                    Dropdown.html("");
                    Dropdown.append("<option value='0'>--Chọn--</option>");


                    for (var i = 0; i < data.d.length; i++) {
                        Dropdown.append("<option value=" + data.d[i].PubID.toString() + ">" + data.d[i].FactNo.toString() + "</option>");
                    }

                });

            }
        });
    });
}