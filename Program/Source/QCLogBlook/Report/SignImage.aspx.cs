using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using System.IO;
using PccBsQCLogBlook;
using PccDbLayerForC;
public partial class QCLogBlook_Report_SignImage : System.Web.UI.Page
{
    public string IdImage;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string xmlLoginInfo = Session["XmlLoginInfo"].ToString();
        //PccMsg myInfoMsg = new PccMsg(xmlLoginInfo);
        //xmlUserID = myInfoMsg.Query("UserID").ToString().Trim();
        IdImage = Request.QueryString["id"].ToString();
        GetImageFile(IdImage);

       
    }
    public void GetImageFile(string IdImage)
    {

        //if (Session["IdImage"].ToString() != null)
        //{

            bs_UserInfo bs_Route = new bs_UserInfo(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
            PccMsg myMsg = new PccMsg();
            myMsg.CreateFirstNode("user_id", IdImage.ToString().Trim());
            string strXML = myMsg.GetXmlStr;
            DataTable ImageTable = bs_Route.DoReturnDataSet("GETUSERNAMESIGN", strXML, "").Tables[0];
            //return dt;
            byte[] byteReturn = { };

            if (ImageTable.Rows.Count > 0)
            {
                byteReturn = (byte[])ImageTable.Rows[0]["sign_pic"];
                Response.BinaryWrite(byteReturn);
            }

      //  }


    }
}