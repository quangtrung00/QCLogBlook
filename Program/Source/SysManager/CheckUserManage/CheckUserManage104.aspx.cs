using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using PccBsSystemForC;
using System.Net.Mail;



public partial class SysManager_CheckUserManage_CheckUserManage104 : System.Web.UI.Page
{
    private const string CHECKUSERMANAGE = "CheckUserManage104.aspx";
    private string m_apid = string.Empty;

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        m_apid = Request.QueryString["ApID"].ToString();
        PccMsg myMsg = new PccMsg();

        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);

            //設定之前User鍵入的查詢資料。 2004/3/8
            if (Request.Params["QueryCondition"] != null && Request.Params["QueryCondition"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["QueryCondition"].ToString());
                user_desc.Text = myMsg.Query("QueryCondition/user_desc");
            }

            GenMasterTable(ref myLabel);

            if (Request.Params["Method"] != null && Request.Params["Method"].ToString() != "")
            {
                try
                {
                    myMsg.LoadXml(Request.Params["Method"].ToString());
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('Load XML Fail');</script>");
                    return;
                }
                switch (myMsg.Query("Method"))
                {
                    case "MasterDelFunc":
                        MasterDelFunc(myMsg.Query("Key"), myMsg.Query("KeyOther"), ref myLabel);
                        break;
                }
            }

        }
        else
        {
            if (txtReturn.Value != "")
            {
                try
                {
                    myMsg.LoadXml(txtReturn.Value);
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('Load XML Fail');</script>");
                    return;
                }

                switch (myMsg.Query("Method"))
                {
                    case "AddCheckUser":
                        AddCheckUser();
                        break;
                }


            }

        }

    }

    private void AddCheckUser()
    {
        string[] tempAsk_id, tempGroup_id;
        string ask_str = Request.Params["CheckAsk"].ToString();
        tempAsk_id = ask_str.Split(',');
        tempGroup_id = Request.Params["ddlGroup"].ToString().Split(',');

        string check_str = "";
        int i = 0;

        for (i = 0; i < tempAsk_id.Length; i++)
        {
            check_str += "Y,";
        }

        check_str = check_str.Substring(0, check_str.Length - 1);

        string group_str = "";
        for (i = 0; i < tempGroup_id.Length; i++)
        {
            if (tempGroup_id[i] != "0")
            {
                group_str += tempGroup_id[i] + ",";
            }
        }

        group_str = group_str.Substring(0, group_str.Length - 1);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ask_str", ask_str);
        myMsg.CreateFirstNode("check_str", check_str);
        myMsg.CreateFirstNode("group_str", group_str);
        myMsg.CreateFirstNode("check_id", Session["UserID"].ToString());
        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs.DoReturnStr("AuditUser", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        DataTable dt;

        if (myMsg.Query("returnValue") == "0")
        {
            //要送信給每位申請者 20040418
            for (i = 0; i < tempAsk_id.Length; i++)
            {
                dt = mybs.DoReturnDataSet("GetAskByAskID", "", tempAsk_id[i]).Tables["Ask"];
                if (!SendMailToManager(dt.Rows[0]["ap_id"].ToString(), dt.Rows[0]["ap_name"].ToString(), dt.Rows[0]["user_desc"].ToString(), dt.Rows[0]["email"].ToString()))
                {
                    return;
                }

            }
            Response.Redirect(CHECKUSERMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
        }

    }


    private bool SendMailToManager(string ap_id, string ap_name, string apply_name, string apply_email)
    {
        try
        {
            string title = "申請使用-" + ap_name + "-審核通過";
            string href = ConfigurationManager.AppSettings["myServer"].ToString() + ConfigurationManager.AppSettings["vpath"].ToString() + "/default.aspx";

            MailMessage mymail = new MailMessage();
            MailAddress myForm = new MailAddress(ConfigurationManager.AppSettings[ap_id + "-Email"].ToString());
            
            mymail.From = myForm;
            foreach (string myTo in apply_email.Split(';'))
            {
                mymail.To.Add(myTo);
            }

            mymail.Subject = title;
            mymail.Body = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=big5'>";
            mymail.Body += "<title>" + title + "</title>";
            mymail.Body += "<style type='text/css'>.a00 {color:#AA0000}";
            mymail.Body += "h3{FONT-SIZE:12pt;line-height:16pt}";
            mymail.Body += "body,td,input{font-family: '細明體';font:9pt;line-height:14pt}</style></head>";
            mymail.Body += "<body bgcolor='#FFFFFF'><font color='#000099'><H3>" + title + "</H3></font><p>";
            mymail.Body += "<font color='#000000'>「" + apply_name + "」您好！";
            mymail.Body += "管理者已對您所提出的帳號申請通過審核，請<A href=" + href + ">由此進入</A>使用本系統";
            mymail.Body += "</body></html>";
            mymail.IsBodyHtml = true;
            mymail.Priority = MailPriority.High;


            SmtpClient mySmtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString());
            mySmtp.Send(mymail);
            
            return true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = apply_name + "---" + ex.Message;
            return false;
        }

    }

    private void MasterDelFunc(string strKey, string strKeyOther, ref PccErrMsg myLabel)
    {
        plDelete.Visible = true;
        plMain.Visible = false;
        lblDelMsg.Text = "請問是否要駁回--<b>" + strKeyOther + "</b>--所提出的申請？";
        btnDelOK.Text = myLabel.GetErrMsg("btnOK");
        btnDelCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = true;
    }

    protected void btnDelCancel_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;
        GenMasterTable();
    }

    protected void btnDelOK_Click(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg(Request.Params["Method"].ToString());
        string strKey = myMsg.Query("Key");
        string strXML = "<PccMsg><user_id>" + Session["UserID"].ToString() + "</user_id><ask_id>" + strKey + "</ask_id></PccMsg>";

        PccBsSystemForC.bs_UserManager mybs = new PccBsSystemForC.bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        DataTable dt;

        string strReturn = mybs.DoReturnStr("DeleteAskUser", strXML, "");
        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            //要送信給被駁回的申請者 20040418
            dt = mybs.DoReturnDataSet("GetAskByAskID", "", strKey).Tables["Ask"];

            if (!SendMailToApply(dt.Rows[0]["ap_id"].ToString(), dt.Rows[0]["ap_name"].ToString(), dt.Rows[0]["user_desc"].ToString(), dt.Rows[0]["email"].ToString()))
            {
                return;
            }
            Response.Redirect(CHECKUSERMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
        }

    }

    private bool SendMailToApply(string ap_id, string ap_name, string apply_name, string apply_email)
    {
        try
        {
            string title = "申請使用-" + ap_name + "-被駁回";
            string href = ConfigurationManager.AppSettings["myServer"].ToString() + ConfigurationManager.AppSettings["vpath"].ToString() + "/default.aspx";

            MailMessage mymail = new MailMessage();
            MailAddress myForm = new MailAddress(ConfigurationManager.AppSettings[ap_id + "-Email"].ToString());
            
            mymail.From = myForm;
            foreach (string myTo in apply_email.Split(';'))
            {
                mymail.To.Add(myTo);
            }
            
            mymail.Subject = title;

            mymail.Body = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=big5'>";
            mymail.Body += "<title>" + title + "</title>";
            mymail.Body += "<style type='text/css'>.a00 {color:#AA0000}";
            mymail.Body += "h3{FONT-SIZE:12pt;line-height:16pt}";
            mymail.Body += "body,td,input{font-family: '細明體';font:9pt;line-height:14pt}</style></head>";
            mymail.Body += "<body bgcolor='#FFFFFF'><font color='#000099'><H3>" + title + "</H3></font><p>";
            mymail.Body += "<font color='#000000'>「" + apply_name + "」您好！";
            mymail.Body += "管理者已駁回您所提出的帳號申請，請查明原因後再申請本系統之使用權！";
            mymail.Body += "</body></html>";
            mymail.IsBodyHtml = true;
            mymail.Priority = MailPriority.High;


            SmtpClient mySmtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString());
            mySmtp.Send(mymail);

            return true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = apply_name + "---" + ex.Message;
            return false;
        }

    }

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }


    #endregion

    #region "Check Data if Null"

    private string CheckDBNull(object oFieldData)
    {
        if (Convert.IsDBNull(oFieldData))
            return string.Empty;
        else
            return oFieldData.ToString().Trim();
    }

    private string CheckQueryString(string strName)
    {
        if (Request.QueryString[strName] == null)
            return "";
        else
            return Request.QueryString[strName].ToString();
    }
    private string CheckForm(string strName)
    {
        if (Request.Form[strName] == null)
            return "";
        else
            return Request.Form[strName].ToString();
    }

    #endregion

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        //lblTitle.Text = "審核使用者";
        lbluser_desc.Text = "使用者名稱";
        btnAddUser.Value = "加入使用者";
    }

    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (tblCheckUser.Rows.Count > 0)
            tblCheckUser.Rows.Clear();

        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTable()
    {
        if (tblCheckUser.Rows.Count > 0)
            tblCheckUser.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTableHader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("cssGridHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //編號
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0007", "SysManager/ApManager"), 5);
        myRow.AddTextCell("姓名", 10);
        myRow.AddTextCell("電子郵件帳號", 33);
        myRow.AddTextCell("廠別", 22);
        //myRow.AddTextCell("Email",25);
        myRow.AddTextCell("分機", 5);
        myRow.AddTextCell("群組", 15);
        myRow.AddTextCell("核准", 5);
        myRow.AddTextCell("駁回", 5);

        tblCheckUser.CssClass = "cssGridTable";
        tblCheckUser.Width = Unit.Percentage(100);
        tblCheckUser.HorizontalAlign = HorizontalAlign.Center;
        tblCheckUser.CellPadding = 2;
        tblCheckUser.CellSpacing = 1;

        tblCheckUser.Rows.Add(myRow.Row);
    }

    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("user_desc", user_desc.Text);
        myMsg.UpdateNode();
        return myMsg.GetXmlStr;
    }

    private string GetMethod(string strMethod, string Key, string KeyOther, DataRow myRow)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Method", strMethod);
        myMsg.CreateFirstNode("Key", myRow[Key].ToString());
        myMsg.CreateFirstNode("KeyOther", myRow[KeyOther].ToString());
        return myMsg.GetXmlStr;
    }

    #endregion

    #region "編號及上下頁的程式碼"

    protected void OnPageClick(object source, EventArgs e)
    {
        GenMasterTable();
    }

    #endregion

    #region "設定主Table的資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccBsSystemForC.bs_UserManager mybs = new PccBsSystemForC.bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("ap_id", m_apid);
        myMsg.CreateFirstNode("user_desc", user_desc.Text);
        //string strXML = myMsg.GetXmlStr;

        GetMenuAuth myAuth = new GetMenuAuth();

        //判斷是否要利用事業群來分設權限 20041118
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("order", "");

        if (ConfigurationManager.AppSettings[m_apid + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_apid + "-FactByGroup"].ToString() == "Y")
        {
            //判斷此使用者是否可以分配不同的事業群權限
            if (!myAuth.IsReportAuth())
            {
                //表示他必須區分事業群
                myMsg.CreateFirstNode("GroupFilter", "Y");
            }
            else
            {
                myMsg.CreateFirstNode("GroupFilter", "N");
            }
        }
        else
        {
            myMsg.CreateFirstNode("GroupFilter", "N");
        }

        if (ConfigurationManager.AppSettings[m_apid + "-FactFilter"] != null && ConfigurationManager.AppSettings[m_apid + "-FactFilter"].ToString() == "Y")
        {
            //表示他必須區分廠管理 20041118 注意要區別廠管理的先決條件是要區分事業群
            myMsg.CreateFirstNode("FactFilter", "Y");
        }

        DataSet myAskDs = mybs.DoReturnDataSet("GetAskByApID", myMsg.GetXmlStr, "");

        PccMsg myTempMsg = new PccMsg();

        DataTable myAskData = myAskDs.Tables["Ask"];

        if (myAskData != null && myAskData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";

            PccRow myRow;
            //取出資料總筆數
            PageControl1.TotalSize = myAskDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myAskData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "cssGridRowAlternating"; else MasterStyle = "cssGridRow";
                myRow = new PccRow();
                myRow.SetRowCss(MasterStyle);
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //姓名
                myRow.AddTextCell(myMasterRow["user_desc"].ToString(), 10);
                //電子郵件帳號
                myRow.AddTextCell(myMasterRow["email"].ToString(), 33);
                //廠別
                myRow.AddTextCell(myMasterRow["fact_nm"].ToString(), 22);
                //Email
                //myRow.AddTextCell(myMasterRow["email"].ToString(),25);
                //分機
                myRow.AddTextCell(myMasterRow["ext"].ToString(), 5);
                //群組
                myRow.SetDefaultCellData("", HorizontalAlign.Center, 0, 0);
                myRow.AddControl(GetGroup(myMasterRow["ask_id"].ToString()), 15);
                //核準
                myRow.AddTextCell(GetCheck(myMasterRow["ask_id"].ToString()), 5);
                //駁回
                myTempMsg.LoadXml();
                myTempMsg.CreateNode("LinkButton");
                myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                myTempMsg.AddToNode("ToolTip", "駁回此使用者之申請");
                myTempMsg.AddToNode("href", CHECKUSERMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                myTempMsg.AddToNode("Method", GetMethod("MasterDelFunc", "ask_id", "user_desc", myMasterRow));
                myTempMsg.UpdateNode();
                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 5);

                tblCheckUser.Rows.Add(myRow.Row);

                MasterCount += 1;
            } // end of foreach datarow
        } // end of if table count is 0
        else
        {
            PageControl1.TotalSize = "0";
            PageControl1.BuildPager();
        }


    }

    private DropDownList GetGroup(string ask_id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_apid);
        myMsg.CreateFirstNode("group_nm", "");

        GetMenuAuth myAuth = new GetMenuAuth();

        //判斷是否要利用事業群來分設權限 20041118
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());

        if (ConfigurationManager.AppSettings[m_apid + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_apid + "-FactByGroup"].ToString() == "Y")
        {
            //判斷此使用者是否可以分配不同的事業群權限
            if (!myAuth.IsReportAuth())
            {
                //表示他必須區分事業群
                myMsg.CreateFirstNode("GroupFilter", "Y");
            }
            else
            {
                myMsg.CreateFirstNode("GroupFilter", "N");
            }
        }
        else
        {
            myMsg.CreateFirstNode("GroupFilter", "N");
        }

        bs_GroupManage mybsGroup = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        DataTable dt = mybsGroup.DoReturnDataSet("GetAllGroupByApID", myMsg.GetXmlStr, "").Tables["Group"];

        DataRow myRow = dt.NewRow();
        myRow["group_id"] = 0;
        myRow["group_nm"] = "--請選擇--";
        myRow["group_type"] = "0";
        dt.Rows.InsertAt(myRow, 0);

        DropDownList myddlGroup = new DropDownList();

        myddlGroup.ID = "ddlGroup";
        myddlGroup.Attributes.Add("onChange", "GroupChange(this)");
        //myddlGroup.Enabled = false;

        myddlGroup.DataSource = dt.DefaultView;
        myddlGroup.DataTextField = "group_nm";
        myddlGroup.DataValueField = "group_id";
        myddlGroup.DataBind();

        return myddlGroup;

    }

    private string GetCheck(string ask_id)
    {
        return "<input type=checkbox name=CheckAsk onClick=CheckChange(this) value=" + ask_id + " />";
    }


    #endregion
}
