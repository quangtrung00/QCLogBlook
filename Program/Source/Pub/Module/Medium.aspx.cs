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
using System.Xml;
using PccBsLayerForC;
using PccCommonForC; 

public partial class Pub_Module_Medium : System.Web.UI.Page
{
    /*var menus = new Array(
				2,	
							
				"<image src=../../images/MenuArea/N_Close.gif />",		
							
				1,
				"應用程式管理",
				"../../images/MenuArea/DgyyWebWinNew/sFile1.gif",
				"../../GenCodeAP/APManage/APManage141.aspx?ApID=141",	
				"1",					
							
				"<image src=../../images/MenuArea/Y_Close.gif />",										
							
				2,					
				"群組管理",									
				"../../images/MenuArea/DgyyWebWinNew/sFile1.gif",						
				"../../SysManager/GroupManage/GroupManage104.aspx?ApID=104",				
				"1",												
							
				"使用者管理",										
				"../../images/MenuArea/DgyyWebWinNew/sQuery1.gif",				
				"../../SysManager/UserManage/UserManage104.aspx?ApID=104",				
				"2"												
				);
				
				//開始寫入
				strLeftMenu += "var menus = new Array (";
				strLeftMenu += "2,"; //總共多少分區
					
				strLeftMenu += "\"<image src=../../images/MenuArea/N_Open.gif />\","; //第一個分區的圖形或文字
				strLeftMenu += "1,"; //第一個大項次的開始，表示有多少個Item
				//第一個分區的第一個小Item參數
				strLeftMenu += "\"應用程式管理\","; //選單名稱
				strLeftMenu += "\"../../images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
				strLeftMenu += "\"../../GenCodeAP/APManage/APManage141.aspx?ApID=141\",";//選單的連結網頁
				strLeftMenu += "\"1\","; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

				strLeftMenu += "\"<image src=../../images/MenuArea/Y_Open.gif />\","; //第二個分區的圖形或文字
				strLeftMenu += "2,"; //第二個大項次的開始，表示有多少個Item
				//第二個分區的第一個小Item參數
				strLeftMenu += "\"群組管理\","; //選單名稱
				strLeftMenu += "\"../../images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
				strLeftMenu += "\"../../SysManager/GroupManage/GroupManage104.aspx?ApID=104\",";//選單的連結網頁
				strLeftMenu += "\"1\","; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。
				//第二個分區的第二個小Item參數
				strLeftMenu += "\"使用者管理\","; //選單名稱
				strLeftMenu += "\"../../images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
				strLeftMenu += "\"../../SysManager/UserManage/UserManage104.aspx?ApID=104\",";//選單的連結網頁
				strLeftMenu += "\"1\""; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

				strLeftMenu += ");";*/

    private Hashtable m_Menu = new Hashtable(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) return;
        // 將使用者程式碼置於此以初始化網頁
        string strLeftMenu = string.Empty;

        int AreaCount = 1;

        //利用LoginInfo取得此User的AP Detail Menu.
        PccCommonForC.PccErrMsg myLabel = new PccCommonForC.PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"), Session["CodePage"].ToString(), "Label");
        string strDetailPageLayer = Session["PageLayer"].ToString();

        if (Request.QueryString["ApID"] != null)
        {
            //取得應用程式之APID
            string strApID = Request.Params["ApID"];
            //string ApFolder = "", MenuItem ="", MenuName ="";
            string ApFolder = string.Empty;


            PccCommonForC.PccMsg myMsg = new PccCommonForC.PccMsg();
            myMsg.LoadXml(Session["XmlLoginInfo"].ToString());

            if (strApID.Length > 0 && strDetailPageLayer.Length > 0)
            {
                //strDetailPageLayer = strDetailPageLayer.Substring(0,strDetailPageLayer.Length - 3);

                //判斷是否有系統可以使用，若沒有則直接跳出
                if (myMsg.QueryNodes("Authorize") != null)
                {
                    foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
                    {
                        //假如不是點選這個系統則直接跳出並判斷下一個
                        if (strApID != myMsg.Query("APID", myNode)) continue;

                        //取得這個AP的最上層之Folder
                        ApFolder = GetApFolder(myMsg.Query("APLink", myNode));

                        //判斷是否有選單系統可以使用，若沒有則直接跳出
                        if (myMsg.QueryNodes("ApMenu", (XmlElement)myNode) != null)
                        {
                            foreach (XmlNode myDetailNode in myMsg.QueryNodes("ApMenu", (XmlElement)myNode))
                            {
                                if (myMsg.Query("show_mk", myDetailNode).Equals("Y"))
                                {
                                    //判斷是否為Web Service的Menu若是則直接跳過 2005/3/8
                                    if (myMsg.Query("MenuLink", myDetailNode).IndexOf(".asmx", 0) > 0)
                                        continue;

                                    //本來在越南區還會做Menu的語系轉換，我認為沒有必要，因為那是直接從資料庫取得的資訊
                                    //這是可以做修改的，而且可利用中(英)對照來解決。實做方式如下：
                                    //有一個很大的缺點，就是若在LabelMsg.xml中沒有加入相對應的值則無法顯示。20060626
                                    /* //060306 判斷語系是否為中文
                                    if(Session["CodePage"].ToString() =="CP950")
                                    {
                                        SaveDataToHashMenu(myMsg.Query("MenuManage",(XmlElement)myDetailNode),strDetailPageLayer,ApFolder,myMsg.Query("MenuNM",(XmlElement)myDetailNode),myMsg.Query("MenuLink",(XmlElement)myDetailNode));
                                    }
                                    else
                                    {
                                        MenuItem = "M" + strApID + int.Parse(myMsg.Query("MENUID",myDetailNode)).ToString();
                                        MenuName = myLabel.GetErrMsg(MenuItem,"MenuItem");
                                        SaveDataToHashMenu(myMsg.Query("MenuManage",(XmlElement)myDetailNode),strDetailPageLayer, ApFolder, MenuName, myMsg.Query("MenuLink",(XmlElement)myDetailNode));

                                    }	*/

                                    SaveDataToHashMenu(myMsg.Query("MenuManage", (XmlElement)myDetailNode), strDetailPageLayer, ApFolder, myMsg.Query("MenuNM", (XmlElement)myDetailNode), myMsg.Query("MenuLink", (XmlElement)myDetailNode));
                                }
                            }
                        }
                    }
                }
            }

            strLeftMenu += "var menus = new Array (";
            int count = 1;

            if (m_Menu.Count > 0)
            {
                AreaCount += m_Menu.Count;
                strLeftMenu += AreaCount.ToString() + ","; //總共多少分區
                strLeftMenu += GetWelcome(strDetailPageLayer, strApID, ref myLabel) + ",";

                /*
                本來在這個地方越南地區也是有做語系的處理，就是在本來設定的image如：N_Open.gif變成N_Open_VN.gif
                我持的理由還是一樣，我覺得Menu區不必做語系的轉換，因為這可以在早期就決定是會在那裡實做，而且可以
                更新圖形即可，而且最好是放在某一個Floder，而其Floder的Name放在Cofig中，這樣比較有彈性。20060626
                */
                //設定一般區
                if (m_Menu.ContainsKey("N"))
                {
                    strLeftMenu += "\"<image src=" + strDetailPageLayer + "images/MenuArea/N_Open.gif />\",";
                    count = m_Menu["N"].ToString().Split(',').Length / 4;
                    strLeftMenu += count.ToString() + ","; //一個大項次的開始，表示有多少個Item
                    strLeftMenu += m_Menu["N"].ToString();
                }

                foreach (string strArea in m_Menu.Keys)
                {
                    if (!strArea.Equals("N") && !strArea.Equals("Y"))
                    {
                        strLeftMenu += "\"<image src=" + strDetailPageLayer + "images/MenuArea/" + strArea + "_Open.gif />\",";
                        count = m_Menu[strArea].ToString().Split(',').Length / 4;
                        strLeftMenu += count.ToString() + ","; //第一個大項次的開始，表示有多少個Item
                        strLeftMenu += m_Menu[strArea].ToString();
                    }
                }

                //設定權限管理區
                if (m_Menu.ContainsKey("Y"))
                {
                    strLeftMenu += "\"<image src=" + strDetailPageLayer + "images/MenuArea/Y_Open.gif />\",";
                    count = m_Menu["Y"].ToString().Split(',').Length / 4;
                    strLeftMenu += count.ToString() + ","; //一個大項次的開始，表示有多少個Item
                    strLeftMenu += m_Menu["Y"].ToString();
                }

                strLeftMenu = strLeftMenu.Substring(0, strLeftMenu.Length - 1);
            }
            else //表示沒有選單權限
            {
                strLeftMenu += AreaCount.ToString() + ","; //總共多少分區
                strLeftMenu += GetWelcome(strDetailPageLayer, strApID, ref myLabel);
            }


            strLeftMenu += ");";


        }

        txtLeftMenu.Value = strLeftMenu; 


    }

    private void SaveDataToHashMenu(string Area, string strLayer, string ApFolder, string menuNm, string menuLink)
    {
        string strReturn = string.Empty;

        if (m_Menu.ContainsKey(Area))
            strReturn = m_Menu[Area].ToString();

        strReturn += "\"" + menuNm + "\","; //選單名稱
        strReturn += "\"" + strLayer + "images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
        //判斷是否為權限管理區
        if (Area.Equals("Y"))
            strReturn += "\"" + strLayer + "SysManager/" + menuLink + "\",";//選單的連結網頁
        else
            strReturn += "\"" + strLayer + ApFolder + "/" + menuLink + "\",";//選單的連結網頁

        strReturn += "\"1\","; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

        if (m_Menu.ContainsKey(Area))
            m_Menu[Area] = strReturn;
        else
            m_Menu.Add(Area, strReturn);

    }

    private string GetApFolder(string ApLink)
    {
        /*int pos = ApLink.IndexOf("/");
        string strReturn = ApLink.Substring(0,pos);
        return strReturn;*/

        int pos = ApLink.LastIndexOf("&");
        string strReturn1 = ApLink.Substring(pos + 1);
        string strReturn = strReturn1.Split('=')[1].Trim();
        return strReturn;
    }

    private string GetWelcome(string strLayer, string strApID, ref PccErrMsg myLabel)
    {
        string strReturn = string.Empty;

        bs_Security mySecurity = new bs_Security(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        string strCount = "0";
        PccCommonForC.PccMsg myMsg1 = new PccCommonForC.PccMsg();

        if (strApID != null && int.Parse(strApID) > 0)
        {
            //新增這個系統的前置詞
            myMsg1.CreateFirstNode("ap_id", strApID);
            myMsg1.CreateFirstNode("user_id", Session["UserID"].ToString());

            if (((Hashtable)Session["APCounts"]).ContainsKey(strApID))
            {
                strCount = ((Hashtable)Session["APCounts"])[strApID].ToString();
            }
            else
            {
                strCount = mySecurity.DoReturnStr("GetAndUpdateApCounts", myMsg1.GetXmlStr, "");
                ((Hashtable)Session["APCounts"]).Add(strApID, strCount);
            }
        }

        //060306 將原本Hard code的Menu內容改由LabelMsg_TC.xml抓取
        string strWel = myLabel.GetErrMsg("M000001", "MenuItem");
        string strCome = myLabel.GetErrMsg("M000002", "MenuItem");
        string strUpdateUser = myLabel.GetErrMsg("M000011", "MenuItem");
        string strAddSystem = myLabel.GetErrMsg("M000012", "MenuItem");
        string strTransfer = myLabel.GetErrMsg("M000013", "MenuItem");

        //strReturn += "\"歡迎" + Session["UserName"].ToString() + "光臨(" + strCount + ")\","; //第一個分區的圖形或文字
        strReturn += "\"" + strWel + Session["UserName"].ToString() + strCome + "(" + strCount + ")\","; //第一個分區的圖形或文字
        strReturn += "3,"; //第一個大項次的開始
        //第一個分區的第一個小Item參數
        //strReturn += "\"個人資料修改\","; //選單名稱
        strReturn += "\"" + strUpdateUser + "\","; //選單名稱
        strReturn += "\"" + strLayer + "images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
        strReturn += "\"" + ConfigurationManager.AppSettings["PFSBaseWeb"] + "usermanage/UpdateUser.aspx?UserID=" + Session["UserID"].ToString() + "&AcctionType=Upd&UserAccount=" + Session["UserAccount"].ToString() + "\",";//選單的連結網頁
        strReturn += "\"1\","; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

        //第一個分區的第二個小Item參數
        //strReturn += "\"加入系統\","; //選單名稱
        strReturn += "\"" + strAddSystem + "\","; //選單名稱
        strReturn += "\"" + strLayer + "images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
        strReturn += "\"ApplyAccount.aspx?Type=Update\",";//選單的連結網頁
        strReturn += "\"1\","; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

        //第一個分區的第三個小Item參數
        //strReturn += "\"中英文轉換\","; //選單名稱
        strReturn += "\"" + strTransfer + "\","; //選單名稱
        strReturn += "\"" + strLayer + "images/MenuArea/DgyyWebWinNew/sFile1.gif\","; //選單的前置圖形
        strReturn += "\"ChangeLanguage.aspx?ApID=0\",";//選單的連結網頁
        strReturn += "\"1\""; //型態表示開啟一個頁面在IFrame上，若為2表示呼叫一個函式。

        return strReturn;
    }



    #region Web Form Designer generated code
    
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: 此呼叫為 ASP.NET Web Form 設計工具的必要項。
        //
        //計算這個網頁所在的層次是那裡
        int i, j = 0;
        string strPageLayer = "";
        string LocalPath = PccCommonForC.PccToolFunc.Upper(Server.MapPath("."));

        j = LocalPath.IndexOf(PccCommonForC.PccToolFunc.Upper(Application["EDPNET"].ToString()));

        try
        {
            for (i = 1; i < LocalPath.Substring(j).Split('\\').Length; i++)
            {
                strPageLayer += "../";
            }
            Session["PageLayer"] = strPageLayer;
        }
        catch
        {
            Session["PageLayer"] = "";
        }

        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// 此為設計工具支援所必需的方法 - 請勿使用程式碼編輯器修改
    /// 這個方法的內容。
    /// </summary>
    private void InitializeComponent()
    {

    }

    #endregion

}
