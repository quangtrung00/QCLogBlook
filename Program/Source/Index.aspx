<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<%@ Register Src="Pub/Module/ToolBar.ascx" TagName="ToolBar" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Css/IndexStyle.css" rel="stylesheet"
        type="text/css" />
 <meta http-equiv="X-UA-Compatible" content="IE=7,IE=8,IE=9" />

    <link rel="stylesheet" type="text/css" href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Layout/Css/easyui.css">
    <link rel="stylesheet" type="text/css" href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Layout/Css/themes/icon.css">
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery191.min.js"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery.easyui.min.js"></script>
    <style>
        .loadingAdmin
        {
            position: absolute;
            background: url('Pub/EasyLayout/ImgBody/loading-bar.gif') #ffffff no-repeat 50% 30%;
            padding: 0px;
            margin: 0px;
            height: 100%;
            width: 100%;
            overflow: hidden;
            opacity: 1;
            filter: alpha(opacity=100) filter:1(opacity:100);
        }
        #iframeBody{
            background-image: url('<%= ResolveClientUrl("~/")%>Images/BgFrame.jpg') !important;
            background-position:bottom right !important; 
            background-size: 100% 100%;
}​
    </style>
    <script type="text/javascript">
        $(function () {

            tab_id = "iframeBody";
            loading_id = "divLoading";
            _InitFrame();
            function _InitFrame() {
                //Phan xu ly loading
                obj = $("#" + tab_id)[0];
                try {
                    obj.attachEvent('onload', function () { _fnframeLoad(tab_id) });
                } catch (e) {
                }
                obj.onload = function () {
                    _fnframeLoad(tab_id)
                }


                //
                $curFrame = getCurFrame();
                try {
                    obj = $(frames[tab_id].window)[0];
                } catch (e) { }

                CallBeforeUnload(obj, tab_id);

                $divLoadFrame = $curFrame.prev();
                //Khống chế lúc đang loading tab 1, thì chọn tab 2 , trở lại tab 1 vẫn còn loading        
                try {
                    iframe = $curFrame[0];
                    iframe = iframe.contentWindow || iframe.contentDocument;
                    if (iframe.document) iframe = iframe.document;
                }
                catch (e) {
                    iframe = $(frames[$curFrame.attr("id")].document)[0];
                }
                //     

                var _timer;
                _timer = setInterval(function () { //lặp tới khi trang load hoàn tất       

                    try {
                        if (iframe.readyState == 'complete') {
                            if ($divLoadFrame.is(":visible")) {
                                isFrameBusy = false;
                                $divLoadFrame.hide(); // Download is complete 
                                clearInterval(_timer);
                                clearTimeout(_timer);
                            }
                            clearInterval(_timer);
                            clearTimeout(_timer);
                        }
                    } catch (e) {
                        clearInterval(_timer);
                        clearTimeout(_timer);

                    }   //end if      
                }, 1)
            }

            /**************** Loading ****************/

            function getCurFrame() {
                return $("#" + tab_id);
            }


            function geCurLoading() {
                return $("#" + loading_id);
            }

            function _fnframeLoad(tab_id) {

                var theframe;
                try {
                    obj = $(frames[tab_id].window)[0];
                }
                catch (e) { }

                /*** Hide loading ***/
                $curFrame = getCurFrame();
                $divLoadFrame = $curFrame.prev();
                $divLoadFrame.hide();
                CallBeforeUnload(obj, tab_id);

            }


            function CallBeforeUnload(obj, tab_id) {

                try {
                    obj.attachEvent('onbeforeunload', function () { _fnframeUnLoad(tab_id) });
                } catch (e) { }
                obj.onbeforeunload = function () {
                    _fnframeUnLoad(tab_id);
                }
            }

            function _fnframeUnLoad(tab_id) {

                $curFrame = getCurFrame();
                $divLoadFrame = $curFrame.prev();

                if ($divLoadFrame) {
                    isFrameBusy = true;
                    $divLoadFrame.show();
                }

                window.onbeforeunload = null;
                window.onunload = null;
            }


            /**************** End Loading ****************/
        });
        //end $(function ()

    </script>
    <script>
        //application
        $(document).ready(function () {
            var apID = '<%=Request.Params["ApID"] %>';
            if (apID == "104") {
                $("#tab104").hide();
                $("#tab183").show();
            }
            else {
                $("#tab104").show();
                $("#tab183").hide();
            }

        });

        function funReLogin() {
            chk = confirm("確認登出現有使用者？");
            if (chk == true) {
                window.location.href = 'Default.aspx?Type=Logout';

            }
        }
        
    </script>
</head>
<body class="easyui-layout" style="overflow: hidden">
    <form id="form1" runat="server">
    <div data-options="region:'north',border:false,split:false" style="overflow: hidden; background-image:url('<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/backgroundQC.jpg')">
        <uc1:ToolBar ID="ToolBar1" runat="server" />
    </div>
    <div data-options="region:'center',split:false" style="overflow: hidden">
        <%--fit="true" fullscreen layout content tab--%>
        <div id="divLoading" class="loadingAdmin">
        </div>
        <iframe runat="server" id="iframeBody" name="iframeBody" frameborder="0" src="Menu.aspx?ApID=249"
            style="width: 100%; height: 100%;"></iframe>
    </div>
    
    </form>
</body>
</html>
