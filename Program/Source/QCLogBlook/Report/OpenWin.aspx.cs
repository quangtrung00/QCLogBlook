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

public partial class QCLogBlook_Report_OpenWin : System.Web.UI.Page
{
    ArrayList arrTitle;
    ArrayList arrTT;
    string strDetailPageLayer = "../../..";
    private string UserName = string.Empty;
    protected string xmlUserID;
    protected string xmlUserName;
   protected  string xmlFact_no = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Ajax.Utility.RegisterTypeForAjax(typeof(QCLogBlook_Report_OpenWin));
        if (Session["UserID"] == null) return;
        // 將使用者程式碼置於此以初始化網頁
        string xmlLoginInfo = Session["XmlLoginInfo"].ToString();
        PccMsg myInfoMsg = new PccMsg(xmlLoginInfo);
        xmlUserID = myInfoMsg.Query("UserID").ToString().Trim();
        xmlUserName = myInfoMsg.Query("UserName").ToString().Trim();

        if (!IsPostBack)
        {

            xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
            lblFactNo.Text = xmlFact_no;
            //lay Dpet
            _DropDownList._Get_ControlDeptByfact(ddlDeptNo, xmlFact_no);
        //    _DropDownList._Get_ControlSecByDept(ddlSec, xmlFact_no, ddlDeptNo.SelectedValue.ToString());
         //  GetSec();
         //  Get_QC();
        }
    }
       
         [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
    public DataTable Getsec_noByFactAjax(string dept_no)//string fact_no, 
    {
        DataTable dt = Get_SecByDept(dept_no.Trim());

        DataTable dtRes = new DataTable();
        dtRes.Columns.Add(new DataColumn("sec_no"));
        //dtRes.Columns.Add(new DataColumn("sec_no"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow row = dtRes.NewRow();
            row[0] = dt.Rows[i]["sec_no"].ToString();
           // row[1] = dt.Rows[i]["sec_no"].ToString();
            dtRes.Rows.Add(row);
        }
        return dtRes;


    }

    private DataTable Get_SecByDept(string dept_no)//string fact_no,
    {
        xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", xmlFact_no.ToString());
        myMsg.CreateFirstNode("dept_no", dept_no.Trim());
        DataSet dsReturn = mybs.DoReturnDataSet("GETSECBYDEPT", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;

    }

    private void GetSec()
    {
        DataTable dt = Get_SecByDept(ddlDeptNo.SelectedValue.Trim());       
        ddlSec.DataSource = dt;
        ddlSec.DataTextField = "sec_no";
        ddlSec.DataValueField = "sec_no";
        ddlSec.DataBind();
     //   ddlSec.Items.Insert(0, new ListItem("--All--", ""));
    }
    //get QC
    [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
    public DataTable GetQC_noBySecAjax( string dept_no,string sec_no)//string fact_no, 
    {
        DataTable dt = Get_QCbySec( dept_no.Trim(),sec_no.Trim());

        DataTable dtRes = new DataTable();
        dtRes.Columns.Add(new DataColumn("user_id"));
        //dtRes.Columns.Add(new DataColumn("sec_no"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow row = dtRes.NewRow();
            row[0] = dt.Rows[i]["user_id"].ToString();
            // row[1] = dt.Rows[i]["sec_no"].ToString();
            dtRes.Rows.Add(row);
        }
        return dtRes;


    }
 
    private DataTable Get_QCbySec( string dept_no,string sec_no)
    {
      //  xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
        // myMsg.CreateFirstNode("user_id", xmlUserID.Trim());
        myMsg.CreateFirstNode("fact_no", xmlFact_no.ToString());
        myMsg.CreateFirstNode("dept_no", dept_no);// ddlDeptNo.SelectedValue.ToString()
        myMsg.CreateFirstNode("sec_no", sec_no.Trim());
        DataSet dsReturn = mybs.DoReturnDataSet("GETQCCODINH", myMsg.GetXmlStr, "");

       DataTable dt = dsReturn.Tables[0];

     
        return dt;

    }
    private void Get_QC()
    {
       DataTable dt = Get_QCbySec(ddlDeptNo.SelectedValue.Trim(),ddlSec.SelectedValue.Trim());
        ddlQC.DataSource = dt;
        ddlQC.DataTextField = "user_nm";
        ddlQC.DataValueField = "user_id";
        ddlQC.DataBind();
        //   ddlSec.Items.Insert(0, new ListItem("--All--", ""));
    }

   

    public static void SetDropDownListSelectIndex(DropDownList ddl, string sValue)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (ddl.Items[i].Text.Trim() == sValue)
            {
                ddl.SelectedIndex = i;
                break;
            }
        }
    }
    protected void btnSeacrh_Click(object sender, EventArgs e)
    {
       // xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
       
       //string dêp _DropDownList._Get_ControlDeptByfact(ddlDeptNo, xmlFact_no);
       // string sUrl = "";
       // sUrl = "LogBook_Report.aspx.aspx?fact_no=" + xmlFact_no + "&dept_no=" + sTypeNo + "&name=" + sName + "&email=" + sEmail + "&ReturnMsg=" + sErrCode;

       // Response.Redirect(sUrl);
    }

    protected void ddlDeptNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSec();
        Get_QC();
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        Get_QC();
    }
}
