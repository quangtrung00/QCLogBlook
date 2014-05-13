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
using System.Xml;

public partial class SysManager_FactGroupManage_FactGroupDetail104 : System.Web.UI.Page
{
    private string m_SrcUp_Id;
    private string m_Up_Id;
    private string m_ApID;
    private string m_FgrpId;
    private string m_FgrpFactId;
    private string m_FactGrpId;
    private string m_UserGrpId;

    private string m_CurrentNodeIndex;
    private DataTable m_Table;

    private const string MYURL = "FactGroupDetail104.aspx";
    private const string PICKFACT = "PickFactToFgrp104.aspx";
    private const string PICKUSER = "PickUserToFgrp104.aspx";

    private const string FACTGROUPMAANGE = "FactGroupManage104.aspx";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // 將使用者程式碼置於此以初始化網頁
        if (Session["UserID"] == null) return;

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = "FactGroupManage104.aspx";

        //設定所要做的動作
        string strAction = string.Empty;

        if (myAuth.IsAddAuth())
            LbtnAddNewFgrp.Visible = true;

        m_ApID = Request.Params["ApID"];

        if (CheckQueryString("SrcUp_Id") != "")
        {
            m_SrcUp_Id = Request.Params["SrcUp_Id"];
        }

        if (CheckQueryString("Up_Id") != "")
        {
            m_Up_Id = Request.Params["Up_Id"];
        }

        PccMsg myMsg = new PccMsg();
        PccMsg myQueryMsg = new PccMsg();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");

        if (CheckQueryString("Method") != "")
        {
            myMsg.LoadXml(Request.QueryString["Method"]);
            strAction = myMsg.Query("Method");
            switch (strAction)
            {
                case ("MasterUpdateFunc"):
                    m_FgrpId = myMsg.Query("Key");
                    break;
                case ("MasterDelFunc"):
                    m_FgrpId = myMsg.Query("Key");
                    break;
                case ("UpdFgrpFact"):
                    m_FgrpFactId = myMsg.Query("Key");
                    break;
                case "FactDelFunc":
                    m_FactGrpId = myMsg.Query("Key");
                    break;
                case "UserDelFunc":
                    m_UserGrpId = myMsg.Query("Key");
                    break;
            }

        }

        if (!IsPostBack)
        {
            string SrcUp_Id = string.Empty;
            if (Request.Params["SrcUp_Id"] != null)
            {
                SrcUp_Id = Request.Params["SrcUp_Id"].ToString();
                GenTree(ref TreeView1);
                //利用GetNodeIndex去設定m_CurrentNodeIndex的值，不需要再去設定一次Selected Lemor 20080312
                string aaa = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                //TreeView1.FindNode(GetNodeIndex(TreeView1.Nodes, SrcUp_Id)).Selected = true;
                //TreeView1.SelectedValue = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                //TreeView1.SelectedNodeIndex = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                if (m_CurrentNodeIndex != null)
                {
                    string[] a;
                    a = m_CurrentNodeIndex.Split('/');
                    TreeView1.ExpandDepth = a.Length - 1;
                    //TreeView1.ExpandLevel = a.Length - 1;
                    TreeView1.FindNode(m_CurrentNodeIndex).Selected = true;
                    TreeView1.FindNode(m_CurrentNodeIndex).Expand();

                    //TreeView1.SelectedValue = m_CurrentNodeIndex;
                    //TreeView1.SelectedNodeIndex = m_CurrentNodeIndex;
                }
            }

            //設定之前User鍵入的查詢資料。 2005/5/23
            if (CheckQueryString("QueryCondition") != "")
            {
                myQueryMsg.LoadXml(CheckQueryString("QueryCondition"));
                TxtQueryFgrpNm.Text = myQueryMsg.Query("QueryCondition/TxtQueryFgrpNm");
            }



            SetLabel(ref myLabel);
            switch (strAction)
            {
                case ("MasterUpdateFunc"):
                    ViewState["ActionType"] = "MasterUpdateFunc";
                    PalFgrp.Visible = false;
                    PalAddFgrp.Visible = true;
                    TxtFgrpNm.Text = myMsg.Query("KeyOther");
                    break;
                case ("MasterDelFunc"):
                    ViewState["ActionType"] = "MasterDeleteFunc";
                    PalFgrp.Visible = false;
                    PalAddFgrp.Visible = false;
                    PalDelFgrp.Visible = true;
                    LblDelMsg.Text = "注意：此動作會刪除此廠群組(<b>" + myMsg.Query("KeyOther") + "</b>)以下的所有廠群組及其相關之廠別和使用者！";
                    break;
                case ("UpdFgrpFact"):
                    m_FgrpFactId = myMsg.Query("Key");
                    break;
                case "FactDelFunc":
                    ViewState["ActionType"] = "FactDelFunc";
                    PalFgrp.Visible = false;
                    PalAddFgrp.Visible = false;
                    PalDelFgrp.Visible = false;
                    PalDelFact.Visible = true;
                    LblDelFactMsg.Text = "注意：此動作會刪除此廠群組(<b>" + CheckQueryString("Fgrp_Nm") + "</b>)以下的廠別(<b>" + myMsg.Query("KeyOther") + "</b>)！";
                    break;
                case "UserDelFunc":
                    ViewState["ActionType"] = "UserDelFunc";
                    PalFgrp.Visible = false;
                    PalAddFgrp.Visible = false;
                    PalDelFgrp.Visible = false;
                    PalDelFact.Visible = false;
                    PalDelUser.Visible = true;
                    LblDelUserMsg.Text = "注意：此動作會刪除此廠群組(<b>" + CheckQueryString("Fgrp_Nm") + "</b>)以下的使用者(<b>" + myMsg.Query("KeyOther") + "</b>)之權限！";
                    break;
                default:
                    GenMasterTable();
                    break;
            }

        }
        else
        {
            string SrcUp_Id = string.Empty;
            if (Request.Params["SrcUp_Id"] != null)
            {
                SrcUp_Id = Request.Params["SrcUp_Id"].ToString();
                GenTree(ref TreeView1);
                //利用GetNodeIndex去設定m_CurrentNodeIndex的值，不需要再去設定一次Selected Lemor 20080312
                string aaa = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                //TreeView1.FindNode(GetNodeIndex(TreeView1.Nodes, SrcUp_Id)).Selected = true;
                //TreeView1.SelectedValue = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                //TreeView1.SelectedNodeIndex = GetNodeIndex(TreeView1.Nodes, SrcUp_Id);
                if (m_CurrentNodeIndex != null)
                {
                    string[] a;
                    a = m_CurrentNodeIndex.Split('/');
                    TreeView1.ExpandDepth = a.Length - 1;
                    //TreeView1.ExpandLevel = a.Length - 1;
                    TreeView1.FindNode(m_CurrentNodeIndex).Selected = true;
                    TreeView1.FindNode(m_CurrentNodeIndex).Expand();
                    //TreeView1.SelectedValue = m_CurrentNodeIndex;
                    //TreeView1.SelectedNodeIndex = m_CurrentNodeIndex;
                }
            }
        }

    }

    #region 設定此頁面的資料

    private void SetLabel(ref PccErrMsg myLabel)
    {
        //LblFgrp_Nm.Text = myLabel.GetErrMsg("lbl0002","SysManager/ApManager"); 
        BtnQuery.Text = myLabel.GetErrMsg("btnQuery");
    }



    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (TblDs_Fgrp.Rows.Count > 0)
            TblDs_Fgrp.Rows.Clear();
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTable()
    {
        if (TblDs_Fgrp.Rows.Count > 0)
            TblDs_Fgrp.Rows.Clear();
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }



    private void GenMasterTableHader(ref PccErrMsg myLabel)
    {
        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //編號
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0007", "SysManager/ApManager"), 5);
        //廠群組名稱
        myRow.AddTextCell("廠群組名稱(點選群組名稱可加入廠別)", 56);
        //廠別檢視
        myRow.AddTextCell("廠別檢視", 12);
        //使用者檢視
        myRow.AddTextCell("使用者檢視", 12);
        //管理
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0013", "SysManager/ApManager"), 15);

        TblDs_Fgrp.CssClass = "ActDocTB";
        TblDs_Fgrp.Width = Unit.Percentage(100);
        TblDs_Fgrp.HorizontalAlign = HorizontalAlign.Center;
        TblDs_Fgrp.CellPadding = 2;
        TblDs_Fgrp.CellSpacing = 1;
        TblDs_Fgrp.Rows.Add(myRow.Row);
    }



    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("TxtQueryFgrpNm", TxtQueryFgrpNm.Text);
        myMsg.UpdateNode();
        return myMsg.GetXmlStr;
    }



    private string GetMethod(string strMethod, string Key, string KeyOther, DataRow myRow)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Method", strMethod);
        myMsg.CreateFirstNode("Key", myRow["Fgrp_Id"].ToString());
        myMsg.CreateFirstNode("KeyOther", myRow["Fgrp_Nm"].ToString());
        return myMsg.GetXmlStr;
    }



    #endregion

    #region 上下頁按鈕的動作

    protected void OnPageClick(object source, EventArgs e)
    {
        GenMasterTable();
    }

    #endregion

    #region 設定主Table的資料

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        #region 取得Login的使用者是否有刪除的權限

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = "FactGroupManage104.aspx";
        bool del_auth = myAuth.IsDeleteAuth();

        #endregion

        #region 取得廠群組的主資料ByUpID

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("Up_Id", m_SrcUp_Id);
        myMsg.CreateFirstNode("Ap_Id", m_ApID);
        myMsg.CreateFirstNode("Fgrp_Nm", TxtQueryFgrpNm.Text);
        string strXML = myMsg.GetXmlStr;

        DataSet myDS_FGRPDs = mybs.DoReturnDataSet("Get_DS_FGRPByUpID", strXML, "");
        DataTable myDS_FGRPData = myDS_FGRPDs.Tables["DS_FGRPByUpID"];

        #endregion

        if (myDS_FGRPData != null && myDS_FGRPData.Rows.Count > 0)
        {
            #region 定義基本變數

            int MasterCount = 0;
            string MasterStyle = string.Empty;
            PccMsg myTempMsg;
            PccMsg myMsg1;
            PccRow myRow;
            //取出資料總筆數
            PageControl1.TotalSize = myDS_FGRPDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            #endregion

            #region 明細資料類別之共同參數

            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            XmlElement myNode;
            //要取得明細資料的Table變數
            DataTable tblUserGroup, tblFactGroup;

            #endregion

            #region 內建廠別明細資料的欄位參數

            string[] FFields = { "編號", "廠別", "廠別名稱", "事業群", "公司編號", "刪除" };
            int[] FFieldsPercent = { 5, 15, 35, 20, 15, 10 };
            string[] FFieldsItem = { "--NO--", "fact_no", "fact_nm", "fgrp_nm", "comp_no", "factgrp_id" };
            string[] FFieldType = new string[6];
            FFieldType[0] = "--NO--";
            FFieldType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[3] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[4] = "<PccMsg><Type>Text</Type></PccMsg>";

            #endregion

            #region 內建使用者明細資料的欄位參數

            string[] UFields = { "編號", "電子郵件帳號", "姓名", "廠別", "事業群", "分機", "刪除" };
            int[] UFieldsPercent = { 5, 30, 15, 15, 15, 10, 10 };
            string[] UFieldsItem = { "--NO--", "email", "user_desc", "fact_no", "fgrp_nm", "ext", "usergrp_id" };
            string[] UFieldType = new string[7];
            UFieldType[0] = "--NO--";
            UFieldType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            UFieldType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            UFieldType[3] = "<PccMsg><Type>Text</Type></PccMsg>";
            UFieldType[4] = "<PccMsg><Type>Text</Type></PccMsg>";
            UFieldType[5] = "<PccMsg><Type>Text</Type></PccMsg>";

            #endregion

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myDS_FGRPData.Rows)
            {
                #region 主要廠群組的資料列設定

                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow("", HorizontalAlign.Center, 0, 0);
                myRow.SetRowCss(MasterStyle);
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //廠群組名稱
                if (myAuth.IsAddAuth())
                {
                    myTempMsg = new PccMsg();
                    myTempMsg.CreateFirstNode("Href", PICKFACT + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString() + "&Method=" + GetMethod("PickFactToFgrpFunc", "Fgrp_Id", "Fgrp_Nm", myMasterRow) + "&QueryCondition=" + GetQueryCondition());
                    myTempMsg.CreateFirstNode("Text", myMasterRow["Fgrp_Nm"].ToString().Trim());
                    myRow.AddLinkHrefCell(myTempMsg.GetXmlStr, 56);
                }
                else
                {
                    myRow.AddTextCell(myMasterRow["Fgrp_Nm"].ToString(), 56);
                }

                //廠別檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", "檢視所屬廠別");
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["Fgrp_Id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_F" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 12);
                //使用者檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", "檢視所屬使用者");
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["Fgrp_Id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_U" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 12);
                //管理
                myTempMsg = new PccMsg();
                //判斷是否和上層ID是一樣的
                if (myMasterRow["Fgrp_Id"].ToString() != Request.Params["SrcUp_Id"])
                {
                    //先加入修改的Item
                    if (myAuth.IsUpdateAuth())
                    {
                        myTempMsg.CreateNode("LinkButton");
                        myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/edit.gif");
                        myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0014", "SysManager/ApManager"));
                        myTempMsg.AddToNode("href", MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString());
                        myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                        myTempMsg.AddToNode("Method", GetMethod("MasterUpdateFunc", "Fgrp_Id", "Fgrp_Nm", myMasterRow));
                        myTempMsg.UpdateNode();
                    }
                    if (del_auth)
                    {
                        //再加入刪除的Item
                        myTempMsg.CreateNode("LinkButton");
                        myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                        myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0015", "SysManager/ApManager"));
                        myTempMsg.AddToNode("href", MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString());
                        myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                        myTempMsg.AddToNode("Method", GetMethod("MasterDelFunc", "Fgrp_Id", "Fgrp_Nm", myMasterRow));
                        myTempMsg.UpdateNode();
                    }
                    //再加入加入使用者的Item
                    if (myAuth.IsAddAuth())
                    {
                        myTempMsg.CreateNode("LinkButton");
                        myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/add.gif");
                        myTempMsg.AddToNode("ToolTip", "加入使用者");
                        myTempMsg.AddToNode("href", PICKUSER + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString());
                        myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                        myTempMsg.AddToNode("Method", GetMethod("AddUserFunc", "Fgrp_Id", "Fgrp_Nm", myMasterRow));
                        myTempMsg.UpdateNode();
                    }
                }

                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 15);
                TblDs_Fgrp.Rows.Add(myRow.Row);

                #endregion

                #region 內建廠別明細資料的欄位參數，此處會取得資料，並設定給主表格

                //設定廠別明細欄位中多個Link Image的參數
                myMsg1 = new PccMsg();

                if (del_auth && myMasterRow["Fgrp_Id"].ToString() != Request.Params["SrcUp_Id"])
                {
                    myMsg1.CreateFirstNode("Type", "MultiLink");

                    myMsg1.CreateNode("LinkButton");
                    myMsg1.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                    myMsg1.AddToNode("ToolTip", "刪除此廠別與此廠群組之關係");
                    myMsg1.AddToNode("href", MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString() + "&Fgrp_Nm=" + myMasterRow["Fgrp_Nm"].ToString());
                    myMsg1.AddToNode("QueryCondition", GetQueryCondition());
                    myNode = myMsg1.CreateParentNode("Method");
                    myMsg1.AddToNode("MethodName", "FactDelFunc", ref myNode);
                    myMsg1.AddToNode("Key", "factgrp_id", ref myNode);
                    myMsg1.AddToNode("KeyOther", "fact_no", ref myNode);
                    myMsg1.UpdateNode(myNode);
                    myMsg1.UpdateNode();

                }
                else
                {
                    myMsg1.CreateFirstNode("Type", "Space");
                }

                FFieldType[5] = myMsg1.GetXmlStr;

                //設定要取得廠別資料的XML
                myMsg1.LoadXml();
                myMsg1.CreateFirstNode("Fgrp_Id", myMasterRow["Fgrp_Id"].ToString());
                tblFactGroup = mybs.DoReturnDataSet("get_DS_FACTGRPByFgrpID", myMsg1.GetXmlStr, string.Empty).Tables["DS_FACTGRP"];

                PccDetailTable myFDTable = new PccDetailTable("FDT" + myMasterRow["Fgrp_Id"].ToString());
                myFDTable.ClassXML = strClassXML;
                myFDTable.Fields = FFields;
                myFDTable.FieldsPercent = FFieldsPercent;
                myFDTable.FieldsItem = FFieldsItem;
                myFDTable.FieldsType = FFieldType;
                myFDTable.NewDataTable = tblFactGroup;

                myFDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_F" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myFDTable.NewTable, 100);

                TblDs_Fgrp.Rows.Add(myRow.Row);

                #endregion

                #region 內建使用者明細資料的欄位參數，此處會取得資料，並設定給主表格

                //設定廠別明細欄位中多個Link Image的參數
                myMsg1 = new PccMsg();

                if (del_auth && myMasterRow["Fgrp_Id"].ToString() != Request.Params["SrcUp_Id"])
                {
                    myMsg1.CreateFirstNode("Type", "MultiLink");

                    myMsg1.CreateNode("LinkButton");
                    myMsg1.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                    myMsg1.AddToNode("ToolTip", "刪除此使用者與此廠群組之關係");
                    myMsg1.AddToNode("href", MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + myMasterRow["Up_Id"].ToString() + "&Fgrp_Nm=" + myMasterRow["Fgrp_Nm"].ToString());
                    myMsg1.AddToNode("QueryCondition", GetQueryCondition());
                    myNode = myMsg1.CreateParentNode("Method");
                    myMsg1.AddToNode("MethodName", "UserDelFunc", ref myNode);
                    myMsg1.AddToNode("Key", "usergrp_id", ref myNode);
                    myMsg1.AddToNode("KeyOther", "user_desc", ref myNode);
                    myMsg1.UpdateNode(myNode);
                    myMsg1.UpdateNode();

                }
                else
                {
                    myMsg1.CreateFirstNode("Type", "Space");
                }

                UFieldType[6] = myMsg1.GetXmlStr;

                //設定要取得廠別資料的XML
                myMsg1.LoadXml();
                myMsg1.CreateFirstNode("Fgrp_Id", myMasterRow["Fgrp_Id"].ToString());
                tblUserGroup = mybs.DoReturnDataSet("get_DS_USERGRPByFgrpID", myMsg1.GetXmlStr, string.Empty).Tables["DS_USERGRP"];

                PccDetailTable myUDTable = new PccDetailTable("UDT" + myMasterRow["Fgrp_Id"].ToString());
                myUDTable.ClassXML = strClassXML;
                myUDTable.Fields = UFields;
                myUDTable.FieldsPercent = UFieldsPercent;
                myUDTable.FieldsItem = UFieldsItem;
                myUDTable.FieldsType = UFieldType;
                myUDTable.NewDataTable = tblUserGroup;

                myUDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_U" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myUDTable.NewTable, 100);

                TblDs_Fgrp.Rows.Add(myRow.Row);

                #endregion

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

    #region 新增廠群組的Function及按鈕動作

    protected void BtnAddNewCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&QueryCondition=" + GetQueryCondition());
    }

    protected void LbtnAddNewFgrp_Click(object sender, System.EventArgs e)
    {
        PalAddFgrp.Visible = true;
        PalFgrp.Visible = false;
        ViewState["ActionType"] = "New";
    }

    protected void BtnAddNewOK_ServerClick(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg();
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = string.Empty;
        string strMsg = string.Empty;
        if (ViewState["ActionType"] != null && ViewState["ActionType"].ToString() == "New")
        {
            strReturn = mybs.DoReturnStr("Insert_DS_FGRP", GenDBXML(), string.Empty);
            strMsg = "新增成功";
        }
        else
        {
            strReturn = mybs.DoReturnStr("Update_DS_FGRP", GenDBXML(), string.Empty);
            strMsg = "修改成功";
        }
        myMsg.LoadXml(strReturn);
        string strUrl = FACTGROUPMAANGE + "?ApID=" + m_ApID + "&SelectedNode=" + TreeView1.SelectedValue;
        //string strUrl = FACTGROUPMAANGE + "?ApID=" + m_ApID + "&SelectedNode=" + TreeView1.SelectedNodeIndex;

        if (myMsg.Query("returnValue") == "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + strMsg + "');window.parent.location.href='" + strUrl + "';</script>");
        }
        else
        {
            LblErrMsg.Font.Size = FontUnit.Large;
            LblErrMsg.Text = myMsg.Query("errmsg");
        }

    }

    private string GenDBXML()
    {
        PccMsg myMsg = new PccMsg();

        myMsg.CreateFirstNode("Ap_Id", m_ApID);
        if (ViewState["ActionType"] != null && ViewState["ActionType"].ToString() != "New")
        {
            myMsg.CreateFirstNode("Fgrp_Id", m_FgrpId);
            myMsg.CreateFirstNode("Up_Id", m_Up_Id);
        }
        else
        {
            myMsg.CreateFirstNode("Fgrp_Id", string.Empty);
            myMsg.CreateFirstNode("Up_Id", m_SrcUp_Id);
        }

        if (m_FactGrpId != null)
        {
            myMsg.CreateFirstNode("FactGrp_Id", m_FactGrpId);
        }

        if (m_UserGrpId != null)
        {
            myMsg.CreateFirstNode("UserGrp_Id", m_UserGrpId);
        }

        myMsg.CreateFirstNode("Fgrp_Nm", TxtFgrpNm.Text);

        myMsg.CreateFirstNode("Upd_Id", Session["UserID"].ToString());
        return myMsg.GetXmlStr;
    }

    #endregion

    #region 查詢的動作

    protected void BtnQuery_Click(object sender, System.EventArgs e)
    {
        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }

    #endregion

    #region "Check Data if Null"

    private string CheckDBNull(object oFieldData)
    {
        if (Convert.IsDBNull(oFieldData))
            return string.Empty;
        else
            return oFieldData.ToString().Trim();
    }

    private string CheckQueryString(string strName)
    {
        if (Request.QueryString[strName] == null)
            return "";
        else
            return Request.QueryString[strName].ToString();
    }


    #endregion

    #region "建立基本的Tree架構"

    private string GetNodeIndex(TreeNodeCollection Nodes, string NodeID)
    {
        string strReturn = string.Empty;
        
        foreach (TreeNode node in Nodes)
        {
            if (node.Value == NodeID)
            {
                //因為是要取得這個節點的路徑，所以必須使用ValuePath，而不是Value (Lemor 20080311)
                strReturn = node.ValuePath;
                //strReturn = node.GetNodeIndex();
                m_CurrentNodeIndex = strReturn;
                break;
            }

            if (node.ChildNodes.Count > 0)
            {
                strReturn = GetNodeIndex(node.ChildNodes, NodeID);
            }
        }

        return strReturn;
    }

    private void GenTree(ref TreeView tree)
    {
        //先把Tree設為空白
        tree.Nodes.Clear();

        //先建立廠別樹的根(Root)寶成寶成國際集團
        TreeNode node = new TreeNode();
        node.Text = "寶成國際集團";
        node.ImageUrl = "../../Images/TreeRoot.gif";
        node.Value = "0";
        //node.ID = "0";
        tree.Nodes.Add(node);

        //取得這顆樹的所有資料
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", Request.Params["ApID"]);
        myMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("SuperAdmin", "N");

        DataSet ds = mybs.DoReturnDataSet("GetFgrpTree", myMsg.GetXmlStr, "");
        m_Table = ds.Tables["FgrpTree"];

        //取得這顆樹的第一層資料，條件為Up_ID = 0
        DataRow[] rows;
        rows = m_Table.Select("up_id = 0");

        //設定所要跑的迴圈變數 i
        int i;
        //設定這顆樹的根節點的集合，所有的節點由此往下長
        TreeNodeCollection RootNodes;
        RootNodes = tree.Nodes[0].ChildNodes;
        //RootNodes = tree.Nodes[0].Nodes;

        //跑第一層的迴圈
        for (i = 0; i < rows.Length; i++)
        {
            AddNode(ref RootNodes, int.Parse(rows[i]["Fgrp_Id"].ToString()), i);
        }
    }

    //建立日期： 2004/01/08
    //建立者  ： LemorYen
    //建立目的： 示範如何撰寫不規則樹的建立，Recurceive的實做方法
    //輸入資料： ParentNodes為所要加入節點的父節點，Tree_ID為這個節點的基本資料ID，Level表示現在是在這個層的第幾個節點
    //輸出資料： Boolean，若已結束，則為False，否則就為True
    private bool AddNode(ref TreeNodeCollection ParentNodes, int Branch_ID, int Level)
    {
        bool bReturn = true;
        DataRow[] rows;
        int i;
        TreeNode Node;

        //首先取得要加入的這一筆Node的資料，判斷如果沒有資料則Return False
        rows = m_Table.Select("Fgrp_Id = " + Branch_ID);

        if (rows.Length > 0)
        {
            //判斷若已加入過了，則跳出此迴圈，否則就加入這個Node
            //				if (rows[0]["IsOK"].ToString()  != "Y")
            //				{

            Node = new TreeNode();

            Node.Text = rows[0]["Fgrp_Nm"].ToString();

            Node.Value = rows[0]["Fgrp_Id"].ToString();
            //Node.ID = rows[0]["Fgrp_Id"].ToString();

            ParentNodes.Add(Node);

        }

        //取得屬於這個Node的所有(Child)子節點
        rows = m_Table.Select("Up_ID = " + Branch_ID);

        //判斷如果有子節點，就進入迴圈做加入節點的工作，如果沒有子節點了，則跳出這個Function並Return False
        if (rows.Length > 0)
        {
            //設定這顆樹的根節點的集合，所有的節點由此往下長
            TreeNodeCollection ParentNodes1;

            for (i = 0; i < rows.Length; i++)
            {
                //利用Recurceive的方式，使每一個節點都能新增其子節點，其結束點即是若已沒有子節點了，表示自已本身已
                //是葉子了，所以這個Recurceive就可結束了。
                ParentNodes1 = ParentNodes[Level].ChildNodes;
                //ParentNodes1 = ParentNodes[Level].Nodes;
                bReturn = AddNode(ref ParentNodes1, int.Parse(rows[i]["Fgrp_Id"].ToString()), i);
            }
        }

        return bReturn;

    }

    #endregion

    #region 刪除廠群組的動作

    protected void BtnDelCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + m_Up_Id + "&QueryCondition=" + GetQueryCondition());

    }

    protected void BtnDelOK_Click(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg();
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = string.Empty;
        string strMsg = string.Empty;
        strReturn = mybs.DoReturnStr("Delete_DS_FGRP", GenDBXML(), string.Empty);
        strMsg = "刪除成功";

        myMsg.LoadXml(strReturn);
        string strUrl = FACTGROUPMAANGE + "?ApID=" + m_ApID + "&SelectedNode=" + TreeView1.SelectedValue;
        //string strUrl = FACTGROUPMAANGE + "?ApID=" + m_ApID + "&SelectedNode=" + TreeView1.SelectedNodeIndex;

        if (myMsg.Query("returnValue") == "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + strMsg + "');window.parent.location.href='" + strUrl + "';</script>");
        }
        else
        {
            LblErrMsg.Font.Size = FontUnit.Large;
            LblErrMsg.Text = myMsg.Query("errmsg");
        }
    }


    #endregion

    #region 刪除廠群組之廠別

    protected void BtnDelFactCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + m_Up_Id + "&QueryCondition=" + GetQueryCondition());
    }

    protected void BtnDelFactOK_Click(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg();
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = string.Empty;
        string strMsg = string.Empty;
        strReturn = mybs.DoReturnStr("delete_DS_FACTGRP", GenDBXML(), string.Empty);
        strMsg = "刪除成功";

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + strMsg + "');</script>");
            Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + m_Up_Id + "&QueryCondition=" + GetQueryCondition());
        }
        else
        {
            LblErrMsg.Font.Size = FontUnit.Large;
            LblErrMsg.Text = myMsg.Query("errmsg");
        }
    }

    #endregion

    #region 刪除廠群組之使用者

    protected void BtnDelUserCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + m_Up_Id + "&QueryCondition=" + GetQueryCondition());
    }

    protected void BtnDelUserOK_Click(object sender, System.EventArgs e)
    {
        PccMsg myMsg = new PccMsg();
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = string.Empty;
        string strMsg = string.Empty;
        strReturn = mybs.DoReturnStr("delete_DS_USERGRP", GenDBXML(), string.Empty);
        strMsg = "刪除成功";

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") == "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('" + strMsg + "');</script>");
            Response.Redirect(MYURL + "?ApID=" + m_ApID + "&SrcUp_Id=" + m_SrcUp_Id + "&Up_Id=" + m_Up_Id + "&QueryCondition=" + GetQueryCondition());
        }
        else
        {
            LblErrMsg.Font.Size = FontUnit.Large;
            LblErrMsg.Text = myMsg.Query("errmsg");
        }

    }

    #endregion
}
