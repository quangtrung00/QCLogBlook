using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for StatisticsObj
/// </summary>
public class StatisticsObj
{
    /*
      a.[fact_no]    
          ,a.[floor]             
      ,c.emp_no
      ,c.user_desc
      ,a.total INSP  
     ,sum(b.qtyOKitem) ACC
   ,substring(b.add_date,0,7) yyyymm
     */
    private string fact_no;
    private string floor;
    private string emp_no;
    private string user_desc;
    private string insp;
    private string acc;
    private string yyyymm;
    private List<CYYYYMM> al;
    
    public List<CYYYYMM> AL
    {
        get { return al; }
        set { al = value; }
    }

    public string FactNo
    {
        get { return fact_no; }
        set { fact_no = value; }
    }

    public string Floor
    {
        get { return floor; }
        set { floor = value; }
    }

    public string EmpNo
    {
        get { return emp_no; }
        set { emp_no = value; }
    }
    public string UserDesc
    {
        get { return user_desc; }
        set { user_desc = value; }
    }
    public string INSP
    {
        get { return insp; }
        set { insp = value; }
    }
    public string ACC
    {
        get { return acc; }
        set { acc = value; }
    }
    public string YYYYMM
    {
        get { return yyyymm; }
        set { yyyymm = value; }
    }

	public StatisticsObj()
	{
		
	}
    public StatisticsObj(string fact_no, string floor, string emp_no, string user_desc, string insp, string acc, string yyyymm)
    {
        this.fact_no = fact_no;
        this.floor = floor;
        this.emp_no = emp_no;
        this.user_desc = user_desc;
        this.insp = insp;
        this.acc = acc;
        this.yyyymm = yyyymm;
    }

    public StatisticsObj(string fact_no, string floor, string emp_no, string user_desc, List<CYYYYMM> al)
    {
        this.fact_no = fact_no;
        this.floor = floor;
        this.emp_no = emp_no;
        this.user_desc = user_desc;      
        this.al = al;
    }

   
}

public class FactStatistics
{
    private string fact_no;

    public string FactNo
    {
        get { return fact_no; }
        set { fact_no = value; }
    }

    public FactStatistics(string fact_no)
    {
        this.fact_no = fact_no;
    }
}

public class CYYYYMM
{
    private string acc;
    private string insp;
    private string column_name;//ten cot trong du lieu

    public string ACC
    {
        get { return acc; }
        set { acc = value; }
    }

    public string INSP
    {
        get { return insp; }
        set { insp = value; }
    }

    public string ColumnName
    {
        get { return column_name; }
        set { column_name = value; }
    }

    public CYYYYMM(string column_name, string acc, string insp)
    {
        this.acc = acc;
        this.insp = insp;
        this.column_name = column_name;
    }
}