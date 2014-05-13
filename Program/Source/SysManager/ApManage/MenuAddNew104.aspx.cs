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
using System.Data.SqlClient;


public partial class SysManager_ApManage_MenuAddNew104 : System.Web.UI.Page
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
            BindDdlData(ref myLabel);

            if (Request.Params["Type"] != null && Request.Params["Type"].ToString() == "New")
            {
                //設定所選的應用程式
                SetApSelectedData(Request.QueryString["Key"]);
                txtManageMK.Text = "N";
            }
            else
            {
                GenUpdateData();
                txtMenuNo.ReadOnly = true;
                ddlApName.Enabled = false;
            }
        }
    }

    #region "設定此頁面之基本資料"

    private void SetApSelectedData(string ap_id)
    {
        ddlApName.Items.FindByValue(ap_id).Selected = true;
        ddlApName.Enabled = false;
    }

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
                    mylblTitle.Text = myLabel.GetErrMsg("lbl0004", "SysManager/MenuManage");
                }
            }
        }
        else
        {
            for (int i = 0; i < Header1.Controls.Count; i++)
            {
                if (Header1.Controls[i].ID == "PccTitle")
                {
                    Label mylblTitle = (Label)(Header1.Controls[i]);
                    mylblTitle.Text = myLabel.GetErrMsg("lbl0019", "SysManager/MenuManage");
                }
            }
        }

        lblMenuNo.Text = myLabel.GetErrMsg("lbl0014", "SysManager/MenuManage");
        lblMenuName.Text = myLabel.GetErrMsg("lbl0003", "SysManager/MenuManage");
        lblMenuLink.Text = myLabel.GetErrMsg("lbl0015", "SysManager/MenuManage");
        lblApName.Text = myLabel.GetErrMsg("lbl0016", "SysManager/MenuManage");
        lblCheckMK.Text = myLabel.GetErrMsg("lbl0017", "SysManager/MenuManage");
        lblManageMK.Text = myLabel.GetErrMsg("lbl0018", "SysManager/MenuManage");


        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");
    }

    private void BindDdlData(ref PccErrMsg myLabel)
    {
        bs_ApManager mybs = new bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vpath", System.Configuration.ConfigurationManager.AppSettings["vpath"].ToString());
        DataTable dt = mybs.DoReturnDataSet("GetAllApByVpath", myMsg.GetXmlStr, "").Tables["AllApByVpath"];

        DataRow myRow = dt.NewRow();
        myRow["ap_id"] = 0;
        myRow["ap_name"] = myLabel.GetErrMsg("SelectPlease");
        myRow["ap_link"] = "bbb";
        myRow["ap_vpath"] = "All";

        dt.Rows.InsertAt(myRow, 0);

        ddlApName.DataSource = dt.DefaultView;
        ddlApName.DataValueField = "ap_id";
        ddlApName.DataTextField = "ap_name";
        ddlApName.DataBind();
    }


    #endregion

    #region "取得欲修改的資料"

    private void GenUpdateData()
    {
        bs_ApManager mybs = new bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("menu_id", GetMenuID());
        string strReturn = mybs.DoReturnStr("GetMenuByMenuID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("Return") == "OK")
        {
            txtMenuNo.Text = myMsg.Query("menu_no");
            txtMenuName.Text = myMsg.Query("menu_nm");
            txtMenuLink.Text = myMsg.Query("menu_link");
            ddlApName.Items.FindByValue(myMsg.Query("ap_id")).Selected = true;
            if (myMsg.Query("check_mk") == "Y") chkCheckMK.Checked = true; else chkCheckMK.Checked = false;
            txtManageMK.Text = myMsg.Query("manage_mk");
        }
        else
        {
            btnOK.Visible = false;
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("Return");
        }
    }

    private string GetMenuID()
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
            strReturn = mybs.DoReturnStr("InsertProMenu", GenDBXML(), "");
        else
            strReturn = mybs.DoReturnStr("UpdateProMenu", GenDBXML(), "");

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
            myMsg.CreateFirstNode("menu_id", GetMenuID());
        }

        myMsg.CreateFirstNode("ap_id", ddlApName.SelectedItem.Value);

        myMsg.CreateFirstNode("menu_no", txtMenuNo.Text);
        myMsg.CreateFirstNode("menu_nm", txtMenuName.Text);
        myMsg.CreateFirstNode("menu_link", txtMenuLink.Text);

        if (chkCheckMK.Checked) myMsg.CreateFirstNode("check_mk", "Y"); else myMsg.CreateFirstNode("check_mk", "N");

        myMsg.CreateFirstNode("manage_mk", txtManageMK.Text.ToString().ToUpper());


        return myMsg.GetXmlStr;
    }

}
