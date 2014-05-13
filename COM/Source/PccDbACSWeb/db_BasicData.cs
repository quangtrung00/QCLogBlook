using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;

namespace PccBsQCLogBlook
{
    public class db_BasicData : DbBaseObject
    {
        #region Contructor
        public db_BasicData() : base()
		{
		
		}

        public db_BasicData(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd)
		{

		}
        #endregion


        #region Return DataSet
        #region Languages
        public DataSet pro_get_Languages(string strXML)
        {
            DataSet dsReturn = new DataSet();
            SqlParameter[] parameters = {
										};

            dsReturn = RunProcedure("pro_get_Languages", parameters, "Languages");
            return dsReturn;
        }
        #endregion
        public DataSet db_pro_get_Connection(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@fact_no",SqlDbType.VarChar, 4)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("fact_no");

            return RunProcedure("pro_get_Connection", para, "Connection");

        }
        public DataSet db_pro_get_FactUserId(string strXML)
        {
            DataSet dsReturn;
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] paraValue = { 
                                        new SqlParameter("@UserID",SqlDbType.Decimal,18),
                                        new SqlParameter("@FactNo",SqlDbType.Char,10)
                                       };
           
            paraValue[0].Value = myMsg.Query("user_id");
            paraValue[1].Direction = ParameterDirection.Output;
            dsReturn = RunProcedure("pro_Get_FactByUserID", paraValue, "shr_userd");
            return dsReturn;
        }

        public DataSet db_pro_get_FactQC()
        {
            SqlParameter[] para = {};
            return RunProcedure("pro_get_FactQC", para, "Connection");
        }

        public DataSet db_pro_get_QCFactDept(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@user_id",SqlDbType.VarChar,18)
	                                ,new SqlParameter("@fact_no",SqlDbType.VarChar,4)
	                                ,new SqlParameter("@dept_no",SqlDbType.VarChar,10)
	                                ,new SqlParameter("@sec_no",SqlDbType.VarChar,10)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("user_id");
            para[1].Value = myMsg.Query("fact_no");
            para[2].Value = myMsg.Query("dept_no");
            para[3].Value = myMsg.Query("sec_no");

            return RunProcedure("pro_get_QCFactDept", para, "QCFactDept");
        }
        

        public DataSet db_pro_get_ApUser(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@ap_id",SqlDbType.VarChar,18)  
	                                ,new SqlParameter("@user_desc",SqlDbType.NVarChar,100)
	                                ,new SqlParameter("@email",SqlDbType.NVarChar, 50)
                                    ,new SqlParameter("@fact_no",SqlDbType.VarChar,4)
                                    ,new SqlParameter("@user_id",SqlDbType.VarChar,18)
                                    ,new SqlParameter("@Counts",SqlDbType.Int)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("ap_id").Trim();
            para[1].Value = myMsg.Query("user_desc").Trim();
            para[2].Value = myMsg.Query("email").Trim();
            para[3].Value = myMsg.Query("fact_no").Trim();
            para[4].Value = myMsg.Query("user_id").Trim();
            para[5].Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            if (myMsg.Query("startRecord") != "")
            {
                int startRecord = int.Parse(myMsg.Query("startRecord"));
                int pageSize = int.Parse(myMsg.Query("pageSize"));

                ds = RunProcedure("pro_get_ApUser", para, "ApUser", startRecord, pageSize);
                
                DataTable tblCounts = new DataTable("TCounts");
                DataColumn dc = tblCounts.Columns.Add("Counts", System.Type.GetType("System.String"));
                DataRow dr = tblCounts.NewRow();
                dr["Counts"] = para[5].Value;
                tblCounts.Rows.Add(dr);
                ds.Tables.Add(tblCounts);
            }
            else
            {
                ds = RunProcedure("pro_get_ApUser", para, "ApUser");
            }

            return ds;
        }

        public DataSet db_pro_get_ImgConditions(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@id",SqlDbType.VarChar, 18)

								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("id").Trim();

            return RunProcedure("pro_get_ImgConditions", para, "ApUser");
        }

        public DataSet db_pro_get_DeptInQCFactDept(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@fact_no",SqlDbType.VarChar, 4)

								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("fact_no").Trim();

            return RunProcedure("pro_get_DeptInQCFactDept", para, "ApUser");
        }
        
        #endregion

        #region Return String
        public string db_pro_insert_QCFactDept(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@user_id",SqlDbType.Decimal, 18)
                                    ,new SqlParameter("@fact_no",SqlDbType.Char,4)
                                    ,new SqlParameter("@dept_no",SqlDbType.Char,10)
                                    ,new SqlParameter("@sec_no",SqlDbType.Char, 10)
                                    ,new SqlParameter("@sec_name",SqlDbType.NVarChar, 50)
                                    ,new SqlParameter("@dept_name",SqlDbType.NVarChar, 50)
                                    ,new SqlParameter("@type_Check",SqlDbType.Char, 1)
                                    ,new SqlParameter("@add_id",SqlDbType.Decimal, 18)
                                    ,new SqlParameter("@add_date",SqlDbType.Char, 8) 
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = decimal.Parse(myMsg.Query("user_id"));
            para[1].Value = myMsg.Query("fact_no");
            para[2].Value = myMsg.Query("dept_no");
            para[3].Value = myMsg.Query("sec_no");
            para[4].Value = myMsg.Query("sec_name");
            para[5].Value = myMsg.Query("dept_name");
            para[6].Value = myMsg.Query("type_Check");
            para[7].Value = decimal.Parse(myMsg.Query("add_id"));
            para[8].Value = DateTime.Now.ToString("yyyyMMdd");


            int rowsAffected = 0;
            string errMsg = "";
            try
            {
                RunProcedure("pro_insert_QCFactDept", para, ref rowsAffected);
            }
            catch (Exception ex) { errMsg = ex.Message; }

            return errMsg;
        }

        public string db_pro_delete_QCFactDept(string strXML)
        {
            SqlParameter[] para = {
	   								new SqlParameter("@user_id", SqlDbType.Decimal,18)
                                    ,new SqlParameter("@fact_no",SqlDbType.Char,4)
                                    ,new SqlParameter("@dept_no",SqlDbType.Char, 10)
                                    ,new SqlParameter("@sec_no",SqlDbType.Char, 10)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = decimal.Parse(myMsg.Query("user_id"));
            para[1].Value = myMsg.Query("fact_no");
            para[2].Value = myMsg.Query("dept_no");
            para[3].Value = myMsg.Query("sec_no");

            int rowsAffected = 0;
            string errMsg = "";
            try
            {
                RunProcedure("pro_delete_QCFactDept", para, ref rowsAffected);
            }
            catch (Exception ex) { errMsg = ex.Message; }

            return errMsg;
        }

        public string db_pro_insert_ImgConditions(string strXML)
        {
            SqlParameter[] para = {
									new SqlParameter("@img_type",SqlDbType.Char, 1)
                                    ,new SqlParameter("@condition",SqlDbType.Char,1)
                                    ,new SqlParameter("@count",SqlDbType.Int)
                                    ,new SqlParameter("@add_id",SqlDbType.Decimal,18)
                                    ,new SqlParameter("@add_date",SqlDbType.Char, 8)
                                    ,new SqlParameter("@close_date",SqlDbType.Char, 8)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = myMsg.Query("img_type").Trim();
            para[1].Value = myMsg.Query("condition").Trim();
            para[2].Value = int.Parse(myMsg.Query("count"));
            para[3].Value = decimal.Parse(myMsg.Query("add_id"));
            para[4].Value = DateTime.Now.ToString("yyyyMMdd");
            para[5].Value = "99999999";


            int rowsAffected = 0;
            string errMsg = "";
            try
            {
                RunProcedure("pro_insert_ImgConditions", para, ref rowsAffected);
            }
            catch (Exception ex) { errMsg = ex.Message; }

            return errMsg;
        }

        public string db_pro_cancel_ImgConditions(string strXML)
        {
            SqlParameter[] para = {
									 new SqlParameter("@ID",SqlDbType.Decimal,18)
                                     ,new SqlParameter("@add_id",SqlDbType.Decimal, 18)
	                                ,new SqlParameter("@close_date",SqlDbType.VarChar,8)
								   };
            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = decimal.Parse(myMsg.Query("ID"));
            para[1].Value = decimal.Parse(myMsg.Query("add_id"));
            para[2].Value = DateTime.Now.ToString("yyyyMMdd");
            
            int rowsAffected = 0;
            string errMsg = "";
            try
            {
                RunProcedure("pro_cancel_ImgConditions", para, ref rowsAffected);
            }
            catch (Exception ex) { errMsg = ex.Message; }

            return errMsg;
        }

        public string db_pro_update_ImgConditions(string strXML)
        {
            SqlParameter[] para = {
									 new SqlParameter("@ID",SqlDbType.Decimal, 18)
	                                ,new SqlParameter("@condition",SqlDbType.Char, 1)
	                                ,new SqlParameter("@count",SqlDbType.Int)
	                                ,new SqlParameter("@add_id",SqlDbType.Decimal,18)
	                                ,new SqlParameter("@add_date",SqlDbType.Char,8)
								   };

            PccMsg myMsg = new PccMsg(strXML);
            para[0].Value = decimal.Parse(myMsg.Query("ID"));
            para[1].Value = myMsg.Query("condition").Trim();
            para[2].Value = int.Parse(myMsg.Query("count"));
            para[3].Value = decimal.Parse(myMsg.Query("add_id"));
            para[4].Value = DateTime.Now.ToString("yyyyMMdd");
            
            int rowsAffected = 0;
            string errMsg = "";
            try
            {
                RunProcedure("pro_update_ImgConditions", para, ref rowsAffected);
            }
            catch (Exception ex) { errMsg = ex.Message; }

            return errMsg;
        }

        #endregion



    }
}
