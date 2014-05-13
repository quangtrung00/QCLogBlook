using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// PccRow 的摘要描述
/// </summary>
public class PccRow
{
    private TableRow m_Row;
    private string m_Css;
    private int m_CellSpan;
    private int m_RowSpan;
    private int m_Height;
    private HorizontalAlign m_HAlign;
    private VerticalAlign m_VAlign;


    public PccRow()
    {
        //
        // TODO: 在這裡加入建構函式的程式碼
        //
        m_Row = new TableRow();
        m_Css = "";
        m_HAlign = 0;
        m_VAlign = 0;
        m_CellSpan = 0;
        m_RowSpan = 0;
        m_Height = 0;
    }

    public PccRow(string Css, HorizontalAlign HAlign, VerticalAlign VAlign, int CellSpan)
    {
        m_Row = new TableRow();
        m_Css = Css;
        m_HAlign = HAlign;
        m_VAlign = VAlign;
        m_CellSpan = CellSpan;
        m_RowSpan = 0;
        m_Height = 0;
    }

    public PccRow(string Css, HorizontalAlign HAlign, VerticalAlign VAlign, int CellSpan, int RowSpan, int Height)
    {
        m_Row = new TableRow();
        m_Css = Css;
        m_HAlign = HAlign;
        m_VAlign = VAlign;
        m_CellSpan = CellSpan;
        m_RowSpan = RowSpan;
        m_Height = Height;
    }

    public PccRow(string Css)
    {
        m_Row = new TableRow();
        m_Css = Css;
        m_HAlign = 0;
        m_VAlign = 0;
        m_CellSpan = 0;
        m_RowSpan = 0;
        m_Height = 0;
    }

    public TableRow Row
    {
        get
        {
            return m_Row;
        }
        set
        {
            if (m_Row.Cells.Count > 0)
            {
                m_Row.Cells.Clear();
            }
            m_Row = value;
        }
    }

    public void Clear()
    {
        if (m_Row.Cells.Count > 0)
        {
            m_Row.Cells.Clear();
        }
    }

    public void Reset()
    {
        m_Row = new TableRow();
    }

    public void SetDefaultCellData(string Css, HorizontalAlign HAlign, VerticalAlign VAlign, int CellSpan)
    {
        m_Css = Css;
        m_HAlign = HAlign;
        m_VAlign = VAlign;
        m_CellSpan = CellSpan;
    }

    public void SetDefaultCellData(string Css, HorizontalAlign HAlign, VerticalAlign VAlign, int CellSpan, int RowSpan, int Height)
    {
        m_Css = Css;
        m_HAlign = HAlign;
        m_VAlign = VAlign;
        m_CellSpan = CellSpan;
        m_RowSpan = RowSpan;
        m_Height = Height;
    }

    public void AddTextCell(string strText, int iPercent)
    {
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        myCell.Text = strText;
        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);
    }

    public void AddTextCell(string strText)
    {
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        myCell.Text = strText;
        m_Row.Cells.Add(myCell);
    }

    public void AddTextCell(string strText, int iPercent, string toolTip)
    {
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        myCell.Text = strText;
        myCell.Width = Unit.Percentage(iPercent);
        myCell.ToolTip = toolTip;
        m_Row.Cells.Add(myCell);
    }

    public void AddTextCell(string strText, int iPercent, HorizontalAlign HAlign)
    {
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        myCell.HorizontalAlign = HAlign;

        myCell.Text = strText;
        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);
    }


    public void AddControl(System.Web.UI.Control innerControl, int iPercent)
    {
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        myCell.Width = Unit.Percentage(iPercent);
        myCell.Controls.Add(innerControl);
        m_Row.Cells.Add(myCell);
    }

    public void SetRowID(string rowID)
    {
        m_Row.ID = rowID;
    }

    public void SetRowCss(string rowCss)
    {
        if (rowCss != "")
            m_Row.CssClass = rowCss;
    }

    private void CheckDefaultSet(ref TableCell myCell)
    {
        if (m_Css != "")
            myCell.CssClass = m_Css;
        if (m_HAlign != 0)
            myCell.HorizontalAlign = m_HAlign;
        if (m_VAlign != 0)
            myCell.VerticalAlign = m_VAlign;
        if (m_CellSpan != 0)
            myCell.ColumnSpan = m_CellSpan;
        if (m_RowSpan != 0)
            myCell.RowSpan = m_RowSpan;
        if (m_Height != 0)
            myCell.Height = Unit.Pixel(m_Height);
    }

    public bool AddLinkCell(string strXML, int iPercent)
    {
        //解開XML中所傳之參數
        PccCommonForC.PccMsg myMsg;
        try
        {
            myMsg = new PccCommonForC.PccMsg(strXML, "");
        }
        catch
        {
            return false;
        }

        string strToolTip = myMsg.Query("ToolTip");
        string strLinkID = myMsg.Query("LinkID");
        string strImage = myMsg.Query("Image");
        string strClickFun = myMsg.Query("ClickFun");

        //建立HyperLink物件
        HyperLink myHLink = new HyperLink();
        myHLink.Style["cursor"] = "pointer";
        myHLink.ImageUrl = strImage;
        myHLink.ToolTip = strToolTip;
        myHLink.ID = strLinkID;
        myHLink.Attributes.Add("onclick", strClickFun);

        //建立新的Cell物件
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);
        myCell.Controls.Add(myHLink);
        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);

        return true;

    }

    public bool AddLinkHrefCell(string strXML, int iPercent)
    {
        //解開XML中所傳之參數
        PccCommonForC.PccMsg myMsg;
        try
        {
            myMsg = new PccCommonForC.PccMsg(strXML, "");
        }
        catch
        {
            return false;
        }

        string strToolTip = myMsg.Query("ToolTip");
        string strLinkID = myMsg.Query("LinkID");
        string strHref = myMsg.Query("Href");
        string strText = myMsg.Query("Text");

        //建立HyperLink物件
        HyperLink myHLink = new HyperLink();
        myHLink.Style["cursor"] = "pointer";
        myHLink.NavigateUrl = strHref;
        myHLink.ToolTip = strToolTip;
        myHLink.ID = strLinkID;
        myHLink.Text = strText;

        //建立新的Cell物件
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);
        myCell.Controls.Add(myHLink);
        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);

        return true;

    }


    public bool AddMultiLinkCell(string strXML, int iPercent)
    {
        //解開XML中所傳之參數
        PccCommonForC.PccMsg myMsg;
        try
        {
            myMsg = new PccCommonForC.PccMsg(strXML, "");
            XmlNodeList myNodes = myMsg.QueryNodes("LinkButton");
            if (myNodes == null)
            {
                AddTextCell("", iPercent);
                return false;
            }
        }
        catch
        {
            AddTextCell("", iPercent);
            return false;
        }

        HyperLink myHLink;
        //建立新的Cell物件
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        foreach (XmlNode myNode in myMsg.QueryNodes("LinkButton"))
        {
            //建立HyperLink物件
            myHLink = new HyperLink();
            myHLink.Style["cursor"] = "pointer";
            myHLink.NavigateUrl = myMsg.Query("href", myNode) + "&QueryCondition=" + myMsg.Query("QueryCondition", myNode) + "&Method=" + myMsg.Query("Method", myNode);
            myHLink.ImageUrl = myMsg.Query("Image", myNode);
            myHLink.ToolTip = myMsg.Query("ToolTip", myNode);
            myHLink.Attributes.Add("onmouseover", "MouseOver_Click(this)");
            myHLink.Attributes.Add("onmouseout", "MouseOut_Click(this)");
            myCell.Controls.Add(myHLink);

        }

        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);
        return true;

    }

    public bool AddCheckBoxByValueCell(string strXML, int iPercent)
    {
        //解開XML中所傳之參數
        PccCommonForC.PccMsg myMsg;
        try
        {
            myMsg = new PccCommonForC.PccMsg(strXML);
        }
        catch
        {
            return false;
        }

        //建立新的Cell物件
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        if (myMsg.Query("Checked") == "Y")
            myCell.Text = "<input type=checkbox name=" + myMsg.Query("Name") + " value=" + myMsg.Query("Value") + " checked>";
        else
            myCell.Text = "<input type=checkbox name=" + myMsg.Query("Name") + " value=" + myMsg.Query("Value") + " >";

        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);
        return true;
    }

    public bool AddCheckBoxReadOnlyCell(string strXML, int iPercent)
    {
        //解開XML中所傳之參數
        PccCommonForC.PccMsg myMsg;
        try
        {
            myMsg = new PccCommonForC.PccMsg(strXML);
        }
        catch
        {
            return false;
        }

        //建立新的Cell物件
        TableCell myCell = new TableCell();
        CheckDefaultSet(ref myCell);

        if (myMsg.Query("Checked") == "Y")
            myCell.Text = "<input type=checkbox name=" + myMsg.Query("Name") + " value=" + myMsg.Query("Value") + " checked disabled>";
        else
            myCell.Text = "<input type=checkbox name=" + myMsg.Query("Name") + " value=" + myMsg.Query("Value") + " disabled>";

        myCell.Width = Unit.Percentage(iPercent);
        m_Row.Cells.Add(myCell);
        return true;
    }
    public static string GenOuterHtml( Table myTbl)
    {
        if (myTbl == null) return string.Empty;

        System.IO.StringWriter mySw = new System.IO.StringWriter();
        HtmlTextWriter myHw = new HtmlTextWriter(mySw);
        myTbl.RenderControl(myHw);
        return mySw.ToString();
    }
}

