using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;
namespace PccDbQCLogBlook
{
    public class db_ApplyData : PccCommonForC.DbBaseObject
    {
        public db_ApplyData() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

        public db_ApplyData(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd)
		{
		}

		//Lấy sum bad_no web.huuminh
        public DataSet db_sum_bad_no(string strXML)
		{
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {
											new SqlParameter("@type_check",SqlDbType.Char,1),
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
                                             new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),
                                            new SqlParameter("@add_date",SqlDbType.Char,8),
										};

            parameters[0].Value = myMsg.Query("type_check");
            parameters[1].Value = myMsg.Query("vou_no");
            parameters[2].Value = myMsg.Query("fact_no");
            parameters[3].Value = myMsg.Query("dept_no");
            parameters[4].Value = myMsg.Query("sec_no");
            parameters[5].Value = myMsg.Query("add_date");
            dsReturn = RunProcedure("pro_sum_bad_no", parameters, "sum_bad_no");
            return dsReturn;
		}

        //web.huuminh lay dieu kien hinh anh
        public DataSet db_getImgConditions(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {
											
										};

            dsReturn = RunProcedure("pro_GetImgConditions", parameters, "etImgConditions");
            return dsReturn;
        }
        //web.huuminh lay sum qty,total
        public DataSet db_sum_qty_total(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {
											new SqlParameter("@type_check",SqlDbType.Char,1),
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
                                            new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),
                                            new SqlParameter("@bad_no",SqlDbType.Char,10),
                                            new SqlParameter("@add_date",SqlDbType.Char,8),
                                            new SqlParameter("@Type",SqlDbType.Char,1),
										};
            parameters[0].Value = myMsg.Query("type_check");
            parameters[1].Value = myMsg.Query("vou_no");
            parameters[2].Value = myMsg.Query("fact_no");
            parameters[3].Value = myMsg.Query("dept_no");
            parameters[4].Value = myMsg.Query("sec_no");
            parameters[5].Value = myMsg.Query("bad_no");
            parameters[6].Value = myMsg.Query("add_date");
            parameters[7].Value = myMsg.Query("Type");
            dsReturn = RunProcedure("pro_sum_qty_total", parameters, "sum_qty_total");
            return dsReturn;
        }
        #region "Apply Data HuuMinh"
        public string db_InsUpdApplyCheck(string strXML)
        {
         
            PccMsg myMsg = new PccMsg(strXML);
            string ac_id = myMsg.Query("ac_id").ToString().Trim();
            string fact_no = myMsg.Query("fact_no").ToString().Trim();
            string dept_no = myMsg.Query("dept_no").ToString().Trim();
            string sec_no = myMsg.Query("sec_no").ToString().Trim();
            string type_check = myMsg.Query("type_check").ToString().Trim();
            string vou_no = myMsg.Query("vou_no").ToString().Trim();
            string total = myMsg.Query("total").ToString().Trim();
            string floor = myMsg.Query("floor").ToString().Trim();
            string build_no = myMsg.Query("build_no").ToString().Trim();
            string myType = myMsg.Query("Type").ToString().Trim();

            if (ac_id == "")
                ac_id = "0";
            if (total == "")
                total = "0";

            SqlParameter[] parameters = { 
                                            new SqlParameter("@ac_id",SqlDbType.Int),  
                                            new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),
                                            new SqlParameter("@type_check",SqlDbType.Char,1),
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
											new SqlParameter("@total",SqlDbType.Int), 
                                            new SqlParameter("@floor",SqlDbType.Char,1),
                                            new SqlParameter("@build_no",SqlDbType.Char,2),                                       
                                             new SqlParameter("@Type",SqlDbType.Char,1),
											new SqlParameter("@Err",SqlDbType.NVarChar,100)
										};

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = fact_no;
            parameters[2].Value = dept_no;
            parameters[3].Value = sec_no;
            parameters[4].Value = type_check;
            parameters[5].Value = vou_no;
            parameters[6].Value = total;
            parameters[7].Value = floor;
            parameters[8].Value = build_no;
            parameters[9].Value = myType;
            parameters[10].Direction = ParameterDirection.Output;

            try
            {
                int iReturn = 0, rowsAffected = 0;
                iReturn = RunProcedure("Pro_InsUpdApplyCheck", parameters, ref rowsAffected);

                myMsg.ClearContext();

                myMsg.CreateFirstNode("ReturnValue", iReturn.ToString());
                myMsg.CreateFirstNode("RowsAffected", rowsAffected.ToString());
                myMsg.CreateFirstNode("Err", parameters[10].Value.ToString());
                myMsg.CreateFirstNode("ac_id", parameters[0].Value.ToString());
            }
            catch (Exception e)
            {
                myMsg.ClearContext();
                myMsg.CreateFirstNode("Err", e.Message.ToString());
            }

            return myMsg.GetXmlStr;
        }
        #endregion

        #region "Apply Check Detail HuuMinh"
        public string db_InsUpdApplyCheckDetail(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string appd_id = myMsg.Query("appd_id").ToString().Trim();
            string ac_id = myMsg.Query("ac_id").ToString().Trim();
            string bad_no = myMsg.Query("bad_no").ToString().Trim();
            string bad_desc = myMsg.Query("bad_desc").ToString().Trim();
            string bad_descVN = myMsg.Query("bad_descVN").ToString().Trim();
            string userQC_id = myMsg.Query("userQC_id").ToString().Trim();
            string qty = myMsg.Query("qty").ToString().Trim();
            string qtyOKitem = myMsg.Query("qtyOKitem").ToString().Trim();
            string myType = myMsg.Query("Type").ToString().Trim();

            if (appd_id == "")
                appd_id = "0";
            if (ac_id == "")
                ac_id = "0";
            if (qty == "")
                qty = "0";
            if (qtyOKitem == "")
                qtyOKitem = "0";
            SqlParameter[] parameters = { 
                                            new SqlParameter("@appd_id",SqlDbType.Decimal,18),  
                                            new SqlParameter("@ac_id",SqlDbType.Int),  
                                            new SqlParameter("@bad_no",SqlDbType.Char,10),
                                            new SqlParameter("@bad_desc",SqlDbType.NVarChar,100),
                                            new SqlParameter("@bad_descVN",SqlDbType.NVarChar,100),
                                            new SqlParameter("@userQC_id",SqlDbType.Decimal,18), 
                                            new SqlParameter("@qty",SqlDbType.Int),
											new SqlParameter("@qtyOKitem",SqlDbType.Int),                                                                                
                                             new SqlParameter("@Type",SqlDbType.Char,1),
											new SqlParameter("@Err",SqlDbType.NVarChar,100)
										};
            parameters[0].Value = appd_id;
            parameters[1].Value = ac_id;
            parameters[2].Value = bad_no;
            parameters[3].Value = bad_desc;
            parameters[4].Value = bad_descVN;
            parameters[5].Value = userQC_id;
            parameters[6].Value = qty;
            parameters[7].Value = qtyOKitem;
            parameters[8].Value = myType;
            parameters[9].Direction = ParameterDirection.Output;

            try
            {
                int iReturn = 0, rowsAffected = 0;
                iReturn = RunProcedure("Pro_InsUpdApplyCheckDetail", parameters, ref rowsAffected);

                myMsg.ClearContext();

                myMsg.CreateFirstNode("ReturnValue", iReturn.ToString());
                myMsg.CreateFirstNode("RowsAffected", rowsAffected.ToString());
                myMsg.CreateFirstNode("Err", parameters[9].Value.ToString());
           
            }
            catch (Exception e)
            {
                myMsg.ClearContext();
                myMsg.CreateFirstNode("Err", e.Message.ToString());
            }

            return myMsg.GetXmlStr;
        }
        #endregion

        #region "Apply Data QC02 HuuMinh"
        public string db_InsUpdApplyCheckQC02(string strXML)
        {

            PccMsg myMsg = new PccMsg(strXML);
            string ac_id = myMsg.Query("ac_id").ToString().Trim();
            string fact_no = myMsg.Query("fact_no").ToString().Trim();
            string dept_no = myMsg.Query("dept_no").ToString().Trim();
            string sec_no = myMsg.Query("sec_no").ToString().Trim();
            string type_check = myMsg.Query("type_check").ToString().Trim();
            string vou_no = myMsg.Query("vou_no").ToString().Trim();
            string total = myMsg.Query("total").ToString().Trim();
            string floor = myMsg.Query("floor").ToString().Trim();
            string build_no = myMsg.Query("build_no").ToString().Trim();
            string myType = myMsg.Query("Type").ToString().Trim();

            if (ac_id == "")
                ac_id = "0";
            if (total == "")
                total = "0";

            SqlParameter[] parameters = { 
                                            new SqlParameter("@ac_id",SqlDbType.Int),  
                                            new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),
                                            new SqlParameter("@type_check",SqlDbType.Char,1),
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
											new SqlParameter("@total",SqlDbType.Int), 
                                            new SqlParameter("@floor",SqlDbType.Char,1),
                                            new SqlParameter("@build_no",SqlDbType.Char,2),                                       
                                             new SqlParameter("@Type",SqlDbType.Char,1),
											new SqlParameter("@Err",SqlDbType.NVarChar,100)
										};

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = fact_no;
            parameters[2].Value = dept_no;
            parameters[3].Value = sec_no;
            parameters[4].Value = type_check;
            parameters[5].Value = vou_no;
            parameters[6].Value = total;
            parameters[7].Value = floor;
            parameters[8].Value = build_no;
            parameters[9].Value = myType;
            parameters[10].Direction = ParameterDirection.Output;

            try
            {
                int iReturn = 0, rowsAffected = 0;
                iReturn = RunProcedure("Pro_InsUpdApplyCheckQC02", parameters, ref rowsAffected);

                myMsg.ClearContext();

                myMsg.CreateFirstNode("ReturnValue", iReturn.ToString());
                myMsg.CreateFirstNode("RowsAffected", rowsAffected.ToString());
                myMsg.CreateFirstNode("Err", parameters[10].Value.ToString());
                myMsg.CreateFirstNode("ac_id", parameters[0].Value.ToString());
            }
            catch (Exception e)
            {
                myMsg.ClearContext();
                myMsg.CreateFirstNode("Err", e.Message.ToString());
            }

            return myMsg.GetXmlStr;
        }
        #endregion


        #region "Get user QC" //web.huuminh lay user QC
        public DataSet db_get_UserQc(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            SqlParameter[] parameters = {
											new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10)
                                            
										};
            parameters[0].Value = myMsg.Query("dept_no");
            parameters[1].Value = myMsg.Query("sec_no");
            dsReturn = RunProcedure("pro_Get_UserQc", parameters, "UserQc");
            return dsReturn;
        }
        #endregion

        #region "sum qty ApplyRatingD"
        //web.huuminh lay sum qty,total
        public DataSet db_sum_qty_total_Qc02(string strXML)
        {
            DataSet dsReturn = new DataSet();
            PccMsg myMsg = new PccMsg(strXML);
            string  userQC_id=myMsg.Query("userQC_id");
            if (userQC_id == "")
                userQC_id = "0";
            SqlParameter[] parameters = {											
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
                                            new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),
                                            new SqlParameter("@bad_no",SqlDbType.Char,10),
                                            new SqlParameter("@userQC_id",SqlDbType.Decimal,18),
                                            new SqlParameter("@add_date",SqlDbType.Char,8),
                                            new SqlParameter("@Type",SqlDbType.Char,1),
										};
            parameters[0].Value = myMsg.Query("vou_no");
            parameters[1].Value = myMsg.Query("fact_no");
            parameters[2].Value = myMsg.Query("dept_no");
            parameters[3].Value = myMsg.Query("sec_no");
            parameters[4].Value = myMsg.Query("bad_no");
            parameters[5].Value = userQC_id;
            parameters[6].Value = myMsg.Query("add_date");
            parameters[7].Value = myMsg.Query("Type");
            dsReturn = RunProcedure("pro_sum_qty_total_Qc02", parameters, "qty_total_Qc02");
            return dsReturn;
        }
        #endregion


        #region "ins upd ApplyRating  HuuMinh"
        public string db_InsUpdApplyRating(string strXML)
        {

            PccMsg myMsg = new PccMsg(strXML);
            string AcRat_id = myMsg.Query("AcRat_id").ToString().Trim();
            string fact_no = myMsg.Query("fact_no").ToString().Trim();
            string dept_no = myMsg.Query("dept_no").ToString().Trim();
            string sec_no = myMsg.Query("sec_no").ToString().Trim();      
            string vou_no = myMsg.Query("vou_no").ToString().Trim();
            string total = myMsg.Query("total").ToString().Trim();
            string floor = myMsg.Query("floor").ToString().Trim();
            string build_no = myMsg.Query("build_no").ToString().Trim();
            string userQC_id = myMsg.Query("userQC_id").ToString().Trim();
            string upd_id = myMsg.Query("upd_id").ToString().Trim();
            string myType = myMsg.Query("Type").ToString().Trim();


            if (AcRat_id == "")
                AcRat_id = "0";
            if (total == "")
                total = "0";
            if (userQC_id == "")
                userQC_id = "0";
            if (upd_id == "")
                upd_id = "0";
            SqlParameter[] parameters = { 
                                            new SqlParameter("@AcRat_id",SqlDbType.Int),  
                                            new SqlParameter("@fact_no",SqlDbType.Char,4),
                                            new SqlParameter("@dept_no",SqlDbType.Char,10),
                                            new SqlParameter("@sec_no",SqlDbType.Char,10),                                         
                                            new SqlParameter("@vou_no",SqlDbType.VarChar,50),
											new SqlParameter("@total",SqlDbType.Int), 
                                            new SqlParameter("@floor",SqlDbType.Char,1),
                                            new SqlParameter("@build_no",SqlDbType.Char,2),
                                            new SqlParameter("@userQC_id",SqlDbType.Decimal,18),
                                            new SqlParameter("@upd_id",SqlDbType.Decimal,18),
                                            new SqlParameter("@Type",SqlDbType.Char,1),
											new SqlParameter("@Err",SqlDbType.NVarChar,100)
										};

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = fact_no;
            parameters[2].Value = dept_no;
            parameters[3].Value = sec_no;
            parameters[4].Value = vou_no;
            parameters[5].Value = total;
            parameters[6].Value = floor;
            parameters[7].Value = build_no;
            parameters[8].Value = userQC_id;
            parameters[9].Value = upd_id;
            parameters[10].Value = myType;
            parameters[11].Direction = ParameterDirection.Output;

            try
            {
                int iReturn = 0, rowsAffected = 0;
                iReturn = RunProcedure("Pro_InsUpdApplyRating", parameters, ref rowsAffected);

                myMsg.ClearContext();

                myMsg.CreateFirstNode("ReturnValue", iReturn.ToString());
                myMsg.CreateFirstNode("RowsAffected", rowsAffected.ToString());
                myMsg.CreateFirstNode("Err", parameters[11].Value.ToString());
                myMsg.CreateFirstNode("AcRat_id", parameters[0].Value.ToString());
            }
            catch (Exception e)
            {
                myMsg.ClearContext();
                myMsg.CreateFirstNode("Err", e.Message.ToString());
            }

            return myMsg.GetXmlStr;
        }
        #endregion


        #region "Ins Upd ApplyRating Detail HuuMinh"
        public string db_InsUpdApplyRatingDetail(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string id = myMsg.Query("id").ToString().Trim();
            string AcRat_id = myMsg.Query("AcRat_id").ToString().Trim();
            string bad_no = myMsg.Query("bad_no").ToString().Trim();
            string bad_desc = myMsg.Query("bad_desc").ToString().Trim();
            string bad_descVN = myMsg.Query("bad_descVN").ToString().Trim();       
            string qty = myMsg.Query("qty").ToString().Trim();
            string qtyOKitem = myMsg.Query("qtyOKitem").ToString().Trim();
            string add_id = myMsg.Query("add_id").ToString().Trim();
            string myType = myMsg.Query("Type").ToString().Trim();

            if (id == "")
                id = "0";
            if (AcRat_id == "")
                AcRat_id = "0";
            if (qty == "")
                qty = "0";
            if (qtyOKitem == "")
                qtyOKitem = "0";
            if (add_id == "")
                add_id = "0";
            SqlParameter[] parameters = { 
                                            new SqlParameter("@id",SqlDbType.Decimal,18),  
                                            new SqlParameter("@AcRat_id",SqlDbType.Int),  
                                            new SqlParameter("@bad_no",SqlDbType.Char,10),
                                            new SqlParameter("@bad_desc",SqlDbType.NVarChar,100),
                                            new SqlParameter("@bad_descVN",SqlDbType.NVarChar,100),                                            
                                            new SqlParameter("@qty",SqlDbType.Int),
											new SqlParameter("@qtyOKitem",SqlDbType.Int),                                                                                
                                             new SqlParameter("@add_id",SqlDbType.Decimal,18), 
                                             new SqlParameter("@Type",SqlDbType.Char,1),
											new SqlParameter("@Err",SqlDbType.NVarChar,100)
										};
            parameters[0].Value = id;
            parameters[1].Value = AcRat_id;
            parameters[2].Value = bad_no;
            parameters[3].Value = bad_desc;
            parameters[4].Value = bad_descVN;
            parameters[5].Value = qty;
            parameters[6].Value = qtyOKitem;
            parameters[7].Value = add_id;
            parameters[8].Value = myType;
            parameters[9].Direction = ParameterDirection.Output;

            try
            {
                int iReturn = 0, rowsAffected = 0;
                iReturn = RunProcedure("Pro_InsUpdApplyRatingDetail", parameters, ref rowsAffected);

                myMsg.ClearContext();

                myMsg.CreateFirstNode("ReturnValue", iReturn.ToString());
                myMsg.CreateFirstNode("RowsAffected", rowsAffected.ToString());
                myMsg.CreateFirstNode("Err", parameters[9].Value.ToString());

            }
            catch (Exception e)
            {
                myMsg.ClearContext();
                myMsg.CreateFirstNode("Err", e.Message.ToString());
            }

            return myMsg.GetXmlStr;
        }
        #endregion
    }

}
