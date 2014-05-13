using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PccBsQCLogBlook;
using Pcc.pfs.Tools.Helper;
using System.Configuration;
using System.Web.UI.WebControls;

using System.Web.SessionState;
/// <summary>
/// Summary description for _DropDownList
/// </summary>
public class _DropDownList
{
	public _DropDownList()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region mimi
    public static string _Get_DeptByfact(string fact_no)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        string sReturn = mybs.DoReturnStr("GETDEPTBYFACTSTR", myMsg.GetXmlStr, "");
        return sReturn.Trim();


    }

    public static void _Get_ControlDeptByfact(DropDownList ddl, string fact_no)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        DataSet dsReturn = mybs.DoReturnDataSet("GETDEPTBYFACT", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        ddl.SelectedIndex = -1;
        ddl.DataSource = dt;

        ddl.DataTextField = "dept_no";
        ddl.DataValueField = "dept_no";        
        ddl.DataBind();
      //  ddl.Items.Insert(0, new ListItem("--All--", ""));
        
    }
    public static void _Get_ControlSecByDept(DropDownList ddl, string fact_no,string dept_no)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        DataSet dsReturn = mybs.DoReturnDataSet("GETSECBYDEPT", myMsg.GetXmlStr, "");

        //DataTable dt = dsReturn.Tables[0];
        //ddl.SelectedIndex = -1;
        //ddl.DataSource = dt;

        //ddl.DataTextField = "sec_no";
        //ddl.DataValueField = "sec_no";

        //ddl.DataBind();

    }

    #endregion
    #region "return fact by user_id -- web.huuminh"
    public static string GetFactNoByUserID(string UserID)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("UserID", UserID);

        string sReturn = mybs.DoReturnStr("GetFactNoByUserID", myMsg.GetXmlStr, "");
        return sReturn.Trim();
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
    #endregion

    //web.huuminh 20140416
    // lây dept dua vào uer_id
    public static void _Get_ControlDept(DropDownList ddl, string UserID,string Type)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("UserID", UserID);
        DataSet dsReturn = mybs.DoReturnDataSet("GETDEPTBYUSERID", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        ddl.SelectedIndex = -1;
        ddl.DataSource = dt;
        if (Type == "1")
        {
            ddl.DataTextField = "dept_name";
            ddl.DataValueField = "dept_no";
        }      
        ddl.DataBind();
        //if (dt.Rows.Count > 1 || dt.Rows.Count == 0)
        //    ddl.Items.Insert(0, new ListItem("--All--", ""));
    }
    //web.huuminh 20140416
    // lây sec dua vào uer_id,dept_no,type_Check
    public static void _Get_ControlSec(DropDownList ddl, string UserID, string dept_no, string type_Check)
    {
        bs_UserInfo mybs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("UserID", UserID);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("type_Check", type_Check);
        DataSet dsReturn = mybs.DoReturnDataSet("GETSECBYUSERID", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        ddl.SelectedIndex = -1;
        ddl.DataSource = dt;
        ddl.DataTextField = "secname";
        ddl.DataValueField = "sec_no";
       
        ddl.DataBind();
        //if (dt.Rows.Count > 1 || dt.Rows.Count == 0)
        //    ddl.Items.Insert(0, new ListItem("--All--", ""));
    }


}