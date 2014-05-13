//建立日期：4/18/2007 11:13:32 PM
//建立者  ：
//建立目的：

using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;

namespace PccDbQCLogBlook
{
	/// <summary>
	/// db_SetTimeMeal 的摘要描述。
	/// </summary>
	public class db_UserInfo : PccCommonForC.DbBaseObject
	{
		public db_UserInfo() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

		public db_UserInfo(string ConnectionType,string ConnectionServer,string ConnectionDB,string ConnectionUser,string ConnectionPwd) : base(ConnectionType,ConnectionServer,ConnectionDB,ConnectionUser,ConnectionPwd)
		{

		}

		//Lấy toàn bộ mã xưởng 
		public DataSet db_GetFactory(string strXML)
		{
			PccMsg myMsg = new PccMsg(strXML);
			string myApID = myMsg.Query("ApID");

			string strSQL = " Select ltrim(rtrim(fact_id))fact_id,(ltrim(rtrim(fact_no))+'---'+fact_nm)fact_nm From  SHR_FACT Where ap_id='"+myApID+"' Order by fact_no";

			SqlDataAdapter objUserFact=new SqlDataAdapter(strSQL,m_Conn);
			DataSet dsReturn = new DataSet();
			objUserFact.Fill(dsReturn,"SHR_FACT"); 
			return dsReturn;
		}
		
		//Lấy toàn bộ mã bộ phận của 1 xưởng 
		public DataSet db_GetDeparment(string strXML)
		{
			PccMsg myMsg = new PccMsg(strXML);
			string myApID = myMsg.Query("ApID");
			string myFact_id = myMsg.Query("Fact_id");
			string strSQL = "  Select dept_id,(ltrim(rtrim(dept_no))+'---'+dept_nm)dept_nm From  SHR_DEPT Where  ltrim(rtrim(fact_id))='"+myFact_id+"'  Order by dept_no";

			SqlDataAdapter objUserFact=new SqlDataAdapter(strSQL,m_Conn);
			DataSet dsReturn = new DataSet();
			objUserFact.Fill(dsReturn,"SHR_DEPT"); 
			return dsReturn;
		}

		//Nickel 20130320
		public DataSet db_GET_SHR_AREA(string strXML)
		{
			PccMsg myMsg = new PccMsg(strXML);
			string myStrConFactNo = myMsg.Query("StrConFactNo");

			SqlParameter[] paraValue= {
										  new SqlParameter("@area_id", SqlDbType.Decimal,12),
										  new SqlParameter("@ap_id", SqlDbType.Decimal,12),
										  new SqlParameter("@area_no", SqlDbType.Char,4),
										  new SqlParameter("@area_name", SqlDbType.NVarChar,50),
										  new SqlParameter("@area_no_name",SqlDbType.VarChar,50)
									  };
			paraValue[0].Value = myMsg.Query("area_id");
			paraValue[1].Value = myMsg.Query("ap_id");
			paraValue[2].Value = myMsg.Query("area_no");
			paraValue[3].Value = myMsg.Query("area_name");
			paraValue[4].Value = myMsg.Query("area_no_name");
			return RunProcedure("pro_get_shr_area",paraValue,"pro_get_shr_area");
	
		}

        #region "Get_Contact_SysManage"
        public DataSet Get_Contact_SysManage(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {
											new SqlParameter("@area_no",SqlDbType.NChar,10)
											,new SqlParameter("@ap_id",SqlDbType.Decimal,12)
										};

            parameters[0].Value = myMsg.Query("area_no");
            parameters[1].Value = decimal.Parse(myMsg.Query("ap_id"));
            dsReturn = RunProcedure("pro_Get_Contact_SysManage", parameters, "Contact");
            return dsReturn;
        }
        #endregion

		//Nickel 20130320
		public string db_DEL_SHR_AREA(string strXML)
		{
			PccMsg myMsg = new PccMsg(strXML);
			string myStrConFactNo = myMsg.Query("area_id");

			SqlParameter[] paraValue= {
										  new SqlParameter("@area_id", SqlDbType.Decimal,12),
										  new SqlParameter("@ErrMsg",SqlDbType.Int)					 
									  };
			int rowsAffected = 0,returnValue;
			paraValue[0].Value = myMsg.Query("area_id");
			paraValue[1].Direction = ParameterDirection.Output;

			returnValue = RunProcedure("pro_Del_shr_area", paraValue, ref rowsAffected);
			myMsg = new PccMsg();

			myMsg.CreateFirstNode("returnValue",returnValue.ToString());
			myMsg.CreateFirstNode("rowsAffected",rowsAffected.ToString());
			myMsg.CreateFirstNode("MsgReturn",paraValue[1].Value.ToString());
			return myMsg.GetXmlStr;
	
	
		}

		//Nickel 20130320
		public string db_Ins_Upd_Shr_Area(string strXML)
		{
			PccMsg myMsg = new PccMsg(strXML);
			string myStrConFactNo = myMsg.Query("area_id");

			SqlParameter[] paraValue= {
										  new SqlParameter("@area_id", SqlDbType.Decimal,12),
										  new SqlParameter("@ap_id", SqlDbType.Decimal,12),
										  new SqlParameter("@area_no", SqlDbType.Char,4),
										  new SqlParameter("@area_name", SqlDbType.NVarChar,50),
										  new SqlParameter("@upd_id", SqlDbType.Decimal,12),
										  new SqlParameter("@ErrMsg",SqlDbType.Int)					 
									  };
			int rowsAffected = 0,returnValue;
			paraValue[0].Value = myMsg.Query("area_id");
			paraValue[1].Value = myMsg.Query("ap_id");
			paraValue[2].Value = myMsg.Query("area_no");
			paraValue[3].Value = myMsg.Query("area_name");
			paraValue[4].Value = myMsg.Query("upd_id");
			paraValue[5].Direction = ParameterDirection.Output;

			returnValue = RunProcedure("Pro_Ins_Upd_Shr_Area", paraValue, ref rowsAffected);
			myMsg = new PccMsg();

			myMsg.CreateFirstNode("returnValue",returnValue.ToString());
			myMsg.CreateFirstNode("rowsAffected",rowsAffected.ToString());
			myMsg.CreateFirstNode("MsgReturn",paraValue[5].Value.ToString());
			return myMsg.GetXmlStr;
	
	
		}


        #region "Pro_Check_DeleteArea"
        public string db_Pro_Check_DeleteArea(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {   new SqlParameter("@area_id", SqlDbType.Decimal, 12),
											new SqlParameter("@delete_mk", SqlDbType.Char, 1)};

            parameters[0].Value = decimal.Parse(myMsg.Query("Area_ID"));
            parameters[1].Direction = ParameterDirection.Output;

            int rowsAffected = 0, returnValue;
            returnValue = RunProcedure("Pro_Check_DeleteArea", parameters, ref rowsAffected);

            PccMsg myTempMsg = new PccMsg();
            myTempMsg.CreateFirstNode("delete_mk", parameters[1].Value.ToString());
            return myTempMsg.GetXmlStr;
        }
        #endregion
        #region get fact,dept mimi
        public DataSet pro_Get_QCCoDinh(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);

           // string suserQC_id = myMsg.Query("user_id");
            string sFactNo = myMsg.Query("fact_no");
            string sdept_no = myMsg.Query("dept_no");
            string ssec_no = myMsg.Query("sec_no");




            SqlParameter[] paraValue ={										 
										 
										  new SqlParameter("@fact_no", SqlDbType.Char,4),
										  new SqlParameter("@dept_no", SqlDbType.Char,10),
										  new SqlParameter("@sec_no",SqlDbType.Char,10),
                                         // new SqlParameter("@user_id", SqlDbType.Decimal,18),
										  

									  };

            paraValue[0].Value = sFactNo;
            paraValue[1].Value = sdept_no;
            paraValue[2].Value = ssec_no;
            //if (suserQC_id != null & suserQC_id != "")
            //{
            //    paraValue[3].Value = decimal.Parse(suserQC_id);
            //}



            dsReturn = RunProcedure("pro_Get_QCCoDinh", paraValue, "pro_Get_QCCoDinh");


            return dsReturn;
        }

        public DataSet GetUserNameSign(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string struser_id = myMsg.Query("user_id");

            SqlParameter[] parameters ={										 
										  new SqlParameter("@user_id",SqlDbType.Decimal)
										  
									  };

            parameters[0].Value = Convert.ToDecimal(struser_id);


            dsReturn = RunProcedure("pro_Get_UserNameSign", parameters, "UserNameSign");


            return dsReturn;
        }
        //
        public DataSet db_sum_Qty(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);

            string sFactNo = myMsg.Query("fact_no");
            string sdept_no = myMsg.Query("dept_no");
            string ssec_no = myMsg.Query("sec_no");
            string suserQC_id = myMsg.Query("userQC_id");
            string sStartDate = myMsg.Query("start_date");
            string sEndDate = myMsg.Query("end_date");

            SqlParameter[] paraValue = {
											new SqlParameter("@fact_no", SqlDbType.Char,4),
										  new SqlParameter("@dept_no", SqlDbType.Char,10),
										  new SqlParameter("@sec_no",SqlDbType.Char,10),
                                          new SqlParameter("@userQC_id", SqlDbType.Decimal,18),
										  new SqlParameter("@start_date", SqlDbType.Char,14),
										  new SqlParameter("@end_date",SqlDbType.Char,14)



										};

            paraValue[0].Value = sFactNo;
            paraValue[1].Value = sdept_no;
            paraValue[2].Value = ssec_no;
            if (suserQC_id != null & suserQC_id!="")
            {
                paraValue[3].Value = decimal.Parse(suserQC_id);
            }
            paraValue[4].Value = sStartDate;
            paraValue[5].Value = sEndDate;


            dsReturn = RunProcedure("pro_GetSum_Qty_total", paraValue, "Sum_Qty_total");
            return dsReturn;
        }

        public DataSet GetSecByDept(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strfact_no = myMsg.Query("fact_no");
            string strdept_no = myMsg.Query("dept_no");
            SqlParameter[] parameters ={										 
										  new SqlParameter("@fact_no",SqlDbType.Decimal),
                                          new SqlParameter("@dept_no",SqlDbType.Decimal)
										  
									  };

            parameters[0].Value = Convert.ToDecimal(strfact_no);
            parameters[1].Value = Convert.ToDecimal(strdept_no);

            dsReturn = RunProcedure("pro_Get_SecByDeptNo", parameters, "MR_FactNo");


            return dsReturn;
        }

        public DataSet GetDeptByFact(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strfact_no = myMsg.Query("fact_no");

            SqlParameter[] parameters ={										 
										  new SqlParameter("@fact_no",SqlDbType.Decimal)
										  
									  };

            parameters[0].Value = Convert.ToDecimal(strfact_no);


            dsReturn = RunProcedure("pro_Get_DeptByFactNo", parameters, "MR_FactNo");


            return dsReturn;
        }
        public string GetDeptByFact_str(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strfact_no = myMsg.Query("fact_no");

            SqlParameter[] parameters ={										 
										  new SqlParameter("@fact_no",SqlDbType.Char,10),
										    new SqlParameter("@dept_no",SqlDbType.Char,10)
									  };

                parameters[0].Value = strfact_no;
              parameters[1].Direction = ParameterDirection.Output;

             dsReturn  = RunProcedure("pro_Get_DeptByFactNo", parameters, "MR_FactNo");
            string dept_no = parameters[1].Value.ToString();

            return dept_no;
        }
        #endregion
        #region "get fact,dept by user_id -- web.huuminh/20140416"
        public string GetFactNoByUserID(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strUserID = myMsg.Query("UserID");

            SqlParameter[] parameters ={										 
										  new SqlParameter("@UserID",SqlDbType.Decimal),
										  new SqlParameter("@FactNo",SqlDbType.Char,10)
									  };

            parameters[0].Value = Convert.ToDecimal(strUserID);
            parameters[1].Direction = ParameterDirection.Output;

            dsReturn = RunProcedure("pro_Get_FactByUserID", parameters, "MR_FactNo");

            string sFactNo = parameters[1].Value.ToString();
            return sFactNo;
        }
        public DataSet GetDeptByUserID(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strUserID = myMsg.Query("UserID");

            SqlParameter[] parameters ={										 
										  new SqlParameter("@UserID",SqlDbType.Decimal)
										  
									  };

            parameters[0].Value = Convert.ToDecimal(strUserID);


            dsReturn = RunProcedure("pro_Get_DeptByUserID", parameters, "MR_FactNo");


            return dsReturn;
        }
        public DataSet GetSecByUserID(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);
            string strUserID = myMsg.Query("UserID");
            string dept_no = myMsg.Query("dept_no");
            string type_Check = myMsg.Query("type_Check");
            SqlParameter[] parameters ={										 
										  new SqlParameter("@UserID",SqlDbType.Decimal),
                                          new SqlParameter("@dept_no",SqlDbType.Char,10),
                                          new SqlParameter("@type_Check",SqlDbType.Char,1)
										  
									  };

            parameters[0].Value = Convert.ToDecimal(strUserID);
            parameters[1].Value = dept_no;
            parameters[2].Value = type_Check;
            dsReturn = RunProcedure("pro_Get_SecByUserID", parameters, "SecNo");


            return dsReturn;
        }
        #endregion

       
	}
}
