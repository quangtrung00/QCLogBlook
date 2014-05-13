namespace WebApply.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for PageControl.
	/// </summary>
	public partial class PageControl : System.Web.UI.UserControl
	{
		
		protected DataGrid dgMaster;
		
		private int iNewPageIndex;
 
		public event EventHandler PageIndexChanged;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if(!IsPostBack)
			{
				if(Request.Params["PageIdx"] !=null)
				{
					if(IsNumeric(Request.Params["PageIdx"]))
					{
						CurrentPageIndex = int.Parse(Request.Params["PageIdx"]) - 1;
						if(PageIndexChanged !=null ) PageIndexChanged(this, e);
					}
				}
    
			}      
		}
		
		public int PageCount
		{
			get
			{
				return int.Parse(txtPageCount.Value);
			}
		}
 
		public int NewPageIndex
		{
			get
			{
				return iNewPageIndex;
			}
		}
		public int  CurrentPageIndex
		{
			get
			{
				return int.Parse(txtPageCount.Value);
				
			}
			set
			{
				int curPageSize = 17;
				if(!IsPostBack)
				{ 
					if(dgMaster.PageSize>0)
						curPageSize = dgMaster.PageSize;
					else if(System.Configuration.ConfigurationSettings.AppSettings["PageSize"] != null)
						curPageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
				}
				else
				{
					curPageSize = int.Parse(txtPageSize.Value.Trim());
					if(curPageSize<1)
					{
						if(System.Configuration.ConfigurationSettings.AppSettings["PageSize"] != null)
							curPageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
						else
							curPageSize = 17;
					}
				}

				dgMaster.PageSize = curPageSize;
				//int iPageCount = (((DataTable)dgMaster.DataSource).Rows.Count + dgMaster.PageSize - 1) / (dgMaster.PageSize);
				Object a=dgMaster.DataSource;
				int b=0;
				try
				{
					b=((DataTable)dgMaster.DataSource).Rows.Count;
				}
				catch
				{
					b=((DataView)dgMaster.DataSource).Table.Rows.Count;
				}

				//int iPageCount = (((DataTable)dgMaster.DataSource).Rows.Count + dgMaster.PageSize - 1) / (dgMaster.PageSize);
				int iPageCount = (b + dgMaster.PageSize - 1) / (dgMaster.PageSize);

				int pageIndex = int.Parse(txtPageIndex.Value);
 
				switch(value)
				{
					case -1:
						dgMaster.CurrentPageIndex = 0;
						break;
					case -2:
						if(pageIndex >0 ) dgMaster.CurrentPageIndex =pageIndex-1;
						break;
					case -3:
						if (pageIndex<iPageCount - 1)  dgMaster.CurrentPageIndex =pageIndex + 1;
						break;
					case -4:
						if(iPageCount>0) dgMaster.CurrentPageIndex=iPageCount - 1;
						break;
     
					default:
						if(value<0) value=0;
						if(value >=iPageCount ) value=iPageCount - 1;
						if (iPageCount>0) dgMaster.CurrentPageIndex=value;
						break;
 
				}
				iNewPageIndex=dgMaster.CurrentPageIndex;
				txtPageIndex.Value=dgMaster.CurrentPageIndex.ToString();
				txtPageCurrentIndex.Value = (dgMaster.CurrentPageIndex+1).ToString();
				txtPageSize.Value=dgMaster.PageSize.ToString();
				txtPageCount.Value=iPageCount.ToString();
				FirstPage.Enabled=(dgMaster.CurrentPageIndex>0);
				PreviousPage.Enabled=FirstPage.Enabled;
				NextPage.Enabled=(iPageCount>1 & dgMaster.CurrentPageIndex<iPageCount - 1);
				LastPage.Enabled=NextPage.Enabled;
				dgMaster.DataBind();
				if(iPageCount>0)
				{
					lblPage.Text =(dgMaster.CurrentPageIndex+1) +  "/" + iPageCount;
				}
				else
				{
					lblPage.Text="0/0";
				}
			}
		}

		public DataGrid BindDataGrid
		{
			get
			{
				return dgMaster;
			}
			set
			{
				dgMaster = value;
				if(!IsPostBack)
				{
					if(value.PageSize>0)
						dgMaster.PageSize=value.PageSize;
					else
						dgMaster.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
				}
			}
		}

		public void cmdMove_Click(object sender,EventArgs e)
		{
			int curPageSize = int.Parse(txtPageSize.Value.Trim());
			if(curPageSize<1)
				curPageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
			if(PageIndexChanged !=null ) 
			{    
				System.Web.UI.Control btn = sender as System.Web.UI.Control;
				string sFlag =  btn.ID;
				switch(sFlag)
				{
					case "FirstPage":
						iNewPageIndex=-1;
						break;
					case "PreviousPage":
						iNewPageIndex=-2;
						break;
					case "NextPage":
						iNewPageIndex=-3;
						break;
					case "LastPage":
						iNewPageIndex=-4;
						break;
					case "txtPageNo":
						iNewPageIndex=-4;
						break;
				}
				PageIndexChanged(this, e);
				dgMaster.PageSize = curPageSize;
				PageIndexChanged(this, e);
			}   
		}  
 
		public void MoveFirst()
		{
			CurrentPageIndex=-1;
		}
		public void MovePrevious()
		{
			CurrentPageIndex=-2;
		}
		public void MoveNext()
		{
			CurrentPageIndex=(-3);
		}
		public void MoveLast()
		{
			CurrentPageIndex=(-4);
		}
 
		protected void btnNewValue_OnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			if(btn.CommandName == "PageSize")
			{
				if(txtPageSize.Value.Trim() != "")
				{
					int curPageSize = int.Parse(txtPageSize.Value.Trim());
					if(curPageSize<1)
						curPageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
					iNewPageIndex = 0;
					if(this.PageIndexChanged != null)
						this.PageIndexChanged(this, e);
					dgMaster.PageSize = curPageSize;
					txtPageSize.Value = curPageSize.ToString();
					if(this.PageIndexChanged != null)
						this.PageIndexChanged(this, e);
					//Page.DataBind();
				}
			}
			else
			{
				if(btn.CommandName == "PageIndex")
				{
					if(txtPageCurrentIndex.Value.Trim() != "" && int.Parse(txtPageCurrentIndex.Value.Trim())>0)
					{
						int curIndex = int.Parse(txtPageCurrentIndex.Value.Trim());
						int curPageSize = int.Parse(txtPageSize.Value.Trim());
						if(curPageSize<1)
							curPageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PageSize"]);
						iNewPageIndex = curIndex - 1;
						if(this.PageIndexChanged != null)
							this.PageIndexChanged(this, e);
						dgMaster.PageSize = curPageSize;
						if(this.PageIndexChanged != null)
							this.PageIndexChanged(this, e);
						//Page.DataBind();
					}
				}
				else
				{
					Response.Write("Not known this command.");
					Response.Flush();
					Response.End();
				}
			}
		}

		private static bool IsNumeric(object o)					 
		{
			bool ret = false;
			try
			{
				decimal dec = Convert.ToDecimal(o);
				ret = true;
			}
			catch
			{
			}
			return ret;
		}
	}
}
