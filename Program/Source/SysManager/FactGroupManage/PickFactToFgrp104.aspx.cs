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

public partial class SysManager_FactGroupManage_PickFactToFgrp104 : System.Web.UI.Page
{
    private const string TITLE = "PccTitle";

    private string m_Ap_Id;
    private string m_Fgrp_Id;
    private string m_Fgrp_Nm;

    private const string FACTGROUPMANAGE = "FactGroupManage104.aspx";
    private const string FACTGROUPDETAIL = "FactGroupDetail104.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        // 將使用者程式碼置於此以初始化網頁
        if (Session["UserID"] == null) return;

        //設定公用變數
        PccMsg myMsg = new PccMsg();
        Session["chkcount"] = 0;
        m_Ap_Id = Request.Params["ApID"];
        if (CheckQueryString("Method") != string.Empty)
        {
            myMsg.LoadXml(CheckQueryString("Method"));
            m_Fgrp_Id = myMsg.Query("Key");
            m_Fgrp_Nm = myMsg.Query("KeyOther");
            GenMasterTable();
        }

        if (!IsPostBack)
        {
            //清除Session
            Session["FactFgrpTemporary"] = string.Empty;

            PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");

            SetLabel(ref myLabel);
        }

    }

    #region "設定此頁面之基本資料"

    private void SetLabel(ref PccErrMsg myLabel)
    {
        //設定Title
        for (int i = 0; i < Header1.Controls.Count; i++)
        {
            if (Header1.Controls[i].ID == TITLE)
            {
                Label mylblTitle = (Label)(Header1.Controls[i]);
                mylblTitle.Text = "挑選廠別至<font color=black><b>" + m_Fgrp_Nm + "</b></font>中";
            }
        }

        /*btnQuery.Text = myLabel.GetErrMsg("btnQuery");
        btnClear.Value = myLabel.GetErrMsg("btnClear");*/

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
        myRow.AddTextCell("廠別編號", 20);
        myRow.AddTextCell("廠別名稱", 25);
        myRow.AddTextCell("公司名稱", 30);
        myRow.AddTextCell("事業群", 15);
        myRow.AddTextCell("挑選", 5);

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

        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", m_Ap_Id);
        myMsg.CreateFirstNode("Fgrp_Id", m_Fgrp_Id);

        myMsg.CreateFirstNode("Fact_No", txtfactno.Text);
        myMsg.CreateFirstNode("Fact_Nm", txtfactnm.Text);
        myMsg.CreateFirstNode("Fgrp_Nm", txtfgrpnm.Text);

        myMsg.CreateFirstNode("StartRecord", PageControl1.StartRecord.ToString());
        myMsg.CreateFirstNode("PageSize", PageControl1.PageSize.ToString());

        DataSet ds;

        //以後可以加入若是有Report和Send的權限，表示是群管理者，所以可以看到這個群的所有資料 20041125
        GetMenuAuth myAuth = new GetMenuAuth();
        myAuth.AspxFile = FACTGROUPMANAGE;

        ds = mybs.DoReturnDataSet("getFactByFgrpId", myMsg.GetXmlStr, "");
        DataTable myTable = ds.Tables["FactByFgrp"];

        PccRow myRow;

        if (myTable.Rows.Count > 0)
        {
            int MasterCount = 1;
            string MasterStyle = "";

            //取出資料總筆數
            PageControl1.TotalSize = ds.Tables["TCounts"].Rows[0]["Counts"].ToString();
            PageControl1.BuildPager();
            string cVal = "";
            foreach (DataRow myMasterRow in myTable.Rows)
            {
                if (MasterCount % 2 == 0) MasterStyle = "eee000"; else MasterStyle = "fff000";
                myRow = new PccRow(MasterStyle, HorizontalAlign.Center, VerticalAlign.Middle, 0);

                //勾選值 ==
                string chk = "";
                string factid = "";

                //session("")不是Nothing 也不是 ""時則執行
                if (Session["FactFgrpTemporary"] != null && Session["FactFgrpTemporary"].ToString() != "")
                {
                    string[] chkarray = Session["FactFgrpTemporary"].ToString().Split(',');
                    factid = myMasterRow["fact_id"].ToString().Trim();
                    for (int i = 0; i < chkarray.Length; i++)
                    {
                        if (factid == chkarray[i].ToString())
                        {
                            chk = "checked";
                            cVal += factid + ",";

                        }
                    }

                }
                //編號
                myRow.AddTextCell(PageControl1.ListCount, 5);
                //廠別編號
                myRow.AddTextCell(myMasterRow["fact_no"].ToString(), 20);
                //廠別名稱
                myRow.AddTextCell(myMasterRow["fact_nm"].ToString(), 25);
                //公司名稱
                myRow.AddTextCell(myMasterRow["comp_nm"].ToString(), 30);
                //事業群
                myRow.AddTextCell(myMasterRow["fgrp_nm"].ToString(), 15);
                //選取
                myRow.AddTextCell("<input type=checkbox name=fact_id id=fact_id" + MasterCount + " value=" + ((int)decimal.Parse(myMasterRow["fact_id"].ToString())).ToString() + " " + chk + ">", 5);

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
            Session["chkcount"] = 0;
            PageControl1.BuildPager();
            myRow = new PccRow("DGridTD", HorizontalAlign.Center, VerticalAlign.Middle, 5);
            myRow.AddTextCell("<b><font color='darkred'>" + myErrMsg.GetErrMsg("AdtWeb/msg0073") + "</font></b>", 100);
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
        if (CheckForm("fact_id") == "")
        {
            RequestLength = 0;
        }
        else
        {
            RequestLength = Request.Form["fact_id"].Split(',').Length;
        }
        ArrayList tmpArray = new ArrayList();
        if (Session["FactFgrpTemporary"].ToString() != "")
        {
            string[] ChangeAry = Session["FactFgrpTemporary"].ToString().Split(',');
            for (int j = 0; j < ChangeAry.Length; j++)
            {
                tmpArray.Add(ChangeAry[j]);
            }
        }

        string strFactID1 = "";

        //移除這一頁舊有的資料
        if (CValLength > 0)
        {

            string[] aBeforeFactID = ViewState["CurrentValue"].ToString().Split(',');

            foreach (string strFactID in aBeforeFactID)
            {
                if (tmpArray.Count > 0)  //Session("Fact1Temporary")記錄未上傳前,所挑選的使用者
                {
                    foreach (string strTemp in tmpArray)
                    {
                        strFactID1 = strTemp.Split('_')[0];
                        if (strFactID1 == strFactID)
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
        if (CheckForm("fact_id") != "")
        {
            pick_fact = Request.Form["fact_id"].Split(',');
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


        Session["FactFgrpTemporary"] = strTemporary;
    }
    #endregion

    #region "Button事件"

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
        string strFact = "";
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        strFact = Session["FactFgrpTemporary"].ToString();
        if (strFact.Length == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(),"New", "<script language=javascript>alert('您還未選擇任何廠別唷！');</script>");
            GenMasterTable();
            return;
        }

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", m_Ap_Id);
        myMsg.CreateFirstNode("Fgrp_Id", m_Fgrp_Id);
        myMsg.CreateFirstNode("Fact_Str", strFact);
        myMsg.CreateFirstNode("Upd_Id", Session["UserID"].ToString());
        string strReturn = mybs.DoReturnStr("PickMultiFactToFgrp", myMsg.GetXmlStr, string.Empty);
        myMsg.LoadXml(strReturn);

        if (myMsg.Query("returnValue") != "0")
        {
            lblMsg.Font.Size = 12;
            lblMsg.Text = myMsg.Query("errmsg");
            return;
        }
        else
        {
            Session["FactFgrpTemporary"] = string.Empty;
            Response.Redirect(FACTGROUPDETAIL + "?ApID=" + m_Ap_Id + "&SrcUp_Id=" + Request.QueryString["SrcUp_Id"] + "&Up_Id=" + Request.QueryString["Up_Id"] + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
        }

    }

    #endregion

}
