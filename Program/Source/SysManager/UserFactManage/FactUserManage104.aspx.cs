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

public partial class SysManager_UserFactManage_FactUserManage104 : System.Web.UI.Page
{
    private const string TITLE = "PccTitle";
    private string m_ap_id;
    private string m_uf_id;
    private string m_fact_nm;

    private const string FACTUSERMANAGE = "FactUserManage104.aspx";
    private const string USERFACTMANAGE = "UserFactManage104.aspx";
    private const string PICKFACT = "PickFactToUser104.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        //設定Title 20041104
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "使用者的廠別權限";
            }
        }
        m_ap_id = Request.Params["ApID"];
        PccMsg myMsg = new PccMsg();
        if (!IsPostBack)
        {
            //設定之前User鍵入的查詢資料。 ...ting...
            if (Request.Params["QueryCondition"] != null && Request.Params["QueryCondition"].ToString() != "")
            {
                myMsg.LoadXml(Request.Params["QueryCondition"]);
                txtusernm.Text = myMsg.Query("QueryCondition/txtusernm");
                txtfactno.Text = myMsg.Query("QueryCondition/txtfactno");
            }

            Session["Usertemporary"] = "";
            Session["Facttemporary"] = "";

            if (CheckQueryString("Method") != "")
            {
                myMsg.LoadXml(Request.QueryString["Method"]);
                string method = myMsg.Query("Method");
                if (method == "MasterDelFunc")
                {
                    panelDelUser.Visible = true;
                    PanelGrid.Visible = false;
                    m_uf_id = myMsg.Query("Key");
                    txtuser.Value = m_uf_id;
                    m_fact_nm = myMsg.Query("KeyOther");
                    lblDelMsg1.Text = "您是否要將<font color=green><b>" + m_fact_nm + "</b></font>此廠別從此使用者移除?";
                }

            }
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            GenMasterTable(ref myLabel);
        }
    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        PccMsg myMsg = new PccMsg();
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

        PccRow myRow = new PccRow("cssGridHeader", HorizontalAlign.Center, VerticalAlign.Middle, 0);

        myRow.AddTextCell("編號", 5);
        myRow.AddTextCell("電子郵件帳號", 40);
        myRow.AddTextCell("廠別", 10);
        myRow.AddTextCell("姓名", 30);
        myRow.AddTextCell("事業群", 15);
        //myRow.AddTextCell("Email",40);
        myRow.AddTextCell("檢視", 10);

        tblUser.CssClass = "cssGridTable";
        tblUser.Width = Unit.Percentage(100);
        tblUser.HorizontalAlign = HorizontalAlign.Center;
        tblUser.CellPadding = 2;
        tblUser.CellSpacing = 1;

        tblUser.Rows.Add(myRow.Row);
    }

    #endregion

    #region "設定主Table資料"

    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        PccMsg myTempMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", Request.QueryString["ApID"]);
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("fact_no", txtfactno.Text);
        myMsg.CreateFirstNode("user_desc", txtusernm.Text);

        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());

        string strGroupFilter = "Y", strFactFilter = "Y";

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = USERFACTMANAGE;
        myMsg.CreateFirstNode("order", "");
        //判斷是否要利用事業群來分設權限 20041124

        if (ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString() == "Y")
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
                strGroupFilter = "N";
            }
        }
        else
        {
            myMsg.CreateFirstNode("GroupFilter", "N");
            strGroupFilter = "N";
        }

        if (ConfigurationManager.AppSettings[m_ap_id + "-FactFilter"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactFilter"].ToString() == "Y")
        {
            //表示他必須區分廠管理 20041125 注意要區別廠管理的先決條件是要區分事業群
            if (myAuth.IsSendAuth())  //表示若有Report及Send的權限則其為群管理者
            {
                myMsg.CreateFirstNode("FactFilter", "N");
                strFactFilter = "N";
            }
            else
            {
                myMsg.CreateFirstNode("FactFilter", "Y");
                strFactFilter = "Y";
            }
        }
        else
        {
            myMsg.CreateFirstNode("FactFilter", "N");
            strFactFilter = "N";
        }

        string strXML = myMsg.GetXmlStr;
        DataSet myUserDs = mybs.DoReturnDataSet("GetUserForApIDAndUserGroup", strXML, "");

        DataTable myUserData = myUserDs.Tables["ApUser"];

        PccRow myRow;

        if (myUserData.Rows.Count > 0)
        {
            int MasterCount = 0;
            string MasterStyle = "";

            //取出資料總筆數
            PageControl1.TotalSize = myUserDs.Tables["GetUserCount"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();

            //明細資料類別之共同參數
            myMsg.LoadXml();
            myMsg.CreateFirstNode("TableClass", "cssGridTable");
            myMsg.CreateFirstNode("RowHeaderClass", "cssGridHeader");
            myMsg.CreateFirstNode("RowClass1", "ffd000");
            myMsg.CreateFirstNode("RowClass2", "fff000");
            string strClassXML = myMsg.GetXmlStr;

            //XmlElement myNode;

            //明細資料的欄位參數
            string[] MFields = { "編號", "廠別編碼", "廠別名稱", "事業群", "刪除" };
            int[] MFieldsPercent = { 5, 10, 50, 25, 10 };
            string[] MFieldsItem = { "--NO--", "fact_no", "fact_nm", "fgrp_nm", "uf_id" };
            string[] MFieldType = new string[5];
            MFieldType[0] = "--NO--";
            MFieldType[1] = "<PccMsg><Type>Text</Type></PccMsg>";
            MFieldType[2] = "<PccMsg><Type>Text</Type></PccMsg>";
            MFieldType[3] = "<PccMsg><Type>Text</Type></PccMsg>";

            //設定明細欄位中多個Link Image的參數
            myMsg.LoadXml();

            //因為要設定其相關權限已改由廠群組來設定，所以在此不能再有刪除的動作了 20050529
            /*if (myAuth.IsDeleteAuth())
            {
                myMsg.CreateFirstNode("Type","MultiLink");
					
                myMsg.CreateNode("LinkButton");
                myMsg.AddToNode("Image",Session["PageLayer"] + "images/del.gif");
                myMsg.AddToNode("ToolTip","刪除此使用者與此廠別之關係");
                myMsg.AddToNode("href",FACTUSERMANAGE + "?ApID=" + m_ap_id);
                myMsg.AddToNode("QueryCondition",GetQueryCondition());
                myNode = myMsg.CreateParentNode("Method");
                myMsg.AddToNode("MethodName","MasterDelFunc",ref myNode);
                myMsg.AddToNode("Key","uf_id",ref myNode);
                myMsg.AddToNode("KeyOther","fact_nm",ref myNode);
                myMsg.UpdateNode(myNode); 
                myMsg.UpdateNode(); 
					
            }
            else
            {
                myMsg.CreateFirstNode("Type","Space");
            }*/

            myMsg.CreateFirstNode("Type", "Space");
            MFieldType[4] = myMsg.GetXmlStr;


            //再利用此Table再取得其每一列的資料，再Gen出主要的Table Row.
            foreach (DataRow myMasterRow in myUserData.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "cssGridRowAlternating"; else MasterStyle = "cssGridRow";
                myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //電子郵件帳號
                myRow.AddTextCell(myMasterRow["email"].ToString(), 40);
                //廠別
                myRow.AddTextCell(myMasterRow["fact_no"].ToString(), 10);
                //姓名
                //因為要設定其相關權限已改由廠群組來設定，所以在此不能再有新增的動作了 20050529
                /*if (myAuth.IsAddAuth()) 
                {
                    // ting
                    //myRow.AddTextCell("<A href=" + PICKFACT + "?ApID=" + m_ap_id + "&UserID=" + myMasterRow["user_id"] + "&UserDesc=" + myMasterRow["user_desc"].ToString().Trim() +">" + myMasterRow["user_desc"].ToString().Trim() + "</A>",20);
                    myTempMsg.CreateFirstNode("Href",PICKFACT + "?ApID=" + m_ap_id + "&UserID=" + myMasterRow["user_id"] + "&UserDesc=" + myMasterRow["user_desc"].ToString().Trim()+"&QueryCondition=" + GetQueryCondition());
                    myTempMsg.CreateFirstNode("Text",myMasterRow["user_desc"].ToString().Trim());
                    myRow.AddLinkHrefCell(myTempMsg.GetXmlStr,30);
                }
                else
                {
                    myRow.AddTextCell(myMasterRow["user_desc"].ToString(),30);
                }*/
                myRow.AddTextCell(myMasterRow["user_desc"].ToString(), 30);
                //事業群
                myRow.AddTextCell(myMasterRow["fgrp_nm"].ToString(), 15);
                //Email
                //myRow.AddTextCell(myMasterRow["email"].ToString(),40);
                //檢視
                myTempMsg = new PccMsg();
                myTempMsg.CreateFirstNode("ToolTip", myLabel.GetErrMsg("lbl0005", "AdtWeb/MaintainArea"));
                myTempMsg.CreateFirstNode("LinkID", "HLinkView" + myMasterRow["user_id"].ToString());
                myTempMsg.CreateFirstNode("Image", Session["PageLayer"] + "images/detal.gif");
                myTempMsg.CreateFirstNode("ClickFun", "doSection(view_" + MasterCount.ToString() + ")");
                myRow.AddLinkCell(myTempMsg.GetXmlStr, 10);

                tblUser.Rows.Add(myRow.Row);

                //要取得明細資料的Table變數
                DataTable myDTable;

                //string StrXml = "<PccMsg><ap_id>" + m_ap_id + "</ap_id><user_id>" + myMasterRow["user_id"] + "</user_id></PccMsg>";
                string StrXml = "<PccMsg><ap_id>" + m_ap_id + "</ap_id><set_user_id>" + myMasterRow["user_id"] + "</set_user_id>";
                StrXml += "<user_id>" + Session["UserID"].ToString() + "</user_id>";
                StrXml += "<GroupFilter>" + strGroupFilter + "</GroupFilter>";
                StrXml += "<FactFilter>" + strFactFilter + "</FactFilter>";
                StrXml += "<order></order>";
                StrXml += "</PccMsg>";
                myDTable = mybs.DoReturnDataSet("GetFactByUserIDAndUserGroup", StrXml, "").Tables["UserFact"];

                //設定選單明細資料類別之參數
                PccDetailTable myMDTable = new PccDetailTable(myMasterRow["user_id"].ToString());
                myMDTable.Fields = MFields;
                myMDTable.FieldsItem = MFieldsItem;
                myMDTable.FieldsPercent = MFieldsPercent;
                myMDTable.FieldsType = MFieldType;
                myMDTable.ClassXML = strClassXML;
                myMDTable.NewDataTable = myDTable;
                myMDTable.Create();

                myRow.Reset();
                myRow.SetRowCss("off");
                myRow.SetRowID("view_" + MasterCount);
                myRow.SetDefaultCellData("DGridTD", HorizontalAlign.Center, 0, 7);
                myRow.AddControl(myMDTable.NewTable, 100);

                tblUser.Rows.Add(myRow.Row);

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

    protected void changeView_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(USERFACTMANAGE + "?ApID=" + m_ap_id);
    }

    protected void btnDelOK1_Click(object sender, System.EventArgs e)
    {
        SendToBsByDelUser();
    }

    private void SendToBsByDelUser()
    {
        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_ap_id);
        myMsg.CreateFirstNode("user_id", "0");
        myMsg.CreateFirstNode("fact_id", "0");
        myMsg.CreateFirstNode("uf_id", txtuser.Value);
        myMsg.CreateFirstNode("upd_id", Session["UserID"].ToString());
        string strReturn = mybs.DoReturnStr("DeleteUserFactByUser", myMsg.GetXmlStr, "");
        myMsg.LoadXml(strReturn);
        if (myMsg.Query("returnValue") == "0")
        {
            lblMsg.Text = "";
            // ting
            Response.Redirect(FACTUSERMANAGE + "?ApID=" + m_ap_id + "&QueryCondition=" + GetQueryCondition());
        }
        else
        {
            lblMsg.Font.Size = 12;
            lblMsg.Text = myMsg.Query("errmsg");
        }
    }

    private void btnClear_ServerClick(object sender, System.EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }

    protected void btnDelCancel_Click(object sender, System.EventArgs e)
    {

    }

    protected void btnDelOK_Click(object sender, System.EventArgs e)
    {

    }


}
