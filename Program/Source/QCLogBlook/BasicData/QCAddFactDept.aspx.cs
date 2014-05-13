using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PccCommonForC;


public partial class QCLogBlook_BasicData_QCAddFactDept : System.Web.UI.Page
{
    DataTable dtQCFactDept;
    PccErrMsg myLabel;
    protected void Page_Init(object sender, EventArgs e)
    {
        ProcessAjaxEvent();
        LoadDDL();
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");

        if (IsPostBack == false)
        {
           
            LoadData();
            SetLabel();
           
        }

        LoadUserInfo();
        
    }
    private void LoadUserInfo()
    {
        BasicData bd = new BasicData();
        DataTable dtInfo = bd.GetApUser(Request.Params["user_id"]);
        lbUserDesc.Text = dtInfo.Rows[0]["user_desc"].ToString();
        lbFact_no.Text = dtInfo.Rows[0]["fact_no"].ToString();
        lbEmail.Text = dtInfo.Rows[0]["email"].ToString();
        lbEmp_no.Text = dtInfo.Rows[0]["emp_no"].ToString();
    }

    private void SetLabel()
    {
        lbDept.Text = myLabel.GetErrMsg("lbl0007", "SysManager/UserManager");
        lbBuild.Text = myLabel.GetErrMsg("lbl0007", "QC/Tilte") + ": ";
        lbFloor.Text = myLabel.GetErrMsg("lbl0008", "QC/Tilte") + ": ";
        lbReturn.Text = myLabel.GetErrMsg("lbl0017", "QC/Tilte");
        btnSearch.Text = myLabel.GetErrMsg("btnQuery");
        btnClear.Value = myLabel.GetErrMsg("btnClear");

        lbDesc_title.Text = myLabel.GetErrMsg("lbl0019", "SysManager/UserManager") + ": ";
        lbFact_title.Text = myLabel.GetErrMsg("lbl0006", "OrdSignWeb/OrdSign") + ": ";
        lbEmp_no_title.Text = myLabel.GetErrMsg("lbl0023", "SysManager/UserManager") + ": ";

        if (gvData.HeaderRow != null)
        {
            gvData.HeaderRow.Cells[0].Text = myLabel.GetErrMsg("lbl0006", "OrdSignWeb/OrdSign");
            gvData.HeaderRow.Cells[1].Text = myLabel.GetErrMsg("lbl0004", "QC/Tilte");
            gvData.HeaderRow.Cells[2].Text = myLabel.GetErrMsg("lbl0001", "QC/Tilte");
            gvData.HeaderRow.Cells[3].Text = myLabel.GetErrMsg("lbl0002", "QC/Tilte");
            gvData.HeaderRow.Cells[4].Text = myLabel.GetErrMsg("lbl0007", "QC/Tilte");
            gvData.HeaderRow.Cells[5].Text = myLabel.GetErrMsg("lbl0008", "QC/Tilte");
            gvData.HeaderRow.Cells[6].Text = myLabel.GetErrMsg("lbl0009", "QC/Tilte");
            gvData.HeaderRow.Cells[7].Text = myLabel.GetErrMsg("lbl0003", "QC/Tilte");
        }

    }


    private void LoadDDL()
    {
        if (Request.Params["user_id"] != null)
        {
            string user_id = Request.Params["user_id"];
            string fact_no = BasicData.GetFactByUser(user_id);

            try
            {

                BasicData bd = new BasicData();

                DataTable dtDept = bd.GetDeptERP(fact_no);
                ddlDept.DataSource = dtDept;
                ddlDept.DataTextField = "dept_name";
                ddlDept.DataValueField = "dept_no";
                ddlDept.DataBind();
                ddlDept.Items.Insert(0, new ListItem("--All--", ""));



                DataTable dtBuild = bd.GetBuildERP(fact_no);
                ddlBuild.DataSource = dtBuild;
                ddlBuild.DataTextField = "BUILD_NO";
                ddlBuild.DataValueField = "BUILD_NO";
                ddlBuild.DataBind();
                ddlBuild.Items.Insert(0, new ListItem("--All--", ""));

                DataTable dtFloor = bd.GetFloorERP(fact_no);
                ddlFloor.DataSource = dtFloor;
                ddlFloor.DataTextField = "FLOOR";
                ddlFloor.DataValueField = "FLOOR";
                ddlFloor.DataBind();
                ddlFloor.Items.Insert(0, new ListItem("--All--", ""));
            }
            catch (Exception ex)
            {
                lbAlert.Visible = true;
                lbAlert.Text = "Fact_no: " + fact_no + "  <br /> " + ex.Message;
                divSearch.Visible = false;
            }

        }
    }
    private void CheckTypeQCFactDept()
    {
        string type_check = Request.Params["type"];
        string[] value = Request.Params["id"].Split('-');
        string user_id = value[0];
        string fact_no = value[1];
        string dept_no = value[2];
        string sec_no = value[3];
        string sec_name = Request.Params["sec_name"];
        string dept_name = Request.Params["dept_name"];

        string add_id = Session["UserID"].ToString();
        BasicData bd = new BasicData();
        string errMsg = bd.InsertQCFactDept(user_id, fact_no, dept_no, sec_no, sec_name, dept_name, type_check, add_id);

        ReturnServer(errMsg);

    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dtrv = (DataRowView)e.Row.DataItem;



            string user_id = Request.Params["user_id"];
            string fact_no = dtrv["fact_no"].ToString().Trim();
            string dept_no = dtrv["dept_no"].ToString().Trim();
            string sec_no = dtrv["sec_no"].ToString().Trim();
            string sec_name = dtrv["sec_name"].ToString().Trim();
            string dept_name = dtrv["dept_name"].ToString().Trim();

            string type_check = getTypeCheck(dept_no, sec_no, dtQCFactDept);
            string checkType1 = "";
            string checkType2 = "";

            if (type_check == "0")
            {
                checkType1 = "checked";
                checkType2 = "checked";
            }
            else
            {
                if (type_check == "1") checkType1 = "checked";
                if (type_check == "2") checkType2 = "checked";
            }


            string id = user_id.Split('.')[0] + "-" + fact_no + "-" + dept_no + "-" + sec_no;
            string id1 = id + "1";
            string id2 = id + "2";

            string CD = myLabel.GetErrMsg("lbl0005", "QC/Tilte");
            string KT = myLabel.GetErrMsg("lbl0006", "QC/Tilte");

            string chk1 = "<span><input type=\"checkbox\" id=\"" + id1 + "\"  " + checkType1 + "  value=\"" + sec_name + "\"  onclick=\"InsertType('" + id + "');\" />" + CD + "</span>&nbsp;&nbsp;&nbsp;";
            string chk2 = "<span><input type=\"checkbox\" id=\"" + id2 + "\"  " + checkType2 + " value=\"" + dept_name + "\"   onclick=\"InsertType('" + id + "');\" />" + KT + "</span>";

            Label lbType_Check = (Label)e.Row.FindControl("lbType_Check");
            lbType_Check.Text = chk1 + chk2;
        }
    }

    private void LoadData()
    {
        string user_id = Request.Params["user_id"];
        string fact_no = BasicData.GetFactByUser(user_id);
        string dept_no = ddlDept.SelectedValue;
        string build_no = ddlBuild.SelectedValue;
        string floor = ddlFloor.SelectedValue;

        try
        {

            BasicData bd = new BasicData();
            DataTable dtData = bd.GetSecERP(fact_no, dept_no, build_no, floor);
            dtQCFactDept = bd.GetQCFactDept(user_id, fact_no);

            gvData.DataSource = dtData;
            gvData.DataBind();
        }
        catch { }

    }


    private string getTypeCheck(string dept_no, string sec_no, DataTable dtQCFactDept)
    {
        string type_check = "";
        DataRow[] dr = dtQCFactDept.Select("dept_no = '" + dept_no + "' AND sec_no = '" + sec_no + "' ");
        if (dr.Length > 0)
            type_check = dr[0]["type_Check"].ToString();

        return type_check;
    }

    #region Ajax
    private void ProcessAjaxEvent()
    {
        string strEventName = Convert.ToString(Request.Params["EventName"]);
        switch (strEventName)
        {
            case "CHECKTYPE":
                CheckTypeQCFactDept();
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


    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        LoadData();
        SetLabel();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
        SetLabel();
    }
}