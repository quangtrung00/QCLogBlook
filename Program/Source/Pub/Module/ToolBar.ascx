<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ToolBar.ascx.cs" Inherits="Pub_Module_ToolBar" %>
<script>
    function getCurFrame() {

        return $("#" + tab_id);
    }
    function _onPrevious() {

        $fCurrent = getCurFrame();
        $fCurrent[0].contentWindow.history.back();
    }
    function _onNext() {
        $fCurrent = getCurFrame();
        $fCurrent[0].contentWindow.history.forward();
    }

    function _onRefesh() {

        var theframe;
        try {
            obj = $(frames[tab_id].window)[0];
        }
        catch (e) { }

        $fCurrent = getCurFrame();
        $fCurrent.attr("src", obj.location.href);
    }

</script>
<table border="0" width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" id="pnWelcome">
                <tr>
                    <td style="padding-left: 5px">
                        <%--<asp:LinkButton ID="lbllogin_user" target="iframeBody" runat="server" Style="color: green"></asp:LinkButton>--%>
                        <asp:Label ID="lblLoginUser" runat="server" Text=""></asp:Label>, welcome to Pcc
                        Ap Home
                    </td>
                    <td align="right" id="area_sys">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="vertical-align: middle">
                                    <asp:Panel ID="pnlAp" runat="server" Visible="false">
                                        <!--<span id="btn_HeadHide" onClick="fnHead(-1)" style="color:#FFFFFF; cursor: pointer;" ><asp:Label ID="lbltitleH" runat=server Font-Bold=false ForeColor="#c102a8">H</asp:Label></span>
						            <span id="btn_HeadShow" onClick="fnHead(0)" style="color:#FFFFFF; cursor: pointer; display:none;"><asp:Label ID="lbltitleS" runat=server Font-Bold=false ForeColor="#c102a8">S</asp:Label></span>-->
                                        <span id="tab104" style="display: none">
                                            <img id="_mApID104" alt="系統管理" src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/ap104.gif"
                                                style="width: 20px; height: 20px">
                                            <asp:LinkButton ID="linkSystem" runat="server" Text="系統管理" ForeColor="White" OnClick="linkSystem_Click"></asp:LinkButton></span>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <span id="tab183" style="display: none">
                                            <img id="_mApID183" alt="聯絡函" src='<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/apCurrent.gif'
                                                style="cursor: pointer">
                                                <asp:LinkButton ID="linkNotifine" runat="server" ForeColor="White" OnClick="linkNotification_Click"
                                                    Text="聯絡函"></asp:LinkButton>
                                            </img>
                                        </span>&nbsp;<span style="width: 2px; color: Yellow; font-size: 12px; vertical-align: middle">/&nbsp;</asp:Panel>
                                </td>
                                <td>
                                   <asp:Label ID="lblOnlineCount" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;
                                    <%  if (System.Configuration.ConfigurationSettings.AppSettings["superAdminEmail"].ToString().Trim().ToLower() == Session["UserAccount"].ToString().Trim().ToLower())
                                        { %>
                                    <input type="button" class="button" value="OnlineUser" onclick="Hello()" />
                                    <% } %>
                                    &nbsp; <a style="display: none" id="lnkContact" href="mailto:<%=System.Configuration.ConfigurationSettings.AppSettings["System-Email"]%>?subject=系統問題反應"
                                        class="A1">
                                        <img src="<%= ResolveClientUrl("~/")%>Images/email2.gif" border="0" name="Image4"
                                            alt="連絡我們">有問題，請反應，Thanks！                                
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="pnToolBar">
        <td style="vertical-align: middle">
            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="width: 70%; text-align: left; vertical-align: middle">
                        <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/ApName.jpg" style="width: 218px;
                            height: 56px" />
                    </td>
                    <td align="right" style="width: 30%; white-space: nowrap; vertical-align: middle">
                        <table cellpadding="5" cellspacing="0" class="area_welcome" border="0">
                            <tr>
                                <%--Language--%>
                                <td>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"
                                        DataTextField="Lang_Nm" DataValueField="Lang_No">
                                    </asp:DropDownList>
                                    <asp:LinkButton ID="linkchange" runat="server" Visible="false" Text="changelang"
                                        ></asp:LinkButton>
                                </td>
                                <td class="splitTools">
                                    <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Split.png" />
                                </td>
                                <%--Back--%>
                                <td>
                                    <a id="A1" href="javascript:void(0)" onclick="_onPrevious()">
                                        <asp:Image ID="Image1" runat="server" ImageUrl='~/Pub/EasyLayout/ImgBody/Icon/ICON-BACK.png' /><br />
                                        Back</a>
                                </td>
                                <td class="splitTools">
                                    <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Split.png" />
                                </td>
                                <%--Next--%>
                                <td>
                                    <a id="A4" href="javascript:void(0)" onclick="_onNext()">
                                        <asp:Image ID="imgNextOver" runat="server" ImageUrl='~/Pub/EasyLayout/ImgBody/Icon/ICON-FORWARD.png' /><br />
                                        Forward</a>
                                </td>
                                <td class="splitTools">
                                    <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Split.png" />
                                </td>
                                <%--Refesh--%>
                                <td>
                                    <a id="ClickRefresh" href="javascript:void(0)" onclick="_onRefesh()">
                                        <asp:Image ID="imgrefreshover" runat="server" ImageUrl='~/Pub/EasyLayout/ImgBody/Icon/ICON-REFESH.png' /><br />
                                        Refesh</a>
                                </td>
                                <td class="splitTools">
                                    <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Split.png" />
                                </td>
                                <%--Home--%>
                                <td>
                                    <asp:LinkButton ID="linkmain" runat="server" OnClick="linkmain_Click" Text=""></asp:LinkButton>
                                </td>
                                <td class="splitTools">
                                    <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Split.png" />
                                </td>
                                <td>
                                    <% if (System.Configuration.ConfigurationManager.AppSettings["SSO"] == "Y")
                                       { %>
                                    <asp:LoginStatus ID="loginStatus" runat="server" LogoutAction="Redirect" LogoutText="Logout"
                                        LogoutPageUrl="~/Default.aspx" OnLoggedOut="LoginStatus_LoggedOut" />
                                    <% }
                                       else
                                       {%>
                                    <a id="" href="javascript:void(0)" onclick="funReLogin()">
                                        <img src="<%= ResolveClientUrl("~/")%>Pub/EasyLayout/ImgBody/Icon/ICON-LOGIN.png"
                                            style="cursor: pointer;" alt="登出系統" /><br />
                                        Logout
                                        <% }%>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td id="pnBottomToolbar">
        </td>
    </tr>
</table>
