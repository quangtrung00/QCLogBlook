using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PccCommonForC;

public partial class QCLogBlook_BasicData_uploadImgCondition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadData();
    }
    private void SetLabel()
    {
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
        lbType.Text = myLabel.GetErrMsg("lbl0010", "QC/Tilte");
        lbPath.Text = myLabel.GetErrMsg("lbl0016", "QC/Tilte");

        btnOK.Text = myLabel.GetErrMsg("btnOK");
        btnCancel.Text = myLabel.GetErrMsg("btnCancel");
    }

    private void LoadData()
    {
        string type = Request.Params["type"];
        imgType.ImageUrl = "~/" + BasicData.GetImgConditionsType(type);
        
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string type = Request.Params["type"];
            string path = Server.MapPath("~") + "/" + BasicData.GetImgConditionsType(type);
            fulImage.SaveAs(path);
            Response.Redirect("ImageConfig.aspx");
        }
        catch(Exception ex)
        {
            lbAlert.Visible = true;
            lbAlert.Text = "Erro: " + ex.Message;
        }
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ImageConfig.aspx");
    }
}