using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PccCommonForC;
using System.Data;

namespace PccBsQCLogBlook
{
    public class db_DataORA : DbOraBaseObject
    {
        #region Contructor
        public db_DataORA() : base()
        {
           
        }
        public db_DataORA(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd)
        {

        }
        #endregion

        public DataSet get_Fact(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            //string fact_no = myMsg.Query("fact_no");

            string strSQL = "  SELECT FACT_NO "
                            + " FROM VIE_SEC_WEB ";                          
                            //+ " GROUP BY DEPT_NO ";

            return Query(strSQL, "Fact"); 
        }
        public DataSet get_dept(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");

            //string strSQL = "  SELECT DEPT_NO "
            //                + " FROM VIE_SEC_WEB "
            //                + " WHERE FACT_NO = '"+fact_no+"'  "
            //                + " GROUP BY DEPT_NO "
            //                + " ORDER BY DEPT_NO ";

            string strSQL = " SELECT dept_no, dept_name "
                            +" FROM vie_dept_web "
                            + " WHERE fact_no = '"+fact_no+"'  "
                            + " GROUP BY dept_no, dept_name "
                            + " ORDER BY dept_no ";

            return Query(strSQL, "Dept"); ;
        }

        public DataSet get_sec(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string build_no = myMsg.Query("build_no");
            string floor = myMsg.Query("floor");


            string strSQL = " SELECT a.FACT_NO, a.BUILD_NO, a.FLOOR, a.DEPT_NO, a.SEC_NO, a.SEC_NAME, a.REMARK "
                        + "      ,b.dept_name "
                        + " FROM VIE_SEC_WEB a, "
                        + "      (SELECT fact_no, dept_no, dept_name FROM vie_dept_web GROUP BY fact_no, dept_no, dept_name) b "
                        + " WHERE a.dept_no = b.dept_no "
                        + " AND a.FACT_NO = '" + fact_no + "' "
                        + " AND b.FACT_NO = '" + fact_no + "' ";
   
            if(dept_no !="")
                strSQL += " AND a.DEPT_NO = '" + dept_no + "' ";

            if (build_no != "")
                strSQL += " AND a.BUILD_NO = '" + build_no + "' ";

            if (floor != "")
                strSQL += " AND a.FLOOR = '" + floor + "' ";


            strSQL += " ORDER BY a.FACT_NO, a.DEPT_NO, a.SEC_NO ";



            return Query(strSQL, "Sec"); ;
        }


        public DataSet get_build(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");


            string strSQL = " SELECT BUILD_NO "
                            + " FROM VIE_SEC_WEB "
                            + " WHERE FACT_NO = '" + fact_no + "' "
                            + " GROUP BY BUILD_NO "
                            + " ORDER BY BUILD_NO ";

            return Query(strSQL, "Build"); 
        }

        public DataSet get_floor(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");


            string strSQL = " SELECT FLOOR "
                            + " FROM VIE_SEC_WEB "
                            + " WHERE FACT_NO = '" + fact_no + "' "
                            + " AND FLOOR <> ' ' "
                            + " GROUP BY FLOOR "
                            + " ORDER BY FLOOR ";

            return Query(strSQL, "Floor");
        }



        //web.huuminh 20140416
        //lay ma don dua vao fact,dept,sec_no
        public DataSet get_fact_odrno(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string sec_no = myMsg.Query("sec_no");

            string strSQL = " select * from vie_odrm_web "
                            + " where fact_no = '" + fact_no + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " AND sec_no = '" + sec_no + "' ";

            return Query(strSQL, "fact_odrno");
        }
        //web.huuminh 20140416
        //lay lỗi dua vao fact,dept
        public DataSet get_bad_reason(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");

            string strSQL = " select * from vie_bad_reason_m_web "
                            + " where fact_no = '" + fact_no + "' "
                            + " AND dept_no = '" + dept_no + "' ";

            return Query(strSQL, "bad_reason");
        }
        //web.huuminh 2014/04/19
        //lay tong so rut kiem
        public DataSet get_sum_act_qty(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string sec_no = myMsg.Query("sec_no");
            string vou_no = myMsg.Query("vou_no");

            string strSQL = " select nvl(sum(act_qty),0) as SumTotal from  vie_odrm_web where "
                            + " fact_odr_no='"+vou_no+"'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " AND sec_no = '" + sec_no + "' ";

            return Query(strSQL, "sum_act_qty");
        }
        //web.huuminh 2014/04/19
        //lay floor
        public DataSet get_floor_erp(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string sec_no = myMsg.Query("sec_no");

            string strSQL = " select floor from vie_sec_web where "                          
                            + "  fact_no = '" + fact_no + "' "
                            + "  AND dept_no = '" + dept_no + "' "
                            + "  AND sec_no = '" + sec_no + "' ";

            return Query(strSQL, "get_floor");
        }
        //web.huuminh 2014/04/19
        //lay tong so rut kiem
        public DataSet get_build_no_erp(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string sec_no = myMsg.Query("sec_no");
            string floor = myMsg.Query("floor");
            string strSQL = " select build_no from vie_sec_web where "
                            + "  fact_no = '" + fact_no + "' "
                            + "  AND dept_no = '" + dept_no + "' "
                            + "  AND sec_no = '" + sec_no + "' "
                            + "  AND floor = '" + floor + "' ";

            return Query(strSQL, "get_floor");
        }

      
        
        //web.huuminh 2014/04/19
        //lây time_no từ erp
        public DataSet get_time_no(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);    
            string fact_no = myMsg.Query("fact_no");
            string dept_no = myMsg.Query("dept_no");
            string time_now = myMsg.Query("time_now");

            string strSQL = " select time_no from vie_dept_web "
                            + " where fact_no = '" + fact_no + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " and " + time_now + ">=time_s  and " + time_now + "<time_e ";

            return Query(strSQL, "time_no");
        }
        //web.huuminh 2014/04/19
        //insert data_qc_web erp
        public string db_insupd_data_qc_web(string strXML)
        {
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);

            string fact_no = myMsg.Query("fact_no");
            string rec_date = myMsg.Query("rec_date");
            string vou_no = myMsg.Query("vou_no");
            string sec_no = myMsg.Query("sec_no");
            string bad_no = myMsg.Query("bad_no");
            string time_no = myMsg.Query("time_no");
            string kind_mk = myMsg.Query("kind_mk");
            string upd_user = myMsg.Query("upd_user");
            string upd_time = myMsg.Query("upd_time");
            string qty = myMsg.Query("qty");
            string qty_chk = myMsg.Query("qty_chk");
            string dept_no = myMsg.Query("dept_no");
            string Type = myMsg.Query("Type");

            int num = int.Parse(qty);
            string strReturn;

            string txt = "";
            string txt2 = "";
            if (Type == "0")
            {
                txt = "insert into data_qc_web(fact_no,rec_date,fact_odr_no,sec_no,bad_no,time_no,kind_mk,upd_user,upd_time,qty,qty_chk,dept_no)";
                txt +=
                    "  VALUES('" + fact_no + "','" + rec_date + "','" + vou_no + "','" + sec_no + "','" + bad_no + "','" + time_no + "','" + kind_mk + "','" + upd_user + "','" + upd_time + "','" + qty + "','" + qty_chk + "','" + dept_no + "')";
            }
            else
            {
                txt = "update data_qc_web set qty=NVL(qty,0)+"+qty+" , qty_chk='" + qty_chk + "'";
                txt += " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND bad_no = '" + bad_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' "
                            + " AND time_no = '" + time_no + "' "
                            + " AND upd_user = '" + upd_user + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " AND dept_no = '" + dept_no + "' ";
            }

            
            try
            {
                int iCount;
                ExcuteSQL(txt, out iCount);
               // ExcuteSQL(txt2, out iCount);
                strReturn = "OK";
            }
            catch (Exception e)
            {
                strReturn = e.ToString();
            }
            return strReturn;
        }

        #region "QC 02"         
        
        //web.huuminh 2014/04/19
        //kiem tra đơn ton tai hay chua
        public DataSet SumQtyQC02(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string vou_no = myMsg.Query("vou_no");
            string fact_no = myMsg.Query("fact_no");
            string rec_date = myMsg.Query("rec_date");
            string sec_no = myMsg.Query("sec_no");         
            string kind_mk = myMsg.Query("kind_mk");

            string strSQL = " select nvl(sum(qty),0) as SumQtyChk from  data_qc_web"
                            + " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' ";

            return Query(strSQL, "fact_odr_no");
        }
        //web.huuminh 2014/04/19
        //kiem tra đơn ton tai hay chua
        public DataSet SumQtyQC02ByBadNo(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string vou_no = myMsg.Query("vou_no");
            string fact_no = myMsg.Query("fact_no");
            string rec_date = myMsg.Query("rec_date");
            string sec_no = myMsg.Query("sec_no");
            string bad_no = myMsg.Query("bad_no");
            if (bad_no == "")
                bad_no = "     ";
            string kind_mk = myMsg.Query("kind_mk");

            string strSQL = " select nvl(sum(qty),0) as SumQtyChk from  data_qc_web"
                            + " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' "
                            + " AND bad_no = '" + bad_no + "' ";

            return Query(strSQL, "fact_odr_no");
        }
        //web.huuminh 2014/04/19
        //kiem tra đơn ton tai hay chua
        public DataSet testCountErpVouNo(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string vou_no = myMsg.Query("vou_no");
            string fact_no = myMsg.Query("fact_no");
            string rec_date = myMsg.Query("rec_date");
            string sec_no = myMsg.Query("sec_no");
            string bad_no = myMsg.Query("bad_no");
            if (bad_no == "")
                bad_no = "     ";
            string kind_mk = myMsg.Query("kind_mk");

            string strSQL = " select fact_odr_no from  data_qc_web "
                            + " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND bad_no = '" + bad_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' ";

            return Query(strSQL, "fact_odr_no");
        }
        //web.huuminh 2014/04/19
        //insert data_qc_web erp
        public string db_insupd_data_qc_web_02OK(string strXML)
        {
            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(strXML);

            string fact_no = myMsg.Query("fact_no");
            string rec_date = myMsg.Query("rec_date");
            string vou_no = myMsg.Query("vou_no");
            string sec_no = myMsg.Query("sec_no");

            string bad_no = myMsg.Query("bad_no");
            if (bad_no == "")
                bad_no = "     ";
            string time_no = myMsg.Query("time_no");
            string kind_mk = myMsg.Query("kind_mk");
            string upd_user = myMsg.Query("upd_user");
            string upd_time = myMsg.Query("upd_time");
            string qty = myMsg.Query("qty");
            string qty_chk = myMsg.Query("qty_chk");
            string dept_no = myMsg.Query("dept_no");
            string Type = myMsg.Query("Type");
            string strReturn;

            string txt = "";
            string txt2 = "";
            if (Type == "0")
            {
                txt = "insert into data_qc_web(fact_no,rec_date,fact_odr_no,sec_no,bad_no,time_no,kind_mk,upd_user,upd_time,qty,qty_chk,dept_no)";
                txt +=
                    "  VALUES('" + fact_no + "','" + rec_date + "','" + vou_no + "','" + sec_no + "','" + bad_no + "','" + time_no + "','" + kind_mk + "','" + upd_user + "','" + upd_time + "','" + qty + "','" + qty_chk + "','" + dept_no + "')";
              
            }
            else
            {
                txt = "update data_qc_web set time_no='" + time_no + "', upd_user='" + upd_user + "' , upd_time='" + upd_time + "', qty='" + qty + "', qty_chk='" + qty_chk + "'";
                txt += " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                             + " AND bad_no = '" + bad_no + "' "
                             + " AND kind_mk = '" + kind_mk + "'";
                
            }
            txt2 = "update data_qc_web set qty_chk='" + qty_chk + "'";
            txt2 += " where fact_odr_no='" + vou_no + "'"
                   + " and fact_no = '" + fact_no + "' "
                   //+ " AND rec_date = '" + rec_date + "' "
                   + " AND sec_no = '" + sec_no + "' "
                    + " AND kind_mk = '" + kind_mk + "'";
            try
            {
                int iCount;
                ExcuteSQL(txt, out iCount);
                ExcuteSQL(txt2, out iCount);
                strReturn = "OK";
            }
            catch (Exception e)
            {
                strReturn = e.ToString();
            }
            return strReturn;
        }
        #endregion

        //web.huuminh 2014/04/19
        //kiem tra đơn ton tai hay chua
        public DataSet SumQtyERP(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string vou_no = myMsg.Query("vou_no");
            string fact_no = myMsg.Query("fact_no");
            string sec_no = myMsg.Query("sec_no");
            string kind_mk = myMsg.Query("kind_mk");
            string time_no = myMsg.Query("time_no");
            string rec_date = myMsg.Query("rec_date");
            string upd_user = myMsg.Query("upd_user");
            string dept_no = myMsg.Query("dept_no");

            string strSQL = " select nvl(sum(qty_chk),0) as SumQty_chk from  data_qc_web"
                            + " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' "
                            + " AND upd_user = '" + upd_user + "' "
                            + " AND time_no <= '" + time_no + "'";
           
            return Query(strSQL, "fact_odr_no");
        }

        //web.huuminh 2014/04/19
        //kiem tra loi da ton tai hay chua
        public DataSet testCountBadNoErp(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string vou_no = myMsg.Query("vou_no");
            string fact_no = myMsg.Query("fact_no");
            string sec_no = myMsg.Query("sec_no");
            string kind_mk = myMsg.Query("kind_mk");
            string time_no = myMsg.Query("time_no");
            string rec_date = myMsg.Query("rec_date");
            string upd_user = myMsg.Query("upd_user");
            string dept_no = myMsg.Query("dept_no");
            string bad_no = myMsg.Query("bad_no");

            string strSQL = " select COUNT(*) as kt from  data_qc_web "
                             + " where fact_odr_no='" + vou_no + "'"
                            + " and fact_no = '" + fact_no + "' "
                            + " AND rec_date = '" + rec_date + "' "
                            + " AND sec_no = '" + sec_no + "' "
                            + " AND dept_no = '" + dept_no + "' "
                            + " AND kind_mk = '" + kind_mk + "' "
                            + " AND upd_user = '" + upd_user + "' "
                            + " AND time_no = '" + time_no + "'"
                            + " AND bad_no = '" + bad_no + "' ";

            return Query(strSQL, "sum_act_qty");
        }

    }
}
