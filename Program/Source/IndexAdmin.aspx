<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndexAdmin.aspx.cs" Inherits="Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Css/IndexStyleAdmin.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Layout/Css/easyui.css">
    <link rel="stylesheet" type="text/css" href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Layout/Css/themes/icon.css">
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery191.min.js"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Js/jquery.tabs.js"></script>
    <script src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Menu/ddaccordion.js" type="text/javascript"></script>
    <link href="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/Menu/J_Menu.css" rel="stylesheet"
        type="text/css" />
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
    </style>
    <%--Menu--%>
    <script type="text/javascript">        var Url = '<%= ResolveClientUrl("~/")%>'; ddaccordion.init({ autoHeight: true, headerclass: "submenuheader", contentclass: "submenu", revealtype: "click", mouseoverdelay: 0, collapseprev: false, defaultexpanded: [0], onemustopen: false, animatedefault: false, persiststate: true, toggleclass: ["", ""], togglehtml: ["suffix", "<img src='" + Url + "Pub/EasyLayout/Menu/images/plus.gif' class='statusicon' />", "<img src='" + Url + "Pub/EasyLayout/Menu/images/minus.gif' class='statusicon' />"], animatespeed: "fast", oninit: function (a, b) { }, onopenclose: function (a, b, c, d) { } })</script>
    <%--Tab--%>
    <script type="text/javascript">$(function(){if(detectIE().toString().toLowerCase()=="false"){alert("系統僅支援IE瀏覽器 !!!(Hệ thống chỉ hỗ trợ trên trình duyệt IE");window.location.href="Default.aspx";return}function detectIE(){return true; var a=window.navigator.userAgent;var b=a.indexOf('MSIE ');var c=a.indexOf('Trident/');if(b>0){return parseInt(a.substring(b+5,a.indexOf('.',b)),10)}if(c>0){var d=a.indexOf('rv:');return parseInt(a.substring(d+3,a.indexOf('.',d)),10)}return false}var f=false;var g="load_";var h=false;$(window).load(function(){addTab("Home",Url+"Pub/Module/LoginBody.aspx?ApID=<%= Request.QueryString["ApID"] %>",false)});$(".menu_Link").click(function(){if(f){alert("正在載入另一個頁面﹐請稍候...");return}var a=$(this).attr("mnuName");var b=$(this).attr("mnuLink");addTab(a,b,true)});var i=1;function addTab(a,b,c){if($('#TabLayout').tabs('exists',a)){$('#TabLayout').tabs('select',a);return}f=true;tab_id="tabcontent_"+i;i++;$('#TabLayout').tabs('add',{title:a,content:'<div id="'+g+tab_id+'" class="loadingAdmin" > </div><iframe  id="'+tab_id+'" name="'+tab_id+'" style="width: 100%; height: 100%;"     src="'+b+'" frameborder="0"></iframe>',closable:eval(c)});obj=$("#"+tab_id)[0];try{obj.attachEvent('onload',function(){_fnframeLoad(tab_id)})}catch(e){}obj.onload=function(){_fnframeLoad(tab_id)}}var j;$('#TabLayout').tabs({onSelect:function(a,b){try{clearInterval(j);clearTimeout(j);j.abort()}catch(e){}$curFrame=getCurFrame();tab_id=$curFrame.attr("id");try{obj=$(frames[tab_id].window)[0]}catch(e){}CallBeforeUnload(obj,tab_id);$divLoadFrame=$curFrame.prev();try{iframe=$curFrame[0];iframe=iframe.contentWindow||iframe.contentDocument;if(iframe.document)iframe=iframe.document}catch(e){iframe=$(frames[$curFrame.attr("id")].document)[0]}j=setInterval(function(){try{if(iframe.readyState=='complete'){if($divLoadFrame.is(":visible")){f=false;$divLoadFrame.hide();clearInterval(j);clearTimeout(j)}clearInterval(j);clearTimeout(j)}}catch(e){clearInterval(j);clearTimeout(j)}},1)}});function getCurFrame(){var a=$('#TabLayout').tabs('getSelected');var b=a.find("iframe");return b}function _fnframeLoad(a){var b;f=false;try{obj=$(frames[a].window)[0]}catch(e){}$curFrame=getCurFrame();$divLoadFrame=$curFrame.prev();$divLoadFrame.hide();CallBeforeUnload(obj,a)}function CallBeforeUnload(a,b){try{a.attachEvent('onbeforeunload',function(){_fnframeUnLoad(b)})}catch(e){}a.onbeforeunload=function(){_fnframeUnLoad(b)}}function _fnframeUnLoad(a){$curFrame=getCurFrame();$divLoadFrame=$curFrame.prev();if($divLoadFrame){f=true;$divLoadFrame.show()}window.onbeforeunload=null;window.onunload=null}});$(document).ready(function(){var a='<%=Request.Params["ApID"] %>';if(a=="104"){$("#tab104").hide();$("#tab183").show()}else{$("#tab104").show();$("#tab183").hide()}});function funReLogin(){chk=confirm("確認登出現有使用者？");if(chk==true){window.location.href='Default.aspx?Type=Logout'}}</script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'north',border:false" style="overflow: hidden">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">           
            <tr>
                <td>
                    <div id="pnToolBar">
                        <table border="0" width="100%">
                            <tr>
                                <td id="area_sys" style="height: 25px; width: 10%; margin: 0px auto;" valign="middle"
                                    align="center">
                                    <!--<span id="btn_HeadHide" onClick="fnHead(-1)" style="color:#FFFFFF; cursor: pointer;" ><asp:Label ID="lbltitleH" runat=server Font-Bold=false ForeColor="#c102a8">H</asp:Label></span>
						            <span id="btn_HeadShow" onClick="fnHead(0)" style="color:#FFFFFF; cursor: pointer; display:none;"><asp:Label ID="lbltitleS" runat=server Font-Bold=false ForeColor="#c102a8">S</asp:Label></span>-->
                                    <span id="tab104" style="display: none">
                                        <img id="_mApID104" alt="系統管理" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/ap104.gif"
                                            style="width: 20px; height: 20px; cursor: pointer"><asp:LinkButton ID="linkSystem"
                                                runat="server" Text="系統管理" OnClick="linkSystem_Click"></asp:LinkButton>&nbsp;</span>
                                    <span id="tab183" style="display: none">
                                        <img id="_mApID183" alt="聯絡函" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/apCurrent.gif"
                                            style="width: 20px; height: 20px; cursor: pointer"><asp:LinkButton ID="linkNotifine"
                                                runat="server" Text="聯絡函" OnClick="linkNotification_Click"></asp:LinkButton></span>
                                </td>
                                <td style="height: 25px; vertical-align: middle">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%" class="area_welcome" border="0">
                                        <tr>
                                            <td style="height: 25px; text-align: left; vertical-align: middle; width: 20%">
                                                <span id="btn_MenuHide" class="btn_menuHide_0" onclick="fnMenu(-1)" title="隱藏選單"
                                                    style="color: #7a1a7a">&nbsp;</span> <span id="btn_MenuShow" class="btn_menuShow_0"
                                                        onclick="fnMenu(0)" title="顯示選單" style="display: none; color: #7a1a7a">&nbsp;</span>
                                                <span class="welcome">
                                                    <asp:Label ID="lblwelcome" runat="server" Text="歡迎 " Style="color: #7a1a7a"></asp:Label><span
                                                        class="user01" style="color: #7a1a7a"><asp:LinkButton ID="lbllogin_user" runat="server"
                                                            OnClick="lbllogin_user_Click" Style="color: green"></asp:LinkButton></span><asp:Label
                                                                ID="lblloginsys" runat="server" Text=" 登入本系統！" Style="color: #7a1a7a"></asp:Label></span>
                                            </td>
                                            <td style="vertical-align: middle; width: 10%">
                                                <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/home.gif" width="18px"
                                                    alt="登出系統" />
                                                <asp:LinkButton ID="linkmain" runat="server" OnClick="linkmain_Click" Text="訊息頁"></asp:LinkButton>
                                            </td>
                                            <td style="vertical-align: middle; width: 20%; text-align: center">
                                                <a id="A1" href="javascript:void(0)" onclick="_onPrevious()">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='Pub/EasyLayout/ImgBody/previousover.gif' />Back&nbsp;
                                                </a><a id="A4" href="javascript:void(0)" onclick="_onNext()">
                                                    <asp:Image ID="imgNextOver" runat="server" ImageUrl='Pub/EasyLayout/ImgBody/NextOver.gif' />Forward&nbsp;
                                                </a><a id="ClickRefresh" href="javascript:void(0)">
                                                    <asp:Image ID="imgrefreshover" runat="server" ImageUrl='Pub/EasyLayout/ImgBody/refreshover.gif' />Refesh</a>
                                            </td>
                                            <td style="height: 25px; text-align: right; width: 25%">
                                                <asp:LinkButton ID="linkchange" runat="server" Text="changelang" OnClick="linkchange_Click"></asp:LinkButton>
                                            </td>
                                            <td style="cursor: pointer; height: 25px; text-align: right; width: 20%">
                                                <% if (System.Configuration.ConfigurationManager.AppSettings["SSO"] == "Y")
                                                   { %>
                                                <img style=" text-align: right; vertical-align: middle"
                                                    src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/logout-sso.png"
                                                    alt="登出系統" />
                                                <asp:LoginStatus ID="loginStatus" runat="server" LogoutAction="Redirect" LogoutText="Logout"
                                                    LogoutPageUrl="~/Default.aspx" Height="18px" OnLoggedOut="LoginStatus_LoggedOut" />
                                                <% }
                                                   else
                                                   {%>
                                                        <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/logout_0.gif" onclick="funReLogin();"
                                                        alt="登出系統" />
                                                <% } %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div data-options="region:'west',split:true,title:' <%= System.Configuration.ConfigurationSettings.AppSettings["AppName"].ToString()%>'" style="width: 230px;"
        id="pnLeftContent">
        <div>
            <asp:Literal ID="ltrLeftMenu" runat="server"></asp:Literal>
        </div>
    </div>
    <div data-options="region:'center'">
        <%--fit="true" fullscreen layout content tab--%>
        <div id="TabLayout" class="easyui-tabs" style="width: 100%; height: 100%" fit="true">
        </div>
    </div>
    <div data-options="region:'south',border:false">
        <div id="pnFooter" align="left">
            <div style="width: 100%; padding-top: 5px">
                <font face="新細明體">線上人數：<%=Application["OnlineCount"]%>&nbsp;&nbsp;
                    <% if (System.Configuration.ConfigurationSettings.AppSettings["superAdminEmail"].ToString().Trim().ToLower() == Session["UserAccount"].ToString().Trim().ToLower())
                       { %>
                    <input type="button" class="button" value="OnlineUser" onclick="Hello()" />
                    <% } %>
                </font>&nbsp; <a id="lnkContact" href="mailto:<%=System.Configuration.ConfigurationSettings.AppSettings["System-Email"]%>?subject=系統問題反應"
                    class="A1">
                    <img src="<%= ResolveClientUrl("~/")%>Images/email2.gif" border="0" name="Image4"
                        alt="連絡我們">有問題，請反應，Thanks！</a>
            </div>
        </div>
    </div>
    <script>
        function getCurFrame() {
            // get the selected tab panel and its tab object
            var pp = $('#TabLayout').tabs('getSelected');
            //var $fCurrent = pp.panel('options').tab;    // the corresponding tab object   
            var $fCurrent = pp.find("iframe");
            return $fCurrent;
        }
        function _onPrevious() {

            $fCurrent = getCurFrame();
            $fCurrent[0].contentWindow.history.back();
        }
        function _onNext() {
            $fCurrent = getCurFrame();
            $fCurrent[0].contentWindow.history.forward();
        }



        $("#ClickRefresh").click(function () {
            $fCurrent = getCurFrame();
            $fCurrent.attr("src", $fCurrent.attr("src"));
        });
    </script>
    </form>
</body>
</html>
