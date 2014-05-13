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
using System.Xml;
using PccBsSystemForC;

public partial class SysManager_GroupManage_GroupAddNew104 : System.Web.UI.Page
{
    private const string GROUPMANAGE = "GroupManage104.aspx";

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
        }
    }

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        if (txtGroupName.Text == "")
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            lblMsg.Text = myLabel.GetErrMsg("lbl0010", "ADTPurDoc/GroupManage");
            return;
        }

        //先判斷是Insert or Update
        bool bUpdate = false;
        string group_id = "";
        PccMsg myMsg = new PccMsg();

        if (Request.Params["Method"] != null)
        {
            bUpdate = true;
            myMsg.LoadXml(Request.Params["Method"].ToString());
            group_id = myMsg.Query("Key");
        }

        myMsg.ClearContext();
        myMsg.CreateFirstNode("group_id", group_id);
        myMsg.CreateFirstNode("ap_id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("group_nm", txtGroupName.Text);
        myMsg.CreateFirstNode("group_type", ddlGroupType.SelectedItem.Value);

        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = "";

        if (bUpdate)
        {
            myMsg.CreateFirstNode("upd_id", Session["UserID"].ToString());
            strReturn = mybs.DoReturnStr("UpdateGroupByGroupID", myMsg.GetXmlStr, "");
        }
        else
        {
            myMsg.CreateFirstNode("add_id", Session["UserID"].ToString());
            strReturn = mybs.DoReturnStr("InsertGroupByApID", myMsg.GetXmlStr, "");
        }

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            Response.Redirect(GROUPMANAGE + "?ApID=" + CheckQueryString("ApID"));
        }
        else
        {
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("errmsg");
        }

    }

    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(GROUPMANAGE + "?ApID=" + CheckQueryString("ApID"));
    }

    #endregion

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");

        string strTitle = "";
        PccMsg myMsg = new PccMsg();
        if (Request.Params["Method"] != null)
        {
            strTitle = "修改群組";
            myMsg.LoadXml(Request.Params["Method"].ToString());
            SetData(ref myMsg);
        }
        else
        {
            strTitle = myLabel.GetErrMsg("lbl0003", "ADTPurDoc/GroupManage");

        }

        /*lblAddNew.Text = myLabel.GetErrMsg("lbl0003","ADTPurDoc/GroupManage");
        lblAddNew.Font.Size = FontUnit.Large;
        lblAddNew.ForeColor = Color.Blue;
        lblAddNew.Font.Bold = true;*/

        //設定抬頭 20040607
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == "PccTitle")
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = strTitle;
            }
        }

        lblGroupName.Text = myLabel.GetErrMsg("lbl0002", "ADTPurDoc/GroupManage");
        lblGroupType.Text = myLabel.GetErrMsg("lbl0009", "ADTPurDoc/GroupManage");
    }

    private void SetData(ref PccCommonForC.PccMsg myMsg)
    {
        PccMsg myMsg1 = new PccMsg();
        myMsg1.CreateFirstNode("group_id", myMsg.Query("Key"));

        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        DataSet dsReturn = mybs.DoReturnDataSet("GetGroupByGroupID", myMsg1.GetXmlStr, "");

        if (dsReturn != null)
        {
            DataTable dt = dsReturn.Tables["Group"];
            txtGroupName.Text = dt.Rows[0]["group_nm"].ToString();

            ddlGroupType.SelectedIndex = -1;
            ddlGroupType.Items.FindByValue(dt.Rows[0]["group_type"].ToString()).Selected = true;
        }
        else
        {
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = "載入資料錯誤";
        }
    }

    #endregion

    #region "Tool Func. ex. CheckDBNull()"

    private string CheckDBNull(object oFieldData)
    {
        if (Convert.IsDBNull(oFieldData))
            return "";
        else
            return oFieldData.ToString();
    }

    private string CheckForm(string strName)
    {
        if (Request.Form[strName] == null)
            return "";
        else
            return Request.Form[strName].ToString();
    }

    private string CheckQueryString(string strName)
    {
        if (Request.QueryString[strName] == null)
            return "";
        else
            return Request.QueryString[strName].ToString();
    }

    private string CheckParams(string strName)
    {
        if (Request.Params[strName] == null)
            return "";
        else
            return Request.Params[strName].ToString();
    }

    #endregion
}
