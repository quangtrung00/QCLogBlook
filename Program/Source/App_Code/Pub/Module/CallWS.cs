using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccBsSystemForC;

/// <summary>
/// CallWS 的摘要描述
/// </summary>
public class CallWS
{
    public CallWS()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    //輸入的資料
    //	<PccMsg>
    //		<fact_no>K016</fact_no>
    //		<user_nm>lemor</user_nm>
    //		<user_desc>顏嘉皇</user_desc>
    //		<ext>7884</ext>
    //		<email>lemor.yen@mail.pouchen.com.tw</email>
    //	</PccMsg>
    public bool SendAccountToWS(string strInXML, ref string errormsg)
    {
        bool bReturn = true;

        return bReturn;
    }

    private string GenXMLFromFactoryRow(string fact_no)
    {
        return "";
    }


    public bool SendAccountToWS(DataTable dt, ref string errormsg)
    {
        bool bReturn = true;
        return bReturn;
    }

    private string CheckDBNull(object oFieldData)
    {
        if (Convert.IsDBNull(oFieldData))
            return "";
        else
            return oFieldData.ToString().Trim();
    }

    //輸入的資料
    //	<PccMsg>
    //		<CompanyNo>58711014</CompanyNo>
    //		<FactoryNo>AA</FactoryNo>
    //		<GroupNo>05</GroupNo>
    //		<FactoryName>寶成工業</FactoryName>
    //		<FactoryAddress>彰化縣</FactoryAddress>
    //		<PostNo>521</PostNo>
    //	</PccMsg>
    public bool SendFactoryToWS(string strInXML, ref string errormsg)
    {
        bool bReturn = true;

        return bReturn;
    }

    //輸入的資料
    //	<PccMsg>
    //		<CompanyNo>58711014</CompanyNo>
    //		<FactoryNo>AA</FactoryNo>
    //		<UserNo>vender1</UserNo>
    //	</PccMsg>
    public bool SendUserFactoryToWS(string strInXML, ref string errormsg)
    {
        bool bReturn = true;
        return bReturn;
    }

}
