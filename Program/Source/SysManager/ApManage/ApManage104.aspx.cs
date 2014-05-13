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
using System.Data.SqlClient; 

public partial class SysManager_ApManage_ApManage104 : System.Web.UI.Page
{
    private const string APADDNEW = "ApAddNew104.aspx";
    private const string MENUADDNEW = "MenuAddNew104.aspx";
    private const string APMANAGE = "ApManage104.aspx";

    #region "Page-Load and Button click function"

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁

        //設定抬頭 20040607
        //			for(int i = 0 ; i < Header1.Controls.Count ; i++)
        //			{
        //				if (Header1.Controls[i].ID == "PccTitle")
        //				{
        //					Label mylblTitle = (Label)(Header1.Controls[i]);
        //					mylblTitle.Text = "應用程式管理";
        //				}
        //			}

        if (!IsPostBack)
        {
            //Calendar1.Text = "20000101";
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            PccMsg myMsg = new PccMsg();
            SetLabel(ref myLabel);

            //設定之前User鍵入的查詢資料。 2004/3/8
            if (Request.Params["QueryCondition"] != null && Request.Params["QueryCondition"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["QueryCondition"].ToString());
                txtApName.Text = myMsg.Query("QueryCondition/txtApName");
            }

            GenMasterTable(ref myLabel);

            if (Request.Params["Method"] != null && Request.Params["Method"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["Method"].ToString());
                switch (myMsg.Query("Method"))
                {
                    case "MasterDelFunc":
                        MasterDelFunc(myMsg.Query("Key"), myMsg.Query("KeyOther"), ref myLabel);
                        break;
                    case "DetailDelFunc":
                        DetailDelFunc(myMsg.Query("Key"), myMsg.Query("KeyOther"), ref myLabel);
                        break;
                }
            }

        }
    }


    private void DetailDelFunc(string strKey, string strKeyOther, ref PccErrMsg myLabel)
    {
        //設定抬頭 20040607
        /*for(int i = 0 ; i < Header1.Controls.Count ; i++)
        {
            if (Header1.Controls[i].ID == "PccTitle")
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "刪除應用程式中的選單";
            }
        }*/
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");
        plDelete.Visible = true;
        plMain.Visible = false;
        string[] strParameters = { "<b>" + strKeyOther + "</b>" };
        lblDelMsg.Text = myErrMsg.GetErrMsgWithParam("msg0064", strParameters);
        btnDelOK.Text = myLabel.GetErrMsg("btnOK");
        btnDelCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = "DetailDelFunc";
    }

    private void MasterDelFunc(string strKey, string strKeyOther, ref PccErrMsg myLabel)
    {
        //設定抬頭 20040607
        /*for(int i = 0 ; i < Header1.Controls.Count ; i++)
        {
            if (Header1.Controls[i].ID == "PccTitle")
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "刪除應用程式";
            }
        }*/
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");
        plDelete.Visible = true;
        plMain.Visible = false;
        string[] strParameters = { "<b>" + strKeyOther + "</b>" };
        lblDelMsg.Text = myErrMsg.GetErrMsgWithParam("msg0044", strParameters);
        btnDelOK.Text = myLabel.GetErrMsg("btnOK");
        btnDelCancel.Text = myLabel.GetErrMsg("btnCancel");
        ViewState["Flag"] = "MasterDelFunc";
    }

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        //Calendar1.Enabled = false;

        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }

    protected void btnDelCancel_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;
        GenMasterTable();
    }

    protected void lbtnAddAp_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(APADDNEW + "?ApID=" + Request.Params["ApID"].ToString() + "&Type=New&QueryCondition=" + GetQueryCondition());
    }

    protected void btnDelOK_Click(object sender, System.EventArgs e)
    {
        plMain.Visible = true;
        plDelete.Visible = false;

        PccMsg mySrcMsg = new PccMsg(Request.QueryString["Method"]);

        PccMsg myMsg = new PccMsg();
        bs_ApManager mybs = new bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strReturn = "";
        switch (mySrcMsg.Query("Method"))
        {
            case "MasterDelFunc":
                myMsg.CreateFirstNode("ap_id", mySrcMsg.Query("Key"));
                strReturn = mybs.DoReturnStr("DeleteProAp", myMsg.GetXmlStr, "");
                break;
            case "DetailDelFunc":
                myMsg.CreateFirstNode("menu_id", mySrcMsg.Query("Key"));
                strReturn = mybs.DoReturnStr("DeleteProMenu", myMsg.GetXmlStr, "");
                break;
        }

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

    #endregion 

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        //lblTitle.Text = myLabel.GetErrMsg("lbl0006","SysManager/ApManager");

        lblApName.Text = myLabel.GetErrMsg("lbl0004", "SysManager/ApManager");
        lbtnAddAp.Text = myLabel.GetErrMsg("lbl0002", "SysManager/ApManager");
    }

    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (tblAp.Rows.Count > 0)
            tblAp.Rows.Clear();

        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTable()
    {
        if (tblAp.Rows.Count > 0)
            tblAp.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    private void GenMasterTableHader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        //編號
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0007", "SysManager/ApManager"), 5);
        //應用程式編碼
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0008", "SysManager/ApManager"), 10);
        //應用程式名稱
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0009", "SysManager/ApManager"), 15);
        //應用程式連結
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0010", "SysManager/ApManager"), 40);
        //應用程式目錄
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0012", "SysManager/ApManager"), 15);
        //檢視
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0017", "SysManager/ApManager"), 5);
        //管理
        myRow.AddTextCell(myLabel.GetErrMsg("lbl0013", "SysManager/ApManager"), 5);


        tblAp.CssClass = "ActDocTB";
        tblAp.Width = Unit.Percentage(100);
        tblAp.HorizontalAlign = HorizontalAlign.Center;
        tblAp.CellPadding = 2;
        tblAp.CellSpacing = 1;

        tblAp.Rows.Add(myRow.Row);
    }

    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("txtApName", txtApName.Text);
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

    #region "編號及上下頁的程式碼"

    protected void OnPageClick(object source, EventArgs e)
    {
        GenMasterTable();
    }

    #endregion

    #region "設定主Table的資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        //取得Login的使用者是否有刪除的權限
        GetMenuAuth myAuth = new GetMenuAuth();
        bool del_auth = myAuth.IsDeleteAuth();

        PccBsSystemForC.bs_ApManager mybs = new PccBsSystemForC.bs_ApManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("Vpath", ConfigurationManager.AppSettings["vpath"].ToString());
        myMsg.CreateFirstNode("ApName", txtApName.Text);
        string strXML = myMsg.GetXmlStr;
        DataSet myApDs = mybs.DoReturnDataSet("GetApByVpath", strXML, "");

        DataTable myApData = myApDs.Tables["Ap"];

        if (myApData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";
            PccMsg myTempMsg;

            PccRow myRow;

            //取出資料總筆數
            PageControl1.TotalSize = myApDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //明細資料類別之共同參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "ActDocTB");
            myMsg.CreateFirstNode("RowHeaderClass", "TDShowHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            XmlElement myNode;

            //明細資料的欄位參數
            string strOrgFields5 = "link-新增選單-" + MENUADDNEW + "?ApID=" + Request.Params["ApID"].ToString() + "&Type=New&QueryCondition=" + GetQueryCondition();
            string[] Fields = { "編號", "選單編碼", "選單名稱", "選單連結", "管理區", strOrgFields5 };
            int[] FieldsPercent = { 5, 8, 17, 50, 10, 10 };
            string[] FieldsItem = { "--NO--", "menu_no", "menu_nm", "menu_link", "manage_mk", "menu_id" };
            string[] FieldsType = new string[6];
            FieldsType[0] = "--NO--";
            FieldsType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[3] = "<PccMsg><Type>Text</Type></PccMsg>";
            FieldsType[4] = "<PccMsg><Type>Text</Type></PccMsg>";

            //設定明細欄位中多個Link Image的參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("Type", "MultiLink");

            //設定第一個更新選單的Image參數
            myMsg.CreateNode("LinkButton");
            myMsg.AddToNode("Image", Session["PageLayer"] + "images/edit.gif");
            myMsg.AddToNode("ToolTip", "更新選單");
            myMsg.AddToNode("href", MENUADDNEW + "?ApID=" + Request.Params["ApID"].ToString());
            myMsg.AddToNode("QueryCondition", GetQueryCondition());
            myNode = myMsg.CreateParentNode("Method");
            myMsg.AddToNode("MethodName", "DetailUpdFunc", ref myNode);
            myMsg.AddToNode("Key", "menu_id", ref myNode);
            myMsg.AddToNode("KeyOther", "menu_nm", ref myNode);
            myMsg.UpdateNode(myNode);
            myMsg.UpdateNode();

            if (del_auth)
            {
                //設定第二個刪除選單的Image參數
                myMsg.CreateNode("LinkButton");
                myMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                myMsg.AddToNode("ToolTip", "刪除選單");
                myMsg.AddToNode("href", APMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                myMsg.AddToNode("QueryCondition", GetQueryCondition());
                myNode = myMsg.CreateParentNode("Method");
                myMsg.AddToNode("MethodName", "DetailDelFunc", ref myNode);
                myMsg.AddToNode("Key", "menu_id", ref myNode);
                myMsg.AddToNode("KeyOther", "menu_nm", ref myNode);
                myMsg.UpdateNode(myNode);
                myMsg.UpdateNode();
            }

            FieldsType[5] = myMsg.GetXmlStr;


            //要取得明細資料的Table變數
            DataTable tblMenuByApID;

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myApData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow();
                myRow.SetRowCss(MasterStyle);
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //應用程式編碼
                myRow.AddTextCell(myMasterRow["ap_id"].ToString(), 10);
                //應用程式名稱
                myRow.AddTextCell(myMasterRow["ap_name"].ToString(), 15);
                //應用程式連結
                myRow.AddTextCell(myMasterRow["ap_link"].ToString(), 40);
                //應用程式目錄
                myRow.AddTextCell(myMasterRow["ap_vpath"].ToString(), 15);
                //檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0018", "SysManager/ApManager"));
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["ap_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 5);
                //管理
                myTempMsg.LoadXml();
                //先加入修改的Item
                myTempMsg.CreateNode("LinkButton");
                myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/edit.gif");
                myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0014", "SysManager/ApManager"));
                myTempMsg.AddToNode("href", APADDNEW + "?ApID=" + Request.Params["ApID"].ToString());
                myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                myTempMsg.AddToNode("Method", GetMethod("MasterUpdateFunc", "ap_id", "ap_name", myMasterRow));
                myTempMsg.UpdateNode();
                if (del_auth)
                {
                    //再加入刪除的Item
                    myTempMsg.CreateNode("LinkButton");
                    myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                    myTempMsg.AddToNode("ToolTip", myLabel.GetErrMsg("lbl0015", "SysManager/ApManager"));
                    myTempMsg.AddToNode("href", APMANAGE + "?ApID=" + Request.Params["ApID"].ToString());
                    myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                    myTempMsg.AddToNode("Method", GetMethod("MasterDelFunc", "ap_id", "ap_name", myMasterRow));
                    myTempMsg.UpdateNode();
                }
                myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 5);

                tblAp.Rows.Add(myRow.Row);

                tblMenuByApID = mybs.DoReturnDataSet("GetMenuByApID", "<PccMsg><ap_id>" + myMasterRow["ap_id"].ToString() + "</ap_id></PccMsg>", "").Tables["Menu"];

                //設定選單明細資料類別之參數
                PccDetailTable myDTable = new PccDetailTable("DT" + myMasterRow["ap_id"].ToString());
                myDTable.ClassXML = strClassXML;
                Fields[5] = strOrgFields5 + "&Key=" + myMasterRow["ap_id"].ToString();
                myDTable.Fields = Fields;
                myDTable.FieldsPercent = FieldsPercent;
                myDTable.FieldsItem = FieldsItem;
                myDTable.FieldsType = FieldsType;
                myDTable.NewDataTable = tblMenuByApID;

                myDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myDTable.NewTable, 100);

                tblAp.Rows.Add(myRow.Row);

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

}
