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

public partial class SysManager_UserManage_UserJoinGroup104 : System.Web.UI.Page
{
    private int m_count = 0;
    private const string USERMANAGE = "UserManage104.aspx";
    private string m_apid = string.Empty;

    public string ListCount
    {
        get
        {
            m_count += 1;
            return m_count.ToString();
        }
        set
        {
            m_count = 10 * int.Parse(value);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        m_apid = Request.QueryString["ApID"].ToString();
        //設定抬頭 20040607
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == "PccTitle")
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "使用者加入群組";
            }
        }

        if (!IsPostBack)
        {
            PccMsg myMsg = new PccMsg();
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            GenMasterTable(ref myLabel);

            if (CheckQueryString("Method") != "")
            {
                myMsg.LoadXml(CheckQueryString("Method"));
                lblMsg.Text = myLabel.GetErrMsg("lbl0023", "ADTPurDoc/GroupManage") + myMsg.Query("KeyOther") + myLabel.GetErrMsg("lbl0024", "ADTPurDoc/GroupManage");
            }

        }

    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");
    }

    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (tblGroup.Rows.Count > 0)
            tblGroup.Rows.Clear();

        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTable()
    {
        if (tblGroup.Rows.Count > 0)
            tblGroup.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTableHader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //編號
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0004", "ADTPurDoc/GroupManage"), 10);
        //群組名稱
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0025", "ADTPurDoc/GroupManage"), 60);
        //檢視選單權限
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0026", "ADTPurDoc/GroupManage"), 20);
        //選取
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0008", "ADTPurDoc/GroupManage"), 10);

        tblGroup.CssClass = "ActDocTB";
        tblGroup.Width = Unit.Percentage(100);
        tblGroup.HorizontalAlign = HorizontalAlign.Center;
        tblGroup.CellPadding = 2;
        tblGroup.CellSpacing = 1;

        tblGroup.Rows.Add(myRow.Row);
    }


    #endregion

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        if (CheckForm("JoinGroupID") == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('You not yet select group!');</script>");
            GenMasterTable();
            return;
        }

        string strReturn = "";
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id", ViewState["UserID"].ToString());
        myMsg.CreateFirstNode("ap_id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("group_str", CheckForm("JoinGroupID"));
        myMsg.CreateFirstNode("group_org", ViewState["group_org"].ToString());

        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        strReturn = mybs.DoReturnStr("JoinGroupByUserID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") != "0")
        {
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("errmsg");
            GenMasterTable();
        }
        else
        {
            Response.Redirect(USERMANAGE + "?ApID=" + CheckQueryString("ApID") + "&QueryCondition=" + CheckQueryString("QueryCondition"));
        }

    }

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


    #region "設定主Table的資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccMsg myMsg = new PccMsg();
        string strUserID = "";
        if (CheckQueryString("Method") != "")
        {
            myMsg.LoadXml(CheckQueryString("Method"));
            strUserID = myMsg.Query("Key");
            ViewState["UserID"] = strUserID;
        }
        else
        {
            lblMsg.Text = "Load Error!!!";
            return;
        }

        bs_GroupManage mybs = new PccBsSystemForC.bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        myMsg.LoadXml();
        myMsg.CreateFirstNode("ap_id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("user_id", strUserID);
        myMsg.CreateFirstNode("StartRecord", "0");
        myMsg.CreateFirstNode("PageSize", "3000");
        myMsg.CreateFirstNode("group_nm", "");

        //加入判斷是否要有事業群之判斷20041116
        myMsg.CreateFirstNode("page_check", "UserJoinGroup104");
        myMsg.CreateFirstNode("login_user_id", Session["UserID"].ToString());
        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = "UserManage104.aspx";

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


        DataSet myGroupDs = mybs.DoReturnDataSet("GetGroupAndUserCheckByApID", myMsg.GetXmlStr, "");
        DataTable myGroupData = myGroupDs.Tables["Group"];

        if (myGroupData.Rows.Count > 0)
        {
            ViewState["group_org"] = "";

            int MasterCount = 0;
            string MasterStyle = "";
            PccMsg myTempMsg;

            PccRow myRow;

            //明細資料類別之共同參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            //明細資料的欄位參數
            string[] Fields = { "編號", "選單名稱", "顯示權限", "新增權限", "更新權限", "刪除權限", "報表權限", "發信權限" };
            int[] FieldsPercent = { 5, 35, 10, 10, 10, 10, 10, 10 };
            string[] FieldsItem = { "--NO--", "menu_nm", "show_mk", "add_mk", "upd_mk", "del_mk", "rpt_mk", "send_mk" };
            string[] FieldsType = new string[8];
            FieldsType[0] = "--NO--";
            FieldsType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[2] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[3] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[4] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[5] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[6] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[7] = "<PccMsg><Type>CheckBoxReadOnly</Type><Value>menu_id</Value></PccMsg>";

            //要取得明細資料的Table變數
            DataTable tblMenuByGroupID;

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myGroupData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow("", HorizontalAlign.Center, 0, 0);
                myRow.SetRowCss(MasterStyle);
                //編號
                myRow.AddTextCell(ListCount, 10);
                //群組名稱
                myRow.AddTextCell(myMasterRow["group_nm"].ToString(), 60);
                //檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0021", "ADTPurDoc/GroupManage"));
                myTempMsg.CreateFirstNode("LinkID", "HlinkView" + myMasterRow["group_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 20);
                //選擇
                myTempMsg.LoadXml();
                myTempMsg.CreateFirstNode("Checked", myMasterRow["UserCheck"].ToString());
                myTempMsg.CreateFirstNode("Name", "JoinGroupID");
                myTempMsg.CreateFirstNode("Value", myMasterRow["group_id"].ToString());
                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, 10);

                //設定原始的GroupID的資料
                if (myMasterRow["UserCheck"].ToString() == "Y")
                    ViewState["group_org"] += myMasterRow["group_id"].ToString() + ",";

                tblGroup.Rows.Add(myRow.Row);

                tblMenuByGroupID = mybs.DoReturnDataSet("GetMenuByGroupID", "<PccMsg><group_id>" + myMasterRow["group_id"].ToString() + "</group_id></PccMsg>", "").Tables["MenuGroup"];

                //設定選單明細資料類別之參數
                PccDetailTable myDTable = new PccDetailTable("DT" + myMasterRow["group_id"].ToString());
                myDTable.ClassXML = strClassXML;
                myDTable.Fields = Fields;
                myDTable.FieldsPercent = FieldsPercent;
                myDTable.FieldsItem = FieldsItem;
                myDTable.FieldsType = FieldsType;
                myDTable.NewDataTable = tblMenuByGroupID;

                myDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myDTable.NewTable, 100);

                tblGroup.Rows.Add(myRow.Row);

                MasterCount += 1;

            } // end of foreach datarow

            if (ViewState["group_org"].ToString() != "")
                ViewState["group_org"] = ViewState["group_org"].ToString().Substring(0, ViewState["group_org"].ToString().Length - 1);
            else
                ViewState["group_org"] = "";

        } // end of if table count is 0
    }



    #endregion

    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(USERMANAGE + "?ApID=" + CheckQueryString("ApID") + "&QueryCondition=" + CheckQueryString("QueryCondition"));
    }

}
