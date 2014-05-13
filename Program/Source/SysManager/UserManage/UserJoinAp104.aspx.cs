using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using PccBsSystemForC;
using System.Configuration;


	/// <summary>
	/// UserJoinAp104 的摘要描述。
	/// </summary>
public partial class SysManager_UserManage_UserJoinAp104 : System.Web.UI.Page
{
		//protected System.Web.UI.WebControls.Label lblUserDesc;
		
		

		private const string USERMANAGE = "UserManage104.aspx";
		private int m_count = 0;
		private string m_apid = "";

		public string GrideCount
		{
			get
			{
				m_count += 1;
				return m_count.ToString();
			}
			set 
			{
				m_count = int.Parse(value);
			}
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"] == null) return;
			
			// 將使用者程式碼置於此以初始化網頁
			m_apid = Request.QueryString["ApID"].ToString();
			if (! IsPostBack)
			{
				PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"),Session["CodePage"].ToString() ,"Label");
				bs_UserManager mybs = new bs_UserManager(ConfigurationSettings.AppSettings["ConnectionType"] , ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"],Session["UserIDAndName"].ToString(),ConfigurationSettings.AppSettings["EventLogPath"]);
				SetLabel(ref myLabel);
				//SetddlDept(ref myLabel,ref mybs);
				BindFactData(ref myLabel,ref mybs);
			//	BindDataGrid("user_desc",string.Empty,string.Empty,"0",ref mybs);
			}
			else
			{
				PanelGrid.Visible = true;
			}
			
		}


		#region "設定此頁面的基本資料"

		private void SetLabel(ref PccErrMsg myLabel)
		{
			//lblTitle.Text = "加入使用者至應用程式";
			//設定抬頭 20040607
			for(int i = 0 ; i < Header1.Controls.Count ; i++)
			{
				if (Header1.Controls[i].ID == "PccTitle")
				{
					Label mylblTitle = (Label)(Header1.Controls[i]);
					mylblTitle.Text = "加入使用者至應用程式"; 
				}
			}
			btnAddComeOn.Text = myLabel.GetErrMsg("lbl0002","SysManager/UserManager");
			btnQuery.Text = myLabel.GetErrMsg("btnQuery");
			btnCancel.Text = myLabel.GetErrMsg("btnCancel");

			lblFact.Text = "廠別：";
			//lblUserDesc.Text = myLabel.GetErrMsg("lbl0013","SysManager/UserManager");
			
			//Set DataGrid HeaderText
			DataGrid1.Columns[0].HeaderText = myLabel.GetErrMsg("lbl0004","ADTPurDoc/GroupManage");//編號
			DataGrid1.Columns[2].HeaderText = myLabel.GetErrMsg("lbl0030","ADTPurDoc/GroupManage");//電子郵件帳號
			DataGrid1.Columns[4].HeaderText = myLabel.GetErrMsg("lbl0005","ADTPurDoc/GroupManage");//使用者名稱
			DataGrid1.Columns[5].HeaderText = myLabel.GetErrMsg("lbl0006","ADTPurDoc/GroupManage");//廠別
			//DataGrid1.Columns[5].HeaderText = myLabel.GetErrMsg("lbl0007","ADTPurDoc/GroupManage");//部門別
			DataGrid1.Columns[3].HeaderText = myLabel.GetErrMsg("lbl0031","ADTPurDoc/GroupManage");//帳號
			DataGrid1.Columns[6].HeaderText = myLabel.GetErrMsg("lbl0008","ADTPurDoc/GroupManage");//選取
        
		}

		private void BindFactData(ref PccCommonForC.PccErrMsg myLabel,ref PccBsSystemForC.bs_UserManager mybs)
		{

			DataSet ds;
			DataTable dt;
			DataRow myRow;
			ds = mybs.DoReturnDataSet("GetFactDataBySecurity","","");
			dt = ds.Tables["Fact"];

			myRow = dt.NewRow();
			myRow["fact_id"] = 0;
			myRow["fact_nm"] = "bbb";
			myRow["fact_desc"] = myLabel.GetErrMsg("SelectPlease") ;
			dt.Rows.InsertAt(myRow,0);
 
			ddlfact_id.DataSource = dt.DefaultView;
			ddlfact_id.DataTextField = "fact_desc";
			ddlfact_id.DataValueField = "fact_id";

			ddlfact_id.DataBind(); 
		}

		private void SetddlDept(ref PccCommonForC.PccErrMsg myLabel,ref bs_UserManager mybs)
		{
			
			DataTable dt = mybs.DoReturnDataSet("GetDeptAllData","","").Tables["Dept"];

			DataRow myRow = dt.NewRow();
			myRow["dept_id"] = 0;
			myRow["dept_no"] = "aaa";
			myRow["dept_nm"] = "bbb";
			myRow["dept_desc"] = myLabel.GetErrMsg("SelectPlease") ;
			dt.Rows.InsertAt(myRow,0);
 
			//			ddldept_id.DataSource = dt.DefaultView;
			//			ddldept_id.DataTextField = "dept_desc";
			//			ddldept_id.DataValueField = "dept_id";
			//			ddldept_id.DataBind();
		}

        private void BindDataGrid(string strOrder, string strUserDesc, string strUserNm, string strDeptID, ref bs_UserManager mybs, int iPage)
		{
            PccMsg myMsg = new PccMsg();
            myMsg.CreateFirstNode("ap_id", Request.QueryString["ApID"]);
            myMsg.CreateFirstNode("dept_id", strDeptID);
            myMsg.CreateFirstNode("user_desc", strUserDesc);
            myMsg.CreateFirstNode("user_nm", strUserNm);
            myMsg.CreateFirstNode("order", strOrder);

            //加入判斷是否要有事業群之判斷20041116
            myMsg.CreateFirstNode("user_id", Session["UserID"].ToString());
            GetMenuAuth myAuth = new GetMenuAuth();
            myAuth.AspxFile = "UserManage104.aspx";

            //判斷是否要利用事業群來分設權限
            if (ConfigurationManager.AppSettings[m_apid + "-FactByGroup"] != null && ConfigurationManager.AppSettings[m_apid + "-FactByGroup"].ToString() == "Y")
            {
                //判斷此使用者是否可以分配不同的事業群權限
                if (!myAuth.IsReportAuth())
                {
                    //表示他必須區分事業群
                    myMsg.CreateFirstNode("GroupFilter", "Y");
                }
                else
                {
                    myMsg.CreateFirstNode("GroupFilter", "N");
                }
            }
            else
            {
                myMsg.CreateFirstNode("GroupFilter", "N");
            }


            if (ConfigurationManager.AppSettings[m_apid + "-FactFilter"] != null && ConfigurationManager.AppSettings[m_apid + "-FactFilter"].ToString() == "Y")
            {
                //表示他必須區分廠管理 20041118 注意要區別廠管理的先決條件是要區分事業群
                myMsg.CreateFirstNode("FactFilter", "Y");
            }


            DataSet ds = mybs.DoReturnDataSet("GetAnotherUser", myMsg.GetXmlStr, "");
            ViewState["order"] = strOrder;

            if (ds != null)
            {
                DataGrid1.DataSource = ds.Tables["AnotherUser"].DefaultView;
                //  DataGrid1.DataBind();
                PageControl1.BindDataGrid = DataGrid1;
                PageControl1.CurrentPageIndex = iPage;

            }
            else
            {
                btnAddComeOn.Visible = false;
            }


		}

		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 此呼叫為 ASP.NET Web Form 設計工具的必要項。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 此為設計工具支援所必需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(USERMANAGE + "?ApID=" + Request.QueryString["ApID"] + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			bs_UserManager mybs = new bs_UserManager(ConfigurationSettings.AppSettings["ConnectionType"] , ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"],Session["UserIDAndName"].ToString(),ConfigurationSettings.AppSettings["EventLogPath"]);
			//BindDataGrid("user_desc",txtUserDesc.Text,ddldept_id.SelectedItem.Value,ref mybs);
			if (ddlQuerySelect.SelectedItem.Value.ToString() == "1")
			{
				BindDataGrid("user_desc",txtUserDesc.Text,string.Empty,ddlfact_id.SelectedItem.Value,ref mybs,-1);  
			}
			else
			{
				BindDataGrid("user_desc",string.Empty,txtUserDesc.Text,ddlfact_id.SelectedItem.Value,ref mybs,-1);  
			}
			
		}

		protected void btnAddComeOn_Click(object sender, System.EventArgs e)
		{
			bs_UserManager mybs = new bs_UserManager(ConfigurationSettings.AppSettings["ConnectionType"] , ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"],Session["UserIDAndName"].ToString(),ConfigurationSettings.AppSettings["EventLogPath"]);
			PccMsg myMsg = new PccMsg();
			string strUserID,strReturn,errormsg = "",strXML = "";
			bool bSelect = false;
			

			for (int i = 0; i < DataGrid1.Items.Count ; i++)
			{
				if (((CheckBox)DataGrid1.Items[i].Cells[6].Controls[1]).Checked)
				{
					strUserID = DataGrid1.Items[i].Cells[1].Text;
					myMsg.LoadXml(); 
					myMsg.CreateFirstNode("ap_id",Request.QueryString["ApID"]);
					myMsg.CreateFirstNode("user_id",strUserID);
					myMsg.CreateFirstNode("mana_mk","N");
					myMsg.CreateFirstNode("upd_id",Session["UserID"].ToString());

					//strXML = GetUserXML(strUserID);

					strReturn = mybs.DoReturnStr("JoinUserToAp",myMsg.GetXmlStr,"");
  
					myMsg.LoadXml(strReturn);

					if (myMsg.Query("returnValue") != "0")
					{
						lblMsg.Font.Size = FontUnit.Large;
						lblMsg.Text = myMsg.Query("errmsg"); 
						break;
					}
 
					bSelect = true;
				}
			}

			if (bSelect)
			{
				if (myMsg.Query("returnValue") == "0")
				{
					Response.Redirect(USERMANAGE + "?ApID=" + Request.QueryString["ApID"] + "&QueryCondition=" + Request.QueryString["QueryCondition"]);
				}
				else
				{
					lblMsg.Font.Size = FontUnit.Large;
					if (errormsg == "")
						lblMsg.Text = myMsg.Query("errmsg"); 
					else
						lblMsg.Text = errormsg; 

				}
			}
			else
			{
				PccErrMsg myLabel = new PccErrMsg(Server.MapPath(Session["PageLayer"] + "XmlDoc"),Session["CodePage"].ToString() ,"Label");
				lblMsg.Font.Size = FontUnit.Large;
				if (errormsg == "")
					lblMsg.Text = myLabel.GetErrMsg("lbl0014","SysManager/UserManager");  
				else
					lblMsg.Text = errormsg; 
				
			}
		
		}
		
		private string GetUserXML(string user_id)
		{
			PccBsSystemForC.bs_UserManager mybs = new PccBsSystemForC.bs_UserManager(ConfigurationSettings.AppSettings["ConnectionType"] , ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"],Session["UserIDAndName"].ToString(),ConfigurationSettings.AppSettings["EventLogPath"]);
			PccCommonForC.PccMsg myMsg = new PccCommonForC.PccMsg();
			myMsg.CreateFirstNode("user_id",user_id); 
			
			PccMsg myReturnMsg = new PccMsg();
			myReturnMsg.LoadXml(mybs.DoReturnStr("GetUserData",myMsg.GetXmlStr,""));

			//10/6還要繼續做XML的方法 20041006
			string fact_id = myReturnMsg.Query("fact_id"); 
			myReturnMsg.CreateFirstNode("fact_no", GetFactNoByFactID(fact_id));

			return myReturnMsg.GetXmlStr; 

		}

		private string GetFactNoByFactID(string fact_id)
		{
			PccMsg myMsg = new PccMsg();
			myMsg.CreateFirstNode("ap_id","0");
			myMsg.CreateFirstNode("fact_no","");
			myMsg.CreateFirstNode("fact_nm","");
			
			bs_UserFactManage mybs = new bs_UserFactManage(ConfigurationSettings.AppSettings["ConnectionType"] , ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"],Session["UserIDAndName"].ToString(),ConfigurationSettings.AppSettings["EventLogPath"]);
			DataSet ds = mybs.DoReturnDataSet("GetQFactByApID",myMsg.GetXmlStr,"");

			DataTable dt = ds.Tables["QFact"];
			DataRow[] drs = dt.Select("fact_id = " + fact_id);
			return drs[0]["fact_no"].ToString(); 
		}
        protected void PageControl1_PageIndexChanged(object sender, System.EventArgs e)
        {
            bs_UserManager mybs = new bs_UserManager(ConfigurationSettings.AppSettings["ConnectionType"], ConfigurationSettings.AppSettings["ConnectionServer"], ConfigurationSettings.AppSettings["ConnectionDB"], ConfigurationSettings.AppSettings["ConnectionUser"], ConfigurationSettings.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationSettings.AppSettings["EventLogPath"]);
            BindDataGrid("user_desc", txtUserDesc.Text, string.Empty, ddlfact_id.SelectedItem.Value, ref mybs, PageControl1.NewPageIndex);
        }
	}

