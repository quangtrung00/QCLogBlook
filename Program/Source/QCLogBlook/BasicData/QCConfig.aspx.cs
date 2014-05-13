using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PccCommonForC;

public partial class QCLogBlook_BasicData_QCConfig : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        ProcessAjaxEvent();
        LoadDDLFact();
        LoadDDLDept();
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack == false)
            GenTable();

        SetLabel();
    }

    private void SetLabel()
    {
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
        lbFact_no.Text = myLabel.GetErrMsg("lbl0006", "OrdSignWeb/OrdSign") + "：";
        lbDept_no.Text = myLabel.GetErrMsg("lbl0007", "SysManager/UserManager");
        lbUserDesc.Text = myLabel.GetErrMsg("lbl0013", "SysManager/UserManager");
        btnSearch.Text = myLabel.GetErrMsg("btnQuery");
        btnClear.Value = myLabel.GetErrMsg("btnClear");
    }

    private void LoadDDLFact()
    {
        BasicData bd = new BasicData();
        ddlFact.DataSource = bd.GetFactQC();
        ddlFact.DataTextField = "fact_no";
        ddlFact.DataValueField = "fact_no";
        ddlFact.DataBind();

        ddlFact.Items.FindByValue("172 ").Selected = true;


      
    }
    private void LoadDDLDept()
    {
        BasicData bd = new BasicData();
        ddlDept.DataSource = bd.GetDeptInQCFactDept(ddlFact.SelectedValue);
        ddlDept.DataTextField = "dept_name";
        ddlDept.DataValueField = "dept_no";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, new ListItem("--ALL--", ""));

    }

    #region User Table
    private void GenTable()
    {
        mTable.Rows.Clear();
        mTable.Width = Unit.Percentage(100);
        mTable.CellPadding = 2;
        mTable.CellSpacing = 0;
        mTable.BorderWidth = Unit.Pixel(1);
        mTable.CssClass = "cssGridTable";

        GenTableHeader();
        GenTableData();
    }

    private void GenTableHeader()
    {
        string cssClass = "cssGridHeader";
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");


        TableRow row = new TableRow();
        row.HorizontalAlign = HorizontalAlign.Center;
        TableCell cell = new TableCell();

        cell.Text = "No";
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0023", "SysManager/UserManager"); // Số Thẻ  
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0032", "SysManager/UserManager");// Tài Khoản   
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0019", "SysManager/UserManager");// Họ Tên  
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0006", "OrdSignWeb/OrdSign");// Xưởng       
        cell.CssClass = cssClass;
        row.Cells.Add(cell);
        
        cell = new TableCell();
        cell.Text = "";
        cell.CssClass = cssClass;
        row.Cells.Add(cell);
        
        
        mTable.Rows.Add(row);
    }

    private void GenTableData()
    {
        BasicData bd = new BasicData();

        string totalRecord = "";
        
        DataTable dt = bd.GetApUser(txtUserDesc.Text, txtEmail.Text, ddlFact.SelectedValue, PageControl1.StartRecord.ToString(), PageControl1.PageSize.ToString(),ref totalRecord);


        SetPageControl(totalRecord, PageControl1.CurrentPage);

        string cssClass = "";
        

        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (i % 2 == 1) cssClass = "cssGridRowAlternating";
            else cssClass = "cssGridRow";


            i++;
            
            TableRow row = new TableRow();
            row.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();

            cell.Text = i.ToString();
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["emp_no"].ToString();
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["email"].ToString();
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["user_desc"].ToString();
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["fact_no"].ToString();
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            string rowID = "R" + dr["user_id"].ToString().Split('.')[0];

            Image imgDetail = new Image();
            imgDetail.Style["cursor"] = "pointer";
            imgDetail.ImageUrl = "~/Images/arrow_down.gif";
            imgDetail.Attributes.Add("onclick", "ShowDetail('" + rowID + "','" + dr["user_id"].ToString() + "');");

            cell = new TableCell();
            cell.Controls.Add(imgDetail);
            cell.CssClass = cssClass;
            row.Cells.Add(cell);


            mTable.Rows.Add(row);

            #region Row Details
            row = new TableRow();
            row.ID = rowID;
            row.Style["display"] = "none";
            
            cell = new TableCell();
            cell.ColumnSpan = 6;
            cell.CssClass = cssClass;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Cells.Add(cell);
            mTable.Rows.Add(row);
            #endregion
        }

        if (dt.Rows.Count == 0)
        {
            TableRow row = new TableRow();
            row.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();
            cell.Text = "No Data";
            cell.CssClass = "cssGridRow";
            cell.ColumnSpan = 6;
            row.Cells.Add(cell);
            mTable.Rows.Add(row);


        }
    }
    #endregion


    #region QC Fact Table

    private Table GenTableQC(string user_id, string fact_no, string dept_no)
    {
        Table dTable = new Table();
        dTable.Rows.Clear();
        dTable.Width = Unit.Percentage(100);
        dTable.CellPadding = 2;
        dTable.CellSpacing = 0;
        dTable.BorderWidth = Unit.Pixel(1);
        dTable.CssClass = "cssGridTable";

        GenTableQCHeader(ref dTable, user_id);
        GenTableQCData(ref dTable, user_id, fact_no, dept_no);
        return dTable;
    }
    private void GenTableQCHeader(ref Table dTable, string user_id)
    {
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");

        string cssClass = "cssGridHeaderDetails";

        TableRow row = new TableRow();
        row.HorizontalAlign = HorizontalAlign.Center;
        TableCell cell = new TableCell();

        cell.Text = myLabel.GetErrMsg("lbl0006", "OrdSignWeb/OrdSign");// Xưởng 
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0004", "QC/Tilte");// Bộ Phận 
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0001", "QC/Tilte");// Mã Tổ 
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0002", "QC/Tilte");// Tên Tổ
        cell.CssClass = cssClass;
        row.Cells.Add(cell);
        
        cell = new TableCell();
        cell.Text = myLabel.GetErrMsg("lbl0003", "QC/Tilte");// Loại Tổ
        cell.CssClass = cssClass;
        row.Cells.Add(cell);

        Image imgAdd = new Image();
        imgAdd.ImageUrl = "~/Images/Icon/addItem.gif";
        imgAdd.Style["cursor"] = "pointer";
        imgAdd.Attributes.Add("onclick", "window.location.replace('QCAddFactDept.aspx?user_id=" + user_id + "');");

        cell = new TableCell();
        cell.Controls.Add(imgAdd);
        cell.CssClass = cssClass;
        

        row.Cells.Add(cell);


        dTable.Rows.Add(row);
    }

    private void GenTableQCData(ref Table dTable, string user_id, string fact_no, string dept_no)
    {
        string cssClass = "";
        BasicData bd = new BasicData();
        DataTable dtData = bd.GetQCFactDept(user_id, fact_no, dept_no, "");

        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");

        int i = 0;
        foreach (DataRow dr in dtData.Rows)
        {
            if (i % 2 == 1) cssClass = "cssGridRowDetailsAlternating";
            else cssClass = "cssGridRow"; 

            i++;
            TableRow row = new TableRow();
            row.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();

            cell.Text = dr["fact_no"].ToString();// Xưởng
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["dept_name"].ToString();// Bộ Phận
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["sec_no"].ToString();// Mã Tổ
            cell.CssClass = cssClass;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dr["sec_name"].ToString();// Tên Tổ
            cell.CssClass = cssClass;
            row.Cells.Add(cell);


            #region type_check

            cell = new TableCell();
            cell.Text = BasicData.GetTypeName(dr["type_Check"].ToString(), myLabel);// Loại Tổ
            cell.Width = Unit.Pixel(200);
            cell.CssClass = cssClass;
            row.Cells.Add(cell);
            #endregion


            string id = user_id.Split('.')[0] + "-" + dr["fact_no"].ToString().Trim() + "-" + dr["dept_no"].ToString().Trim() + "-" + dr["sec_no"].ToString().Trim();

            Image imgDelete = new Image();
            imgDelete.ImageUrl = "~/Images/Icon/delete.gif";
            imgDelete.Style["cursor"] = "pointer";
            imgDelete.Attributes.Add("onclick", "DeleteQCFactDept('" + id + "');");

            cell = new TableCell();
            cell.Controls.Add(imgDelete);
            cell.CssClass = cssClass;


            row.Cells.Add(cell);


            dTable.Rows.Add(row);
        }
        if (dtData.Rows.Count == 0)
        {
            TableRow row = new TableRow();
            row.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();
            cell.Text = "No Data";
            cell.CssClass = "cssGridRow";
            cell.ColumnSpan = 6;
            row.Cells.Add(cell);
            dTable.Rows.Add(row);


        }
    }       
    #endregion


    #region Ajax
    private void ProcessAjaxEvent()
    {
        string strEventName = Convert.ToString(Request.Params["EventName"]);
        switch (strEventName)
        {
            case "GetQCFactDept":
                GetQCFactDept();
                break;
            case "DeleteQCFactDept":
                DeleteQCFactDept();
                break;
            case "GetDept":
                GetDept();
                break;
                
        }
    }

    private void ReturnServer(string strReturn)
    {
        Response.Clear();
        Response.Write(strReturn);
        Response.End();
    }
    #endregion

    private void GetQCFactDept()
    {
        string user_id = Request.Params["user_id"];
        string fact_no = Request.Params["fact_no"];
        string dept_no = Request.Params["dept_no"];

        string htmlTable = PccRow.GenOuterHtml(GenTableQC(user_id, fact_no, dept_no));
        ReturnServer(htmlTable);
    }

    private void DeleteQCFactDept()
    {
        string id = Request.Params["id"];
        string[] arValue = id.Split('-');
        string user_id = arValue[0];
        string fact_no = arValue[1];
        string dept_no = arValue[2];
        string sec_no = arValue[3];

        BasicData bd = new BasicData();
        string errMsg = bd.DeleteQCFactDept(user_id, fact_no, dept_no, sec_no);
        string html = "";
        if (errMsg == "")
        {
            html = PccRow.GenOuterHtml(GenTableQC(user_id, fact_no, dept_no));
        }
        else
        {
            html = "Erro: " + errMsg;
        }

        ReturnServer(html);
    }

    private void GetDept()
    {
        string str_return = "";
        string fact_no = Request.Params["fact_no"];


        BasicData bd = new BasicData();
        DataTable dt = bd.GetDeptInQCFactDept(fact_no);

        foreach (DataRow row in dt.Rows)
        {
            string value = row["dept_no"].ToString().Trim();
            string text = row["dept_no"].ToString().Trim();

            str_return += value + ":" + text + ";";
        }

        if (str_return.Length > 0)
            str_return = str_return.Substring(0, str_return.Length - 1);
        
        ReturnServer(str_return);
    }


    #region "SetPageControl"
    private void SetPageControl(string totalSize, string currentPage)
    {
        string tmp = PageControl1.CurrentPage; //若沒有做這個動作會沒有建出Instance的Error
        PageControl1.TotalSize = totalSize;
        PageControl1.BuildPager();
        PageControl1.CurrentPage = currentPage;
        PageControl1.BuildPager();
    }
    #endregion
    protected void PageControl1_PageClick(object sender, EventArgs e)
    {
        GenTable();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GenTable();
    }
    protected void ddlFact_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDDLDept();
        GenTable();
    }
   
}