using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PccBsQCLogBlook;
using PccCommonForC;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Services;
using System.Collections;

public partial class QCLogBlook_Report_LogBook_Statistics : System.Web.UI.Page
{
    public string sStaFactName = "";
    public string sStaSecNo = "";
    public string sStaPnlNo = "";
    public string sStaQCCD = "";
    public string sStaCancel = "";
    public string sStaOk = "";
    public string sStaSearch = "";
    public string sStaMsgNoData = "";
    public string sStaMsgSelect = "";
    public string sDDMM = "";
    public string sStaSearchForm = "";
    public string sStaTitle = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //GetStatistics("172", "", "");
            SetLabel();
        }
    }

    private void SetLabel()
    {
       // PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("XmlDoc"), Session["CodeLang"].ToString(), "Label", true);
        sStaFactName = myLabel.GetErrMsg("lbl0018", "QC/Tilte"); //廠別    
        sStaSecNo = myLabel.GetErrMsg("lbl0008", "QC/Tilte"); //樓層    
        sStaPnlNo = myLabel.GetErrMsg("lbl0034", "QC/Tilte"); //工號    
        sStaQCCD = myLabel.GetErrMsg("lbl0020", "QC/Tilte"); //定點QC    
        sDDMM = myLabel.GetErrMsg("lbl0019", "QC/Tilte");
        sStaSearchForm = myLabel.GetErrMsg("lbl0035", "QC/Tilte");
        sStaTitle = myLabel.GetErrMsg("lbl0036", "QC/Tilte");
        sStaMsgNoData = myLabel.GetErrMsg("lbl0001", "QC/Message");
        sStaMsgSelect = myLabel.GetErrMsg("lbl0002", "QC/Message");
        sStaOk = myLabel.GetErrMsg("btnOK");
        sStaCancel = myLabel.GetErrMsg("btnCancel");
        sStaSearch = myLabel.GetErrMsg("btnQuery");
        Label1.Text = sStaTitle;
    }

    public static DataSet GetStatistics(string fact_no, string start_date, string end_date)
    {
        bs_Statistics mybs = new bs_Statistics(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("start_date", start_date);
        myMsg.CreateFirstNode("end_date", end_date);

        return mybs.DoReturnDataSet("GETSTATISTICS", myMsg.GetXmlStr, "");
    }

    public static DataSet GetFact(string fact_no)
    {
        bs_Statistics mybs = new bs_Statistics(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);

        return mybs.DoReturnDataSet("GETFACT", myMsg.GetXmlStr, "");
    }

    [WebMethod]
    public static FactStatistics[] GetFactAjax()
    {
        List<FactStatistics> list = new List<FactStatistics>();
        string pfact_no = "";
        DataSet ds = GetFact(pfact_no);
        DataTable dt = ds.Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];
            string fact_no = dr["fact_no"].ToString();

            FactStatistics obj = new FactStatistics(fact_no);
            list.Add(obj);

        }

        return list.ToArray();
    }

    [WebMethod]
    public StatisticsObj[] GetStatisticsAjax(string fact_no, string start_date, string end_date)
    {
        List<StatisticsObj> list = new List<StatisticsObj>();
        //string fact_no="", start_date="20140401",  end_date="20141201";
        fact_no = Request.Form["factno"];
        start_date = Request.Form["startdate"];
        end_date = Request.Form["enddate"];

        DataSet ds = GetStatistics(fact_no, start_date, end_date);
        DataTable dt = ds.Tables[0];//acc



        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];

            string fact_no1 = dr["fact_no"].ToString();
            string floor = dr["floor"].ToString();
            string emp_no = dr["emp_no"].ToString();
            string user_desc = dr["user_desc"].ToString();
            string insp = dr["insp"].ToString();
            string acc = dr["acc"].ToString();
            string yyyymm = dr["yyyymm"].ToString();

            StatisticsObj obj = new StatisticsObj(fact_no1, floor, emp_no, user_desc, insp, acc, yyyymm);
            list.Add(obj);

        }

        return list.ToArray();
    }
    [WebMethod]
    public static StatisticsObj[] GetStatisticsAjaxNew(string pfact_no, string start_date, string end_date)
    {
        List<StatisticsObj> list = new List<StatisticsObj>();
        List<CYYYYMM> listYM = new List<CYYYYMM>();
        //string pfact_no = "", start_date = "20140401", end_date = "20141201";       

        try
        {

            DataSet ds = GetStatistics(pfact_no, start_date, end_date);


            DataTable dt = ds.Tables[0];//acc
            DataTable dt1 = ds.Tables[1];//insp

            DataColumnCollection dcs = dt.Columns;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                DataRow dr1 = dt1.Rows[i];

                string fact_no = dr["fact_no"].ToString();
                string floor = dr["floor"].ToString();
                string emp_no = dr["emp_no"].ToString();
                string user_desc = dr["user_desc"].ToString();
                string insp = "0";
                string acc = "0", sColName = "";

                //get yyyymm columns
                foreach (DataColumn dc in dcs)
                {
                    if (!CheckColumns(dc))
                    {
                        sColName = dc.ColumnName;
                        acc = dr[sColName].ToString();
                        insp = dr1[sColName].ToString();
                        CYYYYMM al = new CYYYYMM(sColName, acc, insp);//mang luu ten cot
                        listYM.Add(al);
                    }
                }

                StatisticsObj obj = new StatisticsObj(fact_no, floor, emp_no, user_desc, listYM);
                list.Add(obj);
                //reset listYM
                listYM = new List<CYYYYMM>();
            }
        }
        catch (Exception ex)
        {
        }

        return list.ToArray();
    }

    public static bool CheckColumns(DataColumn dc)
    {
        bool b = false;//cot khong thuoc nhom cot can lay

        b = dc.ColumnName.Equals("fact_no") || dc.ColumnName.Equals("floor") || dc.ColumnName.Equals("emp_no") || dc.ColumnName.Equals("user_desc");

        return b;
    }
}