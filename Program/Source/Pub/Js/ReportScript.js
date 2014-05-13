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

    //$("#EvaluateButton").off("click");
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