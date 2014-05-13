using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;

/// <summary>
/// myTools 的摘要描述
/// </summary>
public class myTools
{
    public myTools()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    // 摘要描述
    // 函數名稱：
    // 函數類別：
    // 函數功能：
    // 函數說明：UI層經常用的函數
    // 輸入資料：
    // 輸出資料：
    // 注意事項：
    // 原設計者：Leo.Tang
    // 設立日期：開發中

    public PccMsg CreateQueryMsg(string QueryAction, string QueryReturns, string QuerySource, string QueryCondition)
    {
        PccMsg retMsg = new PccMsg();
        retMsg.CreateFirstNode("QueryAction", QueryAction);
        retMsg.CreateFirstNode("QueryReturns", QueryReturns);
        retMsg.CreateFirstNode("QuerySource", QuerySource);
        retMsg.CreateFirstNode("QueryCondition", QueryCondition);

        return retMsg;
    }

    public PccMsg CreateQueryMsg(string queryAction, string queryInput, string procedure)
    {
        PccMsg retMsg = new PccMsg();
        retMsg.CreateFirstNode("QueryAction", queryAction);
        retMsg.CreateFirstNode("QueryInput", queryInput);
        retMsg.CreateFirstNode("Procedure", procedure);

        return retMsg;
    }

    public void SetDDListData(DataTable myTable, DropDownList ddl, string myStr)
    {

        if (myTable.Rows.Count > 0)
        {
            foreach (DataRow myRow in myTable.Rows)
            {
                SetDataToDdl(myRow, ddl, myStr);
            }
        }

    }

    private void SetDataToDdl(DataRow myRow, DropDownList ddl, string myStr)
    {
        PccMsg myMsg = new PccMsg(myStr);

        ListItem ddlItem = new ListItem();
        int col = int.Parse(myMsg.Query("col"));

        for (int i = 0; i < col; i++)
            ddlItem.Text += myRow[myMsg.Query("col" + i.ToString())].ToString() + ":";

        ddlItem.Value = myRow[myMsg.Query("ID")].ToString();
        ddl.Items.Add(ddlItem);
    }

    public static string FormatDate_char8(string date)
    {
        return DateTime.ParseExact(date, "yyyyMMdd", null).ToString("yyyy/MM/dd");
    }

}
