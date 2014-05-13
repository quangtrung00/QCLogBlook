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

public partial class SysManager_UserFactManage_PickFact104 : System.Web.UI.Page
{
    private const string TITLE = "PccTitle";
    private const string USERFACTMANAGE = "UserFactManage104.aspx";
    private string m_ap_id;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;

        // 將使用者程式碼置於此以初始化網頁
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "挑選廠別";
            }
        }
        m_ap_id = Request.Params["ApID"];
        if (!IsPostBack)
        {
            //清除Session的資料 20050217
            Session["Facttemporary"] = string.Empty;

            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
            SetLabel(ref myLabel);
            GenMasterTable();

        }
    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        PccMsg myMsg = new PccMsg();

    }

    private void GenMasterTable()
    {
        if (tab_fact.Rows.Count > 0)
            tab_fact.Rows.Clear();

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        GenMasterTableHeader(ref myLabel);
        GenMasterTableData(ref myLabel);
    }


    private void GenMasterTableHeader(ref PccErrMsg myLabel)
    {

        PccRow myRow = new PccRow("DListHeaderTD", HorizontalAlign.Center, VerticalAlign.Middle, 0);

        myRow.AddTextCell("編號", 5);
        myRow.AddTextCell("廠別編號", 10);
        myRow.AddTextCell("廠別名稱", 60);
        myRow.AddTextCell("事業群", 20);
        myRow.AddTextCell("選取", 5);

        tab_fact.CssClass = "ActDocTB";
        tab_fact.Width = Unit.Percentage(100);
        tab_fact.HorizontalAlign = HorizontalAlign.Center;
        tab_fact.CellPadding = 2;
        tab_fact.CellSpacing = 1;

        tab_fact.Rows.Add(myRow.Row);
    }

    #endregion

    # region "設定主Table資料"
    private void GenMasterTableData(ref PccErrMsg myLabel)
    {
        PccErrMsg myErrMsg = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Error");

        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_ap_id);
        myMsg.CreateFirstNode("fact_no", txtfactno.Text);
        myMsg.CreateFirstNode("fact_nm", txtfactnm.Text);
        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());

        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());

        DataSet ds;

        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = USERFACTMANAGE;

        string strSp = "";
        //判斷是否要利用事業群來分設權限
        if (ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_ap_id + "-FactByGroup"].ToString() == "Y")
        {
            //判斷此使用者是否可以分配不同的事業群權限
            if (myAuth.IsReportAuth())
                strSp = "GetPubFactByApID";
            else
                strSp = "GetPubFactByUserGroupAndApID";
        }
        else
        {
            strSp = "GetPubFactByApID";
        }

        ds = mybs.DoReturnDataSet(strSp, myMsg.GetXmlStr, "");
        DataTable myTable = ds.Tables["PubFact"];

        PccRow myRow;

        if (myTable.Rows.Count > 0)
        {
            int MasterCount = 1;
            string MasterStyle = "";


            //取出資料總筆數
            PageControl1.TotalSize = ds.Tables["GetPubFactCount"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();
            string cVal = "";
            foreach (DataRow myMasterRow in myTable.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);

                //勾選值 ==
                string chk = "";
                string factno = "";

                //session("")不是Nothing 也不是 ""時則執行
                if (Session["Facttemporary"] != null && Session["Facttemporary"].ToString() != "")
                {
                    string[] chkarray = Session["Facttemporary"].ToString().Split(',');
                    factno = myMasterRow["fact_no"].ToString().Trim();
                    for (int i = 0; i < chkarray.Length; i++)
                    {
                        if (factno == chkarray[i].ToString())
                        {
                            chk = "checked";
                            cVal += factno + ",";

                        }
                    }

                }
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //廠別編號
                myRow.AddTextCell(myMasterRow["fact_no"].ToString(), 10);
                //廠別名稱
                myRow.AddTextCell(myMasterRow["fact_nm"].ToString(), 60);
                //事業群
                myRow.AddTextCell(myMasterRow["fgrp_nm"].ToString(), 20);
                //選取
                myRow.AddTextCell("<input type=checkbox name=fact_no id=fact_no" + MasterCount + " value=" + myMasterRow["fact_no"].ToString().Trim() + " " + chk + ">", 5);

                tab_fact.Controls.Add(myRow.Row);
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
            PageControl1.BuildPager();
            myRow = new PccRow("DGridTD", HorizontalAlign.Center, VerticalAlign.Middle, 5);
            myRow.AddTextCell("<b><font color='darkred'>" + myErrMsg.GetErrMsg("AdtWeb/msg0072") + "</font></b>", 100);
            tab_fact.Controls.Add(myRow.Row);
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
        if (CheckForm("fact_no") == "")
        {
            RequestLength = 0;
        }
        else
        {
            RequestLength = Request.Form["fact_no"].Split(',').Length;
        }
        ArrayList tmpArray = new ArrayList();
        if (Session["Facttemporary"].ToString() != "")
        {
            string[] ChangeAry = Session["Facttemporary"].ToString().Split(',');
            for (int j = 0; j < ChangeAry.Length; j++)
            {
                tmpArray.Add(ChangeAry[j]);
            }
        }

        string strFactNO1 = "";

        //移除這一頁舊有的資料
        if (CValLength > 0)
        {

            string[] aBeforeFactNO = ViewState["CurrentValue"].ToString().Split(',');

            foreach (string strFactNO in aBeforeFactNO)
            {
                if (tmpArray.Count > 0)  //Session("Usertemporary")記錄未上傳前,所挑選的使用者
                {
                    foreach (string strTemp in tmpArray)
                    {
                        strFactNO1 = strTemp.Split('_')[0];
                        if (strFactNO1 == strFactNO)
                        {
                            tmpArray.Remove(strTemp);
                            break;
                        }
                    }
                }
            }
        }

        //新增這一頁的現有資料
        string[] pick_fact;
        if (CheckForm("fact_no") != "")
        {
            pick_fact = Request.Form["fact_no"].Split(',');
            if (RequestLength > 0)
            {
                for (int i = 0; i < pick_fact.Length; i++)
                {
                    tmpArray.Add(pick_fact[i]);
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


        Session["Facttemporary"] = strTemporary;
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

    protected void btnQuery_Click(object sender, System.EventArgs e)
    {
        PageControl1.TotalSize = "0";
        PageControl1.CurrentPage = "1";
        PageControl1.ListCount = "0";
        GenMasterTable();
    }

    protected void Button1_Click(object sender, System.EventArgs e)
    {
        if (ViewState["CurrentValue"] == null && tab_fact.Rows.Count == 0) return;

        Temporary();
        string strFactNo = "";
        bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        strFactNo = Session["Facttemporary"].ToString();
        if (strFactNo.Length == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('您還未選擇任何廠別唷！');</script>");
            GenMasterTable();
            return;
        }


        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ap_id", m_ap_id);
        myMsg.CreateFirstNode("no_str", strFactNo);
        myMsg.CreateFirstNode("upd_id", Session["UserID"].ToString());
        string strReturn = mybs.DoReturnStr("PickMultiFact", myMsg.GetXmlStr, "");
        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") != "0")
        {
            lblMsg.Font.Size = 12;
            lblMsg.Text = myMsg.Query("errmsg");
        }
        else
        {
            Response.Redirect(USERFACTMANAGE + "?ApID=" + m_ap_id);
        }

    }
}
