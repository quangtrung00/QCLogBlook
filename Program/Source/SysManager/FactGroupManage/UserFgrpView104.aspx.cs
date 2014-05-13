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

public partial class SysManager_FactGroupManage_UserFgrpView104 : System.Web.UI.Page
{

    private const string TITLE = "PccTitle";

    private string m_ap_id;
    private string m_Fgrp_Id;
    private string m_Fgrp_Nm;

    private const string FACTGROUPMANAGE = "FactGroupManage104.aspx";
    private const string MYURL = "UserFgrpView104.aspx";
    private const string PICKFGRP = "SelectFgrp104.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        m_ap_id = Request.Params["ApID"];
        PccMsg myMsg = new PccMsg();
        if (CheckQueryString("Method") != "")
        {
            myMsg.LoadXml(Request.QueryString["Method"]);
            string method = myMsg.Query("Method");
            if (method == "DetailDelFunc")
            {
                panelDelUser.Visible = true;
                PanelGrid.Visible = false;
                m_Fgrp_Id = myMsg.Query("Key");
                txtuser.Value = m_Fgrp_Id;
                m_Fgrp_Nm = myMsg.Query("KeyOther");
                lblDelMsg1.Text = "您是否要將<font color=green><b>" + m_Fgrp_Nm + "</b></font>此廠群組從此使用者(<font color=green><b>" + CheckQueryString("User_Desc") + "</b>)中移除?";
            }

        }
        if (!IsPostBack)
        {
            //設定之前User鍵入的查詢資料。 ...ting...
            if (Request.Params["QueryCondition"] != null && Request.Params["QueryCondition"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["QueryCondition"]);
                txtusernm.Text = myMsg.Query("QueryCondition/txtusernm");
                txtfactno.Text = myMsg.Query("QueryCondition/txtfactno");
            }


            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            GenMasterTable(ref myLabel);
        }

    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        //設定Title 20041104
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "使用者的廠別群組權限管理";
            }
        }

        //PccMsg myMsg = new PccMsg();
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        //btnClear.Value = myLabel.GetErrMsg("btnClear");

    }

    private void GenMasterTable()
    {
        if (tblUser.Rows.Count > 0)
            tblUser.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHeader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }

    // ting
    private void GenMasterTable(ref PccErrMsg myLabel)
    {
        if (tblUser.Rows.Count > 0)
            tblUser.Rows.Clear();

        GenMasterTableHeader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }



    private void GenMasterTableHeader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);

        myRow.AddTextCell("編號", 5);
        myRow.AddTextCell("電子郵件帳號", 40);
        myRow.AddTextCell("廠別", 10);
        myRow.AddTextCell("姓名(點選姓名可把廠群組加入此使用者)", 30);
        myRow.AddTextCell("事業群", 15);
        myRow.AddTextCell("檢視", 10);

        tblUser.CssClass = "ActDocTB";
        tblUser.Width = Unit.Percentage(100);
        tblUser.HorizontalAlign = HorizontalAlign.Center;
        tblUser.CellPadding = 2;
        tblUser.CellSpacing = 1;

        tblUser.Rows.Add(myRow.Row);
    }

    private void GenFgrpTableHeader(ref Table myRefTable)
    {
        PccRow myRow = new PccRow("TDShowHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        myRow.AddTextCell("編號", 10);
        myRow.AddTextCell("廠群組名稱", 70);
        myRow.AddTextCell("檢視", 10);
        myRow.AddTextCell("管理", 10);

        myRefTable.CssClass = "ActDocTB";
        myRefTable.Width = Unit.Percentage(100);
        myRefTable.HorizontalAlign = HorizontalAlign.Center;
        myRefTable.CellPadding = 2;
        myRefTable.CellSpacing = 1;

        myRefTable.Rows.Add(myRow.Row);

    }

    private void GenFactTableHeader(ref Table myRefTable)
    {
        PccRow myRow = new PccRow("TDShowHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);
        myRow.AddTextCell("編號", 10);
        myRow.AddTextCell("廠別", 10);
        myRow.AddTextCell("廠別名稱", 40);
        myRow.AddTextCell("事業群", 20);
        myRow.AddTextCell("公司編號", 20);

        myRefTable.CssClass = "ActDocTB";
        myRefTable.Width = Unit.Percentage(100);
        myRefTable.HorizontalAlign = HorizontalAlign.Center;
        myRefTable.CellPadding = 2;
        myRefTable.CellSpacing = 1;

        myRefTable.Rows.Add(myRow.Row);

    }

    #endregion

    #region "設定主Table資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        PccMsg myMsg = new PccMsg();
        PccMsg myTempMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", Request.QueryString["ApID"]);
        myMsg.CreateFirstNode("Upd_Id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("Fact_No", txtfactno.Text);
        myMsg.CreateFirstNode("User_Desc", txtusernm.Text);
        myMsg.CreateFirstNode("OrderBy", "a.user_desc");

        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = FACTGROUPMANAGE;

        string strXML = myMsg.GetXmlStr;
        DataSet myUserDs = mybs.DoReturnDataSet("getAllUserInFgrp", strXML, "");

        DataTable myUserData = myUserDs.Tables["UserInFgrp"];
        DataTable myFgrpData, myFactData;
        Table myFgrpTable, myFactTable;

        PccRow myRow;

        if (myUserData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";

            //取出資料總筆數
            PageControl1.TotalSize = myUserDs.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myUserData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //電子郵件帳號
                myRow.AddTextCell(myMasterRow["email"].ToString(), 40);
                //廠別
                myRow.AddTextCell(myMasterRow["fact_no"].ToString(), 10);
                //姓名
                if (myAuth.IsAddAuth())
                {
                    myTempMsg.CreateFirstNode("Href", PICKFGRP + "?ApID=" + m_ap_id + "&UserID=" + myMasterRow["user_id"] + "&UserDesc=" + myMasterRow["user_desc"].ToString().Trim() + "&QueryCondition=" + GetQueryCondition());
                    myTempMsg.CreateFirstNode("Text", myMasterRow["user_desc"].ToString().Trim());
                    myRow.AddLinkHrefCell(myTempMsg.GetXmlStr, 30);
                }
                else
                {
                    myRow.AddTextCell(myMasterRow["user_desc"].ToString(), 30);
                }
                //事業群
                myRow.AddTextCell(myMasterRow["fgrp_nm"].ToString(), 15);
                //檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", "檢視所屬的廠群組");
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["user_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 10);

                tblUser.Rows.Add(myRow.Row);

                //要取得明細資料的Table變數
                myTempMsg.ClearContext();
                myTempMsg.CreateFirstNode("Ap_Id", m_ap_id);
                myTempMsg.CreateFirstNode("User_Id", myMasterRow["user_id"].ToString());
                myTempMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
                myFgrpData = mybs.DoReturnDataSet("get_DS_FGRPByUserID", myTempMsg.GetXmlStr, string.Empty).Tables["FgrpInUser"];

                myFgrpTable = new Table();
                GenFgrpTableHeader(ref myFgrpTable);
                int DetailCount = 1;

                foreach (DataRow myDetailRow in myFgrpData.Rows)
                {
                    if (DetailCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "ffd000";
                    myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);
                    //編號
                    myRow.AddTextCell(DetailCount.ToString(), 10);
                    //廠群組名稱
                    myRow.AddTextCell(myDetailRow["Fgrp_Nm"].ToString(), 70);
                    //檢視
                    myTempMsg = new PccMsg();
                    myTempMsg.CreateFirstNode("ToolTip", "檢視所屬廠別");
                    myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myDetailRow["Fgrp_Id"].ToString());
                    myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                    myTempMsg.CreateFirstNode("ClickFun", "doSection(view_D" + MasterCount.ToString() + "_" + DetailCount.ToString() + ")");
                    myRow.AddLinkCell(myTempMsg.GetXmlStr, 10);
                    //管理
                    myTempMsg = new PccMsg();
                    //先加入修改的Item
                    if (myAuth.IsDeleteAuth())
                    {
                        //再加入刪除的Item
                        myTempMsg.CreateNode("LinkButton");
                        myTempMsg.AddToNode("Image", Session["PageLayer"] + "images/del.gif");
                        myTempMsg.AddToNode("ToolTip", "刪除此使用者與廠群組的關係");
                        myTempMsg.AddToNode("href", MYURL + "?ApID=" + m_ap_id + "&User_Desc=" + myMasterRow["user_desc"].ToString() + "&User_Id=" + myMasterRow["user_id"].ToString());
                        myTempMsg.AddToNode("QueryCondition", GetQueryCondition());
                        myTempMsg.AddToNode("Method", GetMethod("DetailDelFunc", "Fgrp_Id", "Fgrp_Nm", myDetailRow));
                        myTempMsg.UpdateNode();
                    }

                    myRow.AddMultiLinkCell(myTempMsg.GetXmlStr, 10);

                    myFgrpTable.Controls.Add(myRow.Row);

                    //要取得廠別明細資料的Table變數
                    myTempMsg.ClearContext();
                    myTempMsg.CreateFirstNode("Fgrp_Id", myDetailRow["Fgrp_Id"].ToString());
                    myTempMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
                    myFactData = mybs.DoReturnDataSet("get_DS_FACTGRPByFgrpID", myTempMsg.GetXmlStr, string.Empty).Tables["DS_FACTGRP"];

                    myFactTable = new Table();
                    GenFactTableHeader(ref myFactTable);
                    int FactDetailCount = 1;
                    foreach (DataRow myFactDetailRow in myFactData.Rows)
                    {
                        if (DetailCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "ffd000";
                        myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);

                        //編號
                        myRow.AddTextCell(FactDetailCount.ToString(), 10);
                        //廠別
                        myRow.AddTextCell(myFactDetailRow["Fact_No"].ToString(), 10);
                        //廠別名稱
                        myRow.AddTextCell(myFactDetailRow["Fact_Nm"].ToString(), 40);
                        //事業群
                        myRow.AddTextCell(myFactDetailRow["Fgrp_Nm"].ToString(), 20);
                        //公司編號
                        myRow.AddTextCell(myFactDetailRow["Comp_No"].ToString(), 20);

                        myFactTable.Controls.Add(myRow.Row);

                        FactDetailCount += 1;
                    }

                    myRow.Reset();
                    myRow.SetRowCss("off");
                    myRow.SetRowID("view_D" + MasterCount.ToString() + "_" + DetailCount);
                    myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                    myRow.AddControl(myFactTable, 100);

                    myFgrpTable.Controls.Add(myRow.Row);


                    DetailCount += 1;

                }

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 10);
                myRow.AddControl(myFgrpTable, 100);

                tblUser.Controls.Add(myRow.Row);

                MasterCount += 1;
            }
        }
        else
        {
            PageControl1.TotalSize = "0";
            PageControl1.BuildPager();
            myRow = new PccRow("DGridTD", HorizontalAlign.Center, VerticalAlign.Middle, 6);
            myRow.AddTextCell("<b><font color='darkred'>" + myErrMsg.GetErrMsg("AdtWeb/msg0072") + "</font></b>", 100);
            tblUser.Controls.Add(myRow.Row);
        }
    }

    #endregion

    #region "其他 Function"

    /*private string GetQueryCondition()
		{
			string strQueryCondition = "<PccMsg><QueryCondition><fact_no>" + txtfactno.Text + "</fact_no><user_nm>" + txtusernm.Text + "</user_nm></QueryCondition></PccMsg>";
			return strQueryCondition;
		}*/

    // ting
    private string GetQueryCondition()
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateNode("QueryCondition");
        myMsg.AddToNode("txtusernm", txtusernm.Text);
        myMsg.AddToNode("txtfactno", txtfactno.Text);
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

    #region "Check Data if Null"

    private string CheckDBNull(object oFieldData)
    {
        if (Convert.IsDBNull(oFieldData))
            return "";
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

    #region Button Action

    protected void btnDelOK1_Click(object sender, System.EventArgs e)
    {
        string strReturn = "";
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("User_Id", Request.Params["User_ID"]);
        myMsg.CreateFirstNode("Ap_Id", CheckQueryString("ApID"));
        myMsg.CreateFirstNode("Fgrp_Id", m_Fgrp_Id);
        myMsg.CreateFirstNode("Upd_Id", Session["UserID"].ToString());

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        strReturn = mybs.DoReturnStr("deleteUserGrpByFgrpIDAndUserID", myMsg.GetXmlStr, "");

        myMsg.LoadXml(strReturn);
        if (myMsg.Query("returnValue") == "0")
        {
            lblMsg.Text = "";
            // ting
            Response.Redirect(MYURL + "?ApID=" + m_ap_id + "&QueryCondition=" + GetQueryCondition());
        }
        else
        {
            lblMsg.Font.Size = 12;
            lblMsg.Text = myMsg.Query("errmsg");
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
}
