using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PccBsLayerForC;
using System.Configuration;
using PccCommonForC;
using System.Collections;
using System.Data;
using PccBsQCLogBlook;


public partial class Menu : System.Web.UI.Page
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
                catch
                {
                }
            }
        }

        if (!IsPostBack)
        {
            if (Session["UserID"] == null) return;
            SetMenu();
        }
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
    private void SetMenu()
    {
        string strLeftMenu = string.Empty;
        string HTML = "";
        int AreaCount = 1;



        if (Request.QueryString["ApID"] != null)
        {
            //LoginInfoUser of AP Detail Menu.



            strLeftMenu = "";
            string strLang = "";
            ArrayList arrMenu = new ArrayList();

            if (Session["CodePage"].ToString() != "CP950")
            {
                strLang = "_VN";
            }


            HTML += GenMenuDetail();

        }

        ltrLeftMenu.Text = HTML;
    }

    private string GenMenuDetail()
    {
        string HTML = "";
        
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("XmlDoc"), Session["CodeLang"].ToString(), "Label", true);
        PccCommonForC.PccMsg myMsg = new PccCommonForC.PccMsg();
        myMsg.LoadXml(Session["XmlLoginInfo"].ToString());

        //APID            
        string ApFolder = "", MenuItem = "", MenuName = "";
        string strApID = Request.Params["ApID"];
        string strDetailPageLayer = Session["PageLayer"].ToString();
        int index = 0;
        int ColsMenu = int.Parse(ConfigurationManager.AppSettings["ColsMenu"]);


        if (strApID.Length > 0 && strDetailPageLayer.Length > 0)
        {
            //strDetailPageLayer = strDetailPageLayer.Substring(0,strDetailPageLayer.Length - 3);
            if (myMsg.QueryNodes("Authorize") != null)
            {
                DataTable dtUser = GetQCFactDept();


                foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
                {

                    if (strApID != myMsg.Query("APID", myNode)) continue;

                    //Folder
                    ApFolder = GetApFolder(myMsg.Query("APLink", myNode));

                    if (myMsg.QueryNodes("ApMenu", (XmlElement)myNode) != null)
                    {
                        HTML += "<table border='0' cellpadding='5' cellspacing='0' style='margin:0px auto;' class='CoverMenu'>";
                        foreach (XmlNode myDetailNode in myMsg.QueryNodes("ApMenu", (XmlElement)myNode))
                        {
                            if (myMsg.Query("show_mk", myDetailNode).Equals("Y"))
                            {

                                if (myMsg.Query("MenuLink", myDetailNode).IndexOf(".asmx", 0) > 0)
                                    continue;


                                MenuName = myMsg.Query("MenuNM", (XmlElement)myDetailNode);
                                string MenuID = myMsg.Query("MENUID", myDetailNode);
                                string Type_Check="";
                                if (MenuID == "2986") Type_Check = "1"; //QC cố định
                                else if (MenuID == "2987" || MenuID == "2988") Type_Check = "2"; //QC tuần tra

                                if (Type_Check.Length > 0) //xét quyền hạn type check
                                {
                                    bool CheckQC = CheckAutQC(dtUser, Type_Check);
                                    if (!CheckQC) continue;
                                }


                                string Area = myMsg.Query("MenuManage", (XmlElement)myDetailNode);
                                string MenuURL = ResolveUrl("~/Images/MenuArea/MenuItem/" + MenuID + ".png");


                                string MenuLink = myMsg.Query("MenuLink", (XmlElement)myDetailNode);
                                if (Area.Equals("Y"))
                                {
                                    MenuLink = strDetailPageLayer + "SysManager/" + MenuLink + "";
                                    MenuURL = ResolveUrl("~/Images/MenuArea/MenuItem/Default.png");
                                }

                                if (Session["CodeLang"].ToString().ToUpper() != "TC")
                                {
                                    MenuItem = "M" + strApID + int.Parse(myMsg.Query("MENUID", myDetailNode)).ToString();
                                    MenuName = myLabel.GetErrMsg(MenuItem, "MenuItem");
                                }


                                if (index % (ColsMenu) == 0)
                                {
                                    if (index != 0)
                                    {
                                        HTML += "</tr>";
                                    }
                                    HTML += "<tr>";
                                }

                                /*HTML += "<td style='text-align:center'>";
                                HTML += "<a href='" + MenuLink + "'><img src='" + MenuURL + "' border='0' width='60px' height='60px'></a>";
                                HTML += "</td>";*/
                                HTML += "<td >";
                                HTML += "<a href='" + MenuLink + "'><div>";
                                HTML += "<img src='" + MenuURL + "' border='0' width='60px' height='60px'><br>";
                                HTML += MenuName + "";
                                HTML += "</div></a>";
                                HTML += "</td>";

                                index++;



                            }
                        }
                        HTML += "</tr></table>";

                    }
                }

            }
        }

        return HTML;
    }

    private DataTable GetQCFactDept()
    {
        //lấy fact_no
        string xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());

        bs_BasicData mybs = new bs_BasicData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("fact_no", xmlFact_no);
        DataSet dsReturn = mybs.DoReturnDataSet("PRO_GET_QCFACTDEPT", myMsg.GetXmlStr, "");
        return dsReturn.Tables[0];
    }

    private bool CheckAutQC(DataTable dt,string Type_Check)
    {
        try
        {
            DataRow []dr = dt.Select("Type_Check in('0','" + Type_Check + "')");
            if (dr.Length > 0)
                return true;
        }
        catch { }
        return false;
    }

    private void SaveDataToHashMenu(string Area, string strLayer, string ApFolder, string menuNm, string menuLink)
    {
        string strReturn = string.Empty;
        CountUrl = CountUrl + 1;
        if (m_Menu.ContainsKey(Area))
            strReturn = m_Menu[Area].ToString();

        string menuLink1 = Session["PageLayer"] + menuLink + "";
        //判斷是否為權限管理區
        if (Area.Equals("Y"))
        {
            menuLink1 = "" + strLayer + "SysManager/" + menuLink + "";
        }
        string HTML = "";

        strReturn += "<a title='" + menuNm + "' href='" + menuLink1 + "'  >" + menuNm + "</a><br>";


        //strReturn += "<li class='menu_Link'  mnulink='" + menuLink1 + "' mnuname='" + menuNm + "'  ><img  src='" + ResolveUrl("~/") + "Pub/EasyLayout/ImgBody/child_item.png" + "' border='0'>" + menuNm + "</li>";
        if (m_Menu.ContainsKey(Area))
            m_Menu[Area] = HTML;
        else
            m_Menu.Add(Area, HTML);

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
}