using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PccCommonForC;

public partial class QCLogBlook_BasicData_ImageConfig : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GenTable();
        if (hdfDelete.Value != "") DeleteCondition();
    }

    private void GenTable()
    {
        mTable.Rows.Clear();
        mTable.Width = Unit.Pixel(800);
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


        #region Header 1
        TableRow rHeader1 = new TableRow();

        TableCell cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0010", "QC/Tilte");//Hình ảnh không đạt
        cell.RowSpan = 2;
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader1.Cells.Add(cell);

        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0011", "QC/Tilte");//Thiết lập điều kiện
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        rHeader1.Cells.Add(cell);

      



        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0012", "QC/Tilte"); // Người thiết lập;
        cell.RowSpan = 2;
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader1.Cells.Add(cell);


        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0013", "QC/Tilte"); //Thời gian thiết lập;  
        cell.RowSpan = 2;
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader1.Cells.Add(cell);

        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0033", "QC/Tilte");// Chức năng
        cell.Width = Unit.Pixel(40);
        cell.RowSpan = 2;
        cell.ColumnSpan = 2;
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader1.Cells.Add(cell);

       

        mTable.Rows.Add(rHeader1);
        #endregion

        #region Header 2
        TableRow rHeader2 = new TableRow();

        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0014", "QC/Tilte"); //Điều kiện      
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader2.Cells.Add(cell);

        cell = new TableCell();
        cell.CssClass = cssClass;
        cell.Text = myLabel.GetErrMsg("lbl0015", "QC/Tilte"); //Số lần không đạt
        cell.HorizontalAlign = HorizontalAlign.Center;
        rHeader2.Cells.Add(cell);

       

        mTable.Rows.Add(rHeader2);
        #endregion

    }

    private void GenTableData()
    {
        BasicData bd = new BasicData();
        DataTable dtData = bd.GetImgConditions();

        GenRowType("A", dtData.Select("img_type = 'A' "), "cssGridRow");
        GenRowType("B", dtData.Select("img_type = 'B' "), "cssGridRowAlternating");
        GenRowType("C", dtData.Select("img_type = 'C' "), "cssGridRow");
    }


    private void GenRowType(string type, DataRow[] drData, string css)
    {
        string cssClass = css;
        int rowSpan = drData.Length;


        for (int i = 0; i < drData.Length; i++)
        {
            string id = drData[i]["ID"].ToString();

            TableRow rType = new TableRow();
            rType.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();

            if (i == 0)
            {
                #region Cell Type Image
                Image imgType = new Image();
                imgType.Style["cursor"] = "pointer";
                imgType.ImageUrl = "~/" + BasicData.GetImgConditionsType(type);
                imgType.Attributes.Add("onclick", "window.location.replace('UploadImgCondition.aspx?type=" + type + "')");
                imgType.ToolTip = type;
                cell.Controls.Add(imgType);
                cell.CssClass = cssClass;
                cell.RowSpan = rowSpan;
                rType.Cells.Add(cell);
                #endregion
            }

            #region Cell Conditions
            cell = new TableCell();
            cell.Text = drData[i]["condition"].ToString();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region count
            cell = new TableCell();
            cell.Text = drData[i]["count"].ToString();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region User
            cell = new TableCell();
            cell.Text = drData[i]["add_desc"].ToString();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region Date
            cell = new TableCell();
            cell.Text = myTools.FormatDate_char8(drData[i]["add_date"].ToString());
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            if (i == 0)
            {
                #region Add
                Image imgAdd = new Image();
                imgAdd.ImageUrl = "~/Images/Icon/addItem.gif";
                imgAdd.Style["cursor"] = "pointer";
                imgAdd.Attributes.Add("onclick", "window.location.replace('AddConditions.aspx?type=" + type + "')");

                cell = new TableCell();
                cell.Controls.Add(imgAdd);
                cell.CssClass = cssClass;
                cell.RowSpan = rowSpan;
                rType.Cells.Add(cell);
                #endregion
                if (drData.Length == 3) imgAdd.Visible = false;
            }

            #region config



            Image imgDelete = new Image();
            imgDelete.ImageUrl = "~/Images/Icon/delete.gif";
            imgDelete.Style["cursor"] = "pointer";
            imgDelete.Attributes.Add("onclick", "confirmDelete('" + id + "');");

            cell = new TableCell();
            cell.Controls.Add(imgDelete);
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            mTable.Rows.Add(rType);

        }

        #region Row Null
        if (drData.Length == 0)
        {
            TableRow rType = new TableRow();
            rType.HorizontalAlign = HorizontalAlign.Center;
            TableCell cell = new TableCell();

          
            #region Cell Type Image
            Image imgType = new Image();
            imgType.Style["cursor"] = "pointer";
            imgType.ImageUrl = "~/" + BasicData.GetImgConditionsType(type);
            imgType.Attributes.Add("onclick", "window.location.replace('UploadImgCondition.aspx?type=" + type + "')");
            imgType.ToolTip = type;
            cell.Controls.Add(imgType);
            cell.CssClass = cssClass;
            cell.RowSpan = rowSpan;
            rType.Cells.Add(cell);
            #endregion
           

            #region Cell Conditions
            cell = new TableCell();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region count
            cell = new TableCell();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region User
            cell = new TableCell();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            #region Date
            cell = new TableCell();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

        
            #region Add
            Image imgAdd = new Image();
            imgAdd.ImageUrl = "~/Images/Icon/addItem.gif";
            imgAdd.Style["cursor"] = "pointer";
            imgAdd.Attributes.Add("onclick", "window.location.replace('AddConditions.aspx?type=" + type + "')");

            cell = new TableCell();
            cell.Controls.Add(imgAdd);
            cell.CssClass = cssClass;
            cell.RowSpan = rowSpan;
            rType.Cells.Add(cell);
            #endregion


            #region config
            cell = new TableCell();
            cell.CssClass = cssClass;
            rType.Cells.Add(cell);
            #endregion

            mTable.Rows.Add(rType);
        }

        #endregion



    }


    private void DeleteCondition()
    {
        string id = hdfDelete.Value;
        string user_id = Session["UserID"].ToString();
        BasicData bd = new BasicData();
        string strReturn = bd.CancelImgConditions(id, user_id);
        if (strReturn == "")
        {
            hdfDelete.Value = "";
            GenTable();
        }

    }
}