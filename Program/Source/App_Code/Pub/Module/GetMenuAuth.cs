using System;
using PccCommonForC;
using System.Web;
using System.Xml;
using System.Data;

/// <summary>
/// GetMenuAuth 的摘要描述
/// </summary>
public class GetMenuAuth
{
    private string m_Url, m_LoginInfoXml, m_Error, m_ApID;
    private string m_AspxFile = "";
    private DataTable m_DataTable;
    private bool m_bCheckTable = false;

    public GetMenuAuth()
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        HttpContext context = HttpContext.Current;
        m_Url = context.Request.ServerVariables["URL"].ToString();
        if (context.Session["XmlLoginInfo"] != null)
            m_LoginInfoXml = context.Session["XmlLoginInfo"].ToString();
        else
            m_LoginInfoXml = "";

        m_Error = "";
        if (context.Request.Params["ApID"] != null)
            m_ApID = context.Request.Params["ApID"].ToString();
        else
            m_ApID = "";

        if (context.Session["AuthTable"] != null)
        {
            m_DataTable = (DataTable)context.Session["AuthTable"];
            m_bCheckTable = true;
        }

    }

    public GetMenuAuth(string ApID, string URL, string LoginInfoXml)
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        HttpContext context = HttpContext.Current;
        m_Url = URL;
        m_LoginInfoXml = LoginInfoXml;
        m_Error = "";
        m_ApID = ApID;
        if (context.Session["AuthTable"] != null)
        {
            m_DataTable = (DataTable)context.Session["AuthTable"];
            m_bCheckTable = true;
        }
    }

    public GetMenuAuth(string ApID, string URL, string LoginInfoXml, DataTable AuthTable)
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        HttpContext context = HttpContext.Current;
        m_Url = URL;
        m_LoginInfoXml = LoginInfoXml;
        m_Error = "";
        m_ApID = ApID;
        m_DataTable = AuthTable;
        m_bCheckTable = true;
    }

    public string Url
    {
        get
        {
            return m_Url;
        }
        set
        {
            m_Url = value;
        }
    }

    public string LoginInfoXml
    {
        get
        {
            return m_LoginInfoXml;
        }
        set
        {
            m_LoginInfoXml = value;
        }
    }

    public DataTable AuthTable
    {
        get
        {
            return m_DataTable;
        }
        set
        {
            m_DataTable = value;
            m_bCheckTable = true;
        }
    }

    public string ApID
    {
        get
        {
            return m_ApID;
        }
        set
        {
            m_ApID = value;
        }
    }

    public string LastError
    {
        get
        {
            return m_Error;
        }
    }

    public string AspxFile
    {
        get
        {
            return m_AspxFile;
        }
        set
        {
            m_AspxFile = value;
        }
    }


    public string GetAuthByTable(string strAction)
    {
        if (m_ApID == "")
        {
            m_Error = "ApID is space!";
            return m_Error;
        }

        string strReturn = "";
        string AspxFileName = GetAspxFile();

        if (m_bCheckTable)
        {
            try
            {
                strReturn = m_DataTable.Select("ap_id = '" + m_ApID + "' and AspxFile = '" + AspxFileName + "'")[0][strAction].ToString();
            }
            catch
            {
            }
        }
        else
        {
            if (m_LoginInfoXml == "")
            {
                m_Error = "LoginInfoXml is space!";
                return m_Error;
            }

            if (m_Url == "")
            {
                m_Error = "URL is space!";
                return m_Error;
            }



            PccMsg myMsg = new PccMsg();

            try
            {
                myMsg.LoadXml(m_LoginInfoXml);
            }
            catch
            {
                m_Error = "Load LoginInfo XML Error!";
                return m_Error;
            }


            bool bOK = false;

            if (myMsg.QueryNodes("Authorize") != null)
            {
                foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
                {
                    if (myMsg.Query("APID", myNode) == m_ApID)
                    {
                        if (myMsg.QueryNodes("ApMenu", myNode) != null)
                        {
                            foreach (XmlNode myDetailNode in myMsg.QueryNodes("ApMenu", myNode))
                            {
                                if (CheckAspxFile(myMsg.Query("MenuLink", myDetailNode), AspxFileName))
                                {
                                    strReturn = myMsg.Query(strAction, myDetailNode);
                                    bOK = true;
                                    break;
                                }
                            } //end foreach myDeailNode
                        }// end if Menu Nodes.
                    }//end if APID
                    if (bOK) break;  //if find the Action then exit for loop.
                }//end foreach myNode
            }
        }

        return strReturn;

    }

    public string GetAuth(string strAction)
    {
        if (m_ApID == "")
        {
            m_Error = "ApID is space!";
            return m_Error;
        }

        string strReturn = "";
        string AspxFileName = GetAspxFile();

        if (m_LoginInfoXml == "")
        {
            m_Error = "LoginInfoXml is space!";
            return m_Error;
        }

        if (m_Url == "")
        {
            m_Error = "URL is space!";
            return m_Error;
        }



        PccMsg myMsg = new PccMsg();

        try
        {
            myMsg.LoadXml(m_LoginInfoXml);
        }
        catch
        {
            m_Error = "Load LoginInfo XML Error!";
            return m_Error;
        }


        bool bOK = false;

        if (myMsg.QueryNodes("Authorize") != null)
        {
            foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
            {
                if (myMsg.Query("APID", myNode) == m_ApID)
                {
                    if (myMsg.QueryNodes("ApMenu", myNode) != null)
                    {
                        foreach (XmlNode myDetailNode in myMsg.QueryNodes("ApMenu", myNode))
                        {
                            if (CheckAspxFile(myMsg.Query("MenuLink", myDetailNode), AspxFileName))
                            {
                                strReturn = myMsg.Query(strAction, myDetailNode);
                                bOK = true;
                                break;
                            }
                        } //end foreach myDeailNode
                    }// end if Menu Nodes.
                }//end if APID
                if (bOK) break;  //if find the Action then exit for loop.
            }//end foreach myNode
        }


        return strReturn;

    }

    public bool IsApAuth()
    {
        if (m_ApID == "")
        {
            m_Error = "ApID is space!";
            return false;
        }

        if (m_LoginInfoXml == "")
        {
            m_Error = "LoginInfoXml is space!";
            return false;
        }

        if (m_Url == "")
        {
            m_Error = "URL is space!";
            return false;
        }


        PccMsg myMsg = new PccMsg();

        try
        {
            myMsg.LoadXml(m_LoginInfoXml);
        }
        catch
        {
            m_Error = "Load LoginInfo XML Error!";
            return false;
        }

        string AspxFileName = GetAspxFile();
        bool bOK = false;

        if (myMsg.QueryNodes("Authorize") != null)
        {
            foreach (XmlNode myNode in myMsg.QueryNodes("Authorize"))
            {
                if (myMsg.Query("APID", myNode) == m_ApID)
                {
                    bOK = true;
                }//end if APID
                if (bOK) break;  //if find the Action then exit for loop.
            }//end foreach myNode
        }

        return bOK;

    }

    private string GetAspxFile()
    {
        string strUrl = m_Url;
        string strAspxFile = "";
        int posSlash = strUrl.LastIndexOf("/") + 1;

        if (m_AspxFile == "")
        {
            strAspxFile = strUrl.Substring(posSlash, strUrl.Length - posSlash);
        }
        else
        {
            //modify by lemor 20050111
            //strAspxFile = m_AspxFile;
            strAspxFile = m_AspxFile.Split('?')[0];
        }
        //strAspxFile = strUrl.Substring(posSlash,strUrl.Length - posSlash);

        return strAspxFile;
    }

    private string GetAspxFile(string strLink, bool bNoAPID)
    {
        string strUrl = strLink;
        string strAspxFile = "";
        int posSlash = strUrl.LastIndexOf("/") + 1;

        strAspxFile = strUrl.Substring(posSlash, strUrl.Length - posSlash);

        if (bNoAPID)
            strAspxFile = strAspxFile.Split('?')[0];

        return strAspxFile;

    }

    private bool CheckAspxFile(string strLink, string strTarget)
    {
        bool bReturn = false;

        if (GetAspxFile(strLink, true) == strTarget || GetAspxFile(strLink, false) == strTarget)
            bReturn = true;

        /*if (strLink.IndexOf(strTarget) > 0)
        {
            bReturn = true;
        }*/

        return bReturn;
    }

    public string GetMenu()
    {
        return GetAuth("MenuNM");
    }

    public bool IsAddAuth()
    {
        if (GetAuthByTable("add_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsUpdateAuth()
    {
        if (GetAuthByTable("upd_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsDeleteAuth()
    {
        if (GetAuthByTable("del_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsReportAuth()
    {
        if (GetAuthByTable("rpt_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsSendAuth()
    {
        if (GetAuthByTable("send_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsShowAuth()
    {
        if (GetAuthByTable("show_mk") == "Y")
            return true;
        else
            return false;
    }

    public bool IsCheckAuth()
    {
        if (GetAuthByTable("check_mk") == "Y")
            return true;
        else
            return false;
    }

    public string GetMenuManage()
    {
        return GetAuth("MenuManage");
    }

}
