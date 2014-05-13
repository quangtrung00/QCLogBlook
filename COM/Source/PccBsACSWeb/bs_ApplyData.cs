using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;
using PccDbQCLogBlook;

namespace PccBsQCLogBlook
{
    public class  bs_ApplyData: PccCommonForC.BsBaseObject
    {
        public bs_ApplyData() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

		public bs_ApplyData(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd)
		{

		}

        public bs_ApplyData(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd, string UserIDAndName, string EventLogPath)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd, UserIDAndName, EventLogPath)
		{

		}

        private const string COMPONENTNAME = "PccBsQCLogBlook.bs_ApplyData.";
        #region "公用的Function"

        public override string DoReturnStr(string strCommand, string strXML, string strOther)
        {
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            switch (PccToolFunc.Upper(strCommand))
            {
                case "INSUPDAPPLYCHECK":
                    strReturn = bs_InsUpdApplyCheck(strXML);
                    break;
                case "INSUPDAPPLYCHECKDETAIL":
                    strReturn = bs_InsUpdApplyCheckDetail(strXML);
                    break;
                case "INSUPDAPPLYCHECKQ":
                    strReturn = bs_InsUpdApplyCheckQC(strXML);
                    break;
                case "INSUPDAPPLYRATING":
                    strReturn = bs_InsUpdApplyRating(strXML);
                    break;
                case "INSUPDAPPLYRATINGDETAIL":
                    strReturn = bs_InsUpdApplyRatingD(strXML);
                    break;
            }
            GenEventLog(COMPONENTNAME + "DoReturnStr." + strCommand, strXML, strOther, strStartTime, strReturn);
            return strReturn;
        }

        public override SqlDataReader DoReturnDataReader(string strCommand, string strXML, string strOther)
        {
            SqlDataReader drReturn = null;
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            if (drReturn != null)
                strReturn = "DrFirstFieldName---" + drReturn.GetName(0);
            GenEventLog(COMPONENTNAME + "DoReturnDataReader." + strCommand, strXML, strOther, strStartTime, strReturn);
            return drReturn;
        }

        public override DataSet DoReturnDataSet(string strCommand, string strXML, string strOther)
        {
            DataSet dsReturn = null;
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            switch (PccToolFunc.Upper(strCommand))
            {
                case "SUMBABNO":
                    dsReturn = bs_sum_bad_no(strXML);
                    break;
                case "GETIMGCONDITIONS":
                    dsReturn = bs_getImgConditions(strXML);
                    break;
                case "SUMQTYTOAL":
                    dsReturn = bs_sum_qty_total(strXML);
                    break;
                  case "GETUSERQC":
                    dsReturn = bs_get_UserQc(strXML);
                    break;
                   case "SUMQTYTOALQC02":
                    dsReturn = bs_sum_qty_total_Qc02(strXML);
                    break;
                         
            }
            if (dsReturn != null)
                strReturn = "DsFirstTableName---" + dsReturn.Tables[0].TableName;
            GenEventLog(COMPONENTNAME + "DoReturnDataSet." + strCommand, strXML, strOther, strStartTime, strReturn);
            return dsReturn;
        }

        public override bool DoReturnBool(string strCommand, string strXML, string strOther)
        {
            bool bReturn = false;
            string strStartTime = DateTime.Now.ToString();
            GenEventLog(COMPONENTNAME + "DoReturnBool." + strCommand, strXML, strOther, strStartTime, bReturn.ToString());
            return bReturn;
        }

        #endregion

        #region "ins upd function"
        private string bs_InsUpdApplyCheck(string strXML)
        {
            string strReturn;
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_InsUpdApplyCheck(strXML);
            return strReturn;
        }
        private string bs_InsUpdApplyCheckDetail(string strXML)
        {
            string strReturn;
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_InsUpdApplyCheckDetail(strXML);
            return strReturn;
        }
        private string bs_InsUpdApplyCheckQC(string strXML)
        {
            string strReturn;
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_InsUpdApplyCheckQC02(strXML);
            return strReturn;
        }
        private string bs_InsUpdApplyRating(string strXML)
        {
            string strReturn;
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_InsUpdApplyRating(strXML);
            return strReturn;
        }
        private string bs_InsUpdApplyRatingD(string strXML)
        {
            string strReturn;
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_InsUpdApplyRatingDetail(strXML);
            return strReturn;
        }
        #endregion


        #region GetFunction
        private DataSet bs_sum_bad_no(string strXml)
        {
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_sum_bad_no(strXml);
            return ds;
        }
        private DataSet bs_getImgConditions(string strXml)
        {
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_getImgConditions(strXml);
            return ds;
        }
        private DataSet bs_sum_qty_total(string strXml)
        {
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_sum_qty_total(strXml);
            return ds;
        }
        private DataSet bs_get_UserQc(string strXml)
        {
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_get_UserQc(strXml);
            return ds;
        }
        private DataSet bs_sum_qty_total_Qc02(string strXml)
        {
            db_ApplyData mydb = new db_ApplyData(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_sum_qty_total_Qc02(strXml);
            return ds;
        }
        
        #endregion


    }
}
