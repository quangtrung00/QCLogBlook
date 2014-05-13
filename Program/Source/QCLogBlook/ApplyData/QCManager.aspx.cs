using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PccBsQCLogBlook;
using PccCommonForC;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
public partial class QCLogBlook_ApplyData_QCManager : System.Web.UI.Page
{
     BasicData mybs = new BasicData();
     public string sCode = "";
     public string sBadNo = "";
     public string sNumBer = "";
     public string sNumBer2 = "";
     public string sUserQC = "";
     public string sTimeNo = "";
     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("../../Default.aspx?");
        if (!IsPostBack)
        {
            //kiểm tra loại QC nào
            RequestType();

            //lây chuổi connect
            mybs.GetConnectionString(lblFactNo1.Text);

            //lấy fact_no
            string xmlFact_no = _DropDownList.GetFactNoByUserID(Session["UserID"].ToString().Trim());
            lblFactNo1.Text = xmlFact_no;
            // lấy dept_no
            _DropDownList._Get_ControlDept(ddlDepNo, Session["UserID"].ToString().Trim(), "1");
            if (ddlDepNo.SelectedValue == "")
            {
                Response.Write("<script>alert('Ban không đủ quyền hạn !');window.location = '../../Default.aspx?ApID=249';</script>");
            }

            // lấy sec_no
            _DropDownList._Get_ControlSec(ddlSec_no, Session["UserID"].ToString().Trim(), ddlDepNo.SelectedValue.Trim(), Request.QueryString["TypeCheck"].ToString());

            if (ddlDepNo.SelectedValue == "" || ddlSec_no.SelectedValue == "")
            {
                Response.Write("<script>alert('Ban không đủ quyền hạn !');window.location = '../../Default.aspx?ApID=249';</script>");
                return;
            }
            //ngaỳ tháng hiện tại
            lblDateTimeNow1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //Lấy người đăng nhập
            lbllogin_user.Text = Session["UserName"].ToString();

            // lấy danh sách mã đơn
            ltrVouNo.Text = GetOdrno(); 
            // lấy danh sách loi
            ltrBadReason.Text = GetBadReson();
            LoadUserQC();
            SetLabel();
        }
        else
        {
            SetLabel();
            if (Request.Form["hOdrNo"] != "")
            {
                ltrBadReason.Text = GetBadReson();
                GetSumQtyToal();
                RequestType();
            }
        }
    }

    private void SetLabel()
    {
        PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
        lblFactNo.Text = myLabel.GetErrMsg("lbl0018", "QC/Tilte");// Mã xưỡng
        lblDeptNo.Text = myLabel.GetErrMsg("lbl0004", "QC/Tilte");
        lblSecNo.Text = myLabel.GetErrMsg("lbl0001", "QC/Tilte");
        lblDateTimeNow.Text = myLabel.GetErrMsg("lbl0019", "QC/Tilte");
        lblQC.Text = myLabel.GetErrMsg("lbl0020", "QC/Tilte");
        lblNVKT.Text = myLabel.GetErrMsg("lbl0021", "QC/Tilte");
        llbSLDat.Text = myLabel.GetErrMsg("lbl0022", "QC/Tilte");
        lblSLKhongDat.Text = myLabel.GetErrMsg("lbl0023", "QC/Tilte");
        lblSumRutKiem.Text = myLabel.GetErrMsg("lbl0024", "QC/Tilte");
        lblTyLeDat.Text = myLabel.GetErrMsg("lbl0025", "QC/Tilte");

        //
        sCode = myLabel.GetErrMsg("lbl0027", "QC/Tilte");
        sBadNo = myLabel.GetErrMsg("lbl0028", "QC/Tilte");
        sNumBer = myLabel.GetErrMsg("lbl0029", "QC/Tilte");
        sNumBer2 = myLabel.GetErrMsg("lbl0032", "QC/Tilte");
        sUserQC = myLabel.GetErrMsg("lbl0030", "QC/Tilte");
        sTimeNo = myLabel.GetErrMsg("lbl0031", "QC/Tilte");
    }

    private void RequestType()
    {
        string type_check = Request.QueryString["TypeCheck"].ToString();
        if (type_check == "1")
        {            
            hTypeCheck.Value ="1";
            //QC01
            btnAddSLOK.Visible = true;
            btnAddSLErr.Visible = true;
            //QC02
            imgAddSLERRQC01.Visible = false;
            imgAddSLERRQC02.Visible = false;
            imgAddSLOKQC01.Visible = false;
            imgAddSLOKQC02.Visible = false;
            //xet QC
            tdQC.Visible = false;
            tdNV.Visible = true;
           // lblNVKT.Text = "QC cố định";
            lblTitle.Text = "定點QC檢驗回饋(Hồi trả kiểm nghiệm của QC cố định)";
        }
        else
        {
            //xet QC
           // lblNVKT.Text = "NV kiểm tra";
            if (type_check == "2")
            {
                tdQC.Visible = false;
                tdNV.Visible = true;
                lblTitle.Text = "巡檢QC檢驗回饋(Hồi trả kiểm nghiệm của QC kiểm tra)";
            }
            else
            {
                tdQC.Visible = true;
                tdNV.Visible = true;
                lblTitle.Text = "Log Book檢驗回饋(Hồi trả kiểm nghiệm Log Book)";
            }

            hTypeCheck.Value = "0";
            //QC01
            btnAddSLOK.Visible = false;
            btnAddSLErr.Visible = false;
            //QC02

            if (hOdrNo.Value != "")
            {
                imgAddSLERRQC01.Visible = true;
                imgAddSLOKQC01.Visible = true;
                
                imgAddSLERRQC02.Visible = true;
                imgAddSLOKQC02.Visible = true;                
            }
            else
            {
                imgAddSLERRQC01.Visible = false;
                imgAddSLERRQC02.Visible = false;
                imgAddSLOKQC01.Visible = false;
                imgAddSLOKQC02.Visible = false;
            }      
        }
    }

    #region "Get QC cố định"
    private void LoadUserQC()
    {
        string add_date = DateTime.Now.ToString("yyyyMMdd");
        string dep_no=ddlDepNo.SelectedValue.Trim();
        string sec_no = ddlSec_no.SelectedValue.Trim();
        DataTable dt = GetUserQC(dep_no,sec_no);
        ddlQC.DataTextField = "user_desc";
        ddlQC.DataValueField = "user_id";
        ddlQC.DataSource = dt;
        ddlQC.DataBind();
    }
    private DataTable GetUserQC(string dept_no, string sec_no)
    {
        bs_ApplyData mybs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        DataSet dsReturn = mybs.DoReturnDataSet("GETUSERQC", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;
    }
    #endregion

    #region "Get số lượng dat,không đạt,tổng số rút kiểm"
    private void ResetSumQtyToal()
    {
        lblSumRutKiem1.Text = "0";
        //SL không đạt
        lblSLKhongDat1.Text = "0";
        //SL đạt    
        lblSLDat1.Text = "0";
    }
    private void GetSumQtyToal()
    {
        if (hOdrNo.Value != "")
        {
            string vou_no = hOdrNo.Value;
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string fact_no=lblFactNo1.Text.ToString().Trim();
            string dept_no=ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string bad_no="";
            string Type="0";
            string sDateNow = DateTime.Now.ToString("yyyyMMdd");
            if (type_check == "1") //QC cố định
            {

                // Tổng số rút kiểm lấy từ ERP
                DataTable dtTotal = mybs.get_sum_act_qty(fact_no, dept_no, sec_no, vou_no);
                lblSumRutKiem1.Text = dtTotal.Rows[0]["SumTotal"].ToString().Trim();

                //SL không đạt
                DataTable dtQty = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, bad_no,sDateNow, Type);
                lblSLKhongDat1.Text = dtQty.Rows[0]["SumQty"].ToString().Trim();

                //SL đạt
                int SLDat = int.Parse(lblSumRutKiem1.Text) - int.Parse(lblSLKhongDat1.Text);
                lblSLDat1.Text = Convert.ToString(SLDat);
            }
            else
            {
                if (type_check == "2")
                {
                    //SL đạt Type="2"
                    DataTable dtQtyOKitem = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, bad_no,sDateNow, "2");
                    lblSLDat1.Text = dtQtyOKitem.Rows[0]["SumQtyOKitem"].ToString().Trim();

                    //SL không đạt
                    DataTable dtQty = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, bad_no,sDateNow, Type);
                    lblSLKhongDat1.Text = dtQty.Rows[0]["SumQty"].ToString().Trim();
                }
                else
                {
                    //SL đạt
                    string userQC_id = ddlQC.SelectedValue.Trim();
                    DataTable dtQtyOKitem = SumQtyToalQC02(vou_no, fact_no, dept_no, sec_no, bad_no, userQC_id, sDateNow, "5");
                    lblSLDat1.Text = dtQtyOKitem.Rows[0]["SumQty"].ToString().Trim();

                    //SL không đạt
                    DataTable dtQty = SumQtyToalQC02(vou_no, fact_no, dept_no, sec_no, bad_no, userQC_id, sDateNow, "6");
                    lblSLKhongDat1.Text = dtQty.Rows[0]["SumQty"].ToString().Trim();
                }
                // Tổng số rút kiểm
                int SumRutKiem = int.Parse(lblSLKhongDat1.Text) + int.Parse(lblSLDat1.Text);
                lblSumRutKiem1.Text = Convert.ToString(SumRutKiem);
            }
        }
    }
    #endregion

    #region "get Odr_no và bảng lỗi"
    private string GetOdrno()
    {       
        string fact_no = lblFactNo1.Text.ToString().Trim();
        string dept_no = ddlDepNo.SelectedValue.Trim();
        string sec_no = ddlSec_no.SelectedValue.Trim();
        DataTable dtbroot = mybs.GetDataOdrno(fact_no, dept_no, sec_no);
        string sHtml = "";
        foreach (DataRow dr1 in dtbroot.Rows)
        {
            string type_check = Request.QueryString["TypeCheck"].ToString();
            //sHtml += "<a href='QCManager.aspx?TypeCheck=" + type_check + "&dept_no=" + ddlDepNo.SelectedValue.Trim() + "&sec_no=" + ddlSec_no.SelectedValue.Trim() + "&odrno=" + dr1["fact_odr_no"] + "' class='cssAMneu'>" + dr1["fact_odr_no"] + "</a></br>";
            sHtml += "<div class=\"cssAAA\" id=" + dr1["fact_odr_no"].ToString().Trim() + "><a class='cssAMenu' onclick=\"submitOdrnoQC01('" + dr1["fact_odr_no"].ToString().Trim() + "');\"> " + dr1["fact_odr_no"].ToString().Trim() + "</a></div>";
        }
        return sHtml;
    }
    private string GetBadReson()
    {
        string sHtml = "";
        string fact_no = lblFactNo1.Text.ToString().Trim();
        string dept_no = ddlDepNo.SelectedValue.Trim();
        DataTable dtbroot = mybs.GetBbadReason(fact_no, dept_no);
        string type_check = Request.QueryString["TypeCheck"].ToString();

        // tạo table chứa bad_no
        DataTable dtBadNo =new DataTable();

        // tạo table chứa link img
        DataTable dtImgSrc =new DataTable();
        
        //if (Request.QueryString["odrno"] != "" && Request.QueryString["odrno"] != null)
        if(hOdrNo.Value!="")
        {
            //string vou_no = Request.QueryString["odrno"].ToString().Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string vou_no = hOdrNo.Value;
            string userQC_id = ddlQC.SelectedValue.Trim();
            string sDateNow = DateTime.Now.ToString("yyyyMMdd") ;
            if(type_check=="0")
                dtBadNo = SumQtyToalQC02(vou_no, fact_no, dept_no, sec_no, "", userQC_id,sDateNow, "3");
            else
                dtBadNo = GetBadNo(type_check, vou_no, fact_no, dept_no, sec_no, sDateNow);
            dtImgSrc = GetImgLink();
        }
        int NumFor =int.Parse(ConfigurationSettings.AppSettings["NumFor"]);
        int gan = 0;
        sHtml = "<table border='0' style='width:100%;border:0px solid #ccc;border-collapse:collapse'>";
        for (int i = 0; i < dtbroot.Rows.Count; i++)
        {
            string bad_cn = dtbroot.Rows[i]["bad_desc"].ToString();
            string bad_vn = dtbroot.Rows[i]["bad_desc_vn"].ToString();
            string badno = dtbroot.Rows[i]["bad_no"].ToString();
            string src = "../../Images/Icon/smile01.gif";

            // lấy bad_no gán vào
            string bad_no="";
            if (hOdrNo.Value != "") //if (Request.QueryString["odrno"] != "" && Request.QueryString["odrno"] != null)
            {
                DataRow[] drCheckBadNo = dtBadNo.Select("qty is not null and bad_no='" + badno + "'");
                if (drCheckBadNo.Length > 0)
                {
                    bad_no = drCheckBadNo[0].ItemArray[1].ToString();
                    for (int l = 0; l < dtImgSrc.Rows.Count; l++)
                    {
                        string Condition =dtImgSrc.Rows[l]["condition"].ToString();
                        string Count = dtImgSrc.Rows[l]["count"].ToString();
                        string imgType = dtImgSrc.Rows[l]["img_type"].ToString();
                        if (fnOperator(Condition, int.Parse(bad_no), int.Parse(Count)) == true)
                        {
                            src = "../../"+BasicData.GetImgConditionsType(imgType);
                            break;
                        }                        
                    }
                }
            }
         
            if (gan == 0)
                sHtml += "<tr>";

            sHtml += "<td class=\"cssTDIcon\" id=" + badno + " onclick=\"submitBadReason('" + badno + "','" + bad_cn + "','" + bad_vn + "','" + bad_no + "')\">";
            if (bad_no == "0")
                bad_no = "";
            sHtml += "<div style='float:left;text-align:center;font-size:30px;margin-left:5px'><img width='52px' style='vertical-align:text-bottom' src='" + src + "' /><font color=red><b>" + bad_no + "</b></font></div>";
            sHtml += "<div class=\"CssKBadNo\" style='margin-left:70px;'>" + bad_cn + "</br>" + bad_vn + "</div>";                                        
            sHtml += "</td>";

            gan = gan + 1;
            if (gan == NumFor)
                gan = 0;
            if (gan == 0)
            {
                sHtml += "</tr>";
            }
            if (i == dtbroot.Rows.Count - 1 && gan != 0)
            {
                sHtml += "</tr>";
            }

        }
        sHtml += "</table";
        return sHtml;
    }
    #endregion

    public bool fnOperator(string dk, int x, int y)
    {
        switch (dk)
        {
            case ">": return x > y;
            case "<": return x < y;
            case "=": return x == y;
            default: throw new Exception("invalid logic");
        }
        
    }

    private DataTable GetBadNo(string type_check, string vou_no, string fact_no, string dept_no, string sec_no, string add_date)
    {
        bs_ApplyData mybs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("type_check", type_check);
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("add_date", add_date);
        DataSet dsReturn = mybs.DoReturnDataSet("SUMBABNO", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;
    }
    private DataTable GetImgLink()
    {
        bs_ApplyData mybs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
      
        DataSet dsReturn = mybs.DoReturnDataSet("GETIMGCONDITIONS", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;
    }
    private DataTable SumQtyToal(string type_check, string vou_no, string fact_no, string dept_no, string sec_no, string bad_no,string add_date, string Type)
    {
        bs_ApplyData mybs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("type_check", type_check);
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("add_date", add_date);
        myMsg.CreateFirstNode("Type", Type);
        DataSet dsReturn = mybs.DoReturnDataSet("SUMQTYTOAL", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;
    }
    private DataTable SumQtyToalQC02(string vou_no, string fact_no, string dept_no, string sec_no, string bad_no,string userQC_id,string add_date, string Type)
    {
        bs_ApplyData mybs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("userQC_id", userQC_id);
        myMsg.CreateFirstNode("add_date", add_date);        
        myMsg.CreateFirstNode("Type", Type);
        DataSet dsReturn = mybs.DoReturnDataSet("SUMQTYTOALQC02", myMsg.GetXmlStr, "");

        DataTable dt = dsReturn.Tables[0];
        return dt;
    }

    #region "change dropdow"
    protected void ddlDepNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // lấy sec_no
        _DropDownList._Get_ControlSec(ddlSec_no, Session["UserID"].ToString().Trim(), ddlDepNo.SelectedValue.Trim(), Request.QueryString["TypeCheck"].ToString());
        hOdrNo.Value = null;
        hQty.Value = "";
        hBadNo.Value = "";
        RequestType();
        LoadUserQC();
        //idIcon.Visible = false;
        ltrVouNo.Text = GetOdrno();
        ltrBadReason.Text = GetBadReson();
        ResetSumQtyToal();
        
    }
    protected void ddlSec_no_SelectedIndexChanged(object sender, EventArgs e)
    {
        hOdrNo.Value = null;
        hQty.Value = "";
        hBadNo.Value = "";
        RequestType();
        LoadUserQC();
        ltrVouNo.Text = GetOdrno();
        ltrBadReason.Text = GetBadReson();
        ResetSumQtyToal();
    }
    protected void ddlQC_SelectedIndexChanged(object sender, EventArgs e)
    {
        hOdrNo.Value = null;
        hQty.Value = "";
        hBadNo.Value = "";
        RequestType();
        ltrVouNo.Text = GetOdrno();
        ltrBadReason.Text = GetBadReson();
        ResetSumQtyToal();
    }

    #endregion

    #region "Save Data"

    protected void btnAddSLOK_Click(object sender, ImageClickEventArgs e)
    {
        if (lblSLKhongDat1.Text == lblSumRutKiem1.Text)
        {
            Response.Write("<script>alert('" + sNumBer2 + "');</script>");
            return;
        }       
        SaveData("1");      
    }
    protected void btnAddSLErr_Click(object sender, ImageClickEventArgs e)
    {
        if (dtCheckSLQCBadNo() == "0")
        {
            Response.Write("<script>alert('" + sNumBer + "');</script>");
            return;
        }
        SaveData("-1");
    }

    private void SaveData(string qty)
    {
        
        if (hOdrNo.Value != "" )
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
          
            string ac_id = "0";
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string vou_no = hOdrNo.Value;

            //kiểm tra time_no dứơi ERP
            string time_sec = DateTime.Now.ToString("hhmm");
            string time_no="";
            DataTable dtTime_no = mybs.get_time_no(fact_no, dept_no, time_sec);
            if (dtTime_no.Rows.Count > 0)
                time_no = dtTime_no.Rows[0]["time_no"].ToString().Trim();
            else
            {
                Response.Write("<script>alert('" + sTimeNo + "');</script>");
                return;
            }

            // lây total dưới ERP
            DataTable dtTotal = mybs.get_sum_act_qty(fact_no, dept_no, sec_no, vou_no);
            string total_act_qty = dtTotal.Rows[0]["SumTotal"].ToString().Trim();

            //lấy floor
            DataTable dtFloor = mybs.get_floor_erp(fact_no, dept_no, sec_no);
            string floor = "";
            if (dtFloor.Rows.Count > 0)
                floor = dtFloor.Rows[0]["floor"].ToString().Trim();

            //lấy build_no
            DataTable dtBuild = mybs.get_build_no_erp(fact_no, dept_no, sec_no, floor);
            string build_no = "";
            if (dtBuild.Rows.Count > 0)
                build_no = dtBuild.Rows[0]["build_no"].ToString().Trim();

            string myType = "0";
            string ok = SaveApplyCheckData(ac_id, fact_no, dept_no, sec_no, type_check, vou_no, total_act_qty, floor, build_no, myType);

            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(ok);
            string ac_id_e = myMsg.Query("ac_id");
            string Err = myMsg.Query("Err");
            string Mes = myLabel.GetErrMsg("lbl0026", "QC/Tilte"); //"cập nhật thành công !";// myLabelEr.GetErrMsg("msg0004");
            if (Err == "Success!")
            {
                string bad_no = hBadNo.Value;
                string bad_desc = hBad_cn.Value;
                string bad_descVN = hBad_vn.Value;
                string userQC_id = Session["UserID"].ToString().Trim();
               
                string qtyOKitem = "0";
                // câp nhật xuống table ApplyCheckD
                string ok2 = SaveCheckDetail("", ac_id_e, bad_no, bad_desc, bad_descVN, userQC_id, qty, qtyOKitem, "0");
                if (ok2 == "Success!")
                {
                    
                    string rec_date = DateTime.Now.ToString("yyyyMMdd");
                    string kind_mk = "1";
                    string TypeErp = "0";
                    //câp nhật xuống ERP
                    string upd_user = Session["UserEMail"].ToString().Trim();
                    string upd_time=DateTime.Now.ToString("yyyyMMddhhmmss");
                  
                    /*
                    //SL không đạt
                    DataTable dtQty = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, bad_no, rec_date, "4");                  
                    string qtyErp=dtQty.Rows[0]["SumQty"].ToString().Trim();                  
                   */
                    
                    //get sum qty 
                    string cur_time=DateTime.Now.ToString("hhmm");
                    DataTable dtQtyERP = mybs.sum_qty_erp(vou_no, fact_no, sec_no, kind_mk, time_no, rec_date, upd_user, dept_no);
                    
                    //kiểm tra dử liệu dưới ERP? 0 : insert 1: update
                    DataTable dtTest = mybs.testCountBadNoErp(vou_no, fact_no, rec_date, sec_no, bad_no, kind_mk, upd_user, dept_no, time_no);                   
                    if (dtTest.Rows[0]["kt"].ToString()!="0")
                        TypeErp = "1";

                    string qtyErpN = "0";// dtQtyERP.Rows[0]["SumQty"].ToString().Trim();
                    int kq=0;                  
                    if(dtQtyERP.Rows.Count>0)
                        qtyErpN = dtQtyERP.Rows[0]["SumQty_chk"].ToString().Trim();
                    kq = int.Parse(total_act_qty) - int.Parse(qtyErpN);
                    string qty_chk =  kq.ToString();

                    string qtyErp = qty;
                  
                    string ok3=mybs.SaveDataErp(fact_no, rec_date, vou_no, sec_no, bad_no, time_no, type_check, upd_user, upd_time, qtyErp, qty_chk,dept_no, TypeErp);
                    //if(ok3=="OK")
                        //Response.Write("<script>alert('"+Mes+"');</script>");
                    if (ok3 != "OK")
                    {
                        Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                        return;
                    }

                }
                else
                {
                    Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                    return;
                }
                ltrBadReason.Text = GetBadReson();
                GetSumQtyToal();
               
            }
            else
            {
                Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                return;
            }
        }
    
    }
    public string SaveApplyCheckData(string ac_id, string fact_no, string dept_no, string sec_no, string type_check, string vou_no,string total, string floor, string build_no, string myType)
    {
        bs_ApplyData myBs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ac_id", ac_id);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("type_check", type_check);
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("total", total);
        myMsg.CreateFirstNode("floor", floor);
        myMsg.CreateFirstNode("build_no", build_no);
        myMsg.CreateFirstNode("Type", myType);

        string strXML = myMsg.GetXmlStr;
        string strReturn = myBs.DoReturnStr("INSUPDAPPLYCHECK", strXML, "");
        myMsg.LoadXml(strReturn);
        return strReturn;

        //return myMsg.Query("Err");
    }

    #endregion

    #region "Save Data Detail"
    public string SaveCheckDetail(string appd_id, string ac_id, string bad_no, string bad_desc, string bad_descVN, string userQC_id, string qty,string qtyOKitem, string myType)
    {
        bs_ApplyData myBs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("appd_id", appd_id);
        myMsg.CreateFirstNode("ac_id", ac_id);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("bad_desc", bad_desc);
        myMsg.CreateFirstNode("bad_descVN", bad_descVN);
        myMsg.CreateFirstNode("userQC_id", userQC_id);
        myMsg.CreateFirstNode("qty", qty);
        myMsg.CreateFirstNode("qtyOKitem", qtyOKitem);        
        myMsg.CreateFirstNode("Type", myType);
        string strXML = myMsg.GetXmlStr;
        string strReturn = myBs.DoReturnStr("INSUPDAPPLYCHECKDETAIL", strXML, "");
        myMsg.LoadXml(strReturn);
        //return strReturn;
        return myMsg.Query("Err");
    }

    #endregion

    #region "Save data QC"

    private void SaveDataQC(string qtyOKitem,string TypeQC)
    {
        if (hOdrNo.Value != "")
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
            int item = int.Parse(qtyOKitem);
            string ac_id = "0";
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string vou_no = hOdrNo.Value;

            //kiểm tra time_no dứơi ERP
            string time_sec = DateTime.Now.ToString("hhmm");
            string time_no = "";
            DataTable dtTime_no = mybs.get_time_no(fact_no, dept_no, time_sec);
            if (dtTime_no.Rows.Count > 0)
                time_no = dtTime_no.Rows[0]["time_no"].ToString().Trim();
            else
            {
                Response.Write("<script>alert('" + sTimeNo + "');</script>");
                return;
            }

            // lây total dưới SQL
            //DataTable dtTotal = mybs.get_sum_act_qty(fact_no, dept_no, sec_no, vou_no);
            string total = lblSumRutKiem1.Text;// dtTotal.Rows[0]["SumTotal"].ToString().Trim();

            //lấy floor
            DataTable dtFloor = mybs.get_floor_erp(fact_no, dept_no, sec_no);
            string floor = "";
            if (dtFloor.Rows.Count > 0)
                floor = dtFloor.Rows[0]["floor"].ToString().Trim();

            //lấy build_no
            DataTable dtBuild = mybs.get_build_no_erp(fact_no, dept_no, sec_no, floor);
            string build_no = "";
            if (dtBuild.Rows.Count > 0)
                build_no = dtBuild.Rows[0]["build_no"].ToString().Trim();

            string myType = "0";
            string ok = SaveApplyCheckDataQC(ac_id, fact_no, dept_no, sec_no, type_check, vou_no, total, floor, build_no, myType);

            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(ok);
            string ac_id_e = myMsg.Query("ac_id");
            string Err = myMsg.Query("Err");
            string Mes = myLabel.GetErrMsg("lbl0026", "QC/Tilte");
            if (Err == "Success!")
            {
                string bad_no = hBadNo.Value;
                string bad_desc = hBad_cn.Value;
                string bad_descVN = hBad_vn.Value;
                string userQC_id = Session["UserID"].ToString().Trim();

                string qty = "0";
                string qty_OKitem = "0";
                if (TypeQC == "0")
                    qty_OKitem = qtyOKitem;
                else
                    qty = qtyOKitem;

                // câp nhật xuống table ApplyCheckD
                string ok2 = "";
                if (TypeQC == "0")
                    ok2 = SaveCheckDetail("", ac_id_e, "", "", "", userQC_id, qty, qty_OKitem, "0");
                else
                    ok2 = SaveCheckDetail("", ac_id_e, bad_no, bad_desc, bad_descVN, userQC_id, qty, "", "0");

                if (ok2 == "Success!")
                {
                    if (TypeQC == "0")
                    {
                        bad_no = "     ";
                        bad_desc = "";
                        bad_descVN = "";
                    }

                    //kiểm tra dử liệu dưới ERP? 0 : insert 1: update
                    string rec_date = DateTime.Now.ToString("yyyyMMdd");
                    string upd_user = Session["UserEMail"].ToString().Trim();
                    string upd_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    // trường hơp TypeQC == "0" SL đạt -- kiểm tra (fact_no,rec_date,fact_odr_no,sec_no)
                    string kind_mk = "2";
                    string TypeErp = "0";

                    DataTable dtTest = mybs.testCountVouNOErp(vou_no, fact_no, rec_date, bad_no, sec_no, kind_mk);
                    if (dtTest.Rows.Count > 0)
                        TypeErp = "1";


                    //get sum qty dua vao bad_no
                    DataTable dtSumQtyByBadBo = mybs.sum_qty_02ByBad_no(vou_no, fact_no, rec_date, sec_no, bad_no, kind_mk);
                    int sum_qty_bad_no = int.Parse(dtSumQtyByBadBo.Rows[0]["SumQtyChk"].ToString());
                    sum_qty_bad_no = sum_qty_bad_no + item;
                    string qtyErp = Convert.ToString(sum_qty_bad_no);

                    //get sum qty => qty_chk
                    DataTable dtQtyCheck = mybs.sum_qty_02(vou_no, fact_no, rec_date, sec_no, kind_mk);
                    int sum_qty = int.Parse(dtQtyCheck.Rows[0]["SumQtyChk"].ToString());

                    string qty_chk = Convert.ToString(sum_qty + item); //dtQtyCheck.Rows[0]["SumQtyChk"].ToString().Trim();                  
                    /*
                     string qtyErp = qtyOKitem;
                     string qty_chk = qtyOKitem;
                     */
                    //câp nhật xuống ERP
                    string ok3 = mybs.SaveDataErpQC02(fact_no, rec_date, vou_no, sec_no, bad_no, time_no, type_check, upd_user, upd_time, qtyErp, qty_chk, dept_no, TypeErp);
                    //if (ok3 == "OK")
                        //Response.Write("<script>alert('" + Mes + "');</script>");
                    if (ok3 != "OK")
                    {
                        Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                        return;
                    }
                }
                else
                {
                    Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                    return;
                }
                ltrBadReason.Text = GetBadReson();
                GetSumQtyToal();

            }
            else
            {
                Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                return;
            }
        }
    }
    public string SaveApplyCheckDataQC(string ac_id, string fact_no, string dept_no, string sec_no, string type_check, string vou_no, string total, string floor, string build_no, string myType)
    {
        bs_ApplyData myBs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("ac_id", ac_id);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("type_check", type_check);
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("total", total);
        myMsg.CreateFirstNode("floor", floor);
        myMsg.CreateFirstNode("build_no", build_no);
        myMsg.CreateFirstNode("Type", myType);

        string strXML = myMsg.GetXmlStr;
        string strReturn = myBs.DoReturnStr("INSUPDAPPLYCHECKQ", strXML, "");
        myMsg.LoadXml(strReturn);
        return strReturn;

        //return myMsg.Query("Err");
    }

    protected void imgAddSLOKQC01_Click(object sender, ImageClickEventArgs e)
    {
        string type_check = Request.QueryString["TypeCheck"].ToString();
        if (type_check == "2")
        {
            SaveDataQC("1", "0");
            hQty.Value = "";
            hBadNo.Value = "";
        }
        else
        {
            if (ddlQC.SelectedValue == "")
            {
                Response.Write("<script>alert('" + sUserQC + "');</script>");
                return;
            }
            SaveApplyRating("1", "0");
            hQty.Value = "";
            hBadNo.Value = "";
        }
    }

  
    private string dtCheckSLQC()
    {
        string ok = "0";
        if (hOdrNo.Value != "")
        {
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string vou_no = hOdrNo.Value;
            string userQC_id = ddlQC.SelectedValue.Trim();
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string bad_no = hBadNo.Value;
            string sDateNow = DateTime.Now.ToString("yyyyMMdd");
            DataTable dt = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, "",sDateNow, "2");
            if (dt.Rows.Count > 0)
            {
                int sl = int.Parse(dt.Rows[0]["SumQtyOKitem"].ToString().Trim());
                if (sl > 0)
                    ok = "1";
            }
        }
        return ok;
    }
    private string dtCheckSLQCBadNo()
    {
        string ok = "0";
        if (hOdrNo.Value != "")
        {
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string vou_no = hOdrNo.Value;
            string userQC_id = ddlQC.SelectedValue.Trim();
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string bad_no = hBadNo.Value;
            string sDateNow = DateTime.Now.ToString("yyyyMMdd");
            DataTable dt = SumQtyToal(type_check, vou_no, fact_no, dept_no, sec_no, bad_no,sDateNow, "1");
            if (dt.Rows.Count > 0)
            {
                int sl = int.Parse(dt.Rows[0]["SumQty"].ToString().Trim());
                if (sl > 0)
                    ok = "1";
            }
        }
        return ok;
    }

    private string dtCheckSLQC02()
    {
        string ok = "0";
        if (hOdrNo.Value != "")
        {
            string sDateNow = DateTime.Now.ToString("yyyyMMdd");
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string vou_no = hOdrNo.Value;
            string userQC_id = ddlQC.SelectedValue.Trim();
            DataTable dt = SumQtyToalQC02(vou_no, fact_no, dept_no, sec_no, "", userQC_id,sDateNow, "5");
            if (dt.Rows.Count > 0)
            {
                int sl=int.Parse(dt.Rows[0]["SumQty"].ToString().Trim());
                if (sl>0)
                    ok = "1";
            }
        }      
        return ok;
    }
    private string dtCheckSLQC02BadNo()
    {
        string ok = "0";
        if (hOdrNo.Value != "")
        {
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string vou_no = hOdrNo.Value;
            string bad_no = hBadNo.Value;
            string userQC_id = ddlQC.SelectedValue.Trim();
            string sDateNow = DateTime.Now.ToString("yyyyMMdd");
            DataTable dt = SumQtyToalQC02(vou_no, fact_no, dept_no, sec_no, bad_no, userQC_id,sDateNow, "4");
            if (dt.Rows.Count > 0)
            {
                int sl = int.Parse(dt.Rows[0]["SumQty"].ToString().Trim());
                if (sl > 0)
                    ok = "1";
            }
        }
        return ok;
    }
   
    protected void imgAddSLERRQC01_Click(object sender, ImageClickEventArgs e)
    {
        string type_check = Request.QueryString["TypeCheck"].ToString();
        if (type_check == "2")
        {
            if (dtCheckSLQC() == "0")
            {
                Response.Write("<script>alert('"+sNumBer+"');</script>");
                return;
            }
            SaveDataQC("-1", "0");
            hQty.Value = "";
            hBadNo.Value = "";
        }
        else
        {
            if (ddlQC.SelectedValue == "")
            {
                Response.Write("<script>alert('" + sUserQC + "');</script>");
                return;
            }
            if (dtCheckSLQC02() == "0")
            {
                Response.Write("<script>alert('" + sNumBer + "');</script>");
                return;
            }
            SaveApplyRating("-1", "0");
            hQty.Value = "";
            hBadNo.Value = "";
        }
    }
    protected void imgAddSLOKQC02_Click(object sender, ImageClickEventArgs e)
    {
        string type_check = Request.QueryString["TypeCheck"].ToString();
        if (type_check == "2")
        {
            SaveDataQC("1", "1");
        }
        else
        {
            if (ddlQC.SelectedValue == "")
            {
                Response.Write("<script>alert('" + sUserQC + "');</script>");
                return;
            }
            SaveApplyRating("1", "1");
        }
    }
    protected void imgAddSLERRQC02_Click(object sender, ImageClickEventArgs e)
    {
        string type_check = Request.QueryString["TypeCheck"].ToString();
        if (type_check == "2")
        {
            if (dtCheckSLQCBadNo() == "0")
            {
                Response.Write("<script>alert('" + sNumBer + "');</script>");
                return;
            }
            SaveDataQC("-1", "1");
        }
        else
        {
            if (ddlQC.SelectedValue == "")
            {
                Response.Write("<script>alert('" + sUserQC + "');</script>");
                return;
            }

            if (dtCheckSLQC02BadNo() == "0")
            {
                Response.Write("<script>alert('" + sNumBer + "');</script>");
                return;
            }
            SaveApplyRating("-1", "1");
        }
    }
    #endregion

    #region "Apply Rating"
    private void SaveApplyRating(string qtyOKitem, string TypeQC)
    {
        if (hOdrNo.Value != "")
        {
            PccErrMsg myLabel = new PccErrMsg(Server.MapPath("~") + "/XmlDoc", Session["CodePage"].ToString(), "Label");
            int item = int.Parse(qtyOKitem);
            string AcRat_id = "0";
            string fact_no = lblFactNo1.Text.ToString().Trim();
            string dept_no = ddlDepNo.SelectedValue.Trim();
            string sec_no = ddlSec_no.SelectedValue.Trim();
            string type_check = Request.QueryString["TypeCheck"].ToString();
            string vou_no = hOdrNo.Value;

            //kiểm tra time_no dứơi ERP
            string time_sec = DateTime.Now.ToString("hhmm");
            string time_no = "";
            DataTable dtTime_no = mybs.get_time_no(fact_no, dept_no, time_sec);
            if (dtTime_no.Rows.Count > 0)
                time_no = dtTime_no.Rows[0]["time_no"].ToString().Trim();
            else
            {
                Response.Write("<script>alert('" + sTimeNo + "');</script>");
                return;
            }

            // lây total dưới SQL         
            string total = qtyOKitem;// lblSumRutKiem1.Text;// dtTotal.Rows[0]["SumTotal"].ToString().Trim();

            //lấy floor
            DataTable dtFloor = mybs.get_floor_erp(fact_no, dept_no, sec_no);
            string floor = "";
            if (dtFloor.Rows.Count > 0)
                floor = dtFloor.Rows[0]["floor"].ToString().Trim();

            //lấy build_no
            DataTable dtBuild = mybs.get_build_no_erp(fact_no, dept_no, sec_no, floor);
            string build_no = "";
            if (dtBuild.Rows.Count > 0)
                build_no = dtBuild.Rows[0]["build_no"].ToString().Trim();

            string userQC_id = ddlQC.SelectedValue.Trim();
            string upd_id = Session["UserID"].ToString().Trim();
            string myType = "0";
            string ok = SaveApplyRating(AcRat_id, fact_no, dept_no, sec_no, vou_no, total, floor, build_no, userQC_id, upd_id, myType);

            PccMsg myMsg = new PccMsg();
            myMsg.LoadXml(ok);
            string AcRat_id_e = myMsg.Query("AcRat_id");
            string Err = myMsg.Query("Err");
            string Mes = myLabel.GetErrMsg("lbl0026", "QC/Tilte");
            if (Err == "Success!")
            {
                string bad_no = hBadNo.Value;
                string bad_desc = hBad_cn.Value;
                string bad_descVN = hBad_vn.Value;
               

                string qty = "0";
                string qty_OKitem = "0";
                if (TypeQC == "0")
                    qty_OKitem = qtyOKitem;
                else
                    qty = qtyOKitem;

                // câp nhật xuống table RatingD
                string ok2 = "";
                if (TypeQC == "0")
                    ok2 = SaveApplyRatingDetail("", AcRat_id_e, "", "", "",  qty, qty_OKitem,upd_id, "0");
                else
                    ok2 = SaveApplyRatingDetail("", AcRat_id_e, bad_no, bad_desc, bad_descVN,qty, "",upd_id, "0");

                //if (ok2 == "Success!")
                    //Response.Write("<script>alert('"+Mes+"');</script>");
                if (ok2 != "Success!")
                {
                    Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                    return;
                }
                  
                ltrBadReason.Text = GetBadReson();
                GetSumQtyToal();
            }
            else
            {
                Response.Write("<script>alert('Error, Please contact administrator !');</script>");
                return;
            }
        }
    }
    public string SaveApplyRating(string AcRat_id, string fact_no, string dept_no, string sec_no, string vou_no, string total, string floor, string build_no,string userQC_id,string upd_id, string myType)
    {
        bs_ApplyData myBs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("AcRat_id", AcRat_id);
        myMsg.CreateFirstNode("fact_no", fact_no);
        myMsg.CreateFirstNode("dept_no", dept_no);
        myMsg.CreateFirstNode("sec_no", sec_no);
        myMsg.CreateFirstNode("vou_no", vou_no);
        myMsg.CreateFirstNode("total", total);
        myMsg.CreateFirstNode("floor", floor);
        myMsg.CreateFirstNode("build_no", build_no);
        myMsg.CreateFirstNode("userQC_id", userQC_id);
        myMsg.CreateFirstNode("upd_id", upd_id);
        myMsg.CreateFirstNode("Type", myType);
       
        string strXML = myMsg.GetXmlStr;
        string strReturn = myBs.DoReturnStr("INSUPDAPPLYRATING", strXML, "");
        myMsg.LoadXml(strReturn);
        return strReturn;

        //return myMsg.Query("Err");
    }
    #endregion

    #region "Save Apply Rating Detail"
    public string SaveApplyRatingDetail(string id, string AcRat_id, string bad_no, string bad_desc, string bad_descVN,  string qty, string qtyOKitem,string add_id, string myType)
    {
        bs_ApplyData myBs = new bs_ApplyData(ConfigurationSettings.AppSettings["AppConnectionType"], ConfigurationSettings.AppSettings["AppConnectionServer"], ConfigurationSettings.AppSettings["AppConnectionDB"], ConfigurationSettings.AppSettings["AppConnectionUser"], ConfigurationSettings.AppSettings["AppConnectionPwd"], "", ConfigurationSettings.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("id", id);
        myMsg.CreateFirstNode("AcRat_id", AcRat_id);
        myMsg.CreateFirstNode("bad_no", bad_no);
        myMsg.CreateFirstNode("bad_desc", bad_desc);
        myMsg.CreateFirstNode("bad_descVN", bad_descVN);
        myMsg.CreateFirstNode("qty", qty);
        myMsg.CreateFirstNode("qtyOKitem", qtyOKitem);
        myMsg.CreateFirstNode("add_id", add_id);
        myMsg.CreateFirstNode("Type", myType);
        string strXML = myMsg.GetXmlStr;
        string strReturn = myBs.DoReturnStr("INSUPDAPPLYRATINGDETAIL", strXML, "");
        myMsg.LoadXml(strReturn);
        //return strReturn;
        return myMsg.Query("Err");
    }

    #endregion


   
}