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


public partial class SysManager_ApManage_ApAddNew104 : System.Web.UI.Page
{
    private const string APMANAGE = "ApManage104.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "New")
            {
                lblApNo.Visible = false;
                txtApNo.Visible = false;
            }
            else
            {
                GenUpdateData();
                txtApNo.ReadOnly = true;
            }
        }
    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        //設定只能新增或修改此Vpath中的應用程式 2004/3/9
        if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "New")
        {
            //設定抬頭 20040607
            for (int i = 0; i < Header1.Controls.Count; i++)
            {
                if (Header1.Controls[i].ID == "PccTitle")
                {
                    Label mylblTitle = (Label)(Header1.Controls[i]);
                    mylblTitle.Text = myLabel.GetErrMsg("lbl0006", "SysManager/ApManager");
                }
            }
            txtVpath.Text = System.Configuration.ConfigurationManager.AppSettings["vpath"];
        }
        else
        {
            for (int i = 0; i < Header1.Controls.Count; i++)
            {
                if (Header1.Controls[i].ID == "PccTitle")
                {
                    Label mylblTitle = (Label)(Header1.Controls[i]);
                    mylblTitle.Text = myLabel.GetErrMsg("lbl0016", "SysManager/ApManager");
                }
            }
        }
        txtVpath.ReadOnly = true;

        lblApNo.Text = myLabel.GetErrMsg("lbl0003", "SysManager/ApManager");
        lblApName.Text = myLabel.GetErrMsg("lbl0004", "SysManager/ApManager");
        lblApLink.Text = myLabel.GetErrMsg("lbl0005", "SysManager/ApManager");
        lblApVpath.Text = myLabel.GetErrMsg("lbl0011", "SysManager/ApManager");

        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");
    }


    #endregion

    #region "取得欲修改的原始資料"

    private void GenUpdateData()
    {
        bs_ApManager mybs = new bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", GetApID());
        string strReturn = mybs.DoReturnStr("GetApByApID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("Return") == "OK")
        {
            txtApNo.Text = myMsg.Query("ap_id");
            txtApName.Text = myMsg.Query("ap_name");
            txtApLink.Text = myMsg.Query("ap_link");
            txtVpath.Text = myMsg.Query("ap_vpath");
        }
        else
        {
            btnOK.Visible = false;
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("Return");
        }
    }

    private string GetApID()
    {
        PccMsg myMsg = new PccMsg();
        try
        {
            if (Request.Params["Method"] != null)
                myMsg.LoadXml(Request.Params["Method"].ToString());
            return myMsg.Query("Key");
        }
        catch
        {
            return "0";
        }
    }


    #endregion

    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(APMANAGE + "?ApID=" + Request.QueryString["ApID"] + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
    }

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg();

        bs_ApManager mybs = new bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = "";

        if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "New")
            strReturn = mybs.DoReturnStr("InsertProAp", GenDBXML(), "");
        else
            strReturn = mybs.DoReturnStr("UpdateProAp", GenDBXML(), "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            Response.Redirect(APMANAGE + "?ApID=" + Request.QueryString["ApID"] + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
        }
        else
        {
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("errmsg");
        }

    }

    private string GenDBXML()
    {
        PccMsg myMsg = new PccMsg();
        if (Request.Params["Type"] == null || Request.Params["Type"].ToString() != "New")
        {
            myMsg.CreateFirstNode("ap_id", GetApID());
        }

        myMsg.CreateFirstNode("ap_name", txtApName.Text);
        myMsg.CreateFirstNode("ap_link", txtApLink.Text);
        myMsg.CreateFirstNode("ap_vpath", txtVpath.Text);

        return myMsg.GetXmlStr;
    }
}
