using System;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using PccBsLayerForC;
using PccBsQCLogBlook;
using PccDbLayerForC;
using System.Net;
using System.Web.Mail;
using System.Web.Services;
using System.IO;
using System.Collections.Generic;

public partial class QCLogBlook_Report_LogBook_Report : System.Web.UI.Page
{
    ArrayList arrTitle;
    ArrayList arrTT;
    string strDetailPageLayer = "../../..";
     private string UserName = string.Empty;
     public string xmlUserID;
    protected string xmlUserName;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        //PccErrMsg myLabel = new PccErrMsg(Server.MapPath(strDetailPageLayer + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        //SetLabel(ref myLabel);
        if (Session["UserID"] == null) return;
        // 將使用者程式碼置於此以初始化網頁
        string xmlLoginInfo = Session["XmlLoginInfo"].ToString();
        PccMsg myInfoMsg = new PccMsg(xmlLoginInfo);
        xmlUserID = myInfoMsg.Query("UserID").ToString().Trim();
        xmlUserName = myInfoMsg.Query("UserName").ToString().Trim();
        string xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
        if (!IsPostBack)
        {
            
           // BindDataTable();
            lblTitle.Text = "Quality Issue Log Book  品質問題 Log Book(Vấn đề phẩm chất Log Book )";
        }
    }
    private void SetLabel(ref PccErrMsg msg)
    {
        
        arrTitle = new ArrayList();
        arrTitle.Add(msg.GetErrMsg("Ngay"));//STT
        arrTitle.Add(msg.GetErrMsg("So doi kiem nghiem"));//tuyen duong        
        arrTitle.Add(msg.GetErrMsg("Ket qua kiem nghiem"));//so xe
      

    }
   
    #region Table
   
    private void BindDataTable()//string start_date, string end_date
    {
        Table tbl_Month = new Table();
        tbl_Month.Width = Unit.Percentage(100);
        tbl_Month.CssClass = "tbl1";
        tbl_Month.CellPadding = 2;
        tbl_Month.CellSpacing = 0;

        //Add Header
        CreateHeader(ref tbl_Month);
        BindData(ref tbl_Month);

        //AddRows
      
        // pnlMaster.Controls.Add(tbl_Month);
        pnlMaster.Controls.Add(tbl_Month);
        Literal ltrBr = new Literal();
        ltrBr.Text = "<br>";
        pnlMaster.Controls.Add(ltrBr);
    }
    private void BindData(ref Table tblMonth)
    {
        //Table tblMonth = new Table();
       /* tblMonth.Width = Unit.Percentage(100);
        tblMonth.CssClass = "tbl1";
        tblMonth.CellPadding = 2;
        tblMonth.CellSpacing = 0;

        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        int i = 0;
        int j = 0;
        string pfact_no = "172", pdept_no = "3", pSec_no = "AC5P", userQC_id = "524", start_date = "20140617083501", end_date = "20140617083501";
        DataTable dt = new DataTable();
        dt = GetSumQty(pfact_no, pdept_no, pSec_no, userQC_id, start_date, end_date);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow rw in dt.Rows)
            {
                //date
                cell = new TableCell();
                cell.Text = rw["upd_date"].ToString();
                cell.Height = Unit.Pixel(10);

                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);  
                //total
                cell = new TableCell();
                cell.Text = rw["SumTotal"].ToString();

                  decimal total = decimal.Parse(rw["SumTotal"].ToString());
                cell.Height = Unit.Pixel(10);

                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);  
                //qty
                cell = new TableCell();
                cell.Text = rw["qty"].ToString();
                string qty;
                    qty= rw["qty"].ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                cell.Height = Unit.Pixel(10);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);
                //qtyOKitem
                cell = new TableCell();
                cell.Text = rw["qtyOKitem"].ToString();              
                cell.Height = Unit.Pixel(10);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);
                //Pass Rate Tỷ lệ thông qua
                cell = new TableCell();
                decimal Rate=decimal.Parse(qty)/total;
                cell.Text = Rate.ToString();
                cell.Height = Unit.Pixel(10);

                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);
                //note
                cell = new TableCell();
                cell.Text = rw["bad_descVN"].ToString();
                cell.Height = Unit.Pixel(10);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Width = Unit.Percentage(10);
                cell.BorderWidth = 1;
                row.Cells.Add(cell);
                //Signature 
                //cell = new TableCell();
                //cell.Text = rw["bad_descVN"].ToString();
                //cell.Height = Unit.Pixel(10);
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.Width = Unit.Percentage(10);
                //cell.BorderWidth = 1;
                //row.Cells.Add(cell);
            }
            tblMonth.Rows.Add(row);
        }*/
    }

    
    private void CreateHeader(ref Table tblMonth)//, string Dept_no,string sec_no,string QC, string start_date, string end_date
    {
        PccMsg msgCss = new PccMsg();
        msgCss.CreateFirstNode("Css", "tbl1_trHeader");
        TableRow rows = new TableRow();
        TableCell cells = new TableCell();

        //
        //Ho ten
        
      //  tblMonth.Rows.Add(rows);
        //rows = new TableRow();     
        //cells = new TableCell();
        cells.ColumnSpan = 7;
        AddCellHTMLToRow(rows, cells, "Ho ten:" + xmlUserName, msgCss);//tháng lấy ra
        
        cells.Style.Add("text-align", "left");        
        tblMonth.Rows.Add(rows);
        //row2
        //Ngay *
        
        rows = new TableRow(); 
        cells = new TableCell();
        cells.RowSpan = 2;
        cells.Width = Unit.Percentage(1);
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells,"Ngay", msgCss);//, arrTitle[0].ToString()
        //So doi kiem nghiem *
        
        cells = new TableCell();
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.RowSpan = 2;
        cells.Width = Unit.Percentage(1);
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells,"So doi kiem nghiem", msgCss);//, arrTitle[1].ToString()
        //Ket qua kiem nghiem *
       
        cells = new TableCell();
        cells.ColumnSpan = 2;
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.Width = Unit.Percentage(1);
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells,"Ket qua kiem nghiem", msgCss);// arrTitle[2].ToString(),
        //ty le       
        cells = new TableCell();
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.RowSpan = 2;
        cells.Width = Unit.Percentage(1);
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells, "Ty le thong qua", msgCss);//, arrTitle[0].ToString()
        //ghi chu *
        cells = new TableCell();
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.RowSpan = 2;
        cells.Width = Unit.Percentage(1);
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells, "Ghi chu van de", msgCss);//, arrTitle[1].ToString()

        //Nguoi ky *
      
        cells = new TableCell();
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.RowSpan = 2;
        cells.Width = Unit.Percentage(1);
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells, "Nguoi ky", msgCss);//, arrTitle[1].ToString()


        tblMonth.Rows.Add(rows);

        //row3
        //Ngay *
        rows = new TableRow();     
        cells = new TableCell();
        cells.Width = Unit.Percentage(1);
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells, "So luong thong qua", msgCss);//, arrTitle[0].ToString()
        //
        cells = new TableCell();        
        cells.Width = Unit.Percentage(1);
        cells.Attributes.CssStyle.Add("width", "5%");
        cells.HorizontalAlign = HorizontalAlign.Center;
        cells.Wrap = false;
        AddCellHTMLToRow(rows, cells, "So luong khong thong qua", msgCss);//, arrTitle[0].ToString()
        tblMonth.Rows.Add(rows);
    }
    private void AddCellHTMLToRow(TableRow row, TableCell cell, string Text, PccMsg Msg)
    {
        if (Msg.Query("Css") != "") cell.CssClass = Msg.Query("Css");
        Text = Server.HtmlDecode(Text);
        cell.Text = Text;
        cell.BorderWidth = Unit.Pixel(1);
        row.Cells.Add(cell);
    }
    #endregion
   
    public DataTable GetUserNameSign()
    {
        bs_UserInfo bs_Route = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id", Session["UserID"].ToString().Trim());   
        string strXML = myMsg.GetXmlStr;
        DataTable dt = bs_Route.DoReturnDataSet("GETUSERNAMESIGN", strXML, "").Tables[0];
        return dt;
    }

    public static DataSet GetSumQty(string fact_no, string dept_no, string sec_no, string userQC_id, string start_date, string end_date)
    {
        bs_UserInfo bs_Route = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();

        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("userQC_id", userQC_id);
        myMsg.CreateFirstNode("start_date", start_date);
        myMsg.CreateFirstNode("end_date", end_date);


        string strXML = myMsg.GetXmlStr;
        return  bs_Route.DoReturnDataSet("SUMQTY", strXML, "");
      
    }

    #region Get DLL

    [WebMethod]
    public static    FactLogBook_Report[] GetFactAjax(string user_id)
    {
        List<FactLogBook_Report> list = new List<FactLogBook_Report>();
        string fact_no = _DropDownList.GetFactNoByUserID(user_id);
        FactLogBook_Report obj = new FactLogBook_Report(fact_no);
            list.Add(obj);     

        return list.ToArray();
    }

    public static DataSet GetDept(string fact_no)//,string dept_no
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
      //  myMsg.CreateFirstNode("dept_no", dept_no);
        return mybs.DoReturnDataSet("GETDEPTBYFACT", myMsg.GetXmlStr, "");
    }

//dept
    [WebMethod]
    public static DeptLogBook_Report[] GetDeptAjax(string user_id)
    {
        List<DeptLogBook_Report> list = new List<DeptLogBook_Report>();
        string pfact_no = _DropDownList.GetFactNoByUserID(user_id);
        DataTable dsdept = GetDept(pfact_no).Tables[0];


        for (int i = 0; i < dsdept.Rows.Count; i++)
        {
            DataRow dr = dsdept.Rows[i];
            string dept_no = dr["dept_no"].ToString();
            string dept_name = dr["dept_name"].ToString();
            DeptLogBook_Report obj = new DeptLogBook_Report(dept_no, dept_name);
            list.Add(obj);

        }

        return list.ToArray();

    }
    //sec 
    public static DataSet GetSec(string fact_no, string dept_no)//
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        return mybs.DoReturnDataSet("GETSECBYDEPT", myMsg.GetXmlStr, "");
    }

    [WebMethod]   
    public static SecLogBook_Report[] GetSecByDeptAjax(string user_id,string dept_no)
    {
        List<SecLogBook_Report> list = new List<SecLogBook_Report>();
        string pfact_no = _DropDownList.GetFactNoByUserID(user_id);
        DataTable dsdept = GetDept(pfact_no).Tables[0];
        //string dept_no = "";
        //for (int i = 0; i < dsdept.Rows.Count; i++)
        //{
        //    DataRow dr = dsdept.Rows[i];
        //    dept_no = dr["dept_no"].ToString();

        //    SecLogBook_Report obj = new SecLogBook_Report(dept_no);
        //    list.Add(obj);
        //}

        DataTable dsSec = GetSec(pfact_no, dept_no).Tables[0];

        for (int i = 0; i < dsSec.Rows.Count; i++)
        {
            DataRow dr = dsSec.Rows[i];
            string sec_no = dr["sec_no"].ToString();
            string sec_name = dr["sec_name"].ToString();
            SecLogBook_Report obj = new SecLogBook_Report(sec_no, sec_name);
            list.Add(obj);

        }

        return list.ToArray();

    }
    //QC 
    public static DataSet GetQC(string fact_no, string dept_no, string sec_no)//
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        return mybs.DoReturnDataSet("GETQCCODINH", myMsg.GetXmlStr, "");
    }
    [WebMethod]
    public static QCLogBook_Report[] GetQCBySecAjax(string user_id, string dept_no, string sec_no)
    {
        List<QCLogBook_Report> list = new List<QCLogBook_Report>();
        string pfact_no = _DropDownList.GetFactNoByUserID(user_id);
        DataTable dsdept = GetDept(pfact_no).Tables[0];
        //string dept_no = "";
        //for (int i = 0; i < dsdept.Rows.Count; i++)
        //{
        //    DataRow dr = dsdept.Rows[i];
        //    dept_no = dr["dept_no"].ToString();

        //    SecLogBook_Report obj = new SecLogBook_Report(dept_no);
        //    list.Add(obj);
        //}

        DataTable dsQC = GetQC(pfact_no, dept_no, sec_no).Tables[0];

        for (int i = 0; i < dsQC.Rows.Count; i++)
        {
            DataRow dr = dsQC.Rows[i];
            string QC_id = dr["user_id"].ToString();
            string QC_name = dr["user_nm"].ToString();
            QCLogBook_Report obj = new QCLogBook_Report(QC_id,QC_name);
            list.Add(obj);

        }

        return list.ToArray();

    }

    #endregion


    [WebMethod]
    public static LogBook_ReportObj[] GetSumQtyAjax(string pfact_no, string pdept_no, string psec_no, string pQC, string start_date, string end_date)
    {
        List<LogBook_ReportObj> list = new List<LogBook_ReportObj>();
        List<YYYYMM> listYM = new List<YYYYMM>();
        //string pfact_no = "", pdept_no = "", pSec_no = "", userQC_id = "", start_date = "20140401", end_date = "20141201";

        try
        {

            DataSet ds = GetSumQty(pfact_no,pdept_no,psec_no,pQC, start_date, end_date);


            DataTable dt = ds.Tables[0];//acc
           // DataTable dt1 = ds.Tables[1];//insp

            DataColumnCollection dcs = dt.Columns;
          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
               // DataRow dr1 = dt1.Rows[i];substring(a.upd_date,0,9)
                
                string date = dr["date"].ToString();
                string total = dr["SumTotal"].ToString();
                string qty = dr["qty"].ToString();
                string QtyOKitem = dr["qtyOKitem"].ToString();
                string bad = dr["bad_descVN"].ToString();
                float QtyOKitem1 = float.Parse(QtyOKitem);
                float total1 = float.Parse(QtyOKitem);
                float rate1 = 0;
                if (QtyOKitem1 != 0)
                {
                    rate1 = (QtyOKitem1 / total1);
                }
                else
                {
                    rate1 = 0;
                }
                string rate = rate1.ToString();
                string sign="";
                //string insp = "0";
                string acc = "0", sColName = "";

                //get yyyymm columns
                //foreach (DataColumn dc in dcs)
                //{
                //    if (!CheckColumns(dc))
                //    {
                //        sColName = dc.ColumnName;
                //       // acc = dr[sColName].ToString();
                //     //   insp = dr1[sColName].ToString();
                //        YYYYMM al = new YYYYMM(sColName);//mang luu ten cot
                //        listYM.Add(al);
                //    }
                //}

                LogBook_ReportObj obj = new LogBook_ReportObj(date, total, qty, QtyOKitem, rate, bad, sign, listYM);
                list.Add(obj);
                //reset listYM
                listYM = new List<YYYYMM>();
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