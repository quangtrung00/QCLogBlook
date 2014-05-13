//建立日期：4/18/2007 11:13:32 PM
//建立者  ：
//建立目的：

using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;
using PccDbQCLogBlook;

namespace PccBsQCLogBlook
{
	/// <summary>
	/// bs_SetTimeMeal 的摘要描述。
	/// </summary>
	public class bs_UserInfo : PccCommonForC.BsBaseObject
	{
		public bs_UserInfo() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

		public bs_UserInfo(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd)
		{

		}

		public bs_UserInfo(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd,string UserIDAndName,string EventLogPath) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd,UserIDAndName,EventLogPath)
		{

		}

        private const string COMPONENTNAME = "PccBsQCLogBlook.bs_UserInfo.";

		#region "公用的Function"

		public override string DoReturnStr(string strCommand,string strXML,string strOther)
		{
			string strReturn = "";
			string strStartTime = DateTime.Now.ToString();
			switch (PccToolFunc.Upper(strCommand))
			{
                case "CHECKDELETEAREA":
                    strReturn = bs_Pro_Check_DeleteArea(strXML);
                    break;
				case "DEL_SHR_AREA":
					strReturn = bs_DEL_SHR_AREA(strXML);
					break;
				case "INS_UPD_SHR_AREA":
					strReturn = bs_Ins_Upd_Shr_Area(strXML);
					break;
                case "GETFACTNOBYUSERID":
                    strReturn = GetFactNoByUserID(strXML);
                    break;
                case "GETDEPTBYFACTSTR":
                    strReturn = GetDeptByFact_str(strXML);
                    break;
			}
			GenEventLog(COMPONENTNAME + "DoReturnStr." + strCommand,strXML,strOther,strStartTime,strReturn);
			return strReturn;
		}

		public override SqlDataReader DoReturnDataReader(string strCommand,string strXML,string strOther)
		{
			SqlDataReader drReturn = null;
			string strReturn = "";
			string strStartTime = DateTime.Now.ToString();
			if (drReturn != null)
				strReturn = "DrFirstFieldName---" + drReturn.GetName(0);
			GenEventLog(COMPONENTNAME + "DoReturnDataReader." + strCommand,strXML,strOther,strStartTime,strReturn);
			return drReturn;
		}

		public override DataSet DoReturnDataSet(string strCommand,string strXML,string strOther)
		{
			DataSet dsReturn = null;
			string strReturn = "";
			string strStartTime = DateTime.Now.ToString();
			switch (PccToolFunc.Upper(strCommand))
			{
				case "GETFACTORY":
					dsReturn = bs_GetFactory(strXML);
					break;
				case "GETDEPARMENT":
					dsReturn = bs_GetDeparment(strXML);
					break;
				case "GET_SHR_AREA":
					dsReturn = bs_GET_SHR_AREA(strXML);
					break;
                case "GET_CONTACT_SYSMANAGE": //add by Nickel 20121117
                    dsReturn = Get_Contact_SysManage(strXML, strOther);
                    break;
                case "GETDEPTBYUSERID":
                    dsReturn = GetDeptByUserID(strXML, strOther);
                    break;
                case "GETSECBYUSERID":
                    dsReturn = GetSecByUserID(strXML, strOther);
                    break;
                    //mimi
                case "GETDEPTBYFACT":
                    dsReturn = GetDeptByFact(strXML, strOther);
                    break;
                case "GETSECBYDEPT":
                    dsReturn = GetSecByDept(strXML, strOther);
                    break;
                case "GETUSERNAMESIGN":
                    dsReturn = bs_GetUserNameSign(strXML);
                    break;
                case "SUMQTY":
                    dsReturn = bs_sum_Qty(strXML);
                    break;

                case "GETQCCODINH":
                    dsReturn = bs_Get_QCCoDinh(strXML);
                    break;
			}
			if (dsReturn != null)
				strReturn = "DsFirstTableName---" + dsReturn.Tables[0].TableName;
			GenEventLog(COMPONENTNAME + "DoReturnDataSet." + strCommand,strXML,strOther,strStartTime,strReturn);
			return dsReturn;
		}

		public override bool DoReturnBool(string strCommand,string strXML,string strOther)
		{
			bool bReturn = false;
			string strStartTime = DateTime.Now.ToString();
			GenEventLog(COMPONENTNAME + "DoReturnBool." + strCommand,strXML,strOther,strStartTime,bReturn.ToString());
			return bReturn;
		}

		#endregion

        #region get Fact,Dept mimi
        private string GetDeptByFact_str(string strXML)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.GetDeptByFact_str(strXML);
        }
        private DataSet bs_Get_QCCoDinh(string strXml)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.pro_Get_QCCoDinh(strXml);
            return ds;
        }

        private DataSet bs_GetUserNameSign(string strXml)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.GetUserNameSign(strXml);
            return ds;
        }

        private DataSet bs_sum_Qty(string strXml)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.db_sum_Qty(strXml);
            return ds;
        }

        private DataSet GetDeptByFact(string strXml, string strOther)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.GetDeptByFact(strXml);
            return ds;
        }
        private DataSet GetSecByDept(string strXml, string strOther)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.GetSecByDept(strXml);
            return ds;
        }
        #endregion

        #region GetFunction
        private DataSet Get_Contact_SysManage(string strXml, string strOther)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.Get_Contact_SysManage(strXml);
            return ds;
        }
        private DataSet GetDeptByUserID(string strXml, string strOther)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.GetDeptByUserID(strXml);
            return ds;
        }
        private DataSet GetSecByUserID(string strXml, string strOther)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            DataSet ds = mydb.GetSecByUserID(strXml);
            return ds;
        }
        
        #endregion


        private DataSet bs_GetFactory(string strXML)
		{
			DataSet dsReturn;
						
			db_UserInfo mydb = new db_UserInfo(m_connectionType,m_connectionServer,m_connectionDB,m_connectionUser,m_connectionPwd);
			dsReturn = mydb.db_GetFactory(strXML);
			return dsReturn;
		}
		private DataSet bs_GetDeparment(string strXML)
		{
			DataSet dsReturn;
			db_UserInfo mydb = new db_UserInfo(m_connectionType,m_connectionServer,m_connectionDB,m_connectionUser,m_connectionPwd);
			dsReturn = mydb.db_GetDeparment(strXML);
			return dsReturn;
		}
		private DataSet bs_GET_SHR_AREA(string strXML)
		{
			DataSet dsReturn;
			db_UserInfo mydb = new db_UserInfo(m_connectionType,m_connectionServer,m_connectionDB,m_connectionUser,m_connectionPwd);
			dsReturn = mydb.db_GET_SHR_AREA(strXML);
			return dsReturn;
		}
        private string GetFactNoByUserID(string strXML)
        {
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            return mydb.GetFactNoByUserID(strXML);
        }

        private string bs_Pro_Check_DeleteArea(string strXML)
        {
            string strReturn;
            db_UserInfo mydb = new db_UserInfo(m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd);
            strReturn = mydb.db_Pro_Check_DeleteArea(strXML);
            return strReturn;
        }


		private string bs_DEL_SHR_AREA(string strXML)
		{
			string strReturn;
			db_UserInfo mydb = new db_UserInfo(m_connectionType,m_connectionServer,m_connectionDB,m_connectionUser,m_connectionPwd);
			strReturn = mydb.db_DEL_SHR_AREA(strXML);
			return strReturn;
		}
		private string bs_Ins_Upd_Shr_Area(string strXML)
		{
			string strReturn;
			db_UserInfo mydb = new db_UserInfo(m_connectionType,m_connectionServer,m_connectionDB,m_connectionUser,m_connectionPwd);
			strReturn = mydb.db_Ins_Upd_Shr_Area(strXML);
			return strReturn;
		}
	}
}
