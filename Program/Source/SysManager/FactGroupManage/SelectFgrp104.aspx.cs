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


public partial class SysManager_FactGroupManage_SelectFgrp104 : System.Web.UI.Page
{
    private DataTable m_Table;

    private const string MYURL = "SelectFgrp104.aspx";
    private const string USERFGRPVIEW = "UserFgrpView104.aspx";

    private const string TITLE = "PccTitle";

    private string m_ApID;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 將使用者程式碼置於此以初始化網頁
        if (Session["UserID"] == null) return;

        lblMsg.Visible = false;

        m_ApID = Request.Params["ApID"];

        if (!IsPostBack)
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            GenMasterTable(ref myLabel);

            GenTree(ref TreeView1);
            //TreeView1.ExpandDepth = 2;
            //TreeView1.ExpandLevel = 2;
        }

    }

    #region 設定此頁面的資料

    private void SetLabel(ref PccErrMsg myLabel)
    {
        //LblFgrp_Nm.Text = myLabel.GetErrMsg("lbl0002","SysManager/ApManager"); 
        //BtnQuery.Text = myLabel.GetErrMsg("btnQuery");
        //設定Title
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "挑選廠廠群組至<font color=black><b>" + Request.Params["UserDesc"] + "</b></font>中";
            }
        }

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
        myRow.AddTextCell("廠群組名稱", 60);
        //廠別檢視
        myRow.AddTextCell("廠別檢視", 20);
        //選取
        myRow.AddTextCell("選取", 15);

        TblDs_Fgrp.CssClass = "ActDocTB";
        TblDs_Fgrp.Width = Unit.Percentage(100);
        TblDs_Fgrp.HorizontalAlign = HorizontalAlign.Center;
        TblDs_Fgrp.CellPadding = 2;
        TblDs_Fgrp.CellSpacing = 1;
        TblDs_Fgrp.Rows.Add(myRow.Row);
    }


    #endregion

    #region 設定主Table的資料

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        #region 取得Login的使用者是否有刪除的權限

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = "FactGroupManage104.aspx";
        bool del_auth = myAuth.IsDeleteAuth();

        //判斷是否為系統超級管理者
        string superAdmin = "N";
        if (ConfigurationManager.AppSettings["superAdminEmail"].Equals(Session["UserEMail"].ToString()))
        {
            superAdmin = "Y";
        }

        #endregion

        #region 取得廠群組的主資料ByUpID

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", Request.Params["ApID"]);
        myMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("SuperAdmin", superAdmin);
        myMsg.CreateFirstNode("CheckUser_Id", Request.Params["UserID"]);
        string strXML = myMsg.GetXmlStr;

        DataSet myDS_FGRPDs = mybs.DoReturnDataSet("GetFgrpByLoginUserAndCheck", strXML, "");
        DataTable myDS_FGRPData = myDS_FGRPDs.Tables["FgrpByLoginUserAndCheck"];

        #endregion

        if (myDS_FGRPData != null && myDS_FGRPData.Rows.Count > 0)
        {
            #region 定義基本變數

            ViewState["group_org"] = "";
            int MasterCount = 0;
            string MasterStyle = string.Empty;
            PccMsg myTempMsg;
            PccMsg myMsg1;
            PccRow myRow;

            #endregion

            #region 明細資料類別之共同參數

            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            //要取得明細資料的Table變數
            DataTable tblFactGroup;

            #endregion

            #region 內建廠別明細資料的欄位參數

            string[] FFields = { "編號", "廠別", "廠別名稱", "事業群", "公司編號" };
            int[] FFieldsPercent = { 5, 15, 40, 20, 20 };
            string[] FFieldsItem = { "--NO--", "fact_no", "fact_nm", "fgrp_nm", "comp_no" };
            string[] FFieldType = new string[5];
            FFieldType[0] = "--NO--";
            FFieldType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[3] = "<PccMsg><Type>Text</Type></PccMsg>";
            FFieldType[4] = "<PccMsg><Type>Text</Type></PccMsg>";

            #endregion

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myDS_FGRPData.Rows)
            {
                #region 主要廠群組的資料列設定

                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow("", HorizontalAlign.Center, 0, 0);
                myRow.SetRowCss(MasterStyle);
                //編號
                myRow.AddTextCell(MasterCount.ToString(), 5);
                //廠群組名稱
                myRow.AddTextCell(myMasterRow["Fgrp_Nm"].ToString(), 60);
                //廠別檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", "檢視所屬廠別");
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["Fgrp_Id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_F" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 20);
                //選取
                myTempMsg.LoadXml();
                myTempMsg.CreateFirstNode("Checked", myMasterRow["IsChecked"].ToString());
                myTempMsg.CreateFirstNode("Name", "JoinFgrpID");
                myTempMsg.CreateFirstNode("Value", myMasterRow["Fgrp_Id"].ToString());
                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, 15);

                //設定原始的FgrpID的資料
                if (myMasterRow["IsChecked"].ToString() == "Y")
                    ViewState["Fgrp_Org"] += myMasterRow["Fgrp_Id"].ToString() + ",";

                TblDs_Fgrp.Rows.Add(myRow.Row);

                #endregion

                #region 內建廠別明細資料的欄位參數，此處會取得資料，並設定給主表格

                //設定要取得廠別資料的XML
                myMsg1 = new PccMsg();
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

                MasterCount += 1;
            } // end of foreach datarow

            if (ViewState["Fgrp_Org"] != null && ViewState["Fgrp_Org"].ToString() != "")
                ViewState["Fgrp_Org"] = ViewState["Fgrp_Org"].ToString().Substring(0, ViewState["Fgrp_Org"].ToString().Length - 1);
            else
                ViewState["Fgrp_Org"] = "";
        }// end of if table count is 0
        else
        {
            //PageControl1.TotalSize = "0";
            //PageControl1.BuildPager();
        }
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

    private string CheckForm(string strName)
    {
        if (Request.Form[strName] == null)
            return "";
        else
            return Request.Form[strName].ToString();
    }


    #endregion

    #region "建立基本的Tree架構"

    private void GenTree(ref TreeView tree)
    {
        //判斷是否為系統超級管理者
        string superAdmin = "N";
        if (ConfigurationManager.AppSettings["superAdminEmail"].Equals(Session["UserEMail"].ToString()))
        {
            superAdmin = "Y";
        }

        //先把Tree設為空白
        tree.Nodes.Clear();

        //先建立廠別樹的根(Root)寶成寶成國際集團
        TreeNode node = new TreeNode();
        node.ImageUrl = "../../Images/TreeRoot.gif";
        node.Value = "0";
        //node.ID = "0";
        if (superAdmin.Equals("Y"))
        {
            node.Text = "<B>寶成國際集團</B>";
        }
        else
        {
            node.Text = "<Font color=gray>寶成國際集團</Font>";
        }
        tree.Nodes.Add(node);

        //取得這顆樹的所有資料
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", Request.Params["ApID"]);
        myMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("SuperAdmin", superAdmin);

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
            Node = new TreeNode();

            Node.Value = rows[0]["Fgrp_Id"].ToString();
            //Node.ID = rows[0]["Fgrp_Id"].ToString();

            if (rows[0]["IsAuth"].ToString().Equals("Y"))
            {
                Node.Text = "<B>" + rows[0]["Fgrp_Nm"].ToString() + "</B>";
            }
            else
            {
                Node.Text = "<Font color=gray>" + rows[0]["Fgrp_Nm"].ToString() + "</Font>";
            }
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

    #region Botton Action

    protected void BtnCancel_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(USERFGRPVIEW + "?ApID=" + m_ApID + "&QueryCondition=" + Request.Params["QueryCondition"]);
    }

    protected void BtnOK_Click(object sender, System.EventArgs e)
    {
        if (CheckForm("JoinFgrpID") == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('You not yet select Fact group!');</script>");
            GenMasterTable();
            return;
        }

        string strReturn = "";
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("User_Id", Request.Params["UserID"]);
        myMsg.CreateFirstNode("Ap_Id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("Fgrp_Str", CheckForm("JoinFgrpID"));
        myMsg.CreateFirstNode("Fgrp_Org", ViewState["Fgrp_Org"].ToString());
        myMsg.CreateFirstNode("Upd_Id", Session["UserID"].ToString());

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        strReturn = mybs.DoReturnStr("joinFgrpByUserID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") != "0")
        {
            lblMsg.Visible = true;
            lblMsg.Font.Size = FontUnit.Large;
            lblMsg.Text = myMsg.Query("errmsg");
            GenMasterTable();
        }
        else
        {
            Response.Redirect(USERFGRPVIEW + "?ApID=" + m_ApID + "&QueryCondition=" + CheckQueryString("QueryCondition"));
        }


    }


    #endregion
}
