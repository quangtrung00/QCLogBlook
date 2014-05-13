using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PccCommonForC;

public partial class QCLogBlook_BasicData_AddConditions : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        LoadData();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SetLabel();
    }

    private void SetLabel()
    {
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
        lbType.Text = myLabel.GetErrMsg("lbl0010", "QC/Tilte") + ":";
        lbCondition.Text = myLabel.GetErrMsg("lbl0014", "QC/Tilte") + ":";
        lbCount.Text = myLabel.GetErrMsg("lbl0015", "QC/Tilte") + ":";

        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");
    }

    private void LoadData()
    {
        string type = Request.Params["type"];
        imgType.ImageUrl = "~/" + BasicData.GetImgConditionsType(type);

        BasicData bd = new BasicData();
        ArrayList arConditions = bd.GetConditons(type);

        for (int i = 0; i < arConditions.Count; i++)
        {
            string c = arConditions[i].ToString();
            ddlConditions.Items.Add(new ListItem(c, c));
        }

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

        string type = Request.Params["type"];
        string condition = ddlConditions.SelectedValue;
        string count = txtCount.Text.Trim();
        string add_id = Session["UserID"].ToString();
        

        BasicData bd = new BasicData();
        string strReturn = bd.InsertImgConditions(type, condition, count, add_id);
        if (strReturn == "")
        {
            Response.Redirect("ImageConfig.aspx");
        }
        else
        {
            lbAlert.Visible = true;
            lbAlert.Text = "Erro: " + strReturn;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ImageConfig.aspx");
    }
}