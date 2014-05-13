using System;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using PccBsLayerForC;
using PccBsQCLogBlook;
using PccDbLayerForC;
using DotNetCasClient;
using System.Net;
using System.Web.Mail;
public partial class _Default : System.Web.UI.Page
{
    #region "Page_Load與Button事件"
    private void Page_Load(object sender, System.EventArgs e)
    {
        // 將使用者程式碼置於此以初始化網頁
        if (!IsPostBack)
        {
            if (Request.Params["Type"] == "Logout")
            {
                int count;
                //count = int.Parse(Application["OnlineCount"].ToString()) - 1;
                //利用hashtable的數目來當做其Count
                count = ((Hashtable)Application["OnlineUser"]).Count;
                Application["OnlineCount"] = count.ToString();
                //刪除線上的使用者
                if (Session["UserName"] != null)
                {
                    try
                    {
                        ((Hashtable)Application["OnlineUser"]).Remove(Session["UserName"] + Session.SessionID.ToString());
                    }
                    catch { }
                }

                if (Request.Params["Type2"] == "Close")
                {
                    RegisterClientScriptBlock("New", "<script language=javascript>window.close();</script>");
                    return;
                }
            }
            Hashtable myHT = new Hashtable();

            Session.Clear();
            Session["UserName"] = "";
            Session["XmlLoginInfo"] = "";
            Session["APCounts"] = myHT;
            Session["UserIDAndName"] = Request.Params["REMOTE_ADDR"];
            Session["CodePage"] = Application["CodePage"];

            if (Request.Params["Upd_id"] != null && Request.Params["Upd_id"] != "")
            {
                bs_Security mySecurity = new bs_Security(ConfigurationSettings.AppSettings["ConnectionType"], ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationSettings.AppSettings["EventLogPath"]);
                PccMsg myMsg = new PccMsg();
                string strReturn = "";
                strReturn = mySecurity.DoReturnStr("GetUserByUserID", "", Request.Params["Upd_id"]);

                try
                {
                    myMsg.LoadXml(strReturn);
                    if (myMsg.Query("Exist") == "Y")
                    {
                        txtUserName.Text = myMsg.Query("UserName");
                        txtPassword.Text = myMsg.Query("Password");
                        ImageClickEventArgs e1 = new ImageClickEventArgs(1, 2);
                        cmdLogin_Click1(cmdLogin, e1);
                    }
                }
                catch (Exception ex)
                {
                    lblOutput.Text = ex.Message;
                }
            } //end if check upd_id

        } //end if ispostback
        GetContact(); //ManagerContact
            


        //20140225 MinhTan : Login SSO server
        #region SSO
        string conf_SSO=System.Configuration.ConfigurationManager.AppSettings["SSO"];
        string r_Admin = Request.QueryString["Admin"];
        Session["CheckSSO"] = "Y";
        if (conf_SSO == "Y" && string.IsNullOrEmpty(r_Admin))
        {             
            if (!User.Identity.IsAuthenticated)
            {
                OpenLoginPage();
            }
            else
            {
                CheckLoginSSO();
            }
        }
        else
        {
            if (conf_SSO == "N")
            {
                Session["CheckSSO"] = "N";
                tblLogin.Visible = true;
            }
            else
            {
                if (r_Admin == "Y")
                {
                    string IP = Request.UserHostAddress+";";
                    if (System.Configuration.ConfigurationManager.AppSettings["IPAdmin"].IndexOf(IP) != -1)//ton tai IP admin
                    {
                        Session["CheckSSO"] = "N";
                        tblLogin.Visible = true;
                    }
                }
            }
        }
        #endregion
    }

   

    #region SSO
    private void OpenLoginPage()
    {
        string strPage = ConfigurationSettings.AppSettings["myServer"] + ConfigurationSettings.AppSettings["vpath"] + "/default.aspx";

        string casServerLoginUrl = DotNetCasClient.CasAuthentication.CasServerLoginUrl;
        string strUrl = casServerLoginUrl + "?service=" + Server.UrlEncode(strPage) + "&system=" + Server.UrlEncode(DotNetCasClient.CasAuthentication.SystemTitleName);

        Response.Redirect(strUrl);
    }

    private void CheckLoginSSO()
    {
        if (!string.IsNullOrEmpty(User.Identity.Name))
        {
            //System.Collections.Generic.IList<string> attlist;
            string EmailAccount = string.Empty;
            PccMsg myMsg = new PccMsg();
            foreach (CasAuthenticationTicket ticket in CasAuthentication.ServiceTicketManager.GetUserTickets(User.Identity.Name))
            {

                PfsBaseWebService.Service myService = new PfsBaseWebService.Service();
                string strTicketInfo = GetTicketInfomation(ticket);
                Session["GroupEmployee"] = strTicketInfo;
                myMsg.LoadXml(myService.GetEmailAccountByGroupEmployee(strTicketInfo));

                break;
            }

            string strError = "";
            if (myMsg.Query("errmsg") == "")
            {
                EmailAccount = myMsg.Query("emailaccount");

                txtUserName.Text = EmailAccount;
                txtPassword.Text = ConfigurationSettings.AppSettings["adminWebPw"] + DateTime.Now.ToString("MMdd"); ;
                ImageClickEventArgs e1 = new ImageClickEventArgs(1, 2);
                cmdLogin_Click1(cmdLogin, e1);
            }
            else
            {
                strError = myMsg.Query("errmsg");
            }

            SendMailToManager(strError, EmailAccount);
            //Neu khong dang nhap thanh cong thi chua co tai khoan email
            string strMessage = myMsg.Query("errmsg").Replace("'", "\"")
                + " .若有疑問，請聯系系統管理員，謝謝!"
                + " (Neu co van de, vui long lien he nguoi quan ly. Cam on!)";
            //string strUrlApplyAccount = ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/ApplyAccount.aspx?ul=" + ConfigurationSettings.AppSettings["myServer"] + ConfigurationSettings.AppSettings["vpath"] + "/default.aspx&AcctionType=New&User=" + txtUserName.Text + "&vpath=" + ConfigurationSettings.AppSettings["vpath"] + "&sysUser=" + ConfigurationSettings.AppSettings["ApplyManagerName"] + "&sysEmail=" + ConfigurationSettings.AppSettings["ApplyManagerEmail"] + "&emailAccount=" + EmailAccount;
            //Response.Redirect(ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/ApplyAccount.aspx?ul=" + ConfigurationSettings.AppSettings["myServer"] + ConfigurationSettings.AppSettings["vpath"] + "/default.aspx&AcctionType=New&User=" + txtUserName.Text + "&vpath=" + ConfigurationSettings.AppSettings["vpath"] + "&sysUser=" + ConfigurationSettings.AppSettings["ApplyManagerName"] + "&sysEmail=" + ConfigurationSettings.AppSettings["ApplyManagerEmail"]) + "&emailAccount=" + email;

            string JScode = "alert('" + strMessage + "');\n";
            //JScode += "window.location.href='" + strUrlApplyAccount + "';";
            JScode = "<script>" + JScode + "</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "applyaccount", JScode);
        }
    }

    private string GetTicketInfomation(CasAuthenticationTicket ticket)
    {
        PccMsg myMsg = new PccMsg();

        string mail = string.Empty;
        string depart = string.Empty;
        string groupEmployeeID = string.Empty;
        string company = string.Empty;
        string name = string.Empty;
        string empNo = string.Empty;
        string userName = string.Empty;
        string displayName = string.Empty;
        string telephone = string.Empty;
        System.Collections.Generic.IList<string> attlist;

        try
        {
            ticket.Assertion.Attributes.TryGetValue("mail", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
            {
                mail = attlist[0];
            }
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("depart", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                depart = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("groupEmployeeID", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                groupEmployeeID = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("company", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                company = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("name", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                name = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("empNo", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                empNo = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("userName", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                userName = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("displayName", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                displayName = attlist[0];
        }
        catch { }
        try
        {
            ticket.Assertion.Attributes.TryGetValue("telephone", out attlist);
            if (attlist != null && !string.IsNullOrEmpty(attlist[0]))
                telephone = attlist[0];
        }
        catch { }

        myMsg.CreateFirstNode("mail", mail);
        myMsg.CreateFirstNode("depart", depart);
        myMsg.CreateFirstNode("groupEmployeeID", groupEmployeeID);
        myMsg.CreateFirstNode("company", company);
        myMsg.CreateFirstNode("name", name);
        myMsg.CreateFirstNode("empNo", empNo);
        myMsg.CreateFirstNode("userName", userName);
        myMsg.CreateFirstNode("displayName", displayName);
        myMsg.CreateFirstNode("telephone", telephone);

        return myMsg.GetXmlStr;
    }
    void SendMailToManager(string strError, string EmailAccount)
    {
        try
        {
            string title = "Apply Login Error " + ConfigurationSettings.AppSettings["vpath"] + " - " + ConfigurationSettings.AppSettings["area_no"];

            System.Web.Mail.MailMessage mymail = new System.Web.Mail.MailMessage();
            mymail.To = ConfigurationSettings.AppSettings["System-Email"];
            mymail.From = ConfigurationSettings.AppSettings["superAdminEmail"];
            mymail.Subject = title;
            mymail.Body = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=big5'>";
            mymail.Body += "<title>" + title + "</title>";
            mymail.Body += "<style type='text/css'>.a00 {color:#AA0000}";
            mymail.Body += "h3{FONT-SIZE:12pt;line-height:16pt}";
            mymail.Body += "body,td,input{font-family: '細明體';font:10pt;line-height:14pt}</style></head>";
            mymail.Body += "<body bgcolor='#FFFFFF'><font color='#000099'><H3>" + title + "</H3></font><p>";
            mymail.Body += "<font color='#000099'><H4>EmailAccount: " + EmailAccount + "</H4></font><p>";
            mymail.Body += "<font color='#000099'><H4> " + strError + "</H4></font><p>";

            mymail.Body += "</body></html>";
            mymail.BodyFormat = MailFormat.Html;
            mymail.Priority = MailPriority.High;

            SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings["SmtpServer"].ToString();
            SmtpMail.Send(mymail);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    private void cmdLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {

    }

    private void cmdClear_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {

    }



    #endregion

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: 此呼叫為 ASP.NET Web Form 設計工具的必要項。
        //
        //InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// 此為設計工具支援所必需的方法 - 請勿使用程式碼編輯器修改
    /// 這個方法的內容。
    /// </summary>
    /* private void InitializeComponent()
     {
         this.cmdLogin.Click += new System.Web.UI.ImageClickEventHandler(this.cmdLogin_Click);
         this.cmdClear.Click += new System.Web.UI.ImageClickEventHandler(this.cmdClear_Click);
         this.cmdAddAccount.Click += new System.Web.UI.ImageClickEventHandler(this.cmdAddAccount_Click);
         this.dtlContact.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dtlContact_ItemDataBound);
         this.Load += new System.EventHandler(this.Page_Load);

     }*/
    #endregion


    /// <summary>
    /// 這是一個解開Upd Encode的函數，由首頁登入後，即由這個來解開
    /// </summary>
    /// <param name="EncodeStr"></param>
    /// <returns></returns>
    private string DecodeUpdID(string EncodeStr)
    {
        string ReturnStr, FactStr = "";
        int i, d, h;
        string da, ha;
        int[] A1 = { 3, 7, 8, 1, 12, 3, 9, 23, 11, 6 };
        int startIndex, factIndex;
        string OrgUpd_id = "";
        string[] SplitArray;

        d = DateTime.Today.Day;
        h = DateTime.Now.Hour;

        //取回實際的字元數，扣除三的倍數之字元
        for (i = 1; i <= EncodeStr.Length; i++)
        {
            if ((i % 4) != 0)
            {
                FactStr = FactStr + EncodeStr.Substring(i - 1, 1);
            }
        }

        //取得要由那個Index開始取得KeyArray
        startIndex = ((d + h) * 3) % 10;

        for (i = 0; i < FactStr.Length; i++)
        {
            //取得實際要運算的索引
            factIndex = (startIndex + i) % 10;

            //經過反計算後的字元再加上之前的字元
            OrgUpd_id = OrgUpd_id + (char)((int)FactStr.Substring(i, 1).ToCharArray()[0] - A1[factIndex]);
        }

        //取得日期、小時及使用者ID
        SplitArray = OrgUpd_id.Split(':');

        if (SplitArray.Length != 3)
        {
            ReturnStr = "There are not pass.";
            return ReturnStr;
        }

        da = SplitArray[0];
        ha = SplitArray[1];

        if ((Convert.ToInt16(da) != d) || (Convert.ToInt16(ha) != h))
        {
            ReturnStr = "There are not pass1.";
            return ReturnStr;
        }

        ReturnStr = SplitArray[2];

        return ReturnStr;

    }

    private string EncodeUpdID(string upd_id)
    {

        int[] A1 = { 3, 7, 8, 1, 12, 3, 9, 23, 11, 6 };
        int i, d, h, startIndex, factIndex;
        string NetUpd_id, OrgUpd_id;

        d = DateTime.Today.Day;
        h = DateTime.Now.Hour;

        //首先加上日期和小時
        OrgUpd_id = d + ":" + h + ":" + upd_id;
        NetUpd_id = "";


        //取得要由那個Index開始取得KeyArray
        startIndex = ((d + h) * 3) % 10;

        for (i = 0; i < OrgUpd_id.Length; i++)
        {
            //若是三的倍數的字元則多加一個字元
            if (((i % 3) == 0) && (i > 0))
            {
                NetUpd_id = NetUpd_id + (char)(65 + i);
            }

            //取得實際要運算的索引
            factIndex = (startIndex + i) % 10;

            //經過計算後的字元再加上之前的字元
            NetUpd_id = NetUpd_id + (char)((int)OrgUpd_id.Substring(i, 1).ToCharArray()[0] + A1[factIndex]);
        }

        return NetUpd_id;

    }



    private DataTable CreateSecurityTable(string LoginXML)
    {
        DataTable returnDt = CenSecTable();

        PccMsg myMsg = new PccMsg(LoginXML);
        string ap_id = "";
        DataRow drow = null;

        if (myMsg.QueryNodes("Authorize") == null) return returnDt;

        foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
        {
            ap_id = myMsg.Query("APID", myNode);

            if (myMsg.QueryNodes("ApMenu", myNode) == null) continue;

            foreach (XmlNode myDNode in myMsg.QueryNodes("ApMenu", myNode))
            {
                drow = returnDt.NewRow();
                drow["ap_id"] = decimal.Parse(ap_id);
                drow["aspxfile"] = SplitAspxFile(myMsg.Query("MenuLink", myDNode));
                drow["check_mk"] = myMsg.Query("check_mk", myDNode);
                drow["show_mk"] = myMsg.Query("show_mk", myDNode);
                drow["add_mk"] = myMsg.Query("add_mk", myDNode);
                drow["upd_mk"] = myMsg.Query("upd_mk", myDNode);
                drow["del_mk"] = myMsg.Query("del_mk", myDNode);
                drow["rpt_mk"] = myMsg.Query("rpt_mk", myDNode);
                drow["send_mk"] = myMsg.Query("send_mk", myDNode);
                returnDt.Rows.Add(drow);
            }

        }

        return returnDt;
    }

    private string SplitAspxFile(string strUrl)
    {
        string strAspxFile = "";

        int posSlash = strUrl.LastIndexOf("/") + 1;

        strAspxFile = strUrl.Substring(posSlash, strUrl.Length - posSlash);
        strAspxFile = strAspxFile.Split('?')[0];

        return strAspxFile;
    }

    private DataTable CenSecTable()
    {
        DataTable returnDt = new DataTable("Auth");

        returnDt.Columns.Add(new DataColumn("ap_id", System.Type.GetType("System.Decimal")));
        returnDt.Columns.Add(new DataColumn("aspxfile", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("check_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("show_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("add_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("upd_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("del_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("rpt_mk", System.Type.GetType("System.String")));
        returnDt.Columns.Add(new DataColumn("send_mk", System.Type.GetType("System.String")));

        return returnDt;
    }


    #region // by Nickel 20121228
    private void GetContact()
    {
        string area_no = ConfigurationSettings.AppSettings["area_no"];
        string ap_id = ConfigurationSettings.AppSettings["ApID"];
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("area_no", area_no);
        myMsg.CreateFirstNode("ap_id", ap_id);
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["ConnectionType"], ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"]);
        DataSet ds = mybs.DoReturnDataSet("GET_CONTACT_SYSMANAGE", myMsg.GetXmlStr, "");
        DataTable dt = ds.Tables[0];

        dtlContact.DataSource = dt;
        dtlContact.DataBind();
    }
    protected void dtlContact_ItemDataBound1(object sender, DataListItemEventArgs e)
    {
        DataRowView dtrv = (DataRowView)e.Item.DataItem;

        Label lblContact = (Label)e.Item.FindControl("lblContact");
        #region setting
        string[] strLang = { "中文", "越文", "中越文" };
        string Langs = "";
        int Lang = int.Parse(dtrv["language"].ToString());
        if (Lang == 1)
            Langs = strLang[0];
        else if (Lang == 2)
            Langs = strLang[1];
        else if (Lang == 3)
            Langs = strLang[2];

        /*string[] strKind = { "電腦室", "推動組" };
        string kinds = "";
        int kind = int.Parse(dtrv["kind"].ToString());
        if (kind == 1)
            kinds = strKind[0];
        else if (kind == 2)
            kinds = strKind[1];*/

        string kinds = dtrv["kind_nm"].ToString();

        string user_desc = dtrv["user_desc"].ToString();
        if (user_desc.Length <= 2)
            user_desc = "&nbsp;&nbsp;&nbsp;&nbsp;" + user_desc;
        #endregion

        lblContact.Text = dtrv["con_range"].ToString() + " : " + kinds + "聯絡人 - <A id=\"lnkContact\" class='lblContact' href=\"mailto:" + dtrv["email"].ToString() + "?subject=系統問題反應\" title=\""
            + Langs + "\">" + user_desc + "</A> (" + dtrv["ext"].ToString() + ")";
    }
    private void dtlContact_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
    {

    }
    #endregion



    protected void cmdClear_Click1(object sender, ImageClickEventArgs e)
    {
        txtUserName.Text = "";
        txtPassword.Text = "";
    }
    protected void cmdLogin_Click1(object sender, ImageClickEventArgs e)
    {
        PccMsg myMsg = new PccMsg("", "Big5");
        bs_Security mySecurity = new bs_Security(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strXmlReturn;

        myMsg.CreateFirstNode("UserName", txtUserName.Text);
        myMsg.CreateFirstNode("Password", txtPassword.Text);

        myMsg.CreateFirstNode("vpath", ConfigurationManager.AppSettings["vpath"]);
        //因為現在是使用Email登入所以之必須用Email做比較 20050126
        //myMsg.CreateFirstNode("superAdmin",ConfigurationManager.AppSettings["superAdmin"]);
        myMsg.CreateFirstNode("superAdmin", ConfigurationManager.AppSettings["superAdminEmail"]);
        string datenow = "";

        if (DateTime.Today.Month < 10)
        {
            datenow = datenow + "0" + DateTime.Today.Month.ToString();
        }
        else
        {
            datenow = datenow + DateTime.Today.Month.ToString();
        }
        if (DateTime.Today.Day < 10)
        {
            datenow = datenow + "0" + DateTime.Today.Day.ToString();
        }
        else
        {
            datenow = datenow + DateTime.Today.Day.ToString();
        }
        string passadmin = ConfigurationSettings.AppSettings["adminWebPw"] + datenow;
        if (txtPassword.Text.Trim() == passadmin)
        {
            strXmlReturn = GetUserInfo(myMsg.GetXmlStr, "");
        }
        else
        {
            strXmlReturn = mySecurity.DoReturnStr("GetUserInfo", myMsg.GetXmlStr, "");
        }

        myMsg.LoadXml(strXmlReturn);

        if (myMsg.Query("Exist") == "Y")
        {
            Session["XmlLoginInfo"] = strXmlReturn;
            Session["AuthTable"] = CreateSecurityTable(strXmlReturn);
            Session["UserName"] = myMsg.Query("UserDesc");
            Session["UserAccount"] = myMsg.Query("UserName");
            Session["UserID"] = myMsg.Query("UserID");
            Session["UserEMail"] = myMsg.Query("Email");
            Session["UserPWD"] = txtPassword.Text;
            Session["UserIDAndName"] = myMsg.Query("UserID") + "---" + myMsg.Query("UserDesc") + "---" + Request.Params["REMOTE_ADDR"];

            //取得這個使用者的加密後之SessionID 20050707
            PccMsg myTempMsg = new PccMsg();
            myTempMsg.CreateFirstNode("upd_id", myMsg.Query("UserID"));
            myTempMsg.CreateFirstNode("email", myMsg.Query("Email"));
            myTempMsg.CreateFirstNode("user_pass", txtPassword.Text);
            Session["EncodeUpdID"] = Server.UrlEncode(mySecurity.DoReturnStr("NewEncode", myTempMsg.GetXmlStr, string.Empty));

            //新增一個線上使用者
            int count;
            //count = int.Parse(Application["OnlineCount"].ToString()) + 1;
            //利用hashtable的數目來當做其Count
            try
            {
                ((Hashtable)Application["OnlineUser"]).Add(Session["UserName"] + Session.SessionID.ToString(), Request.Params["REMOTE_ADDR"] + "--" + DateTime.Now.ToString());
            }
            catch { }
            count = ((Hashtable)Application["OnlineUser"]).Count;
            Application["OnlineCount"] = count.ToString();

            //修改ap_id=126使其直接進入電子發票之Menu區20050223
          // Response.Redirect("PccApHome.aspx?ApID=249");

          //  Response.Redirect(ResolveUrl("~/Index.aspx?ApID=" + ConfigurationSettings.AppSettings["ApID"] + ""));
            Response.Redirect("Index.aspx?ApID=249");

            //RegisterClientScriptBlock("New", "<script language=javascript>window.showModalDialog('PccApHome.aspx?ApID=0','new','dialogWidth:1024px;dialogHeight:1600px;center=yes;help=no;status=no;resizable=no');</script>");
        }
        else
        {
            lblOutput.Text = myMsg.Query("Return");

            if (myMsg.Query("Return").Equals("F"))
            {
                Response.Redirect(ConfigurationManager.AppSettings["PFSBaseWeb"] + "usermanage/updatepwd.aspx?ul=" + ConfigurationManager.AppSettings["myServer"] + ConfigurationManager.AppSettings["vpath"] + "/default.aspx&User=" + txtUserName.Text + "&UserID=" + myMsg.Query("UserID"));
            }
            else
            {
                lblOutput.Text = myMsg.Query("Return");
            }
        }
    }
    private string GetUserInfo(string strXML, string strOther)
    {
        string xmlStr = "";
        PccMsg msg = new PccMsg("", "Big5");
        try
        {
            msg.LoadXml(strXML);
            string userName = msg.Query("UserName");
            string password = msg.Query("Password");
            string vpath = msg.Query("vpath");
            string superAdmin = msg.Query("superAdmin");
            xmlStr = new db_Security(ConfigurationSettings.AppSettings["ConnectionType"], ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"]).GetUserInfo(userName, password, vpath, superAdmin);

        }
        catch (Exception exception)
        {
            msg.ClearContext();
            msg.CreateFirstNode("Exist", "N");
            msg.CreateFirstNode("Return", exception.Message);
            xmlStr = msg.GetXmlStr;
        }
        return xmlStr;
    }
    protected void cmdAddAccount_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect(ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/ApplyAccount.aspx?ul=" + ConfigurationSettings.AppSettings["myServer"] + ConfigurationSettings.AppSettings["vpath"] + "/default.aspx&AcctionType=New&User=" + txtUserName.Text + "&vpath=" + ConfigurationSettings.AppSettings["vpath"] + "&sysUser=" + ConfigurationSettings.AppSettings["ApplyManagerName"] + "&sysEmail=" + ConfigurationSettings.AppSettings["ApplyManagerEmail"]);

    }


}
