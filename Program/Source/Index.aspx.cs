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

    protected void Page_Load(object sender, EventArgs e)
    {
       
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


   
}