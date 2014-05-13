using System;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Web;
using System.Configuration;

	/// <summary>
	/// 列印的模式
	/// </summary>
	//Create on 2005/08/23
	public enum  PageOrientation 
	{
		/// <summary>
		///  列印的模式為縱向
		/// </summary>
		Vertical =  1,
		/// <summary>
		///   列印的模式為橫向
		/// </summary>
		Horizontal = 2
	}
	
	/// <summary>
	/// ExcelHelper 提供基本的產生Excel的VB Script的程式。
	/// </summary>
	/// <remarks>日期：20070211</remarks> 
public class ExcelHelper
{
    /// <summary>
    /// 字串內容物件：Content
    /// </summary>
    private StringBuilder Content;
    /// <summary>
    /// 字串寫入物件：m_Sw
    /// </summary>
    private StringWriter m_Sw;

    /// <summary>
    /// 建構式，先建立要存放VB Script的內容物件(Content)及字串寫入物件(m_Sw)。
    /// </summary>
    public ExcelHelper()
    {
        Content = new StringBuilder();
        m_Sw = new StringWriter(Content);
    }

    /// <summary>
    /// 建構式，帶入內容物件及字串寫入物件參數
    /// </summary>
    /// <param name="content">內容物件</param>
    /// <param name="sw">字串寫入物件</param>
    public ExcelHelper(StringBuilder content, StringWriter sw)
    {
        Content = content;
        m_Sw = sw;
    }

    /// <summary>
    /// 開始一個vbsScript的宣告
    /// </summary>
    public void BeginScript()
    {
        m_Sw.WriteLine("<script language=\"vbscript\">");
    }

    /// <summary>
    /// 開始一個子程序的宣告
    /// </summary>
    /// <param name="subName">子程序的名稱</param>
    public void BeingSub(string subName)
    {
        m_Sw.WriteLine("Sub  {0}()", subName);
    }

    /// <summary>
    /// 建立一個OLE的物件
    /// </summary>
    /// <param name="objN">物件的名稱</param>
    /// <param name="oleObject">物件的ProgID</param>
    public void DeclareOleObject(string objN, string oleObject)
    {
        m_Sw.WriteLine("Set {0} = CreateObject(\"{1}\")", objN, oleObject);
    }

    /// <summary>
    /// 建立一個Excel應用程式的物件
    /// </summary>
    /// <param name="objN">物件名稱</param>
    public void DeclareExcelApplication(string objN)
    {
        m_Sw.WriteLine("Set {0} = CreateObject(\"EXCEL.APPLICATION\")", objN);
    }

    /// <summary>
    /// 設定Excel應用程式的可視性
    /// </summary>
    /// <param name="appN">Excel應用程式名稱</param>
    /// <param name="isVisi">是否可視，為Bool值</param>
    public void AppVisible(string appN, bool isVisi)
    {
        m_Sw.WriteLine("{0}.Visible = {1}", appN, isVisi);
    }

    /// <summary>
    /// 致能要使用的視窗
    /// </summary>
    /// <param name="appN">Excel應用程式名稱</param>
    /// <param name="fileName">檔案名稱</param>
    public void AppWindowActivate(string appN, string fileName)
    {
        m_Sw.WriteLine("{0}.Windows(\"{1}\").Activate", appN, fileName);
    }

    /// <summary>
    /// 關閉一個正在使用中的視窗
    /// </summary>
    /// <param name="appN">Excel應用程式名稱</param>
    /// <param name="windowN">視窗名稱即檔案名稱</param>
    /// <param name="isSave">是否存檔</param>
    public void AppCloseWindow(string appN, string windowN, bool isSave)
    {
        m_Sw.WriteLine(" {0}.Windows(\"{1}\").Close ({2})", appN, windowN, isSave);
    }

    /// <summary>
    /// 選擇當前的Sheet中的所有表格
    /// </summary>
    /// <param name="appN">Excel應用程式名稱</param>
    public void AppSelectCurrentSheet(string appN)
    {
        m_Sw.WriteLine("{0}.Cells.Select", appN);
    }


    /// <summary>
    /// 致能Excel檔中的某一個Sheet
    /// </summary>
    /// <param name="appN">Excel應用程式名稱</param>
    /// <param name="workbookN">Workbook的名稱</param>
    /// <param name="sheetN">Sheet的名稱</param>
    public void AppSheetActivate(string appN, string workbookN, string sheetN)
    {
        m_Sw.WriteLine("{0}.Workbooks(\"{1}\").Sheets(\"{2}\").Activate", appN, workbookN, sheetN);
    }

    /// <summary>
    /// 當有錯誤時恢復的指令
    /// </summary>
    public void ErrResumeNext()
    {
        m_Sw.WriteLine("On Error Resume Next");
    }

    /// <summary>
    /// 致能特定Book的某一個Sheet
    /// </summary>
    /// <param name="workbookObj">BookName</param>
    /// <param name="sheetN">SheetName</param>
    public void BookSheetActivate(string workbookObj, string sheetN)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Activate", workbookObj, sheetN);
    }

    /// <summary>
    /// Copy某一個範圍的資料到另一個目標
    /// </summary>
    /// <param name="appN">Excel應用程式物件</param>
    /// <param name="srcWorkbookN">來源的BookName</param>
    /// <param name="srcSheetN">來源的SheetName</param>
    /// <param name="srcRangeN">來源的範圍名稱或起始儲存格</param>
    /// <param name="targetWorkbookN">目標的BookName</param>
    /// <param name="targetSheetN">目標的SheetName</param>
    /// <param name="targetRangeN">目標的範圍名稱或起始儲存格</param>
    public void AppRangeCopy(string appN, string srcWorkbookN, string srcSheetN, string srcRangeN,
        string targetWorkbookN, string targetSheetN, string targetRangeN)
    {
        m_Sw.Write("{0}.Workbooks(\"{1}\").Sheets(\"{2}\").Range(\"{3}\").Copy (",
            appN, srcWorkbookN, srcSheetN, srcRangeN);
        m_Sw.WriteLine(" {0}.Workbooks(\"{1}\").Sheets(\"{2}\").Range(\"{3}\") )",
            appN, targetWorkbookN, targetSheetN, targetRangeN);
    }


    /// <summary>
    ///  Copy一個Book中的某一個Sheet的Rang
    /// </summary>
    /// <param name="srcWorkbookObj">來源Book物件</param>
    /// <param name="srcSheetN">來源的SheetName</param>
    /// <param name="srcRangeN">來源的範圍名稱或起始儲存格</param>
    /// <param name="targetWorkbookObj">目標Book物件</param>
    /// <param name="targetSheetN">目標的SheetName</param>
    /// <param name="targetRangeN">目標的範圍名稱或起始儲存格</param>
    public void AppRangeCopy(string srcWorkbookObj, string srcSheetN, string srcRangeN, string targetWorkbookObj, string targetSheetN, string targetRangeN)
    {
        m_Sw.Write("{0}.Sheets(\"{1}\").Range(\"{2}\").Copy (", srcWorkbookObj, srcSheetN, srcRangeN);
        m_Sw.WriteLine(" {0}.Sheets(\"{1}\").Range(\"{2}\") )", targetWorkbookObj, targetSheetN, targetRangeN);
    }

    /// <summary>
    /// 新增一個工作薄
    /// </summary>
    /// <param name="appObj">要新增的Excel	Application 對象</param>
    /// <param name="workbookObj">新增的工作薄對象</param>
    //Create on 2005/08/23 
    public void AppAddWorkbook(string appObj, string workbookObj)
    {
        m_Sw.WriteLine("Set {0} =  {1}.Workbooks.Add", workbookObj, appObj);
    }

    /// <summary>
    /// 取得指定工作薄的名稱
    /// </summary>
    /// <param name="workbookObj">工作薄對象</param>
    /// <returns>工作薄名稱屬性</returns>
    //Create on 2005/08/23
    public string WorkbookName(string workbookObj)
    {
        return string.Format("{0}.Name", workbookObj);
    }
    /// <summary>
    /// 選中一個指定的區域
    /// </summary>
    /// <param name="appObj">一個指定的Excel對象</param>
    /// <param name="range">Range名稱</param>
    /// <example>Application.Range("A1:K21").Select</example>
    //Create on 2005/08/23
    public void AppSelectRange(string appObj, string range)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").Select", appObj, range);
    }

    /// <summary>
    /// 工作表的整頁拷貝
    /// </summary>
    /// <param name="srcSheetObj">源工作表對象</param>
    /// <param name="targetWorkbookObj">目標工作表對象名字</param>
    /// <param name="targetSheetN">目標活頁簿對象名字</param>
    //Create on 2005/08/23
    public void AppSheetCopy(string srcSheetObj, string srcWindowN, string targetWorkbookObj, string targetSheetN)
    {
        m_Sw.WriteLine("{0}.Application.Windows(\"{1}\").Activate", srcSheetObj, srcWindowN);
        m_Sw.WriteLine("{0}.Cells.Select", srcSheetObj);
        m_Sw.WriteLine("{0}.Application.Selection.Copy", srcSheetObj);
        this.BookSheetActivate(targetWorkbookObj, targetSheetN);
        m_Sw.WriteLine("{0}.Application.ActiveSheet.Paste", srcSheetObj);
    }





    /// <summary>
    /// 開啟 Excel活頁簿
    /// </summary>
    /// <param name="workbookObj">Excel Workbooks對象</param>
    /// <param name="appObj">Excel Application對象</param>
    /// <param name="filePath">Excel 文件的路徑</param>
    /// <example>Set xlBook = xlApp.Workbooks.open("c:/WebsitePolling/Report/WebMCList.xls")</example>
    //Create on 2005/08/23 
    public void AppOpenBook(string workbookObj, string appObj, string filePath)
    {
        m_Sw.WriteLine("Set {0} = {1}.Workbooks.open(\"{2}\")", workbookObj, appObj, filePath);
    }


    /// <summary>
    /// 如果在宏執行時 Excel 顯示特定的警告和訊息為true ,否則為false
    /// </summary>
    /// <param name="appObj">Excel Application對象</param>
    /// <param name="isAlert">宏執行時 Excel 顯示特定的警告和訊息為true ,否則為false</param>
    //Create on 2005/08/23 
    public void AppDisplayAlerts(string appObj, bool isAlert)
    {
        m_Sw.WriteLine("{0}.DisplayAlerts={1}", appObj, isAlert);
    }

    /// <summary>
    ///   將指定的工作表設為使用中的工作表
    /// </summary>
    /// <param name="appObj">Excel Application對象</param>
    /// <param name="sheetN">將指定的工作表的名字</param>
    //Create on 2005/08/23
    public void AppSetActivateSheet(string appObj, string sheetN)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Activate", appObj, sheetN);
    }

    /// <summary>
    /// 設定一個工作表對象為Workbooks集合中正在使用的工作表
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    /// <param name="workbookObj">Workbooks集合</param>
    //Create on 2005/08/23
    public void BookGetActiveSheet(string sheetObj, string workbookObj)
    {
        m_Sw.WriteLine("Set {0} = {1}.ActiveSheet", sheetObj, workbookObj);
    }

    /// <summary>
    /// 設定一個變量為指定的工作表對象
    /// </summary>
    /// <param name="sheetObj">設定的一個變量</param>
    /// <param name="workbookObj">要指定的工作表對象所屬的工作薄對象</param>
    /// <param name="sheetN">工作表對象的名稱</param>
    //Create on 2005/08/23 
    public void BookSetSheet(string sheetObj, string workbookObj, string sheetN)
    {
        m_Sw.WriteLine("Set {0} = {1}.Sheets(\"{2}\")", sheetObj, workbookObj, sheetN);
    }
    /// <summary>
    /// 為指定WorkBook新增一個sheet
    /// </summary>
    /// <param name="workbookName">要指定的工作表對象所屬的工作薄對象</param>
    /// <param name="sheetName">工作表對象的名稱</param>
    public void AddSheet(string workbookName, string sheetName)
    {
        m_Sw.WriteLine("{0}.Sheets.Add", workbookName);
        m_Sw.WriteLine("{0}.ActiveSheet.Name = \"{1}\"", workbookName, sheetName);
        //			m_Sw.WriteLine("Sheets(\"{0}\").Move After:=Sheets(Sheets.Count)",sheetName);
    }

    /// <summary>
    /// 在指定的WorkBook刪除指定sheet
    /// </summary>
    /// <param name="workbookName">要指定的工作表對象所屬的工作薄對象</param>
    /// <param name="sheetName">工作表對象的名稱</param>
    public void DeleteSheet(string workbookName, string sheetName)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Delete", workbookName, sheetName);
    }

    /// <summary>
    /// 在當前WorkBook把指定的sheet改名
    /// </summary>
    /// <param name="oldName">原名</param>
    /// <param name="newName">新名</param>
    public void RenameSheet(string workbookName, string oldName, string newName)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").name=\"{2}\"", workbookName, oldName, newName);
    }

    /// <summary>
    /// 設定行高
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="row1">起始行</param>
    /// <param name="row2">結束行</param>
    /// <param name="rowWidth">行高</param>
    public void SetRowHeight(string sheetObj, int row1, int row2, double rowWidth)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{2}\").RowHeight={3}", sheetObj, row1, row2, rowWidth);
    }
    /// <summary>
    /// 設定列寬
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="cell1">起始列</param>
    /// <param name="cell2">結束列</param>
    /// <param name="cellHeight">列寬</param>
    public void SetCellWidth(string sheetObj, string cell1, string cell2, double cellHeight)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{2}\").ColumnWidth={3}", sheetObj, cell1, cell2, cellHeight);
    }
    /// <summary>
    /// 設定cell對齊方式
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="cell">cell座標</param>
    /// <param name="align">對齊方式</param>
    public void SetCellHAlign(string sheetObj, string cell, string align)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").HorizontalAlignment ={2}", sheetObj, cell, align);
    }
    /// <summary>
    /// 在指定sheet指定列之前插入一列
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="column">列號</param>
    public void InsertColumn(string sheetObj, string column)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{1}\").Insert", sheetObj, column);
    }
    /// <summary>
    /// 在指定sheet刪除指定列
    /// </summary>
    /// <param name="sheetObj"></param>
    /// <param name="column"></param>
    public void DeleteColumn(string sheetObj, string column)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{1}\").Delete", sheetObj, column);
    }
    /// <summary>
    /// 在指定sheet指定行之前插入一行
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="row">行號</param>
    public void InsertRow(string sheetObj, int row)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{1}\").Insert", sheetObj, row);
    }
    /// <summary>
    /// 在指定sheet刪除指定行
    /// </summary>
    /// <param name="sheetObj">sheet對象</param>
    /// <param name="row">行號</param>
    public void DeleteRow(string sheetObj, int row)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{1}\").Delete", sheetObj, row);
    }
    /// <summary>
    /// 設定列印的模式為縱向或橫向
    /// </summary>
    /// <param name="sheetObj">列印的工作表對象</param>
    /// <param name="orientation">列印的模式</param>
    //Create on 2005/08/23 
    public void SetSheetOrientation(string sheetObj, PageOrientation orientation)
    {
        m_Sw.WriteLine("{0}.PageSetup.Orientation = {1}", sheetObj, (int)orientation);
    }

    /// <summary>
    /// 設定工作對象的Range的值
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    /// <param name="range">Range名稱</param>
    /// <param name="val">要設定的值</param>
    //Create on 2005/08/23
    public void SheetSetRange(string sheetObj, string range, string val)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\")=\"{2}\"", sheetObj, range, val);
    }

    public void SheetSetRange(string sheetObj, string range, decimal val)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\")= {2} ", sheetObj, range, val);
    }


    /// <summary>
    /// 設定工作表對象的一行單元格的行高
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    /// <param name="row">要設定的行索引</param>
    /// <param name="rowHeight">行高</param>
    //Create on 2005/08/23
    public void SheetSetRowHeight(string sheetObj, int row, double rowHeight)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{2}\").rowHeight = {3}", sheetObj, row, row, rowHeight);
    }

    /// <summary>
    /// 清除工作對象的Range的值﹐但保留其格式設定
    /// </summary>
    /// <param name="sheetObj">要清除的工作表對象</param>
    /// <param name="range">Range名稱</param>
    //Create on 2005/08/23
    public void SheetClearRange(string sheetObj, string range)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").ClearContents", sheetObj, range);
    }

    /// <summary>
    /// 設定工作表中特定單元格的值 
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    /// <param name="posX">列號</param>
    /// <param name="posY">行號</param>
    /// <param name="val">要設定的值</param>
    //Create on 2005/08/23
    public void SheetSetCells(string sheetObj, int posX, int posY, string val)
    {
        m_Sw.WriteLine("{0}.Cells({1},{2}) = {3}", sheetObj, posX, posY, val);
    }

    public void SheetPageSetup(string sheetObj, double leftMargin, double rightMargin, double topMargin, double bottomMargin, double headerMargin, double footerMargin)
    {
        m_Sw.WriteLine("With {0}.PageSetup", sheetObj);
        m_Sw.WriteLine(".LeftMargin = {0}", leftMargin);
        m_Sw.WriteLine(".RightMargin = {0}", rightMargin);
        m_Sw.WriteLine(".TopMargin = {0}", topMargin);
        m_Sw.WriteLine(".BottomMargin = {0}", bottomMargin);
        m_Sw.WriteLine(".HeaderMargin = {0}", headerMargin);
        m_Sw.WriteLine(".FooterMargin = {0}", footerMargin);
        m_Sw.WriteLine("End With");
    }

    public void setMergeCells(string sheetObj, string range)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").MergeCells = true", sheetObj, range);
    }

    /// <summary>
    /// 印列特定的工作表
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    //Create on 2005/08/23 
    public void SheetPrintPreview(string sheetObj)
    {
        m_Sw.WriteLine("{0}.PrintPreview", sheetObj);
        System.Diagnostics.Debug.WriteLine(string.Format("{0}.PrintPreview", sheetObj));
    }

    /// <summary>
    /// 關閉Excel應用程式
    /// </summary>
    /// <param name="appObj">Excel Application對象</param>
    //Create on 2005/08/23 
    public void AppQuit(string appObj)
    {
        m_Sw.WriteLine("{0}.Quit() ", appObj);
    }



    /// <summary>
    /// 設定打印機的頁邊界屬性
    /// </summary>
    /// <param name="sheetObj">工作表對象</param>
    /// <param name="margin">邊界名稱</param>
    /// <param name="appObj">Excel Application對象</param>
    /// <param name="inches">邊距（以英寸為單位）</param>
    //Create on 2005/08/23
    public void SetMarginInfo(string sheetObj, string margin, string appObj, decimal inches)
    {
        m_Sw.WriteLine("{0}.PageSetup.{1} = {2}.InchesToPoints({3})", sheetObj, margin, appObj, inches);

    }
    /// <summary>
    /// 結束一個方法
    /// </summary>
    //Create on 2005/08/23
    public void EndSub()
    {
        m_Sw.WriteLine("end sub");
    }

    /// <summary>
    /// 結束一個腳本塊
    /// </summary>
    //Create on 2005/08/23 
    public void EndScript()
    {
        m_Sw.WriteLine("</script>");
    }

    /// <summary>
    /// 調用一個方法
    /// </summary>
    /// <param name="methodName">方法名稱</param>
    //Create on 2005/08/23 
    public void CallMethod(string methodName)
    {
        m_Sw.WriteLine(methodName);
    }

    /// <summary>
    /// 產生腳本的內容
    /// </summary>
    //Create on 2005/08/23 
    public string ScriptContent
    {
        get
        {
            m_Sw.Flush();
            m_Sw.Close();
            return Content.ToString();
        }
    }

}

