using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PccCommonForC;

namespace PccDbQCLogBlook
{
    public class db_Statistics : PccCommonForC.DbBaseObject
    {
        public db_Statistics() : base()
		{
			//
			// TODO: 在這裡加入建構函式的程式碼
			//
		}

        public db_Statistics(string ConnectionType, string ConnectionServer, string ConnectionDB, string ConnectionUser, string ConnectionPwd)
            : base(ConnectionType, ConnectionServer, ConnectionDB, ConnectionUser, ConnectionPwd)
		{

		}

        public DataSet pro_GetLogBookStatistics(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string sFactNo = myMsg.Query("fact_no");
            string sStartDate = myMsg.Query("start_date");
            string sEndDate = myMsg.Query("end_date");

            SqlParameter[] paraValue = {										 
										  new SqlParameter("@fact_no", SqlDbType.Char,4),
										  new SqlParameter("@start_date", SqlDbType.Char,8),
										  new SqlParameter("@end_date",SqlDbType.Char,8)

									  };
            paraValue[0].Value = sFactNo;
            paraValue[1].Value = sStartDate;
            paraValue[2].Value = sEndDate;

            return RunProcedure("pro_GetLogBookStatistics", paraValue, "pro_GetLogBookStatistics");

        }

        public DataSet pro_GetFactStatistics(string strXML)
        {
            PccMsg myMsg = new PccMsg(strXML);
            string sFactNo = myMsg.Query("fact_no");
         

            SqlParameter[] paraValue = {										 
										  new SqlParameter("@fact_no", SqlDbType.Char,4)
									
									  };
            paraValue[0].Value = sFactNo;            

            return RunProcedure("pro_GetFactStatistics", paraValue, "pro_GetLogBookStatistics");

        }
    }
}
