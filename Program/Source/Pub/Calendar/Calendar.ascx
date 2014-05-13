<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pub_Module_Calendar" CodeFile="Calendar.ascx.cs" %>
<script src="<%= ResolveClientUrl("~/")%>Pub/Calendar/Datepicker/jquery-1_4.js" type="text/javascript"></script>
<link rel="stylesheet" href="<%= ResolveClientUrl("~/")%>Pub/Calendar/Datepicker/jquery-ui.css">
<script src="<%= ResolveClientUrl("~/")%>Pub/Calendar/Datepicker/jquery-ui.min.js"></script>
<script>
    var Url = '<%= ResolveClientUrl("~/")%>';
    $(function () {
        $("#<%= UC_Calendar.ClientID%>").datepicker({
            showOn: "<%= sButtonImage %>",
            buttonImage: Url + "Pub/Calendar/Datepicker/Images/calendar.gif",
            buttonImageOnly: true,
            changeMonth: eval("<%= bchangeMonthAndYear %>".toLowerCase()),
            changeYear: eval("<%= bchangeMonthAndYear %>".toLowerCase()),
            showButtonPanel: eval("<%= bshowButtonPanel %>".toLowerCase()),
            dateFormat: "<%= sFormatDate %>",
            closeText: 'Clear',
            beforeShowDay: fnOffSet,
            onClose: fnClose
        });

        $("#<%= UC_Calendar.ClientID%>").datepicker("<%= bDisabledButton %>".toLowerCase() == "true" ? "disable" : "enable");
        $("#<%= UC_Calendar.ClientID%>").attr('readOnly', eval("<%= bReadOnly %>".toLowerCase()));

        $UC_Calendar_OffsetX = "0";
        $UC_Calendar_OffsetY = "0";
        function fnOffSet() {
            $UC_Calendar_OffsetX = "<%= sOffSetX %>";
            $UC_Calendar_OffsetY = "<%= sOffSetY %>";
            return [1];
        }
        function fnClose() {
            $('.ui-datepicker-close').bind('click', function () {
                $("#<%= UC_Calendar.ClientID%>").val("");
            });
        }
        //disable backspace
        document.onkeydown = function () {            
            var e = event || window.event;
            var keyASCII = parseInt(e.keyCode, 10);
            var src = e.srcElement;
            var tag = src.tagName.toUpperCase();
            /*if (keyASCII == 13) {
                return false;
            }*/
            if (keyASCII == 8) {
                if (src.readOnly || src.disabled || (tag != "INPUT" && tag != "TEXTAREA")) {
                    return false;
                }
                if (src.type) {
                    var type = ("" + src.type).toUpperCase();
                    return type != "CHECKBOX" && type != "RADIO" && type != "BUTTON";
                }
            }
            return true;
        }



    });

</script>
<asp:TextBox ID="UC_Calendar" runat="server" Width="90px"></asp:TextBox>
