using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using PccCommonForC;
using System.Web.UI.WebControls;

/// <summary>
/// PccDetailTable 的摘要描述
/// </summary>
public class PccDetailTable
{
    private Table m_Table;
    private int m_index;
    private string m_TableName;

    DataTable m_myDTableData;

    private string[] m_Fields, m_FieldsItem, m_FieldsType, m_TransFunc = { };
    private int[] m_FieldsPercent;
    DataRow[] m_myRows;

    private string m_SqlStr;
    private string m_connectionXML, m_connectionType, m_connectionServer, m_connectionDB, m_connectionUser, m_connectionPwd, m_connectionString;

    private string m_ClassXML = "";
    private string m_TableClass = "";
    private string m_RowHeaderClass = "";
    private string m_RowClass1 = "";
    private string m_RowClass2 = "";

    private object m_objThis;

    #region "Constructor"

    public PccDetailTable()
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        m_Table = new Table();

        m_SqlStr = "";

        m_connectionXML = "";
        m_connectionType = "";
        m_connectionServer = "";
        m_connectionDB = "";
        m_connectionUser = "";
        m_connectionPwd = "";
        m_connectionString = "";

        m_index = 0;
        m_TableName = "";

        m_objThis = null;
    }

    public PccDetailTable(string strName)
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        m_Table = new Table();

        m_SqlStr = "";

        m_connectionXML = "";
        m_connectionType = "";
        m_connectionServer = "";
        m_connectionDB = "";
        m_connectionUser = "";
        m_connectionPwd = "";
        m_connectionString = "";

        m_index = 0;
        m_TableName = strName;
        m_Table.ID = strName;

        m_objThis = null;
    }

    #endregion

    #region "各個屬性之建立"

    public object objThis
    {
        set
        {
            m_objThis = value;
        }
    }

    public Table NewTable
    {
        get
        {
            return m_Table;
        }
    }

    public DataTable NewDataTable
    {
        get
        {
            return m_myDTableData;
        }
        set
        {
            m_myDTableData = value;
        }
    }

    public DataRow[] NewDataRows
    {
        get
        {
            return m_myRows;
        }
        set
        {
            m_myRows = value;
        }
    }

    public string SqlStr
    {
        get
        {
            return m_SqlStr;
        }
        set
        {
            m_SqlStr = value;
        }
    }

    public string[] Fields
    {
        get
        {
            return m_Fields;
        }
        set
        {
            m_Fields = value;
            m_index = m_Fields.Length - 1;
        }
    }

    public string[] FieldsItem
    {
        get
        {
            return m_FieldsItem;
        }
        set
        {
            m_FieldsItem = value;
        }
    }

    public string[] FieldsType
    {
        get
        {
            return m_FieldsType;
        }
        set
        {
            m_FieldsType = value;
        }
    }

    public int[] FieldsPercent
    {
        get
        {
            return m_FieldsPercent;
        }
        set
        {
            m_FieldsPercent = value;
        }
    }

    public string[] TransFunc
    {
        get
        {
            return m_TransFunc;
        }
        set
        {
            m_TransFunc = value;
        }
    }

    public string ConnectionXML
    {
        get
        {
            return m_connectionXML;
        }
        set
        {
            try
            {

                PccMsg myMsg = new PccMsg(value);
                m_connectionType = myMsg.Query("ConnectionType");
                m_connectionServer = myMsg.Query("ConnectionServer");
                m_connectionDB = myMsg.Query("ConnectionDB");
                m_connectionUser = myMsg.Query("ConnectionUser");
                m_connectionPwd = myMsg.Query("ConnectionPwd");
                m_connectionString = "Server=" + m_connectionServer + ";Database=" + m_connectionDB + ";Uid=" + m_connectionUser + ";Pwd=" + m_connectionPwd + ";";

                m_connectionXML = value;
            }
            catch
            {
                m_connectionString = "Server=LemorYen;Database=PccGlobal;Uid=globaluser;Pwd=123;";
            }
        }

    }

    public string ClassXML
    {
        get
        {
            return m_ClassXML;
        }
        set
        {
            try
            {

                PccMsg myMsg = new PccMsg(value);
                m_TableClass = myMsg.Query("TableClass");
                m_RowHeaderClass = myMsg.Query("RowHeaderClass");
                m_RowClass1 = myMsg.Query("RowClass1");
                m_RowClass2 = myMsg.Query("RowClass2");

                m_ClassXML = value;
            }
            catch
            {
                m_ClassXML = "";
            }

        }
    }

    #endregion

    #region "建立Detail Table"

    public bool Create()
    {
        bool bReturn = false;

        if (m_Table == null) return bReturn;

        GenDetailTableHeader();

        int i, Start_SC = 1;
        string Style_SC;
        PccRow myRow;
        PccMsg myMsg;
        PccMsg myTempMsg;

        GetMenuAuth myAuth = new GetMenuAuth();

        if (m_myDTableData != null)
        {
            foreach (DataRow myDRow in m_myDTableData.Rows)
            {
                if (Start_SC % 2 == 0) Style_SC = m_RowClass1; else Style_SC = m_RowClass2;

                myRow = new PccRow("", HorizontalAlign.Center, VerticalAlign.Middle, 0);
                myRow.SetRowCss(Style_SC);

                string strData = "";

                for (i = 0; i < m_FieldsItem.Length; i++)
                {
                    if (m_FieldsItem[i] != "--NO--")
                    {
                        myMsg = new PccMsg(m_FieldsType[i]);

                        if (myMsg.Query("Type") != "Space")
                        {
                            if (Convert.IsDBNull(myDRow[m_FieldsItem[i]]))
                            {
                                strData = "";
                            }
                            else
                            {
                                if (m_TransFunc.Length != 0)
                                {
                                    try
                                    {
                                        strData = CallTrnasFunc(m_objThis, m_TransFunc[i], myDRow[m_FieldsItem[i]].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        strData = myDRow[m_FieldsItem[i]].ToString() + ex.Message;
                                    }

                                }
                                else
                                {
                                    strData = myDRow[m_FieldsItem[i]].ToString();
                                }
                            }
                        }


                        switch (myMsg.Query("Type"))
                        {
                            case "Text":
                                //myRow.AddTextCell(myDRow[m_FieldsItem[i]].ToString(),m_FieldsPercent[i]);  
                                //myRow.AddTextCell(strData,m_FieldsPercent[i]);  
                                if (myMsg.Query("HAlign").Equals(string.Empty))
                                    myRow.AddTextCell(strData, m_FieldsPercent[i]);
                                else
                                {
                                    switch (myMsg.Query("HAlign"))
                                    {
                                        case "Center":
                                            myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Center);
                                            break;
                                        case "Right":
                                            if ((myMsg.Query("Round").Equals(string.Empty)) || (strData.Equals(string.Empty)))
                                            {
                                                myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Right);
                                            }
                                            else
                                            {
                                                int round = int.Parse(myMsg.Query("Round"));
                                                decimal temp = decimal.Parse(strData);
                                                temp = Math.Round(temp, round);
                                                myRow.AddTextCell(temp.ToString(), m_FieldsPercent[i], HorizontalAlign.Right);

                                            }
                                            break;
                                        case "Left":
                                            myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Left);
                                            break;
                                    }
                                }
                                break;
                            case "ImageButton":
                                break;
                            case "HyperLink":
                                break;
                            case "LinkButton":
                                break;
                            case "CheckBox":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxReadOnly":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxReadOnlyCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxByValue":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxByValueJudgeUserRight":
                                myTempMsg = new PccMsg();
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myAuth.Url = myDRow["menu_link"].ToString();
                                if (myAuth.GetAuth(m_FieldsItem[i]) == "Y")
                                    myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                else
                                    myRow.AddCheckBoxReadOnlyCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);

                                break;
                            case "MultiLink":
                                myRow.AddMultiLinkCell(GenMultiLinkXML(myDRow, m_FieldsType[i]), m_FieldsPercent[i]);
                                break;
                            case "Space":
                                myRow.AddTextCell("", m_FieldsPercent[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        myRow.AddTextCell(Start_SC.ToString(), m_FieldsPercent[i]);
                    }
                }


                m_Table.Controls.Add(myRow.Row);

                Start_SC += 1;
            }
        }
        else
        {
            foreach (DataRow myDRow in m_myRows)
            {
                if (Start_SC % 2 == 0) Style_SC = m_RowClass1; else Style_SC = m_RowClass2;

                myRow = new PccRow("", HorizontalAlign.Center, VerticalAlign.Middle, 0);
                myRow.SetRowCss(Style_SC);

                string strData = "";

                for (i = 0; i < m_FieldsItem.Length; i++)
                {
                    if (m_FieldsItem[i] != "--NO--")
                    {
                        myMsg = new PccMsg(m_FieldsType[i]);

                        if (myMsg.Query("Type") != "Space")
                        {
                            if (Convert.IsDBNull(myDRow[m_FieldsItem[i]]))
                            {
                                strData = "";
                            }
                            else
                            {
                                if (m_TransFunc.Length != 0)
                                {
                                    try
                                    {
                                        strData = CallTrnasFunc(m_objThis, m_TransFunc[i], myDRow[m_FieldsItem[i]].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        strData = myDRow[m_FieldsItem[i]].ToString() + ex.Message;
                                    }

                                }
                                else
                                {
                                    strData = myDRow[m_FieldsItem[i]].ToString();
                                }
                            }
                        }


                        switch (myMsg.Query("Type"))
                        {
                            case "Text":
                                //myRow.AddTextCell(myDRow[m_FieldsItem[i]].ToString(),m_FieldsPercent[i]);  
                                if (myMsg.Query("HAlign").Equals(string.Empty))
                                    myRow.AddTextCell(strData, m_FieldsPercent[i]);
                                else
                                {
                                    switch (myMsg.Query("HAlign"))
                                    {
                                        case "Center":
                                            myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Center);
                                            break;
                                        case "Right":
                                            if ((myMsg.Query("Round").Equals(string.Empty)) || (strData.Equals(string.Empty)))
                                            {
                                                myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Right);
                                            }
                                            else
                                            {
                                                int round = int.Parse(myMsg.Query("Round"));
                                                decimal temp = decimal.Parse(strData);
                                                temp = Math.Round(temp, round);
                                                myRow.AddTextCell(temp.ToString(), m_FieldsPercent[i], HorizontalAlign.Right);

                                            }
                                            break;
                                        case "Left":
                                            myRow.AddTextCell(strData, m_FieldsPercent[i], HorizontalAlign.Left);
                                            break;
                                    }
                                }

                                break;
                            case "ImageButton":
                                break;
                            case "HyperLink":
                                break;
                            case "LinkButton":
                                break;
                            case "CheckBox":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxReadOnly":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxReadOnlyCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxByValue":
                                myTempMsg = new PccMsg();
                                //myTempMsg.CreateFirstNode("Checked",myDRow[m_FieldsItem[i]].ToString());
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                break;
                            case "CheckBoxByValueJudgeUserRight":
                                myTempMsg = new PccMsg();
                                myTempMsg.CreateFirstNode("Checked", strData);
                                myTempMsg.CreateFirstNode("Name", m_FieldsItem[i] + m_TableName);
                                myTempMsg.CreateFirstNode("Value", myDRow[myMsg.Query("Value")].ToString());
                                myAuth.Url = myDRow["menu_link"].ToString();
                                if (myAuth.GetAuth(m_FieldsItem[i]) == "Y")
                                    myRow.AddCheckBoxByValueCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);
                                else
                                    myRow.AddCheckBoxReadOnlyCell(myTempMsg.GetXmlStr, m_FieldsPercent[i]);

                                break;
                            case "MultiLink":
                                myRow.AddMultiLinkCell(GenMultiLinkXML(myDRow, m_FieldsType[i]), m_FieldsPercent[i]);
                                break;
                            case "Space":
                                myRow.AddTextCell("", m_FieldsPercent[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        myRow.AddTextCell(Start_SC.ToString(), m_FieldsPercent[i]);
                    }
                }


                m_Table.Controls.Add(myRow.Row);

                Start_SC += 1;
            }
        }

        bReturn = true;
        return bReturn;


    }


    private string GenMultiLinkXML(DataRow myDRow, string strXML)
    {
        PccMsg myMsg = new PccMsg(strXML);
        PccMsg myTempMsg = new PccMsg();

        foreach (XmlNode myNode in myMsg.QueryNodes("LinkButton"))
        {
            myTempMsg.CreateNode("LinkButton");
            myTempMsg.AddToNode("Image", myMsg.Query("Image", myNode));
            myTempMsg.AddToNode("ToolTip", myMsg.Query("ToolTip", myNode));
            myTempMsg.AddToNode("href", myMsg.Query("href", myNode));
            myTempMsg.AddToNode("QueryCondition", myMsg.Query("QueryCondition", myNode));
            myTempMsg.AddToNode("Method", GetMethod(myMsg.Query("Method/MethodName", myNode), myMsg.Query("Method/Key", myNode), myMsg.Query("Method/KeyOther", myNode), myDRow));
            myTempMsg.UpdateNode();
        }

        return myTempMsg.GetXmlStr;

    }

    private string GetMethod(string strMethod, string Key, string KeyOther, DataRow myRow)
    {
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Method", strMethod);
        myMsg.CreateFirstNode("Key", myRow[Key].ToString());
        myMsg.CreateFirstNode("KeyOther", myRow[KeyOther].ToString());
        return myMsg.GetXmlStr;
    }


    private void GenDetailTableHeader()
    {
        m_Table.Width = Unit.Percentage(100);
        m_Table.CssClass = m_TableClass;
        m_Table.CellSpacing = 1;
        m_Table.CellPadding = 0;

        int i;
        string[] SplitArray;
        string strTemp = "";

        PccRow myRow = new PccRow(m_RowHeaderClass, HorizontalAlign.Center, VerticalAlign.Middle, 0);

        for (i = 0; i < m_Fields.Length; i++)
        {
            //SplitArray = m_Fields[i].Split("--".ToCharArray());
            SplitArray = m_Fields[i].Split(new char[] { '-' });
            if (SplitArray.Length == 3)
            {
                switch (SplitArray[0])
                {
                    case "btn":
                        strTemp = "<input type=button class=button id='btn-" + m_TableName + "' value='" + SplitArray[1] + "' onClick='" + SplitArray[2] + "'>";
                        break;
                    case "link":
                        strTemp = "<A id='link-" + m_TableName + "' href='" + SplitArray[2] + "' >" + SplitArray[1] + "</A>";
                        break;
                    default:
                        strTemp = m_Fields[i];
                        break;
                }
            }
            else
            {
                strTemp = m_Fields[i];
            }

            myRow.AddTextCell(strTemp, m_FieldsPercent[i]);
        }

        m_Table.Controls.Add(myRow.Row);

    }

    public string CallTrnasFunc(object objThis, string method, string orgData)
    {
        if (method == "") return orgData;


        System.Reflection.MethodInfo[] AllMethod;
        AllMethod = objThis.GetType().GetMethods();
        object[] parameters = { orgData };

        object objReturn = null;

        foreach (System.Reflection.MethodInfo myMethod in AllMethod)
        {
            if (myMethod.Name == method)
            {
                objReturn = myMethod.Invoke(objThis, parameters);
            }
        }

        if (objReturn != null)
            return (string)objReturn;
        else
            return orgData;

    }

    #endregion

    #region "取得其他資料"

    public string[] GetDataArray(string ItemName)
    {
        string[] strReturn;
        strReturn = new string[m_myDTableData.Rows.Count];
        int i;
        for (i = 0; i < m_myDTableData.Rows.Count; i++)
        {
            strReturn[i] = m_myDTableData.Rows[i][ItemName].ToString();
        }

        return strReturn;
    }

    #endregion

}
