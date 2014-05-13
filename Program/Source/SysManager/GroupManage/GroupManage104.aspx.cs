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


public partial class SysManager_GroupManage_GroupManage104 : System.Web.UI.Page
{
    private const string USERADDCOMEON = "UserAddComeOn104.aspx";
    private const string GROUPMANAGE = "GroupManage104.aspx";
    private const string GROUPADDNEW = "GroupAddNew104.aspx";

    private string[] m_menuIDArray;
    private string m_apid = string.Empty;

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁

        GetMenuAuth myAuth = new GetMenuAuth();
        if (myAuth.IsAddAuth())
        {
            lbtnAddGroup.Visible = true;
        }

        m_apid = Request.QueryString["ApID"].ToString();
        PccMsg myMsg = new PccMsg();
        string MethodFunc = "";

        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);

            //設定之前User鍵入的查詢資料。 2004/3/10
            if (CheckQueryString("QueryCondition") != "")
            {
                myMsg.LoadXml(CheckQueryString("QueryCondition"));
                txtGroupName.Text = myMsg.Query("QueryCondition/txtGroupName");
            }

            GenMasterTable(ref myLabel);

            if (CheckQueryString("Method") != "")
            {
                try
                {
                    myMsg.LoadXml(CheckQueryString("Method"));
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
                    case "DeleteUserByGroup":
                        DeleteUserByGroup(txtReturn.Value);
                        break;
                    case "UpdateMenuByGroup":
                        UpdateMenuByGroup(txtReturn.Value);
                        break;
                }
            }
        }

    }

    protected void lbtnAddGroup_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(GROUPADDNEW + "?ApID=" + CheckQueryString("ApID"));
    }

    protected void btnDelCancel_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;
        GenMasterTable();
    }

    protected void btnDelOK_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;

        PccMsg myMsg = new PccMsg(CheckQueryString("Method"));
        string strGroupID = myMsg.Query("Key");
        myMsg.LoadXml();
        myMsg.CreateFirstNode("group_id", strGroupID);
        myMsg.CreateFirstNode("ap_id", CheckQueryString("ApID"));

        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs.DoReturnStr("DeleteGroupByGroupID", myMsg.GetXmlStr, "");

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

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }

    protected void btnClear_Click(object sender, System.EventArgs e)
    {
        txtGroupName.Text = "";
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
        btnClear.Text = myLabel.GetErrMsg("btnClear");

        //lblTitle.Text = myLabel.GetErrMsg("lbl0001","ADTPurDoc/GroupManage");
        lblGroupName.Text = myLabel.GetErrMsg("lbl0002", "ADTPurDoc/GroupManage");
        lbtnAddGroup.Text = myLabel.GetErrMsg("lbl0003", "ADTPurDoc/GroupManage");
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

        PccRow myRow = new PccRow("cssGridHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //編號
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0004", "ADTPurDoc/GroupManage"), 10);
        //群組名稱(點選群組名稱可把使用者加入此群組)
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0022", "ADTPurDoc/GroupManage"), 60);
        //檢視使用者
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0020", "ADTPurDoc/GroupManage"), 10);
        //檢視選單
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0021", "ADTPurDoc/GroupManage"), 10);
        //群組管理
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0001", "ADTPurDoc/GroupManage"), 10);

        tblGroup.CssClass = "cssGridTable";
        tblGroup.Width = Unit.Percentage(100);
        tblGroup.HorizontalAlign = HorizontalAlign.Center;
        tblGroup.CellPadding = 2;
        tblGroup.CellSpacing = 1;

        tblGroup.Rows.Add(myRow.Row);
    }

    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("txtGroupName", txtGroupName.Text);
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
        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("ap_id", Request.QueryString["ApID"]);
        myMsg.CreateFirstNode("group_nm", txtGroupName.Text);

        //加入判斷是否要有事業群之判斷20041116
        string strGroupFilter = "N", strFactFilter = "N";
        string strTemp = "";
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
                strGroupFilter = "Y";
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
            strFactFilter = "Y";
        }


        string strXML = myMsg.GetXmlStr;
        DataSet myGroupDs = mybs.DoReturnDataSet("GetGroupbyApID", strXML, "");

        DataTable myGroupData = myGroupDs.Tables["Group"];

        if (myGroupData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";
            PccMsg myTempMsg;

            PccRow myRow;
            //取出資料總筆數
            PageControl1.TotalSize = myGroupDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //明細資料類別之共同參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            //使用者明細資料類別之參數
            string strOrgFields5 = "";
            if (myAuth.IsDeleteAuth())
            {
                strOrgFields5 = "btn-刪除-Return_Click(this)";
            }
            string[] Fields = { "編號", "電子郵件帳號", "使用者名稱", "廠別", strOrgFields5 };
            int[] FieldsPercent = { 5, 30, 20, 35, 10 };
            string[] FieldsItem = { "--NO--", "email", "user_desc", "fact_nm", "user_id" };
            //string[] TransFunc = {"","TransUserNm","TransUserDesc","TransFactNm",""};
            string[] FieldsType = new string[5];
            FieldsType[0] = "--NO--";
            FieldsType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[3] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[4] = "<PccMsg><Type>CheckBox</Type><Value>user_id</Value></PccMsg>";

            //選單明細資料類別之參數
            string strOrgFields8 = "";
            if (myAuth.IsUpdateAuth())
            {
                strOrgFields8 = "btn-更新送出-Return_ClickM(this)"; ;
            }

            //判斷是否為超級管理者，若是則不控管自己Menu的CheckBox權限 20041119
            string strcheckbox = "";
            if (Session["UserEMail"].ToString() != ConfigurationManager.AppSettings["superAdminEmail"].ToString())
                strcheckbox = "CheckBoxByValueJudgeUserRight";
            else
                strcheckbox = "CheckBoxByValue";

            string[] MFields = { "編號", "選單名稱", "顯示權限", "新增權限", "更新權限", "刪除權限", "報表權限", "發信權限", strOrgFields8 };
            int[] MFieldsPercent = { 5, 25, 10, 10, 10, 10, 10, 10, 10 };
            string[] MFieldsItem = { "--NO--", "menu_nm", "show_mk", "add_mk", "upd_mk", "del_mk", "rpt_mk", "send_mk", "groupd_id" };
            string[] MFieldsType = new string[9];
            MFieldsType[0] = "--NO--";
            MFieldsType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            MFieldsType[2] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[3] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[4] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[5] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[6] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[7] = "<PccMsg><Type>" + strcheckbox + "</Type><Value>menu_id</Value></PccMsg>";
            MFieldsType[8] = "<PccMsg><Type>Space</Type></PccMsg>";


            //要取得明細資料的Table變數
            DataTable tblUserGroup, tblMenuGroup;

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myGroupData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "cssGridRowAlternating"; else MasterStyle = "cssGridRow";
                myRow = new PccRow("", HorizontalAlign.Center, 0, 0);
                myRow.SetRowCss(MasterStyle);

                //編號
                myRow.AddTextCell(PageControl1.ListCount, 10);
                //群組名稱(點選群組名稱可把使用者加入此群組)
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myMasterRow["group_nm"].ToString());
                myTempMsg.CreateFirstNode("LinkID", "ULink" + myMasterRow["group_id"].ToString());
                myTempMsg.CreateFirstNode("Href", USERADDCOMEON + "?ApID=" + CheckQueryString("ApID") + "&GroupID=" + myMasterRow["group_id"].ToString() + "&GroupNm=" + myMasterRow["group_nm"].ToString() + "&QueryCondition=" + GetQueryCondition());
                myTempMsg.CreateFirstNode("Text", myMasterRow["group_nm"].ToString());
                myRow.AddLinkHrefCell(myTempMsg.GetXmlStr, 60);
                //檢視使用者
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0020", "ADTPurDoc/GroupManage"));
                myTempMsg.CreateFirstNode("LinkID", "HLinkViewU" + myMasterRow["group_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_U" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 10);
                //檢視選單
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0021", "ADTPurDoc/GroupManage"));
                myTempMsg.CreateFirstNode("LinkID", "HLinkViewM" + myMasterRow["group_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_M" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 10);
                //群組管理
                myTempMsg.LoadXml();
                //加入群組管理修改及刪除的Item
                if (myAuth.IsUpdateAuth())
                {
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/edit.gif");
                    myTempMsg.AddToNode("ToolTip", "修改群組");
                    myTempMsg.AddToNode("href", GROUPADDNEW + "?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                    myTempMsg.AddToNode("Method", GetMethod("MasterUpdateFunc", "group_id", "group_nm", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                if (myAuth.IsDeleteAuth())
                {
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                    myTempMsg.AddToNode("ToolTip", "刪除群組");
                    myTempMsg.AddToNode("href", GROUPMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                    myTempMsg.AddToNode("Method", GetMethod("MasterDelFunc", "group_id", "group_nm", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 10);

                tblGroup.Rows.Add(myRow.Row);


                //設定使用者明細資料類別之參數
                strTemp = "<PccMsg>";
                strTemp += "<group_id>" + myMasterRow["group_id"].ToString() + "</group_id>";
                strTemp += "<ap_id>" + m_apid + "</ap_id>";
                strTemp += "<user_id>" + Session["UserID"].ToString() + "</user_id>";
                strTemp += "<GroupFilter>" + strGroupFilter + "</GroupFilter>";
                strTemp += "<FactFilter>" + strFactFilter + "</FactFilter>";
                strTemp += "</PccMsg>";
                tblUserGroup = mybs.DoReturnDataSet("GetUserByGroupID", strTemp, "").Tables["UserGroup"];

                PccDetailTable myDTable = new PccDetailTable("UDT" + myMasterRow["group_id"].ToString());
                myDTable.ClassXML = strClassXML;
                myDTable.Fields = Fields;
                myDTable.FieldsPercent = FieldsPercent;
                myDTable.FieldsItem = FieldsItem;
                myDTable.FieldsType = FieldsType;
                myDTable.NewDataTable = tblUserGroup;

                //myDTable.TransFunc = TransFunc;
                //myDTable.objThis = this;

                myDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_U" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myDTable.NewTable, 100);

                tblGroup.Rows.Add(myRow.Row);


                //設定選單明細資料類別之參數
                tblMenuGroup = mybs.DoReturnDataSet("GetMenuByGroupID", "<PccMsg><group_id>" + myMasterRow["group_id"].ToString() + "</group_id></PccMsg>", "").Tables["MenuGroup"];

                PccDetailTable myMDTable = new PccDetailTable("MDT" + myMasterRow["group_id"].ToString());
                myMDTable.ClassXML = strClassXML;
                myMDTable.Fields = MFields;
                myMDTable.FieldsPercent = MFieldsPercent;
                myMDTable.FieldsItem = MFieldsItem;
                myMDTable.FieldsType = MFieldsType;
                myMDTable.NewDataTable = tblMenuGroup;

                myMDTable.Create();

                if (m_menuIDArray == null)
                {
                    m_menuIDArray = myMDTable.GetDataArray("menu_id");
                    ViewState["MenuID"] = m_menuIDArray;
                }

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_M" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myMDTable.NewTable, 100);

                tblGroup.Rows.Add(myRow.Row);

                MasterCount += 1;

            } // end of foreach datarow
        } // end of if table count is 0
        else
        {
            PageControl1.TotalSize = "0";
            PageControl1.BuildPager();
        }

    }

    #endregion

    #region "按鈕之後所傳回來的Method Functio"

    private void DeleteUserByGroup(string strXML)
    {
        PccMsg myMsg = new PccMsg(strXML);
        string strRequestFormName = myMsg.Query("btnID");
        strRequestFormName = strRequestFormName.Split('-')[1];
        string strUserID = CheckForm(strRequestFormName);

        if (strUserID != "")
        {
            string group_id = strRequestFormName.Substring(3);
            myMsg.ClearContext();
            myMsg.CreateFirstNode("group_id", group_id);
            myMsg.CreateFirstNode("user_str", strUserID);

            bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
            string strReturn = mybs.DoReturnStr("DeleteUGrp", myMsg.GetXmlStr, "");

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
        else
        {
            GenMasterTable();
        }
    }

    private void UpdateMenuByGroup(string strXML)
    {
        string menu_id = "";
        int i;
        m_menuIDArray = (string[])ViewState["MenuID"];
        for (i = 0; i < m_menuIDArray.Length; i++)
        {
            menu_id += m_menuIDArray[i] + ",";
        }
        if (menu_id != "")
        {
            menu_id = menu_id.Substring(0, menu_id.Length - 1);
        }
        else
        {
            GenMasterTable();
            return;
        }

        PccMsg myMsg = new PccMsg(strXML);
        string strRequestFormName = myMsg.Query("btnID").Split('-')[1];
        string strShow = CheckForm("show_mk" + strRequestFormName);
        string strAdd = CheckForm("add_mk" + strRequestFormName);
        string strUpd = CheckForm("upd_mk" + strRequestFormName);
        string strDel = CheckForm("del_mk" + strRequestFormName);
        string strRpt = CheckForm("rpt_mk" + strRequestFormName);
        string strSend = CheckForm("send_mk" + strRequestFormName);

        string show_mk, add_mk, upd_mk, del_mk, rpt_mk, send_mk;
        string group_id = strRequestFormName.Substring(3);
        show_mk = CompareTwoStrArray(m_menuIDArray, strShow, "Y,", "N,");
        add_mk = CompareTwoStrArray(m_menuIDArray, strAdd, "Y,", "N,");
        upd_mk = CompareTwoStrArray(m_menuIDArray, strUpd, "Y,", "N,");
        del_mk = CompareTwoStrArray(m_menuIDArray, strDel, "Y,", "N,");
        rpt_mk = CompareTwoStrArray(m_menuIDArray, strRpt, "Y,", "N,");
        send_mk = CompareTwoStrArray(m_menuIDArray, strSend, "Y,", "N,");

        myMsg.LoadXml();

        myMsg.CreateFirstNode("group_id", group_id);
        myMsg.CreateFirstNode("menu_str", menu_id);
        myMsg.CreateFirstNode("show_str", show_mk);
        myMsg.CreateFirstNode("add_str", add_mk);
        myMsg.CreateFirstNode("upd_str", upd_mk);
        myMsg.CreateFirstNode("del_str", del_mk);
        myMsg.CreateFirstNode("rpt_str", rpt_mk);
        myMsg.CreateFirstNode("send_str", send_mk);

        bs_GroupManage mybs = new bs_GroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = mybs.DoReturnStr("UpdateGroupdByGroupID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            GenMasterTable();
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "New","<script language=javascript>alert('" + myMsg.Query("errmsg") + "');</script>");
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

    #region "刪除群組的程式碼"

    private void MasterDelFunc(string strKey, string strKeyOther)
    {
        plDelete.Visible = true;
        plMain.Visible = false;
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        string[] strParameters = { "<b>" + strKeyOther + "</b>" };
        lblDelMsg.Text = myErrMsg.GetErrMsgWithParam("msg0039", strParameters);
        btnDelOK.Text = myLabel.GetErrMsg("btnOK");
        btnDelCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = "MasterDelFunc";
    }

    #endregion

}
