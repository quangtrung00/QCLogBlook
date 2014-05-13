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

public partial class SysManager_UserFactManage_PickUserToFact104 : System.Web.UI.Page
{
    private const string TITLE = "PccTitle";
    private string m_ap_id;
    private string m_fact_id;
    private const string PICKUSER = "UserFactManage104.aspx";
		
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "挑選使用者至<font color=red><b>" + Request.Params["FactName"] + "</b></font>中";
            }
        }
        m_ap_id = Request.Params["ApID"];
        m_fact_id = Request.Params["FactID"];


        if (!IsPostBack)
        {
            //清除Session
            Session["Usertemporary"] = string.Empty;

            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            bs_UserManager mybsS = new bs_UserManager(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
            BindFactData(ref myLabel, ref mybsS);
            SetLabel(ref myLabel);
            GenMasterTable();
        }

    }

    #region "設定此頁面之基本資料"

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

        listfact.DataSource = dt.DefaultView;
        listfact.DataTextField = "fact_desc";
        listfact.DataValueField = "fact_id";

        listfact.DataBind();
    }

    private void SetLabel(ref PccErrMsg myLabel)
    {
        PccMsg myMsg = new PccMsg();
        btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        btnClear.Value = myLabel.GetErrMsg("btnClear");

    }

    private void GenMasterTable()
    {
        if (tab_user.Rows.Count > 0)
            tab_user.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHeader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }


    private void GenMasterTableHeader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);

        myRow.AddTextCell("編號", 5);
        myRow.AddTextCell("使用者名稱", 10);
        myRow.AddTextCell("廠別", 35);
        myRow.AddTextCell("事業群", 15);
        myRow.AddTextCell("電子郵件帳號", 30);
        myRow.AddTextCell("挑選", 5);

        tab_user.CssClass = "ActDocTB";
        tab_user.Width = Unit.Percentage(100);
        tab_user.HorizontalAlign = HorizontalAlign.Center;
        tab_user.CellPadding = 2;
        tab_user.CellSpacing = 1;

        tab_user.Rows.Add(myRow.Row);
    }

    #endregion

    # region "設定主Table資料"
    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_ap_id);
        myMsg.CreateFirstNode("fact_id", m_fact_id);
        myMsg.CreateFirstNode("Ufact_id", listfact.SelectedItem.Value.ToString());
        myMsg.CreateFirstNode("user_desc", txtuser.Text);
        if (ConfigurationManager.AppSettings[m_ap_id + "-UserMultiFact"] != null)
            myMsg.CreateFirstNode("UserMultiFact", ConfigurationManager.AppSettings[m_ap_id + "-UserMultiFact"]);
        else
            myMsg.CreateFirstNode("UserMultiFact", "Y");
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());

        DataSet ds;

        //判斷是否要利用事業群來分設權限 20041125

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = PICKUSER;

        myMsg.CreateFirstNode("order", "");

        if (ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString() == "Y")
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

        if (ConfigurationManager.AppSettings[m_ap_id + "-FactFilter"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactFilter"].ToString() == "Y")
        {
            //表示他必須區分廠管理 20041125 注意要區別廠管理的先決條件是要區分事業群
            if (myAuth.IsSendAuth())  //表示若有Report及Send的權限則其為群管理者
            {
                myMsg.CreateFirstNode("FactFilter", "N");
            }
            else
            {
                myMsg.CreateFirstNode("FactFilter", "Y");
            }
        }
        else
        {
            myMsg.CreateFirstNode("FactFilter", "N");
        }

        ds = mybs.DoReturnDataSet("GetUserByApIDAndUserGroupIn", myMsg.GetXmlStr, "");

        /*string strSp = "";
        //判斷是否要利用事業群來分設權限
        if (ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString() == "Y")
        {
            //判斷此使用者是否可以分配不同的事業群權限
            if (myAuth.IsReportAuth())
                strSp = "GetUserForAp";
            else
                strSp = "GetUserForUserGroupAndAp";
        }
        else
        {
            strSp = "GetUserForAp";
        }

			
        ds = mybs.DoReturnDataSet(strSp, myMsg.GetXmlStr, "");*/


        DataTable myTable = ds.Tables["User"];

        PccRow myRow;

        if (myTable.Rows.Count > 0)
        {
            int MasterCount = 1;
            string MasterStyle = "";


            //取出資料總筆數
            PageControl1.TotalSize = ds.Tables["GetUserCount"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();
            string cVal = "";
            foreach (DataRow myMasterRow in myTable.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);

                //勾選值 ==
                string chk = "";
                string userid = "";

                //session("")不是Nothing 也不是 ""時則執行
                if (Session["Usertemporary"] != null && Session["Usertemporary"].ToString() != "")
                {
                    string[] chkarray = Session["Usertemporary"].ToString().Split(',');
                    userid = ((int)decimal.Parse(myMasterRow["user_id"].ToString())).ToString();
                    for (int i = 0; i < chkarray.Length; i++)
                    {
                        if (userid == chkarray[i].ToString())
                        {
                            chk = "checked";
                            cVal += userid + ",";

                        }
                    }

                }
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //管理者名稱
                myRow.AddTextCell(myMasterRow["user_desc"].ToString(), 10);
                //廠別
                myRow.AddTextCell(myMasterRow["fact_nm"].ToString(), 35);
                //事業群
                myRow.AddTextCell(myMasterRow["fgrp_nm"].ToString(), 15);
                //Email
                myRow.AddTextCell(myMasterRow["email"].ToString(), 30);
                //選取
                myRow.AddTextCell("<input type=checkbox name=user_id id=user_id" + MasterCount + " value=" + ((int)decimal.Parse(myMasterRow["user_id"].ToString())).ToString() + " " + chk + ">", 5);

                tab_user.Controls.Add(myRow.Row);
                MasterCount += 1;
            }
            Session["chkcount"] = MasterCount;
            ViewState["CurrentValue"] = "";
            if (cVal != "")
            {
                ViewState["CurrentValue"] = cVal.Substring(0, cVal.Length - 1);
            }
        }
        else
        {
            PageControl1.TotalSize = "0";
            Session["chkcount"] = 0;
            PageControl1.BuildPager();
            myRow = new PccRow("DGridTD", HorizontalAlign.Center, VerticalAlign.Middle, 5);
            myRow.AddTextCell("<b><font color='darkred'>" + myErrMsg.GetErrMsg("AdtWeb/msg0073") + "</font></b>", 100);
            tab_user.Controls.Add(myRow.Row);
        }
    }
    #endregion

    #region "編號及上下頁的程式碼"

    protected void OnPageClick(object source, EventArgs e)
    {
        Temporary();
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
    private string CheckForm(string strName)
    {
        if (Request.Form[strName] == null)
            return "";
        else
            return Request.Form[strName].ToString();
    }

    #endregion

    #region "Button事件"

    protected void Button1_Click(object sender, System.EventArgs e)
    {
        if (ViewState["CurrentValue"] == null && tab_user.Rows.Count == 0) return;

        Temporary();
        string strUser = "";
        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        strUser = Session["Usertemporary"].ToString();
        if (strUser.Length == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('您還未選擇任何使用者唷！');</script>");
            GenMasterTable();
            return;
        }

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_ap_id);
        myMsg.CreateFirstNode("fact_id", m_fact_id);
        myMsg.CreateFirstNode("user_str", strUser);
        if (ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"] != null)
            //myMsg.CreateFirstNode("FactByGroup",ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString());
            // ting
            myMsg.CreateFirstNode("FactByGroup", ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString() + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
        else
            myMsg.CreateFirstNode("FactByGroup", "N");

        myMsg.CreateFirstNode("upd_id", Session["UserID"].ToString());
        string strReturn = mybs.DoReturnStr("PickMultiUserFact", myMsg.GetXmlStr, "");
        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") != "0")
        {
            lblMsg.Font.Size = 12;
            lblMsg.Text = myMsg.Query("errmsg");
            return;
        }
        else
        {
            //Response.Redirect(PICKUSER + "?ApID=" + m_ap_id);
            // ting
            Response.Redirect(PICKUSER + "?ApID=" + m_ap_id + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
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

    #region "Temporary"

    private void Temporary()
    {
        int CValLength, RequestLength;
        if (ViewState["CurrentValue"].ToString() == "")
        {
            CValLength = 0;
        }
        else
        {
            CValLength = ViewState["CurrentValue"].ToString().Split(',').Length;
        }
        if (CheckForm("user_id") == "")
        {
            RequestLength = 0;
        }
        else
        {
            RequestLength = Request.Form["user_id"].Split(',').Length;
        }
        ArrayList tmpArray = new ArrayList();
        if (Session["Usertemporary"].ToString() != "")
        {
            string[] ChangeAry = Session["Usertemporary"].ToString().Split(',');
            for (int j = 0; j < ChangeAry.Length; j++)
            {
                tmpArray.Add(ChangeAry[j]);
            }
        }

        string strUserID1 = "";

        //移除這一頁舊有的資料
        if (CValLength > 0)
        {

            string[] aBeforeUserID = ViewState["CurrentValue"].ToString().Split(',');

            foreach (string strUserID in aBeforeUserID)
            {
                if (tmpArray.Count > 0)  //Session("Usertemporary")記錄未上傳前,所挑選的使用者
                {
                    foreach (string strTemp in tmpArray)
                    {
                        strUserID1 = strTemp.Split('_')[0];
                        if (strUserID1 == strUserID)
                        {
                            tmpArray.Remove(strTemp);
                            break;
                        }
                    }
                }
            }
        }

        //新增這一頁的現有資料
        string[] pick_user;
        if (CheckForm("user_id") != "")
        {
            pick_user = Request.Form["user_id"].Split(',');
            if (RequestLength > 0)
            {
                for (int i = 0; i < pick_user.Length; i++)
                {
                    tmpArray.Add(pick_user[i]);
                }
            }

        }


        //將ArrayList轉成字串
        string strTemporary = "";
        if (tmpArray.ToString() != "" && tmpArray != null)
        {
            for (int k = 0; k < tmpArray.Count; k++)
            {
                strTemporary += tmpArray[k] + ",";
            }
            if (strTemporary != "") strTemporary = strTemporary.Substring(0, strTemporary.Length - 1);
        }


        Session["Usertemporary"] = strTemporary;
    }
    #endregion
}
