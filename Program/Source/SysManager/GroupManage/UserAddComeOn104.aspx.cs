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

public partial class SysManager_GroupManage_UserAddComeOn104 : System.Web.UI.Page
{
    private const string GROUPMANAGE = "GroupManage104.aspx";


    private int m_count = 0;
    private string m_apid = "";

    public string GrideCount
    {
        get
        {
            m_count += 1;
            return m_count.ToString();
        }
        set
        {
            m_count = int.Parse(value);
        }
    }

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        m_apid = Request.QueryString["ApID"].ToString();
        if (!IsPostBack)
        {
            //lblTitle.Text = "請選擇欲加入<font color=blue>" + Request.Params["GroupNm"] + "</font>群組的使用者";
            //設定抬頭 20040607
            for (int i = 0; i < Header1.Controls.Count; i++)
            {
                if (Header1.Controls[i].ID == "PccTitle")
                {
                    Label mylblTitle = (Label)(Header1.Controls[i]);
                    mylblTitle.Text = "請選擇欲加入<font color=blue>" + Request.Params["GroupNm"] + "</font>群組的使用者";
                }
            }

            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
            bs_UserManager myUbs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
            SetLabel(ref myLabel);
            BindFactData(ref myLabel, ref myUbs);
            //SetddlDept(ref myLabel,ref myUbs);
            BindDataGrid("user_desc", "", "0", ref mybs);
        }
        else
        {
            PanelGrid.Visible = true;
        }

    }

    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(GROUPMANAGE + "?ApID=" + CheckQueryString("ApID") + "&QueryCondition=" + CheckQueryString("QueryCondition"));
    }

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        //BindDataGrid("user_desc",txtUserDesc.Text,ddldept_id.SelectedItem.Value,ref mybs); 
        BindDataGrid("user_desc", txtUserDesc.Text, ddlfact_id.SelectedItem.Value, ref mybs);
    }

    protected void btnAddComeOn_Click(object sender, System.EventArgs e)
    {
        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        string strUserID = "", strReturn = "";

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            if (((CheckBox)DataGrid1.Items[i].Cells[6].Controls[1]).Checked)
            {
                strUserID += DataGrid1.Items[i].Cells[1].Text + ",";
            }
        }

        if (strUserID != "")
        {
            strUserID = strUserID.Substring(0, strUserID.Length - 1);

            myMsg.LoadXml();
            myMsg.CreateFirstNode("group_id", CheckQueryString("GroupID"));
            myMsg.CreateFirstNode("user_str", strUserID);
            strReturn = mybs.DoReturnStr("InsertUGrp", myMsg.GetXmlStr, "");

            myMsg.LoadXml(strReturn);

            if (myMsg.Query("returnValue") == "0")
            {
                Response.Redirect(GROUPMANAGE + "?ApID=" + CheckQueryString("ApID") + "&QueryCondition=" + CheckQueryString("QueryCondition"));
            }
            else
            {
                lblMsg.Font.Size = FontUnit.Large;
                lblMsg.Text = myMsg.Query("errmsg");
            }
        }
        else
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myLabel.GetErrMsg("lbl0014", "SysManager/UserManager");
        }
    }


    #endregion

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnAddComeOn.Text = myLabel.GetErrMsg("lbl0002", "SysManager/UserManager");
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");

        //lblDept.Text = myLabel.GetErrMsg("lbl0012","SysManager/UserManager");
        lblFact.Text = "廠別：";
        lblUserDesc.Text = myLabel.GetErrMsg("lbl0013", "SysManager/UserManager");

        //Set DataGrid HeaderText
        DataGrid1.Columns[0].HeaderText = myLabel.GetErrMsg("lbl0004", "ADTPurDoc/GroupManage");//編號
        DataGrid1.Columns[2].HeaderText = myLabel.GetErrMsg("lbl0030", "ADTPurDoc/GroupManage");//電子郵件帳號
        DataGrid1.Columns[4].HeaderText = myLabel.GetErrMsg("lbl0005", "ADTPurDoc/GroupManage");//使用者名稱
        DataGrid1.Columns[5].HeaderText = myLabel.GetErrMsg("lbl0006", "ADTPurDoc/GroupManage");//廠別
        //DataGrid1.Columns[5].HeaderText = myLabel.GetErrMsg("lbl0007","ADTPurDoc/GroupManage");//部門別
        DataGrid1.Columns[3].HeaderText = myLabel.GetErrMsg("lbl0031", "ADTPurDoc/GroupManage");//帳號
        DataGrid1.Columns[6].HeaderText = myLabel.GetErrMsg("lbl0008", "ADTPurDoc/GroupManage");//選取

    }

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

    private void SetddlDept(ref PccCommonForC.PccErrMsg myLabel, ref bs_UserManager mybs)
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

    private void BindDataGrid(string strOrder, string strUserDesc, string strFactID, ref bs_GroupManage mybs)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("group_id", CheckQueryString("GroupID"));
        myMsg.CreateFirstNode("fact_id", strFactID);
        myMsg.CreateFirstNode("user_desc", strUserDesc);
        myMsg.CreateFirstNode("order", strOrder);

        //加入判斷是否要有事業群之判斷20041116
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = "GroupManage104.aspx";

        //判斷是否要利用事業群來分設權限
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


        DataSet ds = mybs.DoReturnDataSet("GetAnotherUserByGroup", myMsg.GetXmlStr, "");
        ViewState["order"] = strOrder;

        if (ds != null)
        {
            DataGrid1.DataSource = ds.Tables["AnotherUserByGroup"].DefaultView;
            DataGrid1.DataBind();
        }
        else
        {
            btnAddComeOn.Visible = false;
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

    protected void Button1_Click(object sender, System.EventArgs e)
    {
        System.IO.StringWriter mySW = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter myHW = new System.Web.UI.HtmlTextWriter(mySW);
        DataGrid1.RenderControl(myHW);
        Response.AddHeader("content-disposition", "attachment; filename=temp.xls");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Write(mySW.ToString());
        Response.End();
        mySW.Close();
        myHW.Close();


    }
}
