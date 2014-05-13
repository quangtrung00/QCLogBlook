using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Security;
using System.Configuration;
using System.Data;
using PccCommonForC;
using PccBsQCLogBlook;
public partial class Pub_Module_ToolBar : System.Web.UI.UserControl
{
    private int CountUrl;
    string conf_SSO = System.Configuration.ConfigurationManager.AppSettings["SSO"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (conf_SSO == "Y")
        {
            loginStatus.LogoutText = "<img style='text-align: right; vertical-align: middle; cursor: pointer;' src='" + ResolveUrl("~/") + "Pub/EasyLayout/ImgBody/Icon/ICON-LOGIN.png' alt='登出系統' /><br />Logout";
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
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
            BindLanguages();
            string strLanguages = Application["CodeLang"].ToString();
            if (Session["CodeLang"] != null)
                strLanguages = Session["CodeLang"].ToString();
            try
            {
                ddlLanguage.SelectedIndex = -1;
                ddlLanguage.Items.FindByValue(strLanguages).Selected = true;
            }
            catch { }
            Session["CodeLang"] = strLanguages;
            Session["CodePage"] = Application["CodePage"];
        }

        lblOnlineCount.Text =" 線上人數： " + Application["OnlineCount"].ToString();
        string HrefUpdUser = ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/UpdateUser.aspx?UserID=" + Session["UserID"].ToString() + "&AcctionType=Upd&UserAccount=" + Session["UserAccount"].ToString();
        lblLoginUser.Text = "<a target='iframeBody' href='" + HrefUpdUser + "' style='color:#FFFFFF'>" + Session["UserName"].ToString() + "</a>";

        linkmain.Text = "<img src='" + ResolveUrl("~/") + "Pub/EasyLayout/ImgBody/Icon/ICON-HOME.png' /><br>Home";

        //lblLoginUser.Text = Session["UserName"].ToString();
        //SetLanguage();

       
        SetManagerPage();
    }

    #region Languages

    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CodeLang"] = ddlLanguage.SelectedValue;

       
        
    }
    private void BindLanguages()
    {
        DataTable dt = get_Languages();
        ddlLanguage.DataSource = dt;
        ddlLanguage.DataBind();
    }

    private DataTable get_Languages()
    {
        DataTable dt = new DataTable();

        PccMsg myMsg = new PccMsg();
        bs_BasicData mybs = new bs_BasicData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        DataSet ds = mybs.DoReturnDataSet("pro_get_languages", myMsg.GetXmlStr, "");
        if (ds.Tables.Count > 0)
            dt = ds.Tables[0];

        return dt;
    } 
    #endregion

    private void SetManagerPage()
    {
        try
        {
            DataTable dt = (DataTable)Session["AuthTable"];
            DataRow[] dr = dt.Select("ap_id = 104");
            if (dr.Length > 0)
            {
                pnlAp.Visible = true;
            }
        }
        catch { }
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
        /* string strReturn = "";
         strReturn += ConfigurationSettings.AppSettings["PFSBaseWeb"] + "usermanage/UpdateUser.aspx?UserID=" + Session["UserID"].ToString() + "&AcctionType=Upd&UserAccount=" + Session["UserAccount"].ToString();//選單的連結網頁
         //strReturn = "UpdateLoginUser.aspx";//選單的連結網頁     
         Response.Redirect(strReturn);*/
    }






    protected void linkmain_Click(object sender, EventArgs e)
    {
        Response.Redirect(ResolveUrl("~/Index.aspx?ApID=" + System.Configuration.ConfigurationSettings.AppSettings["ApID"] + ""));
    }

    protected void linkSystem_Click(object sender, EventArgs e)
    {
        Response.Redirect("IndexAdmin.aspx?ApID=104");
    }

    protected void linkNotification_Click(object sender, EventArgs e)
    {
        Response.Redirect("Index.aspx?ApID=" + System.Configuration.ConfigurationSettings.AppSettings["ApID"]);
    }

}