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

public partial class Pub_Module_ChangeLanguage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ApID"] != null && Request.QueryString["ApID"] == "")
        {
            DefaultPage();
        }
        else
        {
            switch (Request.QueryString["ApID"].ToString())
            {
                case "0":
                    DefaultPage();
                    break;
                case "A":
                    AboutBody();
                    break;
                case "104":
                    SysManageBody();
                    break;
            }
        }

    }

    private void DefaultPage()
    {
        //新增PccApHome中所要顯示的元素 20040525

        PccRow myRow = new PccRow();
        myRow.AddTextCell("&nbsp;&nbsp;&nbsp;<font size=large color=blue><b>" + Session["UserName"] + "</b>Welcome to Pcc Ap Home</font>", 100);
        tblBody.Rows.Add(myRow.Row);
        myRow.Reset();
        myRow.AddTextCell("<br><font color=red><b>如您需修改個人資料,可點選左上方第一個分區點選個人資料修改選單，以進行個人資料修改</b></font>", 100);
        tblBody.Rows.Add(myRow.Row);

        //設定中英文轉換之Button
        if (Session["CodePage"].ToString() == "CP950")
            //LinkButton1.Text = "Do you want transfer to English?";
            LinkButton1.Text = "Ban co muon dich sang tieng Viet khong?";
        else
            LinkButton1.Text = "您想要轉換到中文嗎?";


        LinkButton1.Visible = true;
    }

    private void AboutBody()
    {
        PccRow myRow = new PccRow();
        myRow.AddTextCell("&nbsp;&nbsp;&nbsp;<font size=large color=blue><b>寶成國際集團系統網頁</b></font>", 100);
        tblBody.Rows.Add(myRow.Row);
        myRow.Reset();
        myRow.AddTextCell("<br><font color=red>所有的權利屬於<font color=blue><b>寶成國際集團</b></font></font>", 100);
        tblBody.Rows.Add(myRow.Row);
        myRow.Reset();
        myRow.AddTextCell("<br><font color=blue>版本：1.0</font>", 100);
        tblBody.Rows.Add(myRow.Row);
    }

    private void SysManageBody()
    {
        //在這裡可以寫入進入這個系統中所要顯示的首頁資訊，或是
        //由UserLogin.aspx中傳入之Params，利用其XML傳入的
        //參數做判斷以使這頁導引至正確的網頁。	
    }

    protected void LinkButton1_Click(object sender, System.EventArgs e)
    {
        //設定中英文轉換之Button
        if (Session["CodePage"].ToString() == "CP950")
        {
            //Session["CodePage"] = "CP437";
            Session["CodePage"] = "CP1258";
            LinkButton1.Text = "您想要轉換到中文嗎?";
            //在越南區中連Menu區都會轉換，所以會有以下的這一段Code
            //不過我認為這是沒有必要的，所以就沒有加上去了！ 20060626
            /* //060306 Session["CodePage"]重設後,重新整理medium1.aspx,必須確認Request.QueryString["ApID"]可否取到正確的APID
            RegisterClientScriptBlock("New", "<script language=javascript>window.parent.location.href='Medium1.aspx?APID=" + Request.QueryString["ApID"] + "';</script>");
            */
        }
        else
        {
            Session["CodePage"] = "CP950";
            //LinkButton1.Text = "Do you want transfer to English?";
            LinkButton1.Text = "Ban co muon dich sang tieng Viet khong?";
            //在越南區中連Menu區都會轉換，所以會有以下的這一段Code
            //不過我認為這是沒有必要的，所以就沒有加上去了！ 20060626
            /* //060306 Session["CodePage"]重設後,重新整理medium1.aspx,必須確認Request.QueryString["ApID"]可否取到正確的APID
            RegisterClientScriptBlock("New", "<script language=javascript>window.parent.location.href='Medium1.aspx?APID=" + Request.QueryString["ApID"] + "';</script>");
            */
        }
    }
}
