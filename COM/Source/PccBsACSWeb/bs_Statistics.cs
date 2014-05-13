using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;
using PccDbQCLogBlook;

namespace PccBsQCLogBlook
{
    public class bs_Statistics : PccCommonForC.BsBaseObject
    {
        public bs_Statistics() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

		public bs_Statistics(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd)
		{

		}

        public bs_Statistics(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd, string UserIDAndName, string EventLogPath)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd, UserIDAndName, EventLogPath)
		{

		}

        private const string COMPONENTNAME = "PccBsQCLogBlook.bs_Statistics.";

        #region "公用的Function"

        public override string DoReturnStr(string strCommand, string strXML, string strOther)
        {
            string strReturn = "";
            string strStartTime = DateTime.Now.ToString();
            switch (PccToolFunc.Upper(strCommand))
            {/*
                case "CHECKDELETEAREA":
                    strReturn = bs_Pro_Check_DeleteArea(strXML);
                    break;            
              */
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
                case "GETSTATISTICS":
                    dsReturn = bs_GetStatistics(strXML);
                    break;
                case "GETFACT":
                    dsReturn = bs_GetFactStatistics(strXML);
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

        private DataSet bs_GetStatistics(string strXML)
        {
            DataSet dsReturn;

            db_Statistics mydb = new db_Statistics(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            dsReturn = mydb.pro_GetLogBookStatistics(strXML);
            return dsReturn;
        }

        private DataSet bs_GetFactStatistics(string strXML)
        {
            DataSet dsReturn;

            db_Statistics mydb = new db_Statistics(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            dsReturn = mydb.pro_GetFactStatistics(strXML);
            return dsReturn;
        }
    }
}
