using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using PccBsLayerForC;
using System.Configuration;
using PccCommonForC;


public partial class Index : System.Web.UI.Page
{
    private const string MENUXML = "XmlDoc/ApMenu.xml";
    private const string MENUXML_VN = "XmlDoc/ApMenu_VN.xml";

    private Hashtable m_Menu = new Hashtable();
    private int CountUrl;
    string conf_SSO = System.Configuration.ConfigurationManager.AppSettings["SSO"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (conf_SSO == "Y")
        {
            if (!User.Identity.IsAuthenticated)
            {
                try
                {
                    if (Session["CheckSSO"].ToString() == "Y")//khong dang nhap tu SSO
                    {
                        OpenLoginPage();
                    }
                }
                catch{
                }
            }
        }

        if (!IsPostBack)
        {
            if (Session["UserID"] == null) return;
            SetMenu();
        }


        lbllogin_user.Text = Session["UserName"].ToString();

        if (Session["CodePage"].ToString() == "CP950")
            linkchange.Text = "Chuyển sang tiếng Việt?";
        else
            linkchange.Text = "轉換到中文嗎?";
    }



    private void OpenLoginPage()
    {
        string strPage = ConfigurationSettings.AppSettings["myServer"] + ConfigurationSettings.AppSettings["vpath"] + "/default.aspx";
        string casServerLoginUrl = DotNetCasClient.CasAuthentication.CasServerLoginUrl;
        string strUrl = casServerLoginUrl + "?service=" + Server.UrlEncode(strPage) + "&system=" + Server.UrlEncode(DotNetCasClient.CasAuthentication.SystemTitleName);

        Response.Redirect(strUrl);
    }
    protected void LoginStatus_LoggedOut(object sender, EventArgs e)
    {       
        if (conf_SSO == "Y")
        {
            try
            {
                ((Hashtable)Application["OnlineUser"]).Remove(Session["UserName"] + Session.SessionID.ToString());
            }
            catch { }

            DotNetCasClient.CasAuthentication.SingleSignOut();
        }
        else
        {
            Response.Redirect(ResolveUrl("~/Default.aspx?Type=Logout"));
        }

    }


    protected void lbllogin_user_Click(object sender, EventArgs e)
    {
        string strReturn = "";
        strReturn += ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/UpdateUser.aspx?UserID=" + Session["UserID"].ToString() + "&AcctionType=Upd&UserAccount=" + Session["UserAccount"].ToString();//選單的連結網頁
        //strReturn = "UpdateLoginUser.aspx";//選單的連結網頁
        Response.Redirect(strReturn);
    }

    private void SetMenu()
    {
        string strLeftMenu = string.Empty;
        int AreaCount = 1;

        //LoginInfoUser of AP Detail Menu.
        PccCommonForC.PccErrMsg myLabel = new PccCommonForC.PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        string strDetailPageLayer = Session["PageLayer"].ToString();

        if (Request.QueryString["ApID"] != null)
        {
            //APID
            string strApID = Request.Params["ApID"];
            string ApFolder = "", MenuItem = "", MenuName = "";



            PccCommonForC.PccMsg myMsg = new PccCommonForC.PccMsg();
            myMsg.LoadXml(Session["XmlLoginInfo"].ToString());


            if (strApID.Length > 0 && strDetailPageLayer.Length > 0)
            {
                //strDetailPageLayer = strDetailPageLayer.Substring(0,strDetailPageLayer.Length - 3);
                if (myMsg.QueryNodes("Authorize") != null)
                {
                    foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
                    {

                        if (strApID != myMsg.Query("APID", myNode)) continue;

                        //Folder
                        ApFolder = GetApFolder(myMsg.Query("APLink", myNode));

                        if (myMsg.QueryNodes("ApMenu", (XmlElement)myNode) != null)
                        {
                            foreach (XmlNode myDetailNode in myMsg.QueryNodes("ApMenu", (XmlElement)myNode))
                            {
                                if (myMsg.Query("show_mk", myDetailNode).Equals("Y"))
                                {

                                    if (myMsg.Query("MenuLink", myDetailNode).IndexOf(".asmx", 0) > 0)
                                        continue;

                                    if (Session["CodePage"].ToString() == "CP950")
                                    {
                                        SaveDataToHashMenu(myMsg.Query("MenuManage", (XmlElement)myDetailNode), strDetailPageLayer, ApFolder, myMsg.Query("MenuNM", (XmlElement)myDetailNode), myMsg.Query("MenuLink", (XmlElement)myDetailNode));
                                    }
                                    else
                                    {
                                        MenuItem = "M" + strApID + int.Parse(myMsg.Query("MENUID", myDetailNode)).ToString();
                                        MenuName = myLabel.GetErrMsg(MenuItem, "MenuItem");
                                        SaveDataToHashMenu(myMsg.Query("MenuManage", (XmlElement)myDetailNode), strDetailPageLayer, ApFolder, MenuName, myMsg.Query("MenuLink", (XmlElement)myDetailNode));

                                    }

                                    // SaveDataToHashMenu(myMsg.Query("MenuManage", (XmlElement)myDetailNode), strDetailPageLayer, ApFolder, myMsg.Query("MenuNM", (XmlElement)myDetailNode), myMsg.Query("MenuLink", (XmlElement)myDetailNode));
                                }
                            }
                        }
                    }
                }
            }

            strLeftMenu = "<div class='glossymenu'>";
            string strLang = "";
            ArrayList arrMenu = new ArrayList();

            if (Session["CodePage"].ToString() != "CP950")
            {
                strLang = "_VN";
            }

            myMsg = new PccMsg();
            myMsg.Load(Server.MapPath(Session["PageLayer"] + @"XmlDoc\ApMenu" + strLang + ".xml"));

            if (myMsg.QueryNodes("Applications/Application") != null)
            {
                foreach (XmlNode apNode in myMsg.QueryNodes("Applications/Application"))
                {
                    if (myMsg.Query("ApID", apNode) == Request.QueryString["ApID"])
                    {
                        if (myMsg.QueryNodes("ApAreas/Area", apNode) != null)
                        {
                            foreach (XmlNode areaNode in myMsg.QueryNodes("ApAreas/Area", apNode))
                            {
                                string AreaMK = myMsg.Query("AreaMK", areaNode);
                                string AreaName = myMsg.Query("AreaName", areaNode);
                                if (m_Menu.ContainsKey(AreaMK))//xet quyen han thay menu
                                {
                                    strLeftMenu += "<a class='menuitem submenuheader' style='cursor:pointer'><span class='area_item'><img  src='" + ResolveUrl("~/") + "Pub/EasyLayout/ImgBody/group_item.png" + "' border='0'>" + AreaName.ToUpper() + "</span></a>";
                                    strLeftMenu += "<div  class='submenu'>";
                                    strLeftMenu += "<ul>";
                                    //add child menu
                                    strLeftMenu += m_Menu[AreaMK].ToString();
                                    strLeftMenu += "</ul>";
                                    strLeftMenu += "</div>"; //end class=menu_body
                                }
                            }
                        }
                        break;
                    }
                }
            }




            /*
            if (m_Menu.Count > 0)
            {
                AreaCount += m_Menu.Count;
                //strLeftMenu += GetWelcome(strDetailPageLayer, strApID);

                string[] strArrMenu = { "N", "R", "S", "W", "M", "Q" };

                for (int w = 0; w < strArrMenu.Length; w++)
                {
                    if (m_Menu.ContainsKey(strArrMenu[w]))
                    {

                        string area_mk = strArrMenu[w];
                        arrMenu.Add(area_mk);
                        //GroupMenu
                        strLeftMenu += "<a class='menuitem submenuheader' style='cursor:pointer'><img src='" + ResolveUrl("~/") + "images/MenuArea/" + area_mk + "_Open" + strLang + ".gif' border='0' ></a>";
                        strLeftMenu += "<div  class='submenu'>";
                        strLeftMenu += "<ul>";
                        //child menu
                        strLeftMenu += m_Menu[area_mk].ToString();
                        strLeftMenu += "</ul>";
                        strLeftMenu += "</div>"; //end class=menu_body
                    }
                }


                foreach (string strArea in m_Menu.Keys) //cac nhom menu khac
                {
                    bool isExits = false;
                    for (int k = 0; k < arrMenu.Count; k++) //kiem tra co ton tai trong menu da co chua
                    {
                        if (strArea == arrMenu[k].ToString() || strArea == "Y")
                        {
                            isExits = true;
                            break;
                        }
                    }

                    if (!isExits)
                    {
                        string area_mk = strArea;

                        //GroupMenu
                        strLeftMenu += "<a class='menuitem submenuheader' style='cursor:pointer'><img src='" + ResolveUrl("~/") + "images/MenuArea/" + area_mk + "_Open" + strLang + ".gif' border='0' ></a>";
                        strLeftMenu += "<div  class='submenu'>";
                        strLeftMenu += "<ul>";
                        //child menu
                        strLeftMenu += m_Menu[area_mk].ToString();
                        strLeftMenu += "</ul>";
                        strLeftMenu += "</div>"; //end class=menu_body
                    }
                }


                //Khu quan ly
                if (m_Menu.ContainsKey("Y"))
                {
                    string area_mk = "Y";
                    arrMenu.Add(area_mk);
                    //GroupMenu
                    strLeftMenu += "<a class='menuitem submenuheader' style='cursor:pointer'><img src='" + ResolveUrl("~/") + "images/MenuArea/" + area_mk + "_Open" + strLang + ".gif' border='0' ></a>";
                    strLeftMenu += "<div  class='submenu'>";
                    strLeftMenu += "<ul>";
                    //child menu
                    strLeftMenu += m_Menu[area_mk].ToString();
                    strLeftMenu += "</ul>";
                    strLeftMenu += "</div>"; //end class=menu_body
                }



            }
            else //表示沒有選單權限
            {
                //strLeftMenu += GetWelcome(strDetailPageLayer, strApID);
            }*/

            strLeftMenu += "</div>"; //end div  class='glossymenu'
        }


        ltrLeftMenu.Text = strLeftMenu;
    }




    /*private string ConvertAreaName(string area_mk, string ap_id)
    {
        string strReturn = "";
        PccMsg myMsg = new PccMsg();

        if (Session["CodePage"].ToString() == "CP950")
            myMsg.Load(Server.MapPath(Session["PageLayer"] + MENUXML));

        else
            myMsg.Load(Server.MapPath(Session["PageLayer"] + MENUXML_VN));

        if (myMsg.QueryNodes("Applications/Application") != null)
        {
            foreach (XmlNode apNode in myMsg.QueryNodes("Applications/Application"))
            {
                if (myMsg.Query("ApID", apNode) == ap_id)
                {
                    if (myMsg.QueryNodes("ApAreas/Area", apNode) != null)
                    {
                        foreach (XmlNode areaNode in myMsg.QueryNodes("ApAreas/Area", apNode))
                        {
                            if (myMsg.Query("AreaMK", areaNode) == area_mk)
                                return myMsg.Query("AreaName", areaNode);
                        }
                    }
                    break;
                }
            }
        }
        return strReturn;
    }*/
    private void SaveDataToHashMenu(string Area, string strLayer, string ApFolder, string menuNm, string menuLink)
    {
        string strReturn = string.Empty;
        CountUrl = CountUrl + 1;
        if (m_Menu.ContainsKey(Area))
            strReturn = m_Menu[Area].ToString();

        string menuLink1 = "" + strLayer + ApFolder + "/" + menuLink + "";
        //判斷是否為權限管理區
        if (Area.Equals("Y"))
        {
            menuLink1 = "" + strLayer + "SysManager/" + menuLink + "";
        }

        //strReturn += "<a title='" + menuNm + "' href=\"javascript:void(0)\" onclick=\"addTab('" + menuNm + "', '" + menuLink1 + "','" + CountUrl + "','true')\"  ><div class=\"sysMenu02_0\" onMouseOver=\"fn_btnOver(this);\" onMouseOut=\"fn_btnOut(this);\"><nobr>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + menuNm + "</nobr></div></a>";	//選單名稱
        strReturn += "<li class='menu_Link'  mnulink='" + menuLink1 + "' mnuname='" + menuNm + "'  ><img  src='" + ResolveUrl("~/") + "Pub/EasyLayout/ImgBody/child_item.png" + "' border='0'>" + menuNm + "</li>";
        if (m_Menu.ContainsKey(Area))
            m_Menu[Area] = strReturn;
        else
            m_Menu.Add(Area, strReturn);

    }
    private string GetApFolder(string ApLink)
    {
        int pos = ApLink.LastIndexOf("&");
        string strReturn1 = ApLink.Substring(pos + 1);
        string strReturn = strReturn1.Split('=')[1].Trim();
        return strReturn;
    }

    private string GetWelcome(string strLayer, string strApID)
    {
        string strReturn = string.Empty;

        bs_Security mySecurity = new bs_Security(ConfigurationSettings.AppSettings["ConnectionType"], ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationSettings.AppSettings["EventLogPath"]);
        string strCount = "0";
        PccCommonForC.PccMsg myMsg1 = new PccCommonForC.PccMsg();

        if (strApID != null && int.Parse(strApID) > 0)
        {
            //新增這個系統的前置詞
            myMsg1.CreateFirstNode("ap_id", strApID);
            myMsg1.CreateFirstNode("user_id", Session["UserID"].ToString());

            if (((Hashtable)Session["APCounts"]).ContainsKey(strApID))
            {
                strCount = ((Hashtable)Session["APCounts"])[strApID].ToString();
            }
            else
            {
                strCount = mySecurity.DoReturnStr("GetAndUpdateApCounts", myMsg1.GetXmlStr, "");
                ((Hashtable)Session["APCounts"]).Add(strApID, strCount);
            }
        }

        strReturn += "<div id=\"menu01\" class=\"sysMenu01_0\" onmouseover=\"fn_btnOver(this);\" onmouseout=\"fn_btnOut(this);\" onClick=\"fn_switchVisible(div_m00);\"><nobr>" + "歡迎&nbsp;" + Session["UserName"].ToString() + "&nbsp;光臨</nobr></div>"; //第一個分區的圖形或文字
        strReturn += "<div id=\"div_m00\" class=\"sysMenu_div02\" style=\"display:none;\">";
        strReturn += "<div><nobr><a href=\"" + strLayer + "UpdateLoginUser.aspx\" class=\"sysMenu02_0\" target=\"menuFrame\"  onMouseOver=\"fn_btnOver(this);\" onMouseOut=\"fn_btnOut(this);\">個人資料修改</a></nobr></div>";	//選單名稱
        strReturn += "<div><nobr><a href=\"../Pub/Module/ChangeLanguage.aspx?ApID=0\" class=\"sysMenu02_0\" target=\"menuFrame\"  onMouseOver=\"fn_btnOver(this);\" onMouseOut=\"fn_btnOut(this);\">中英文轉換</a></nobr></div>";	//選單名稱
        strReturn += "</div>";

        return strReturn;
    }
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
        //

        if (Session["UserID"] == null)
        {
            Response.Redirect(ResolveUrl("~/") + "Default.aspx?Type=Logout");
        }

        int i, j = 0;
        string strPageLayer = "";
        string LocalPath = PccCommonForC.PccToolFunc.Upper(Server.MapPath("."));
        string stest = Application["EDPNET"].ToString();

        j = LocalPath.IndexOf(PccCommonForC.PccToolFunc.Upper(Application["EDPNET"].ToString()));

        Session["PageLayer"] = ResolveUrl("~/");
        /*
        try
        {
            string s = LocalPath.Substring(j);
            string[] a = LocalPath.Substring(j).Split('\\');
            for (i = 1; i < LocalPath.Substring(j).Split('\\').Length; i++)
            {
                strPageLayer += "../";
            }
            Session["PageLayer"] = strPageLayer;
        }
        catch
        {
            Session["PageLayer"] = "";
        }*/

        // InitializeComponent();
        //base.OnInit(e);
    }


    protected void linkchange_Click(object sender, EventArgs e)
    {
        //Button
        if (Session["CodePage"].ToString() == "CP950")
        {
            Session["CodePage"] = "CP1258";
            linkchange.Text = "轉換到中文嗎?"; //您想要轉換到中文嗎?
        }
        else
        {
            Session["CodePage"] = "CP950";
            linkchange.Text = "Chuyển sang tiếng Việt ?";    //Ban co muon dich sang tieng Viet ?
        }
        SetMenu();
    }

    protected void linkmain_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.PathAndQuery);
    }

    protected void linkSystem_Click(object sender, EventArgs e)
    {
        Response.Redirect("IndexAdmin.aspx?ApID=104");
    }

    protected void linkNotification_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["ApID"]))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Not config ApID!!!')", true);
            return;
        }
        Response.Redirect("Index.aspx?ApID=" + System.Configuration.ConfigurationSettings.AppSettings["ApID"]);
    }

}