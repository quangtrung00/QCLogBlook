using System;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Web;
using System.Configuration;

	/// <summary>
	/// �C�L���Ҧ�
	/// </summary>
	//Create on 2005/08/23
	public enum  PageOrientation 
	{
		/// <summary>
		///  �C�L���Ҧ����a�V
		/// </summary>
		Vertical =  1,
		/// <summary>
		///   �C�L���Ҧ�����V
		/// </summary>
		Horizontal = 2
	}
	
	/// <summary>
	/// ExcelHelper ���Ѱ򥻪�����Excel��VB Script���{���C
	/// </summary>
	/// <remarks>����G20070211</remarks> 
public class ExcelHelper
{
    /// <summary>
    /// �r�ꤺ�e����GContent
    /// </summary>
    private StringBuilder Content;
    /// <summary>
    /// �r��g�J����Gm_Sw
    /// </summary>
    private StringWriter m_Sw;

    /// <summary>
    /// �غc���A���إ߭n�s��VB Script�����e����(Content)�Φr��g�J����(m_Sw)�C
    /// </summary>
    public ExcelHelper()
    {
        Content = new StringBuilder();
        m_Sw = new StringWriter(Content);
    }

    /// <summary>
    /// �غc���A�a�J���e����Φr��g�J����Ѽ�
    /// </summary>
    /// <param name="content">���e����</param>
    /// <param name="sw">�r��g�J����</param>
    public ExcelHelper(StringBuilder content, StringWriter sw)
    {
        Content = content;
        m_Sw = sw;
    }

    /// <summary>
    /// �}�l�@��vbsScript���ŧi
    /// </summary>
    public void BeginScript()
    {
        m_Sw.WriteLine("<script language=\"vbscript\">");
    }

    /// <summary>
    /// �}�l�@�Ӥl�{�Ǫ��ŧi
    /// </summary>
    /// <param name="subName">�l�{�Ǫ��W��</param>
    public void BeingSub(string subName)
    {
        m_Sw.WriteLine("Sub  {0}()", subName);
    }

    /// <summary>
    /// �إߤ@��OLE������
    /// </summary>
    /// <param name="objN">���󪺦W��</param>
    /// <param name="oleObject">����ProgID</param>
    public void DeclareOleObject(string objN, string oleObject)
    {
        m_Sw.WriteLine("Set {0} = CreateObject(\"{1}\")", objN, oleObject);
    }

    /// <summary>
    /// �إߤ@��Excel���ε{��������
    /// </summary>
    /// <param name="objN">����W��</param>
    public void DeclareExcelApplication(string objN)
    {
        m_Sw.WriteLine("Set {0} = CreateObject(\"EXCEL.APPLICATION\")", objN);
    }

    /// <summary>
    /// �]�wExcel���ε{�����i����
    /// </summary>
    /// <param name="appN">Excel���ε{���W��</param>
    /// <param name="isVisi">�O�_�i���A��Bool��</param>
    public void AppVisible(string appN, bool isVisi)
    {
        m_Sw.WriteLine("{0}.Visible = {1}", appN, isVisi);
    }

    /// <summary>
    /// �P��n�ϥΪ�����
    /// </summary>
    /// <param name="appN">Excel���ε{���W��</param>
    /// <param name="fileName">�ɮצW��</param>
    public void AppWindowActivate(string appN, string fileName)
    {
        m_Sw.WriteLine("{0}.Windows(\"{1}\").Activate", appN, fileName);
    }

    /// <summary>
    /// �����@�ӥ��b�ϥΤ�������
    /// </summary>
    /// <param name="appN">Excel���ε{���W��</param>
    /// <param name="windowN">�����W�٧Y�ɮצW��</param>
    /// <param name="isSave">�O�_�s��</param>
    public void AppCloseWindow(string appN, string windowN, bool isSave)
    {
        m_Sw.WriteLine(" {0}.Windows(\"{1}\").Close ({2})", appN, windowN, isSave);
    }

    /// <summary>
    /// ��ܷ�e��Sheet�����Ҧ����
    /// </summary>
    /// <param name="appN">Excel���ε{���W��</param>
    public void AppSelectCurrentSheet(string appN)
    {
        m_Sw.WriteLine("{0}.Cells.Select", appN);
    }


    /// <summary>
    /// �P��Excel�ɤ����Y�@��Sheet
    /// </summary>
    /// <param name="appN">Excel���ε{���W��</param>
    /// <param name="workbookN">Workbook���W��</param>
    /// <param name="sheetN">Sheet���W��</param>
    public void AppSheetActivate(string appN, string workbookN, string sheetN)
    {
        m_Sw.WriteLine("{0}.Workbooks(\"{1}\").Sheets(\"{2}\").Activate", appN, workbookN, sheetN);
    }

    /// <summary>
    /// �����~�ɫ�_�����O
    /// </summary>
    public void ErrResumeNext()
    {
        m_Sw.WriteLine("On Error Resume Next");
    }

    /// <summary>
    /// �P��S�wBook���Y�@��Sheet
    /// </summary>
    /// <param name="workbookObj">BookName</param>
    /// <param name="sheetN">SheetName</param>
    public void BookSheetActivate(string workbookObj, string sheetN)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Activate", workbookObj, sheetN);
    }

    /// <summary>
    /// Copy�Y�@�ӽd�򪺸�ƨ�t�@�ӥؼ�
    /// </summary>
    /// <param name="appN">Excel���ε{������</param>
    /// <param name="srcWorkbookN">�ӷ���BookName</param>
    /// <param name="srcSheetN">�ӷ���SheetName</param>
    /// <param name="srcRangeN">�ӷ����d��W�٩ΰ_�l�x�s��</param>
    /// <param name="targetWorkbookN">�ؼЪ�BookName</param>
    /// <param name="targetSheetN">�ؼЪ�SheetName</param>
    /// <param name="targetRangeN">�ؼЪ��d��W�٩ΰ_�l�x�s��</param>
    public void AppRangeCopy(string appN, string srcWorkbookN, string srcSheetN, string srcRangeN,
        string targetWorkbookN, string targetSheetN, string targetRangeN)
    {
        m_Sw.Write("{0}.Workbooks(\"{1}\").Sheets(\"{2}\").Range(\"{3}\").Copy (",
            appN, srcWorkbookN, srcSheetN, srcRangeN);
        m_Sw.WriteLine(" {0}.Workbooks(\"{1}\").Sheets(\"{2}\").Range(\"{3}\") )",
            appN, targetWorkbookN, targetSheetN, targetRangeN);
    }


    /// <summary>
    ///  Copy�@��Book�����Y�@��Sheet��Rang
    /// </summary>
    /// <param name="srcWorkbookObj">�ӷ�Book����</param>
    /// <param name="srcSheetN">�ӷ���SheetName</param>
    /// <param name="srcRangeN">�ӷ����d��W�٩ΰ_�l�x�s��</param>
    /// <param name="targetWorkbookObj">�ؼ�Book����</param>
    /// <param name="targetSheetN">�ؼЪ�SheetName</param>
    /// <param name="targetRangeN">�ؼЪ��d��W�٩ΰ_�l�x�s��</param>
    public void AppRangeCopy(string srcWorkbookObj, string srcSheetN, string srcRangeN, string targetWorkbookObj, string targetSheetN, string targetRangeN)
    {
        m_Sw.Write("{0}.Sheets(\"{1}\").Range(\"{2}\").Copy (", srcWorkbookObj, srcSheetN, srcRangeN);
        m_Sw.WriteLine(" {0}.Sheets(\"{1}\").Range(\"{2}\") )", targetWorkbookObj, targetSheetN, targetRangeN);
    }

    /// <summary>
    /// �s�W�@�Ӥu�@��
    /// </summary>
    /// <param name="appObj">�n�s�W��Excel	Application ��H</param>
    /// <param name="workbookObj">�s�W���u�@����H</param>
    //Create on 2005/08/23 
    public void AppAddWorkbook(string appObj, string workbookObj)
    {
        m_Sw.WriteLine("Set {0} =  {1}.Workbooks.Add", workbookObj, appObj);
    }

    /// <summary>
    /// ���o���w�u�@�����W��
    /// </summary>
    /// <param name="workbookObj">�u�@����H</param>
    /// <returns>�u�@���W���ݩ�</returns>
    //Create on 2005/08/23
    public string WorkbookName(string workbookObj)
    {
        return string.Format("{0}.Name", workbookObj);
    }
    /// <summary>
    /// �襤�@�ӫ��w���ϰ�
    /// </summary>
    /// <param name="appObj">�@�ӫ��w��Excel��H</param>
    /// <param name="range">Range�W��</param>
    /// <example>Application.Range("A1:K21").Select</example>
    //Create on 2005/08/23
    public void AppSelectRange(string appObj, string range)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").Select", appObj, range);
    }

    /// <summary>
    /// �u�@���㭶����
    /// </summary>
    /// <param name="srcSheetObj">���u�@���H</param>
    /// <param name="targetWorkbookObj">�ؼФu�@���H�W�r</param>
    /// <param name="targetSheetN">�ؼЬ���ï��H�W�r</param>
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
    /// �}�� Excel����ï
    /// </summary>
    /// <param name="workbookObj">Excel Workbooks��H</param>
    /// <param name="appObj">Excel Application��H</param>
    /// <param name="filePath">Excel ��󪺸��|</param>
    /// <example>Set xlBook = xlApp.Workbooks.open("c:/WebsitePolling/Report/WebMCList.xls")</example>
    //Create on 2005/08/23 
    public void AppOpenBook(string workbookObj, string appObj, string filePath)
    {
        m_Sw.WriteLine("Set {0} = {1}.Workbooks.open(\"{2}\")", workbookObj, appObj, filePath);
    }


    /// <summary>
    /// �p�G�b������� Excel ��ܯS�w��ĵ�i�M�T����true ,�_�h��false
    /// </summary>
    /// <param name="appObj">Excel Application��H</param>
    /// <param name="isAlert">������� Excel ��ܯS�w��ĵ�i�M�T����true ,�_�h��false</param>
    //Create on 2005/08/23 
    public void AppDisplayAlerts(string appObj, bool isAlert)
    {
        m_Sw.WriteLine("{0}.DisplayAlerts={1}", appObj, isAlert);
    }

    /// <summary>
    ///   �N���w���u�@��]���ϥΤ����u�@��
    /// </summary>
    /// <param name="appObj">Excel Application��H</param>
    /// <param name="sheetN">�N���w���u�@���W�r</param>
    //Create on 2005/08/23
    public void AppSetActivateSheet(string appObj, string sheetN)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Activate", appObj, sheetN);
    }

    /// <summary>
    /// �]�w�@�Ӥu�@���H��Workbooks���X�����b�ϥΪ��u�@��
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    /// <param name="workbookObj">Workbooks���X</param>
    //Create on 2005/08/23
    public void BookGetActiveSheet(string sheetObj, string workbookObj)
    {
        m_Sw.WriteLine("Set {0} = {1}.ActiveSheet", sheetObj, workbookObj);
    }

    /// <summary>
    /// �]�w�@���ܶq�����w���u�@���H
    /// </summary>
    /// <param name="sheetObj">�]�w���@���ܶq</param>
    /// <param name="workbookObj">�n���w���u�@���H���ݪ��u�@����H</param>
    /// <param name="sheetN">�u�@���H���W��</param>
    //Create on 2005/08/23 
    public void BookSetSheet(string sheetObj, string workbookObj, string sheetN)
    {
        m_Sw.WriteLine("Set {0} = {1}.Sheets(\"{2}\")", sheetObj, workbookObj, sheetN);
    }
    /// <summary>
    /// �����wWorkBook�s�W�@��sheet
    /// </summary>
    /// <param name="workbookName">�n���w���u�@���H���ݪ��u�@����H</param>
    /// <param name="sheetName">�u�@���H���W��</param>
    public void AddSheet(string workbookName, string sheetName)
    {
        m_Sw.WriteLine("{0}.Sheets.Add", workbookName);
        m_Sw.WriteLine("{0}.ActiveSheet.Name = \"{1}\"", workbookName, sheetName);
        //			m_Sw.WriteLine("Sheets(\"{0}\").Move After:=Sheets(Sheets.Count)",sheetName);
    }

    /// <summary>
    /// �b���w��WorkBook�R�����wsheet
    /// </summary>
    /// <param name="workbookName">�n���w���u�@���H���ݪ��u�@����H</param>
    /// <param name="sheetName">�u�@���H���W��</param>
    public void DeleteSheet(string workbookName, string sheetName)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").Delete", workbookName, sheetName);
    }

    /// <summary>
    /// �b��eWorkBook����w��sheet��W
    /// </summary>
    /// <param name="oldName">��W</param>
    /// <param name="newName">�s�W</param>
    public void RenameSheet(string workbookName, string oldName, string newName)
    {
        m_Sw.WriteLine("{0}.Sheets(\"{1}\").name=\"{2}\"", workbookName, oldName, newName);
    }

    /// <summary>
    /// �]�w�氪
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="row1">�_�l��</param>
    /// <param name="row2">������</param>
    /// <param name="rowWidth">�氪</param>
    public void SetRowHeight(string sheetObj, int row1, int row2, double rowWidth)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{2}\").RowHeight={3}", sheetObj, row1, row2, rowWidth);
    }
    /// <summary>
    /// �]�w�C�e
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="cell1">�_�l�C</param>
    /// <param name="cell2">�����C</param>
    /// <param name="cellHeight">�C�e</param>
    public void SetCellWidth(string sheetObj, string cell1, string cell2, double cellHeight)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{2}\").ColumnWidth={3}", sheetObj, cell1, cell2, cellHeight);
    }
    /// <summary>
    /// �]�wcell����覡
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="cell">cell�y��</param>
    /// <param name="align">����覡</param>
    public void SetCellHAlign(string sheetObj, string cell, string align)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").HorizontalAlignment ={2}", sheetObj, cell, align);
    }
    /// <summary>
    /// �b���wsheet���w�C���e���J�@�C
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="column">�C��</param>
    public void InsertColumn(string sheetObj, string column)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{1}\").Insert", sheetObj, column);
    }
    /// <summary>
    /// �b���wsheet�R�����w�C
    /// </summary>
    /// <param name="sheetObj"></param>
    /// <param name="column"></param>
    public void DeleteColumn(string sheetObj, string column)
    {
        m_Sw.WriteLine("{0}.Columns(\"{1}:{1}\").Delete", sheetObj, column);
    }
    /// <summary>
    /// �b���wsheet���w�椧�e���J�@��
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="row">�渹</param>
    public void InsertRow(string sheetObj, int row)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{1}\").Insert", sheetObj, row);
    }
    /// <summary>
    /// �b���wsheet�R�����w��
    /// </summary>
    /// <param name="sheetObj">sheet��H</param>
    /// <param name="row">�渹</param>
    public void DeleteRow(string sheetObj, int row)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{1}\").Delete", sheetObj, row);
    }
    /// <summary>
    /// �]�w�C�L���Ҧ����a�V�ξ�V
    /// </summary>
    /// <param name="sheetObj">�C�L���u�@���H</param>
    /// <param name="orientation">�C�L���Ҧ�</param>
    //Create on 2005/08/23 
    public void SetSheetOrientation(string sheetObj, PageOrientation orientation)
    {
        m_Sw.WriteLine("{0}.PageSetup.Orientation = {1}", sheetObj, (int)orientation);
    }

    /// <summary>
    /// �]�w�u�@��H��Range����
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    /// <param name="range">Range�W��</param>
    /// <param name="val">�n�]�w����</param>
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
    /// �]�w�u�@���H���@��椸�檺�氪
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    /// <param name="row">�n�]�w�������</param>
    /// <param name="rowHeight">�氪</param>
    //Create on 2005/08/23
    public void SheetSetRowHeight(string sheetObj, int row, double rowHeight)
    {
        m_Sw.WriteLine("{0}.Rows(\"{1}:{2}\").rowHeight = {3}", sheetObj, row, row, rowHeight);
    }

    /// <summary>
    /// �M���u�@��H��Range���ȡM���O�d��榡�]�w
    /// </summary>
    /// <param name="sheetObj">�n�M�����u�@���H</param>
    /// <param name="range">Range�W��</param>
    //Create on 2005/08/23
    public void SheetClearRange(string sheetObj, string range)
    {
        m_Sw.WriteLine("{0}.Range(\"{1}\").ClearContents", sheetObj, range);
    }

    /// <summary>
    /// �]�w�u�@���S�w�椸�檺�� 
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    /// <param name="posX">�C��</param>
    /// <param name="posY">�渹</param>
    /// <param name="val">�n�]�w����</param>
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
    /// �L�C�S�w���u�@��
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    //Create on 2005/08/23 
    public void SheetPrintPreview(string sheetObj)
    {
        m_Sw.WriteLine("{0}.PrintPreview", sheetObj);
        System.Diagnostics.Debug.WriteLine(string.Format("{0}.PrintPreview", sheetObj));
    }

    /// <summary>
    /// ����Excel���ε{��
    /// </summary>
    /// <param name="appObj">Excel Application��H</param>
    //Create on 2005/08/23 
    public void AppQuit(string appObj)
    {
        m_Sw.WriteLine("{0}.Quit() ", appObj);
    }



    /// <summary>
    /// �]�w���L����������ݩ�
    /// </summary>
    /// <param name="sheetObj">�u�@���H</param>
    /// <param name="margin">��ɦW��</param>
    /// <param name="appObj">Excel Application��H</param>
    /// <param name="inches">��Z�]�H�^�o�����^</param>
    //Create on 2005/08/23
    public void SetMarginInfo(string sheetObj, string margin, string appObj, decimal inches)
    {
        m_Sw.WriteLine("{0}.PageSetup.{1} = {2}.InchesToPoints({3})", sheetObj, margin, appObj, inches);

    }
    /// <summary>
    /// �����@�Ӥ�k
    /// </summary>
    //Create on 2005/08/23
    public void EndSub()
    {
        m_Sw.WriteLine("end sub");
    }

    /// <summary>
    /// �����@�Ӹ}����
    /// </summary>
    //Create on 2005/08/23 
    public void EndScript()
    {
        m_Sw.WriteLine("</script>");
    }

    /// <summary>
    /// �եΤ@�Ӥ�k
    /// </summary>
    /// <param name="methodName">��k�W��</param>
    //Create on 2005/08/23 
    public void CallMethod(string methodName)
    {
        m_Sw.WriteLine(methodName);
    }

    /// <summary>
    /// ���͸}�������e
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

