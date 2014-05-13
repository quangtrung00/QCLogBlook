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
using System.Drawing;
using System.Drawing.Imaging;

public partial class Pub_Module_ApplyAccount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "../../XmlDoc"),Session["CodePage"].ToString() ,"Label");
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath("../../XmlDoc"), Session["CodePage"].ToString(), "Label");
            bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
            SetLabel(ref myLabel);
            BindFactData(ref myLabel, ref mybs);
            //SetddlDept(ref myLabel,ref mybs);
            SetddlApplication(ref myLabel, ref mybs);
            btnApply.Enabled = false;
            if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "Update")
            {
                btnReLogin.Text = "回上頁";
                GetUserData(ref myLabel, ref mybs);
            }
        }

    }

    #region "設定此頁面之基本資料"

    private void BindFactData(ref PccCommonForC.PccErrMsg myLabel, ref PccBsSystemForC.bs_UserManager mybs)
    {

        DataSet ds;
        DataTable dt;
        DataRow myRow;
        ds = mybs.DoReturnDataSet("GetFactDataBySecurity", "", "");
        dt = ds.Tables["Fact"];

        myRow = dt.NewRow();
        myRow["fact_id"] = 0;
        myRow["fact_nm"] = "bbb";
        myRow["fact_desc"] = myLabel.GetErrMsg("SelectPlease");
        dt.Rows.InsertAt(myRow, 0);

        ddlfact_id.DataSource = dt.DefaultView;
        ddlfact_id.DataTextField = "fact_desc";
        ddlfact_id.DataValueField = "fact_id";

        ddlfact_id.DataBind();
    }


    private void GetUserData(ref PccCommonForC.PccErrMsg myLabel, ref PccBsSystemForC.bs_UserManager mybs)
    {
        PccCommonForC.PccMsg myMsg = new PccCommonForC.PccMsg();
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("ap_id", ddlApplcation.SelectedItem.Value);
        string strXML = myMsg.GetXmlStr;

        try
        {
            myMsg.LoadXml(mybs.DoReturnStr("GetUserData", strXML, ""));
            txtuser_desc.Text = myMsg.Query("user_desc");
            txtuser_nm.Text = myMsg.Query("email");
            //password always is 'password'
            txtusr_pas.Attributes["value"] = myMsg.Query("usr_pas");
            txtReusr_pas.Attributes["value"] = myMsg.Query("usr_pas");

            //ddldept_id.Items.FindByValue(myMsg.Query("dept_id")).Selected = true;
            ddlfact_id.Items.FindByValue(myMsg.Query("fact_id")).Selected = true;

            txtemp_no.Text = myMsg.Query("emp_no");
            txtext.Text = myMsg.Query("ext");

            SetTextColor();

        }
        catch
        {
            lblMsg.Text = myLabel.GetErrMsg("msgLoadDataError");
            btnApply.Enabled = false;
        }
    }

    private void SetTextColor()
    {
        txtuser_desc.ReadOnly = true;
        txtuser_desc.BackColor = Color.PowderBlue;
        txtuser_nm.ReadOnly = true;
        txtuser_nm.BackColor = Color.PowderBlue;
        txtusr_pas.ReadOnly = true;
        txtusr_pas.BackColor = Color.PowderBlue;
        txtReusr_pas.ReadOnly = true;
        txtReusr_pas.BackColor = Color.PowderBlue;
        ddlfact_id.Enabled = false;
        ddlfact_id.BackColor = Color.PowderBlue;
        //ddldept_id.Enabled = false;
        //ddldept_id.BackColor = Color.PowderBlue; 
        txtemp_no.ReadOnly = true;
        txtemp_no.BackColor = Color.PowderBlue;
        txtext.ReadOnly = true;
        txtext.BackColor = Color.PowderBlue;

    }

    private void SetLabel(ref PccErrMsg myLabel)
    {
        lbluser_desc.Text = myLabel.GetErrMsg("lbl0003", "SysManager/UserManager");
        lbluser_nm.Text = myLabel.GetErrMsg("lbl0004", "SysManager/UserManager");
        lblusr_pas.Text = myLabel.GetErrMsg("lbl0005", "SysManager/UserManager");
        lblReusr_pas.Text = myLabel.GetErrMsg("lbl0006", "SysManager/UserManager");
        //lbldept_id.Text = myLabel.GetErrMsg("lbl0007","SysManager/UserManager"); 
        lblemp_no.Text = myLabel.GetErrMsg("lbl0008", "SysManager/UserManager");
        lblext.Text = myLabel.GetErrMsg("lbl0009", "SysManager/UserManager");

    }

    private void SetddlDept(ref PccCommonForC.PccErrMsg myLabel, ref PccBsSystemForC.bs_UserManager mybs)
    {

        DataTable dt = mybs.DoReturnDataSet("GetDeptAllData", "", "").Tables["Dept"];

        DataRow myRow = dt.NewRow();
        myRow["dept_id"] = 0;
        myRow["dept_no"] = "aaa";
        myRow["dept_nm"] = "bbb";
        myRow["dept_desc"] = myLabel.GetErrMsg("SelectPlease");
        dt.Rows.InsertAt(myRow, 0);

        //			ddldept_id.DataSource = dt.DefaultView;
        //			ddldept_id.DataTextField = "dept_desc";
        //			ddldept_id.DataValueField = "dept_id";
        //			ddldept_id.DataBind();
    }

    private void SetddlApplication(ref PccCommonForC.PccErrMsg myLabel, ref PccBsSystemForC.bs_UserManager mybs)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vpath", ConfigurationManager.AppSettings["vpath"]);

        //此為判斷若是這個User是要新增的則user_id輸入0
        try
        {
            if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "Update")
            {
                myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
            }
            else
            {
                myMsg.CreateFirstNode("user_id", "0");
            }
        }
        catch
        {
            myMsg.CreateFirstNode("user_id", "0");
        }

        DataTable dt = mybs.DoReturnDataSet("GetApplyAp", myMsg.GetXmlStr, "").Tables["ApplyAp"];

        DataRow myRow = dt.NewRow();
        myRow["ap_id"] = 0;
        myRow["ap_name"] = myLabel.GetErrMsg("SelectPlease");
        dt.Rows.InsertAt(myRow, 0);

        ddlApplcation.DataSource = dt.DefaultView;
        ddlApplcation.DataTextField = "ap_name";
        ddlApplcation.DataValueField = "ap_id";
        ddlApplcation.DataBind();

        ddlApplcation.Attributes.Add("onChange", "ApplicationChange()");
    }


    #endregion

    protected void btnApply_Click(object sender, System.EventArgs e)
    {
        if (!CheckVerifyNumber()) return;

        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = GetSendXML();
        strReturn = mybs.DoReturnStr("InsertAskUser", strReturn, "");

        PccMsg myMsg = new PccMsg(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            lblMsg.Font.Size = FontUnit.Medium;
            lblMsg.Text = "申請成功，請等待管理者Mail通知！";
            txtusr_pas.Attributes["value"] = txtusr_pas.Text;
            txtReusr_pas.Attributes["value"] = txtusr_pas.Text;
            SetTextColor();
            ddlApplcation.Enabled = false;
            btnApply.Enabled = false;
            //20050630新增可以給多個管理者
            string ap_id = ddlApplcation.SelectedItem.Value;
            string[] arrEmail = System.Configuration.ConfigurationManager.AppSettings[ap_id + "-Email"].ToString().Split(';');
            string[] arrName = System.Configuration.ConfigurationManager.AppSettings[ap_id + "-Name"].ToString().Split(';');

            for (int i = 0; i < arrEmail.Length; i++)
            {
                if (!SendMailToManager(arrEmail[i], arrName[i]))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(),"new", "<script language=javascript>alert('寄送郵件失敗！');</script>");
                }
            }
            //-------------------------------

        }
        else
        {
            lblMsg.Font.Size = FontUnit.Medium;
            lblMsg.Text = myMsg.Query("errmsg");
        }

    }

    protected void btnReLogin_Click(object sender, System.EventArgs e)
    {
        if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "Update")
        {
            Response.Redirect("LoginBody.aspx?ApID=");
        }
        else
        {
            Response.Redirect("../../Default.aspx");
        }
    }

    protected void ddlApplcation_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlApplcation.SelectedItem.Value == "0")
            btnApply.Enabled = false;
        else
            btnApply.Enabled = true;
    }

    private bool CheckVerifyNumber()
    {
        bool bReturn = false;

        if (ValidNumber1.IsValid())
        {
            //RegisterClientScriptBlock("New", "<script language=javascript>alert('驗證碼正確');</script>");
            bReturn = true;
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('驗證碼錯誤，請重新輸入驗證碼！');</script>");
        }

        return bReturn;

    }

    private bool SendMailToManager()
    {
        try
        {
            string ap_id = ddlApplcation.SelectedItem.Value;
            string title = "審核申請-" + ddlApplcation.SelectedItem.Text + "-使用者通知";
            string href = System.Configuration.ConfigurationManager.AppSettings["myServer"].ToString() + System.Configuration.ConfigurationManager.AppSettings["vpath"].ToString() + "/default.aspx";

            MailMessage mymail = new MailMessage();
            foreach (string myTo in ConfigurationManager.AppSettings[ap_id + "-Email"].ToString().Split(';'))
            {
                mymail.To.Add(myTo);
            }
            MailAddress myFrom = new MailAddress(txtuser_nm.Text);
            mymail.From = myFrom;
            mymail.Subject = title;
            mymail.Body = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=big5'>";
            mymail.Body += "<title>" + title + "</title>";
            mymail.Body += "<style type='text/css'>.a00 {color:#AA0000}";
            mymail.Body += "h3{FONT-SIZE:12pt;line-height:16pt}";
            mymail.Body += "body,td,input{font-family: '細明體';font:9pt;line-height:14pt}</style></head>";
            mymail.Body += "<body bgcolor='#FFFFFF'><font color='#000099'><H3>" + title + "</H3></font><p>";
            mymail.Body += "<font color='#000000'>「" + System.Configuration.ConfigurationManager.AppSettings[ap_id + "-Name"].ToString() + "」您好！";
            mymail.Body += txtuser_desc.Text + "提出帳號申請，請<A href=" + href + ">由此進入</A>審核";
            mymail.Body += "</body></html>";
            mymail.IsBodyHtml = true;
            mymail.Priority = MailPriority.High;

            SmtpClient mySmtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString());
            mySmtp.Send(mymail);
            return true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            return false;
        }

    }

    //20050630新增可以給多個管理者
    private bool SendMailToManager(string myEmail, string myName)
    {
        try
        {
            string ap_id = ddlApplcation.SelectedItem.Value;
            string title = "審核申請-" + ddlApplcation.SelectedItem.Text + "-使用者通知";
            string href = System.Configuration.ConfigurationManager.AppSettings["myServer"].ToString() + System.Configuration.ConfigurationManager.AppSettings["vpath"].ToString() + "/default.aspx";

            MailMessage mymail = new MailMessage();
            foreach (string myTo in myEmail.Split(';'))
            {
                mymail.To.Add(myTo);
            }
            MailAddress myFrom = new MailAddress(txtuser_nm.Text);
            mymail.From = myFrom;

            mymail.Subject = title;
            mymail.Body = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=big5'>";
            mymail.Body += "<title>" + title + "</title>";
            mymail.Body += "<style type='text/css'>.a00 {color:#AA0000}";
            mymail.Body += "h3{FONT-SIZE:12pt;line-height:16pt}";
            mymail.Body += "body,td,input{font-family: '細明體';font:9pt;line-height:14pt}</style></head>";
            mymail.Body += "<body bgcolor='#FFFFFF'><font color='#000099'><H3>" + title + "</H3></font><p>";
            mymail.Body += "<font color='#000000'>「" + myName + "」您好！";
            mymail.Body += txtuser_desc.Text + "提出帳號申請，請<A href=" + href + ">由此進入</A>審核";
            mymail.Body += "</body></html>";
            mymail.IsBodyHtml = true;
            mymail.Priority = MailPriority.High;

            SmtpClient mySmtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString());
            mySmtp.Send(mymail);
            return true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            return false;
        }

    }

    private string GetSendXML()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", ddlApplcation.SelectedItem.Value);
        //因為在越南地區是以User_nm來檢查其帳號，所以不需要把@之後的文字去掉	Lemor 20060506
        //myMsg.CreateFirstNode("user_nm",txtuser_nm.Text.Split('@')[0].ToString());
        myMsg.CreateFirstNode("user_nm", txtuser_nm.Text);
        myMsg.CreateFirstNode("usr_pas", txtusr_pas.Text);
        myMsg.CreateFirstNode("comp_id", "1");
        myMsg.CreateFirstNode("fact_id", ddlfact_id.SelectedItem.Value);
        myMsg.CreateFirstNode("area_id", "158");
        myMsg.CreateFirstNode("user_desc", txtuser_desc.Text);
        myMsg.CreateFirstNode("email", txtuser_nm.Text);
        //myMsg.CreateFirstNode("dept_id",ddldept_id.SelectedItem.Value);
        myMsg.CreateFirstNode("emp_no", txtemp_no.Text);
        myMsg.CreateFirstNode("ext", txtext.Text);
        //因為以這個程式來說，都是要回覆通知的 20040416
        myMsg.CreateFirstNode("info_mk", "Y");
        myMsg.CreateFirstNode("check_id", "1");
        string upd_id;
        if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "Update")
            upd_id = Session["UserID"].ToString();
        else
            upd_id = "0";
        myMsg.CreateFirstNode("upd_id", upd_id);

        return myMsg.GetXmlStr;

    }


}
