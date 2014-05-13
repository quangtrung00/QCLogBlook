using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LogBook_Report
/// </summary>
public class LogBook_ReportObj
{
    private string date;
    private string total;
    private string qty;
    private string qtyOKitem;
    private string rate;
    private string bad;   
    private string sign;
    private string yyyymm;

    private List<CYYYYMM> al;
    private string p;
    //private string qtyOK;
  
    private List<global::YYYYMM> listYM;

    public List<CYYYYMM> Al
    {
        get { return al; }
        set { al = value; }
    }

    public string Date
    {
        get { return date; }
        set { date = value; }
    }

    public string Total
    {
        get { return total; }
        set { total = value; }
    }
    public string Qty
    {
        get { return qty; }
        set { qty = value; }
    }
    public string QtyOKitem
    {
        get { return qtyOKitem; }
        set { qtyOKitem = value; }
    }
    public string Rate
    {
        get { return rate; }
        set { rate = value; }
    }
    public string Bad
    {
        get { return bad; }
        set { bad = value; }
    }

    public string Sign
    {
        get { return sign; }
        set { sign = value; }
    }
    public string YYYYMM
    {
        get { return yyyymm; }
        set { yyyymm = value; }
    }


    
    public LogBook_ReportObj()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public LogBook_ReportObj(string date, string total, string qty, string QtyOKitem, string rate, string bad, string sign, string yyyymm)
    {
        this.date = date;
        this.total = total;
        this.qty = qty;
        this.qtyOKitem = QtyOKitem;
        this.rate = rate;
        this.bad = bad;
        this.sign = sign;
        this.yyyymm = yyyymm;

    }

    public LogBook_ReportObj(string date, string total, string qty, string QtyOKitem, string rate, string bad, string sign, List<global::YYYYMM> listYM)
    {
        // TODO: Complete member initialization
        this.date = date;
        this.total = total;
        this.qty = qty;
        this.QtyOKitem = QtyOKitem;
        this.rate = rate;
        this.bad = bad;
        this.sign = sign;
        this.listYM = listYM;
    }
   //public StatisticsObj(string fact_no, string floor, string emp_no, string user_desc, List<CYYYYMM> al)
   // {
   //     this.fact_no = fact_no;
   //     this.floor = floor;
   //     this.emp_no = emp_no;
   //     this.user_desc = user_desc;      
   //     this.al = al;
   // }
  

}
public class YYYYMM
{
    
    private string column_name;//ten cot trong du lieu
    private string date;

    public string Date
    {
        get { return date; }
        set { date = value; }
    }
   

    public string ColumnName
    {
        get { return column_name; }
        set { column_name = value; }
    }

    public YYYYMM(string column_name,string date)//, string acc, string insp
    {
       
        this.column_name = column_name;
        this.date = date;
    }
} 

public class FactLogBook_Report
{
    private string fact_no;

    public string FactNo
    {
        get { return fact_no; }
        set { fact_no = value; }
    }

    public FactLogBook_Report(string fact_no)
    {
        this.fact_no = fact_no;
    }
}
public class DeptLogBook_Report
{
    private string dept_no;
    private string dept_name;

    public string Dept_name
    {
        get { return dept_name; }
        set { dept_name = value; }
    }
public string Dept_no
{
  get { return dept_no; }
  set { dept_no = value; }
}



public DeptLogBook_Report(string dept_no, string dept_name)
    {
        this.dept_no = dept_no;
        this.dept_name = dept_name;
    }
}

public class SecLogBook_Report
{
    private string sec_no;
    private string sec_name;

    public string Sec_name
    {
        get { return sec_name; }
        set { sec_name = value; }
    }
    public string Sec_no
    {
        get { return sec_no; }
        set { sec_no = value; }
    }

    public SecLogBook_Report(string sec_no, string sec_name)
    {
        this.sec_no = sec_no;
        this.sec_name = sec_name;
    }
}
public class QCLogBook_Report
{
    private string user_id;
    private string user_name;

    public string User_name
    {
        get { return user_name; }
        set { user_name = value; }
    }
    public string User_id
    {
        get { return user_id; }
        set { user_id = value; }
    }

    public QCLogBook_Report(string user_id, string user_name)
    {
        this.user_id = user_id;
        this.user_name = user_name;
    }
}