using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PccBsQCLogBlook;
using System.Data;
using System.Configuration;
using PccCommonForC;
using System.Collections;

/// <summary>
/// Summary description for BasicData
/// </summary>
public class BasicData
{
    bs_BasicData myBs;
	public BasicData()
	{
        myBs = new bs_BasicData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
	}

    public string GetConnectionString(string fact_no)
    {
        string connectionString = "";


        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);

        DataTable dt = myBs.DoReturnDataSet("PRO_GET_CONNECTION", myMsg.GetXmlStr, "").Tables[0];
        if (dt.Rows.Count > 0)
            connectionString = dt.Rows[0]["connectString"].ToString();
      

        return connectionString;
    }
    public bs_BasicData GetERPConnect(string fact_no)
    {
        string connectionString = "<PccMsg>" + GetConnectionString(fact_no) + "</PccMsg>";
        
        PccMsg conMsg = new PccMsg(connectionString);
        string Type = conMsg.Query("Type");
        string sv = conMsg.Query("Server");
        string DB = conMsg.Query("DB");
        string User = conMsg.Query("User");
        string Pwd = conMsg.Query("Pwd");

        return new bs_BasicData(Type, sv, DB, User, Pwd);

    }

    #region ERP Data
    public DataTable GetDeptERP(string fact_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);

        return oraBs.DoReturnDataSet("GETDEPT", myMsg.GetXmlStr, "").Tables[0];
    }

    public DataTable GetSecERP(string fact_no, string dept_no, string build_no, string floor)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("build_no", build_no);
        myMsg.CreateFirstNode("floor", floor);

        return oraBs.DoReturnDataSet("GETSEC", myMsg.GetXmlStr, "").Tables[0];
    }

    public DataTable GetBuildERP(string fact_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);

        return oraBs.DoReturnDataSet("GETBUILD", myMsg.GetXmlStr, "").Tables[0];
    }

    public DataTable GetFloorERP(string fact_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
;

        return oraBs.DoReturnDataSet("GETFLOOR", myMsg.GetXmlStr, "").Tables[0];
    }

    //web.huuminh - 2014 04 16
    public DataTable GetDataOdrno(string fact_no, string dept_no, string sec_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);

        return oraBs.DoReturnDataSet("GETFACTODRNO", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 16
    public DataTable GetBbadReason(string fact_no, string dept_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);

        return oraBs.DoReturnDataSet("GET_BAD_REASON", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 16
    public DataTable get_sum_act_qty(string fact_no, string dept_no, string sec_no, string vou_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("vou_no", vou_no);
        return oraBs.DoReturnDataSet("GETSUMACT_QTY", myMsg.GetXmlStr, "").Tables[0];
    }

    //web.huuminh -2014 04 19
    public DataTable get_floor_erp(string fact_no, string dept_no, string sec_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        return oraBs.DoReturnDataSet("GETFLOORERP", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 19
    public DataTable get_build_no_erp(string fact_no, string dept_no, string sec_no, string floor)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("floor", floor);
        return oraBs.DoReturnDataSet("GETBUILDNOERP", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 19
    public DataTable testCountBadNoErp(string vou_no, string fact_no, string rec_date, string sec_no, string bad_no, string kind_mk, string upd_user, string dept_no, string time_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("kind_mk", kind_mk);
        myMsg.CreateFirstNode("upd_user", upd_user);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("time_no", time_no);

        return oraBs.DoReturnDataSet("TESTCOUNTBADNOERP", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 19
    public DataTable testCountVouNOErp(string vou_no, string fact_no, string rec_date,string bad_no, string sec_no, string kind_mk)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("kind_mk", kind_mk);
        return oraBs.DoReturnDataSet("TESTCOUNTERPVOUNO", myMsg.GetXmlStr, "").Tables[0];
    }
    //web.huuminh -2014 04 19
    public DataTable get_time_no(string fact_no, string dept_no, string time_now)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();       
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("time_now", time_now);
        return oraBs.DoReturnDataSet("GET_TIME_NO", myMsg.GetXmlStr, "").Tables[0];
    }
    public string SaveDataErp(string fact_no,string rec_date,string fact_odr_no,string sec_no,string bad_no,string time_no,string kind_mk,string upd_user,string upd_time,string qty,string qty_chk,string dept_no,string Type)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("vou_no", fact_odr_no);

        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("time_no", time_no);

        myMsg.CreateFirstNode("kind_mk", kind_mk);
        myMsg.CreateFirstNode("upd_user", upd_user);
        myMsg.CreateFirstNode("upd_time", upd_time);
        myMsg.CreateFirstNode("qty", qty);
        myMsg.CreateFirstNode("qty_chk", qty_chk);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("Type", Type);

        string strXML = myMsg.GetXmlStr;
        string strReturn = oraBs.DoReturnStr("INSUPDDATA_QC_WEB", strXML, "");

        return strReturn;

    }
    public string SaveDataErpQC02(string fact_no, string rec_date, string fact_odr_no, string sec_no, string bad_no, string time_no, string kind_mk, string upd_user, string upd_time, string qty, string qty_chk,string dept_no, string Type)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("vou_no", fact_odr_no);

        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("time_no", time_no);

        myMsg.CreateFirstNode("kind_mk", kind_mk);
        myMsg.CreateFirstNode("upd_user", upd_user);
        myMsg.CreateFirstNode("upd_time", upd_time);
        myMsg.CreateFirstNode("qty", qty);
        myMsg.CreateFirstNode("qty_chk", qty_chk);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("Type", Type);

        string strXML = myMsg.GetXmlStr;
        string strReturn = oraBs.DoReturnStr("INSUPDDATA_QC_WEB_02OK", strXML, "");

        return strReturn;

    }
    public DataTable sum_qty_02(string vou_no, string fact_no, string rec_date, string sec_no, string kind_mk)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("kind_mk", kind_mk);
        return oraBs.DoReturnDataSet("SUMQTY02", myMsg.GetXmlStr, "").Tables[0];
    }
    public DataTable sum_qty_02ByBad_no(string vou_no, string fact_no, string rec_date, string sec_no, string bad_no, string kind_mk)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("kind_mk", kind_mk);
        return oraBs.DoReturnDataSet("SUMQTY02BYBADNO", myMsg.GetXmlStr, "").Tables[0];
    }
    public DataTable sum_qty_erp(string vou_no, string fact_no, string sec_no, string kind_mk, string time_no, string rec_date, string upd_user, string dept_no)
    {
        bs_BasicData oraBs = GetERPConnect(fact_no);

        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("kind_mk", kind_mk);
        myMsg.CreateFirstNode("time_no", time_no);
        myMsg.CreateFirstNode("rec_date", rec_date);
        myMsg.CreateFirstNode("upd_user", upd_user);
        myMsg.CreateFirstNode("dept_no", dept_no);


        return oraBs.DoReturnDataSet("SUMQTYERP", myMsg.GetXmlStr, "").Tables[0];
    }
    #endregion


    #region QCFact_Dept
    public DataTable GetFactQC()
    {
        return myBs.DoReturnDataSet("PRO_GET_FACTQC", "", "").Tables[0];
    }

    public DataTable GetApUser(string user_id)
    {
        string ap_id = ConfigurationSettings.AppSettings["ApID"];
        PccMsg myMsg = new PccMsg();

        myMsg.CreateFirstNode("ap_id", ap_id);
        myMsg.CreateFirstNode("user_id", user_id);

        return myBs.DoReturnDataSet("PRO_GET_APUSER", myMsg.GetXmlStr, "").Tables["ApUser"];
    }

   

    public DataTable GetApUser(string user_desc, string email, string fact_no,  string startRecord, string pageSize, ref string totalRecord )
    {
        string ap_id = ConfigurationSettings.AppSettings["ApID"];
        PccMsg myMsg = new PccMsg();
        
        myMsg.CreateFirstNode("ap_id",ap_id);
        myMsg.CreateFirstNode("user_desc",user_desc);
        myMsg.CreateFirstNode("email", email);
        myMsg.CreateFirstNode("fact_no", fact_no);
 
        myMsg.CreateFirstNode("startRecord", startRecord);
        myMsg.CreateFirstNode("pageSize", pageSize);

        DataSet ds = myBs.DoReturnDataSet("PRO_GET_APUSER", myMsg.GetXmlStr, "");

        totalRecord = ds.Tables["TCounts"].Rows[0]["Counts"].ToString();

        return ds.Tables["ApUser"];


    }

    public string InsertQCFactDept(string user_id, string fact_no, string dept_no, string sec_no, string sec_name, string dept_name, string type_check, string add_id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id",user_id);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("sec_name", sec_name);
        myMsg.CreateFirstNode("dept_name", dept_name);
        myMsg.CreateFirstNode("type_Check", type_check);
        myMsg.CreateFirstNode("add_id", add_id);
        return myBs.DoReturnStr("PRO_INSERT_QCFACTDEPT", myMsg.GetXmlStr, "");
    }
    public string DeleteQCFactDept(string user_id, string fact_no, string dept_no, string sec_no)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id", user_id);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);

        return myBs.DoReturnStr("PRO_DELETE_QCFACTDEPT", myMsg.GetXmlStr, "");
    }

    public DataTable GetQCFactDept(string user_id, string fact_no)
    {
        return GetQCFactDept(user_id, fact_no, "", "");
    
    }
    public DataTable GetQCFactDept(string user_id, string fact_no, string dept_no, string sec_no)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("user_id", user_id);
        myMsg.CreateFirstNode("fact_no",fact_no);
        myMsg.CreateFirstNode("dept_no",dept_no);
        myMsg.CreateFirstNode("sec_no",sec_no);

        return myBs.DoReturnDataSet("PRO_GET_QCFACTDEPT", myMsg.GetXmlStr, "").Tables[0];
    }
    public DataTable GetDeptInQCFactDept(string fact_no)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("fact_no",fact_no);

        return myBs.DoReturnDataSet("PRO_GET_DEPTINQCFACTDEPT", myMsg.GetXmlStr, "").Tables[0];
    }

    public static string GetTypeName(string type_check, PccErrMsg myLabel)
    {

        string CD = myLabel.GetErrMsg("lbl0005", "QC/Tilte");
        string KT = myLabel.GetErrMsg("lbl0006", "QC/Tilte");


        string type_name = "";
        if (type_check == "0")
            type_name = CD + ", " + KT;
        else if (type_check == "1")
            type_name = CD;
        else if (type_check == "2")
            type_name = KT;

        return type_name;
    }
    public static string GetFactByUser(string user_id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("UserID", user_id);

        bs_UserInfo bs = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        return bs.DoReturnStr("GETFACTNOBYUSERID", myMsg.GetXmlStr, "").Trim();
    }
    #endregion

    #region ImgConditions
    public DataTable GetImgConditions()
    {
        return GetImgConditions("");
    }
    public DataTable GetImgConditions(string id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("id", id);

        return myBs.DoReturnDataSet("PRO_GET_IMGCONDITIONS", myMsg.GetXmlStr, "").Tables[0];
    }
     public string InsertImgConditions(string img_type, string condition, string count, string add_id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("img_type",img_type);
        myMsg.CreateFirstNode("condition",condition);
        myMsg.CreateFirstNode("count", count);
        myMsg.CreateFirstNode("add_id", add_id);

        return myBs.DoReturnStr("PRO_INSERT_IMGCONDITIONS", myMsg.GetXmlStr, "");
    }
    
    public string CancelImgConditions(string ID, string user_id)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ID",ID);
        myMsg.CreateFirstNode("add_id", user_id);

        return myBs.DoReturnStr("PRO_CANCEL_IMGCONDITIONS", myMsg.GetXmlStr, "");
    }
    public string UpdateImgConditions(string ID, string condition, string count, string add_id)
    {
        PccMsg myMsg = new PccMsg();

        myMsg.CreateFirstNode("ID",ID);
        myMsg.CreateFirstNode("condition",condition);
        myMsg.CreateFirstNode("count",count);
        myMsg.CreateFirstNode("add_id", add_id);

        return myBs.DoReturnStr("PRO_UPDATE_IMGCONDITIONS", myMsg.GetXmlStr, "");
    }
    public static string GetImgConditionsType(string type)
    {
        string dataPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/XmlDoc/Data.xml";

        PccMsg myMsg = new PccMsg();
        myMsg.Load(dataPath);
        return myMsg.Query("IMGConditions/" + type);
    }

    public ArrayList GetConditons(string type)
    {
        string[] conditions = {"<","=",">"};
        ArrayList arOutput = new ArrayList();
        DataTable dtData = GetImgConditions();

        foreach(string s in conditions)
        {
            DataRow[] drData = dtData.Select("img_type = '" + type + "' AND condition='"+s+"' ");
            if (drData.Length == 0)
                arOutput.Add(s);
        }

        return arOutput;
        
    }
    #endregion

}