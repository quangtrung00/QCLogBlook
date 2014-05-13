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


public partial class SysManager_UserManage_UserManage104 : System.Web.UI.Page
{
    private const string USERMANAGE = "UserManage104.aspx";
    private const string USERJOINGROUP = "UserJoinGroup104.aspx";
    private string USERADDNEW = ConfigurationManager.AppSettings["PFSBaseWeb"] + "usermanage/UpdateUser.aspx";
    private const string USERJOINAP = "UserJoinAp104.aspx";

    private string[] m_menuIDArray;
    private string m_apid = string.Empty;

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        m_apid = Request.QueryString["ApID"].ToString();
        PccMsg myMsg = new PccMsg();
        string MethodFunc = "";

        //判斷是否有新增的權限 20041118
        GetMenuAuth myAuth = new GetMenuAuth();
        if (myAuth.IsAddAuth())
        {
            lbtnAddNewUser.Visible = true;
            lbtnJoinUser.Visible = true;
        }

        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            //設定廠別的資訊 20060717
            BindFactData(ref myLabel);


            //設定之前User鍵入的查詢資料。 2004/3/10
            if (Request.Params["QueryCondition"] != null && Request.Params["QueryCondition"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["QueryCondition"].ToString());
                txtUserName.Text = myMsg.Query("QueryCondition/txtUserName");
                ddlUserType.SelectedIndex = -1;
                ddlUserType.Items.FindByValue(myMsg.Query("QueryCondition/ddlUserType")).Selected = true;
            }

            GenMasterTable(ref myLabel);

            if (CheckRequestQueryString("Method") != "")
            {
                try
                {
                    myMsg.LoadXml(Request.QueryString["Method"]);
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('Load XML Fail');</script>");
                    return;
                }
                MethodFunc = myMsg.Query("Method");
                switch (MethodFunc)
                {
                    case "MasterDelFunc":
                        MasterDelFunc(myMsg.Query("Key"), myMsg.Query("KeyOther"));
                        break;
                    case "MasterReturnFunc":
                        MasterReturnFunc(myMsg.Query("Key"), myMsg.Query("KeyOther"));
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
                MethodFunc = myMsg.Query("Method");
                switch (MethodFunc)
                {
                    case "UpdateMenuByUser":
                        UpdateMenuByUser(txtReturn.Value);
                        break;
                }
            }
        }

    }

    protected void lbtnAddNewUser_Click(object sender, System.EventArgs e)
    {
        //Response.Redirect(USERADDNEW + "?ApID=" + Request.QueryString["ApID"].ToString() + "&Type=New&QueryCondition=" + GetQueryCondition());
        Response.Redirect(USERADDNEW + "?ApID=" + Request.Params["ApID"].ToString() + "&AcctionType=New&ul=" + ConfigurationManager.AppSettings["myServer"] + ConfigurationManager.AppSettings["vpath"] + "/SysManager/UserManage/UserManage104.aspx?ApID=" + Request.Params["ApID"].ToString() + "&upd_id=" + Session["UserID"].ToString());

    }

    protected void lbtnJoinUser_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(USERJOINAP + "?ApID=" + Request.QueryString["ApID"].ToString() + "&Type=New&QueryCondition=" + GetQueryCondition());

    }

    protected void btnDelCancel_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;
        GenMasterTable();
    }

    protected void btnReturnCancel_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plReturnGroup.Visible = false;
        GenMasterTable();
    }

    protected void btnReturnOK_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plReturnGroup.Visible = false;

        PccMsg myMsg = new PccMsg(CheckRequestQueryString("Method"));
        string strUserID = myMsg.Query("Key");
        myMsg.LoadXml();
        myMsg.CreateFirstNode("user_id", strUserID);
        myMsg.CreateFirstNode("ap_id", CheckRequestQueryString("ApID"));

        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs.DoReturnStr("ReturnGroupByUserID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            GenMasterTable();
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
        }
    }

    protected void btnDelOK_Click(object sender, System.EventArgs e)
    {

        plMain.Visible = true;
        plDelete.Visible = false;

        PccMsg myMsg = new PccMsg(CheckRequestQueryString("Method"));
        PccMsg myReturnMsg = new PccMsg();
        string strUserID = myMsg.Query("Key");
        myMsg.LoadXml();
        myMsg.CreateFirstNode("user_id", strUserID);
        myMsg.CreateFirstNode("ap_id", CheckRequestQueryString("ApID"));

        //增加一個刪除e_userfact的關連 20040924
        myMsg.CreateFirstNode("upd_id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("fact_id", "0");
        myMsg.CreateFirstNode("uf_id", "0");
        bs_UserFactManage mybs1 = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs1.DoReturnStr("DeleteUserFactByUser", myMsg.GetXmlStr, "");
        myReturnMsg.LoadXml(strReturn);

        if (myReturnMsg.Query("returnValue") != "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
            return;
        }


        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        strReturn = mybs.DoReturnStr("DeleteUserByApID", myMsg.GetXmlStr, "");

        myReturnMsg.LoadXml(strReturn);

        if (myReturnMsg.Query("returnValue") == "0")
        {
            GenMasterTable();
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
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

    #region "編號及上下頁的程式碼"

    protected void OnPageClick(object source, EventArgs e)
    {
        GenMasterTable();
    }

    #endregion

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");

        //lblTitle.Text = myLabel.GetErrMsg("lbl0027","SysManager/UserManager");
        lblUserType.Text = myLabel.GetErrMsg("lbl0029", "SysManager/UserManager");
        //lblUserName.Text = myLabel.GetErrMsg("lbl0030","SysManager/UserManager");

        lbtnAddNewUser.Text = myLabel.GetErrMsg("lbl0001", "SysManager/UserManager");
        lbtnJoinUser.Text = myLabel.GetErrMsg("lbl0002", "SysManager/UserManager");

    }

    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (tblApUser.Rows.Count > 0)
            tblApUser.Rows.Clear();

        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTable()
    {
        if (tblApUser.Rows.Count > 0)
            tblApUser.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTableHader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("cssGridHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //註記
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0031", "SysManager/UserManager"), 4);
        //姓名
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0019", "SysManager/UserManager"), 8);
        //部門
        //myRow.AddTextCell(myLabel.GetErrMsg("lbl0022","SysManager/UserManager"),10);
        //廠別
        myRow.AddTextCell("廠別", 10);
        //帳號
        //myRow.AddTextCell(myLabel.GetErrMsg("lbl0032","SysManager/UserManager"),10);
        //電子郵件
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0033", "SysManager/UserManager"), 48);
        //分機
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0034", "SysManager/UserManager"), 5);
        //檢視
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0035", "SysManager/UserManager"), 5);
        //管理
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0036", "SysManager/UserManager"), 5);
        //群組
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0037", "SysManager/UserManager"), 5);
        //使用者管理
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0038", "SysManager/UserManager"), 10);


        tblApUser.CssClass = "cssGridTable";
        tblApUser.Width = Unit.Percentage(100);
        tblApUser.HorizontalAlign = HorizontalAlign.Center;
        tblApUser.CellPadding = 2;
        //tblApUser.CellSpacing = 1;

        tblApUser.Rows.Add(myRow.Row);
    }

    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("ddlUserType", ddlUserType.SelectedItem.Value);
        myMsg.AddToNode("txtUserName", txtUserName.Text);
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

    private void BindFactData(ref PccCommonForC.PccErrMsg myLabel)
    {

        DataSet ds;
        DataTable dt;
        DataRow myRow;

        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", Request.QueryString["ApID"]);

        ds = mybs.DoReturnDataSet("GetFactsRelUsersByApID", myMsg.GetXmlStr, "");
        dt = ds.Tables["FactsRelUsersByAp"];

        myRow = dt.NewRow();
        myRow["fact_id"] = 0;
        myRow["fact_no"] = string.Empty;
        myRow["fact_desc"] = myLabel.GetErrMsg("SelectPlease");
        dt.Rows.InsertAt(myRow, 0);

        ddlFact.DataSource = dt.DefaultView;
        ddlFact.DataTextField = "fact_desc";
        ddlFact.DataValueField = "fact_no";

        ddlFact.DataBind();
    }


    #endregion

    #region "設定主Table的資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("ap_id", Request.QueryString["ApID"]);
        myMsg.CreateFirstNode("fact_no", ddlFact.SelectedItem.Value);
        myMsg.CreateFirstNode("order", "user_desc");
        myMsg.CreateFirstNode("orderType", "desc");
        if (ddlUserType.SelectedItem.Value.ToString() == "All")
            myMsg.CreateFirstNode("mana_mk", "");
        else
            myMsg.CreateFirstNode("mana_mk", ddlUserType.SelectedItem.Value.ToString());

        if (ddlQuerySelect.SelectedItem.Value.ToString() == "1")
        {
            myMsg.CreateFirstNode("user_desc", txtUserName.Text);
            myMsg.CreateFirstNode("user_nm", string.Empty);
        }
        else
        {
            myMsg.CreateFirstNode("user_desc", string.Empty);
            myMsg.CreateFirstNode("user_nm", txtUserName.Text);
        }


        //加入判斷是否要有事業群之判斷20041116
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        GetMenuAuth myAuth = new GetMenuAuth();

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

        string strXML = myMsg.GetXmlStr;
        DataSet myUserDs = mybs.DoReturnDataSet("GetUserAndMarkByApID_V1", strXML, "");

        DataTable myUserData = myUserDs.Tables["UserAndMark"];

        if (myUserData != null && myUserData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";
            PccMsg myTempMsg;

            PccRow myRow;
            //取出資料總筆數
            PageControl1.TotalSize = myUserDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //明細資料類別之共同參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            //判斷是否為超級管理者，若是則不控管自己Menu的CheckBox權限 20041116
            string strcheckbox = "";
            if (Session["UserEMail"].ToString() != ConfigurationManager.AppSettings["superAdminEmail"].ToString())
                strcheckbox = "CheckBoxByValueJudgeUserRight";
            else
                strcheckbox = "CheckBoxByValue";

            //明細資料的欄位參數
            string strOrgFields8 = "btn-更新送出-Return_ClickM(this)";
            string[] Fields = { "編號", "選單名稱", "顯示權限", "新增權限", "更新權限", "刪除權限", "報表權限", "發信權限", strOrgFields8 };
            int[] FieldsPercent = { 5, 25, 10, 10, 10, 10, 10, 10, 10 };
            string[] FieldsItem = { "--NO--", "menu_nm", "show_mk", "add_mk", "upd_mk", "del_mk", "rpt_mk", "send_mk", "menud_id" };
            string[] FieldsType = new string[9];
            FieldsType[0] = "--NO--";
            FieldsType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[2] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[3] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[4] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[5] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[6] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[7] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            FieldsType[8] = "<PccMsg><Type>Space</Type></PccMsg>";


            //要取得明細資料的Table變數
            DataTable tblMenuByUserID;

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myUserData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "cssGridRowAlternating"; else MasterStyle = "cssGridRow";
                myRow = new PccRow("", HorizontalAlign.Center, 0, 0);
                myRow.SetRowCss(MasterStyle);

                //註記
                if (myMasterRow["remark"].ToString() == "N")
                    myRow.AddTextCell("", 4);
                else
                    myRow.AddTextCell("*", 4);

                //姓名
                myRow.AddTextCell(myMasterRow["user_desc"].ToString(), 8);
                //部門
                //myRow.AddTextCell(myMasterRow["dept_desc"].ToString(),10);
                //廠別
                myRow.AddTextCell(myMasterRow["fact_nm"].ToString(), 10);
                //帳號
                //myRow.AddTextCell(myMasterRow["user_nm"].ToString(),10);
                //電子郵件
                myRow.AddTextCell(CheckDBNull(myMasterRow["email"]), 48);
                //分機
                myRow.AddTextCell(CheckDBNull(myMasterRow["ext"]), 5);
                //檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0026", "ADTPurDoc/GroupManage"));
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + ((int)((decimal)myMasterRow["user_id"])).ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_M" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 5);
                //管理
                if (myMasterRow["mana_mk"].ToString() == "N")
                    myRow.AddTextCell("<input type=checkbox value=N disabled>", 5);
                else
                    myRow.AddTextCell("<input type=checkbox value=Y checked disabled>", 5);
                //群組
                myTempMsg.LoadXml();
                //加入群組的Item
                myTempMsg.CreateNode("LinkButton");
                myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/add.gif");
                myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0001", "ADTPurDoc/GroupManage"));
                myTempMsg.AddToNode("href", USERJOINGROUP + "?ApID=" + Request.Params["ApID"].ToString());
                myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                myTempMsg.AddToNode("Method", GetMethod("MasterGroupFunc", "user_id", "user_desc", myMasterRow));
                myTempMsg.UpdateNode();
                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 5);
                //使用者管理
                myTempMsg.LoadXml();
                //先加入修改的Item
                if (myAuth.IsUpdateAuth()) //判斷是否有修改使用者的權限 20041118
                {
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/edit.gif");
                    myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0028", "ADTPurDoc/GroupManage"));
                    //myTempMsg.AddToNode("href",USERADDNEW + "?ApID=" + Request.Params["ApID"].ToString());
                    //Modify by Lemor Beacause must redirect to PfsBaseWeb
                    myTempMsg.AddToNode("href", USERADDNEW + "?ApID=" + Request.Params["ApID"].ToString() + "&UserID=" + myMasterRow["user_id"].ToString() + "&AcctionType=UpdByAdmin&UserAccount=" + CheckDBNull(myMasterRow["email"]) + "&ul=" + ConfigurationManager.AppSettings["myServer"] + ConfigurationManager.AppSettings["vpath"] + "/SysManager/UserManage/UserManage104.aspx?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("Method", GetMethod("MasterUpdateFunc", "user_id", "user_desc", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                //再加入刪除的Item
                if (myAuth.IsDeleteAuth()) //判斷是否有修改使用者的權限 20041118
                {
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                    myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0029", "ADTPurDoc/GroupManage"));
                    myTempMsg.AddToNode("href", USERMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                    myTempMsg.AddToNode("Method", GetMethod("MasterDelFunc", "user_id", "user_desc", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                if (myMasterRow["remark"].ToString() != "N")
                {
                    //如果是不受群組管控則再加入回歸群組的Item
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/mana.gif");
                    myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0027", "ADTPurDoc/GroupManage"));
                    myTempMsg.AddToNode("href", USERMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                    myTempMsg.AddToNode("Method", GetMethod("MasterReturnFunc", "user_id", "user_desc", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 5);

                tblApUser.Rows.Add(myRow.Row);

                tblMenuByUserID = mybs.DoReturnDataSet("GetMenudByUserID", "<PccMsg><ap_id>" + Request.QueryString["ApID"] + "</ap_id><user_id>" + myMasterRow["user_id"].ToString() + "</user_id></PccMsg>", "").Tables["MenuByUser"];

                //設定選單明細資料類別之參數
                PccDetailTable myDTable = new PccDetailTable("MDT" + ((int)((decimal)myMasterRow["user_id"])).ToString());
                myDTable.ClassXML = strClassXML;
                myDTable.Fields = Fields;
                myDTable.FieldsPercent = FieldsPercent;
                myDTable.FieldsItem = FieldsItem;
                myDTable.FieldsType = FieldsType;
                myDTable.NewDataTable = tblMenuByUserID;

                myDTable.Create();

                if (m_menuIDArray == null)
                {
                    m_menuIDArray = myDTable.GetDataArray("menu_id");
                    ViewState["MenuID"] = m_menuIDArray;
                }

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_M" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myDTable.NewTable, 100);

                tblApUser.Rows.Add(myRow.Row);

                MasterCount += 1;

            } // end of foreach datarow
        }// end of if table count is 0
        else
        {
            PageControl1.TotalSize = "0";
            PageControl1.BuildPager();
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

    private string CheckRequestForm(string strName)
    {
        if (Request.Form[strName] == null)
            return "";
        else
            return Request.Form[strName].ToString();
    }

    private string CheckRequestQueryString(string strName)
    {
        if (Request.QueryString[strName] == null)
            return "";
        else
            return Request.QueryString[strName].ToString();
    }

    private string CheckRequestParams(string strName)
    {
        if (Request.Params[strName] == null)
            return "";
        else
            return Request.Params[strName].ToString();
    }

    #endregion

    #region "更新選單的程式碼"

    private void UpdateMenuByUser(string strXML)
    {
        string menu_id = "";
        int i;

        m_menuIDArray = (string[])ViewState["MenuID"];
        for (i = 0; i < m_menuIDArray.Length; i++)
        {
            menu_id += m_menuIDArray[i] + ",";
        }

        if (menu_id == "")
        {
            GenMasterTable();
            return;
        }
        else
        {
            menu_id = menu_id.Substring(0, menu_id.Length - 1);
        }

        PccMsg myMsg = new PccMsg(strXML);
        string strRequestFormName = myMsg.Query("btnID").Split('-')[1];
        string strShow = CheckRequestForm("show_mk" + strRequestFormName);
        string strAdd = CheckRequestForm("add_mk" + strRequestFormName);
        string strUpd = CheckRequestForm("upd_mk" + strRequestFormName);
        string strDel = CheckRequestForm("del_mk" + strRequestFormName);
        string strRpt = CheckRequestForm("rpt_mk" + strRequestFormName);
        string strSend = CheckRequestForm("send_mk" + strRequestFormName);

        string show_mk, add_mk, upd_mk, del_mk, rpt_mk, send_mk;
        string user_id;

        user_id = strRequestFormName.Substring(3);
        show_mk = CompareTwoStrArray(m_menuIDArray, strShow, "Y,", "N,");
        add_mk = CompareTwoStrArray(m_menuIDArray, strAdd, "Y,", "N,");
        upd_mk = CompareTwoStrArray(m_menuIDArray, strUpd, "Y,", "N,");
        del_mk = CompareTwoStrArray(m_menuIDArray, strDel, "Y,", "N,");
        rpt_mk = CompareTwoStrArray(m_menuIDArray, strRpt, "Y,", "N,");
        send_mk = CompareTwoStrArray(m_menuIDArray, strSend, "Y,", "N,");

        myMsg.LoadXml();

        myMsg.CreateFirstNode("user_id", user_id);
        myMsg.CreateFirstNode("menu_str", menu_id);
        myMsg.CreateFirstNode("show_str", show_mk);
        myMsg.CreateFirstNode("add_str", add_mk);
        myMsg.CreateFirstNode("upd_str", upd_mk);
        myMsg.CreateFirstNode("del_str", del_mk);
        myMsg.CreateFirstNode("rpt_str", rpt_mk);
        myMsg.CreateFirstNode("send_str", send_mk);

        bs_UserManager mybs = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs.DoReturnStr("UpdateMenudByUserID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            GenMasterTable();
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
        }

    }



    private string CompareTwoStrArray(string[] strSrc, string strDest, string strExistValue, string strNoExistValue)
    {
        int i, j;
        string strReturn = "";
        bool bExist = false;
        string[] tempArray;

        for (i = 0; i < strSrc.Length; i++)
        {
            if (strDest == "")
            {
                strReturn += strNoExistValue;
            }
            else
            {
                tempArray = strDest.Split(',');
                for (j = 0; j < tempArray.Length; j++)
                {
                    if (m_menuIDArray[i] == tempArray[j])
                    {
                        strReturn += strExistValue;
                        bExist = true;
                        break;
                    }
                }
                if (!bExist)
                {
                    strReturn += strNoExistValue;
                }
                bExist = false;
            }
        }
        strReturn = strReturn.Substring(0, strReturn.Length - 1);
        return strReturn;
    }

    #endregion

    #region "回歸群組的程式碼"

    private void MasterReturnFunc(string strKey, string strKeyOther)
    {
        plReturnGroup.Visible = true;
        plMain.Visible = false;
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        string[] strParameters = { "<b>" + strKeyOther + "</b>" };
        lblReturnMsg.Text = myErrMsg.GetErrMsgWithParam("msg0041", strParameters);
        btnReturnOK.Text = myLabel.GetErrMsg("btnOK");
        btnReturnCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = "MasterReturnFunc";
    }

    #endregion

    #region "刪除使用者的程式碼"

    private void MasterDelFunc(string strKey, string strKeyOther)
    {
        plDelete.Visible = true;
        plMain.Visible = false;
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        string[] strParameters = { "<b>" + strKeyOther + "</b>" };
        lblDelMsg.Text = myErrMsg.GetErrMsgWithParam("msg0040", strParameters);
        btnDelOK.Text = myLabel.GetErrMsg("btnOK");
        btnDelCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = "MasterDelFunc";
    }

    #endregion
}
