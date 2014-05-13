using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PccCommonForC;
using System.Data;
using PccDbQCLogBlook;

namespace PccBsQCLogBlook
{
    public class bs_BasicData : BsBaseObject
    {
        private const string COMPONENTNAME = "PccBsQCLogBlook.bs_BasicData.";
        #region Contructor
        public bs_BasicData() : base()
		{

		}

		public bs_BasicData(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd)
		{

		}

        public bs_BasicData(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd, string UserIDAndName, string EventLogPath)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd, UserIDAndName, EventLogPath)
		{

        }
        #endregion

        #region Function
        public override string DoReturnStr(string strCommand, string strXML, string strOther)
        {
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            switch (PccToolFunc.Upper(strCommand))
            {
                case "PRO_INSERT_QCFACTDEPT":
                    strReturn = bs_pro_insert_QCFactDept(strXML);
                    break;
                case "PRO_DELETE_QCFACTDEPT":
                    strReturn = bs_pro_delete_QCFactDept(strXML);
                    break;
                case "PRO_INSERT_IMGCONDITIONS":
                    strReturn = bs_pro_insert_ImgConditions(strXML);
                    break;
                case "PRO_CANCEL_IMGCONDITIONS":
                    strReturn = bs_pro_cancel_ImgConditions(strXML);
                    break;
                case "PRO_UPDATE_IMGCONDITIONS":
                    strReturn = bs_pro_update_ImgConditions(strXML);
                    break;
                case "INSUPDDATA_QC_WEB":
                    strReturn = bs_ins_data_qc_web(strXML);
                    break;
                case "INSUPDDATA_QC_WEB_02OK":
                    strReturn = bs_insupd_data_qc_web_02OK(strXML);
                    break;
              
            }
            GenEventLog(COMPONENTNAME + "DoReturnStr." + strCommand, strXML, strOther, strStartTime, strReturn);
            return strReturn;
        }

      

        public override DataSet DoReturnDataSet(string strCommand, string strXML, string strOther)
        {
            DataSet dsReturn = null;
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            switch (PccToolFunc.Upper(strCommand))
            {
                case "GETFACT":
                    dsReturn = bs_get_FactUserId(strXML);
                    break;
                case "GETDEPT":
                    dsReturn = bs_get_dept(strXML);
                    break;
                case "GETSEC":
                    dsReturn = bs_get_sec(strXML);
                    break;
                case "GETBUILD":
                    dsReturn = bs_get_build(strXML);
                    break;
                case "GETFLOOR":
                    dsReturn = bs_get_floor(strXML);
                    break;
                case "PRO_GET_CONNECTION":
                    dsReturn = bs_pro_get_Connection(strXML);
                    break;
                case "PRO_GET_FACTQC":
                    dsReturn = bs_pro_get_FactQC();
                    break;
                case "PRO_GET_QCFACTDEPT":
                    dsReturn = bs_pro_get_QCFactDept(strXML);
                    break;
                case "PRO_GET_APUSER":
                    dsReturn = bs_pro_get_ApUser(strXML);
                    break;
                case "GETFACTODRNO":
                    dsReturn = bs_get_fact_odrno(strXML);
                    break;  
                case "GET_BAD_REASON":
                    dsReturn = bs_get_bad_reason(strXML);
                    break;
                case "GETSUMACT_QTY":
                    dsReturn = bs_get_sum_act_qty(strXML);
                    break;
                case "GETFLOORERP":
                    dsReturn = bs_get_floor_erp(strXML);
                    break;
                case "GETBUILDNOERP":
                    dsReturn = bs_get_build_erp(strXML);
                    break;
                case "TESTCOUNTBADNOERP":
                    dsReturn = bs_testCountBadNoErp(strXML);
                    break;
                case "GET_TIME_NO":
                    dsReturn = bs_get_time_no(strXML);
                    break; 
                    
                case "PRO_GET_IMGCONDITIONS":
                    dsReturn = bs_pro_get_ImgConditions(strXML);
                    break;
                case "PRO_GET_DEPTINQCFACTDEPT":
                    dsReturn = bs_pro_get_DeptInQCFactDept(strXML);
                    break;
                case "TESTCOUNTERPVOUNO":
                    dsReturn = bs_testCountErpVouno(strXML);
                    break;
                case "SUMQTY02":
                    dsReturn = bs_SumQtyQC02(strXML);
                    break;
                case "SUMQTY02BYBADNO":
                    dsReturn = bs_SumQtyQC02ByBadNo(strXML);
                    break;
                 case "SUMQTYERP":
                    dsReturn = bs_SumQtyERP(strXML);
                    break;
                 case "PRO_GET_LANGUAGES":
                    dsReturn = pro_get_Languages(strXML, strOther);
                    break;
                    
                    
            }
            if (dsReturn != null)
                strReturn = "DsFirstTableName---" + dsReturn.Tables[0].TableName;
            GenEventLog(COMPONENTNAME + "DoReturnDataSet." + strCommand, strXML, strOther, strStartTime, strReturn);
            return dsReturn;
        }

        #endregion


        #region Return Dataset
        private DataSet pro_get_Languages(string strXML, string strOther)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            ds = mydb.pro_get_Languages(strXML);

            return ds;
        }
        private DataSet bs_get_FactUserId(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet dsReturn =mydb.db_pro_get_FactUserId(strXML);         
            return dsReturn;
        }

        private DataSet bs_get_Fact(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_Fact(strXML);
        }
        private DataSet bs_get_dept(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_dept(strXML);
        }
        private DataSet bs_get_sec(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_sec(strXML);
        }
        private DataSet bs_get_build(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_build(strXML);
        }
        private DataSet bs_get_floor(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_floor(strXML);
        }
        private DataSet bs_pro_get_Connection(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_get_Connection(strXML);
        }
        private DataSet bs_pro_get_FactQC()
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_get_FactQC();
        }
        private DataSet bs_pro_get_QCFactDept(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_get_QCFactDept(strXML);
        }
        private DataSet bs_pro_get_ApUser(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_get_ApUser(strXML);
        }
        private DataSet bs_get_fact_odrno(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_fact_odrno(strXML);
        }
        private DataSet bs_get_bad_reason(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_bad_reason(strXML);
        }
        private DataSet bs_get_sum_act_qty(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_sum_act_qty(strXML);
        }
        private DataSet bs_get_floor_erp(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_floor_erp(strXML);
        }
        private DataSet bs_get_build_erp(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_build_no_erp(strXML);
        }
        private DataSet bs_testCountBadNoErp(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.testCountBadNoErp(strXML);
        }
        private DataSet bs_get_time_no(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.get_time_no(strXML);
        }
        
        private DataSet bs_pro_get_ImgConditions(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet dsReturn = mydb.db_pro_get_ImgConditions(strXML);         
            return dsReturn;
        }
        private DataSet bs_pro_get_DeptInQCFactDept(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet dsReturn = mydb.db_pro_get_DeptInQCFactDept(strXML);         
            return dsReturn;
        }
        private DataSet bs_testCountErpVouno(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.testCountErpVouNo(strXML);
        }
        private DataSet bs_SumQtyQC02(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.SumQtyQC02(strXML);
        }
        private DataSet bs_SumQtyQC02ByBadNo(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.SumQtyQC02ByBadNo(strXML);
        }
        private DataSet bs_SumQtyERP(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.SumQtyERP(strXML);
        }
        
        #endregion

        #region Return String
        private string bs_pro_insert_QCFactDept(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_insert_QCFactDept(strXML);
        }
        private string bs_pro_delete_QCFactDept(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_delete_QCFactDept(strXML);
        }
        private string bs_pro_insert_ImgConditions(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_insert_ImgConditions(strXML);
        }
        private string bs_pro_cancel_ImgConditions(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_cancel_ImgConditions(strXML);
        }
        private string bs_pro_update_ImgConditions(string strXML)
        {
            db_BasicData mydb = new db_BasicData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_pro_update_ImgConditions(strXML);
        }
        private string bs_ins_data_qc_web(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_insupd_data_qc_web(strXML);
        }
        private string bs_insupd_data_qc_web_02OK(string strXML)
        {
            db_DataORA mydb = new db_DataORA(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.db_insupd_data_qc_web_02OK(strXML);
        }
        
        
        #endregion

    }
}
