using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Drawing;


namespace Pcc
{
    public class PageControl : Control, INamingContainer
    {
        // Fields
        private TextBox currentPage;
        private LinkButton downLink;
        private string downLinkText = "下一頁";
        private LinkButton firstLink;
        private string firstLinkText = "第一頁";
        private Button goPage;
        private LinkButton lastLink;
        private string lastLinkText = "最未頁";
        private Label lblPage;
        private int listCount;
        private int pageSize;
        private string totalSize = string.Empty;
        private LinkButton upLink;
        private string upLinkText = "上一頁";

        // Events
        public event EventHandler DownClick;

        public event EventHandler FirstClick;

        public event EventHandler LastClick;

        public event EventHandler PageClick;

        public event EventHandler UpClick;

        // Methods
        public PageControl()
        {
            try
            {
                this.listCount = 0;
                this.pageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["PageSize"]);
            }
            catch (Exception)
            {
                this.pageSize = 100;
            }
        }

        public void BuildPager()
        {
            if ((int.Parse(this.ViewState["TotalSize"].ToString()) % this.pageSize) == 0)
            {
                this.lblPage.Text = this.CurrentPage + "/" + ((int.Parse(this.ViewState["TotalSize"].ToString()) / this.pageSize)).ToString();
            }
            else
            {
                decimal tmp = Math.Floor((decimal)(int.Parse(this.ViewState["TotalSize"].ToString()) / this.pageSize)) + 1;
                this.lblPage.Text = this.CurrentPage + "/" + tmp.ToString();
            }
            if ((int.Parse(this.CurrentPage) != 1) && (int.Parse(this.CurrentPage) > 1))
            {
                this.firstLink.Enabled = true;
                this.firstLink.CssClass = "GridHeaderLink";
            }
            else
            {
                this.firstLink.Enabled = false;
                this.firstLink.CssClass = "GridHeaderLinkDisabled";
            }
            if (int.Parse(this.CurrentPage) <= 1)
            {
                this.upLink.Enabled = false;
                this.upLink.CssClass = "GridHeaderLinkDisabled";
            }
            else
            {
                this.upLink.Enabled = true;
                this.upLink.CssClass = "GridHeaderLink";
            }
            if ((int.Parse(this.CurrentPage) * this.pageSize) >= int.Parse(this.ViewState["TotalSize"].ToString()))
            {
                this.downLink.Enabled = false;
                this.downLink.CssClass = "GridHeaderLinkDisabled";
            }
            else
            {
                this.downLink.Enabled = true;
                this.downLink.CssClass = "GridHeaderLink";
            }
            if ((int.Parse(this.CurrentPage) * this.PageSize) < int.Parse(this.ViewState["TotalSize"].ToString()))
            {
                this.lastLink.Enabled = true;
                this.lastLink.CssClass = "GridHeaderLink";
            }
            else
            {
                this.lastLink.Enabled = false;
                this.lastLink.CssClass = "GridHeaderLinkDisabled";
            }
        }

        protected override void CreateChildControls()
        {
            this.firstLink = new LinkButton();
            this.firstLink.Text = this.firstLinkText;
            this.firstLink.CommandName = "First";
            this.firstLink.CssClass = "GridHeaderLink";
            this.Controls.Add(this.firstLink);

            this.Controls.Add(new LiteralControl("&nbsp;"));

            this.upLink = new LinkButton();
            this.upLink.Text = this.upLinkText;
            this.upLink.CommandName = "Up";
            this.upLink.CssClass = "GridHeaderLink";
            this.Controls.Add(this.upLink);

            this.Controls.Add(new LiteralControl("&nbsp;"));

            this.downLink = new LinkButton();
            this.downLink.Text = this.downLinkText;
            this.downLink.CommandName = "Down";
            this.downLink.CssClass = "GridHeaderLink";
            this.Controls.Add(this.downLink);

            this.Controls.Add(new LiteralControl("&nbsp;"));

            this.lastLink = new LinkButton();
            this.lastLink.Text = this.lastLinkText;
            this.lastLink.CommandName = "Last";
            this.lastLink.CssClass = "GridHeaderLink";
            this.Controls.Add(this.lastLink);

            this.Controls.Add(new LiteralControl("&nbsp;"));

            this.lblPage = new Label();
            this.lblPage.Text = "0/0";
            this.lblPage.Font.Bold = true;
            this.Controls.Add(this.lblPage);

            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;GOTO"));

            this.currentPage = new TextBox();
            this.currentPage.Text = "1";
            this.currentPage.Width = Unit.Pixel(30);
            this.currentPage.Height = Unit.Pixel(20);
            this.Controls.Add(this.currentPage);
            this.Controls.Add(new LiteralControl("Page&nbsp;&nbsp;"));

            this.goPage = new Button();
            this.goPage.Text = "Go";
            this.goPage.CommandName = "Go";
            this.goPage.CssClass = "cssDocButton";
            this.Controls.Add(this.goPage);
        }

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            bool flag = false;
            if (!(e is CommandEventArgs))
            {
                return flag;
            }
            CommandEventArgs args = (CommandEventArgs)e;
            string commandName = args.CommandName;
            if (commandName == null)
            {
                return flag;
            }
            if (!(commandName == "First"))
            {
                if (commandName != "Up")
                {
                    int num;
                    if (commandName == "Down")
                    {
                        if ((int.Parse(this.CurrentPage) * this.pageSize) < int.Parse(this.ViewState["TotalSize"].ToString()))
                        {
                            num = int.Parse(this.CurrentPage) + 1;
                            this.CurrentPage = num.ToString();
                        }
                        this.ListCount = Convert.ToString((int)(int.Parse(this.CurrentPage) - 1));
                        this.OnDownClick(args);
                        this.OnPageClick(args);
                        return true;
                    }
                    if (commandName == "Last")
                    {
                        if ((int.Parse(this.ViewState["TotalSize"].ToString()) % this.pageSize) == 0)
                        {
                            num = int.Parse(this.ViewState["TotalSize"].ToString()) / this.pageSize;
                            this.CurrentPage = num.ToString();
                        }
                        else
                        {
                            decimal tmp = Math.Floor((decimal)(int.Parse(this.ViewState["TotalSize"].ToString()) / this.pageSize)) + 1;
                            this.CurrentPage = tmp.ToString();
                        }
                        this.ListCount = Convert.ToString((int)(int.Parse(this.CurrentPage) - 1));
                        this.OnLastClick(args);
                        this.OnPageClick(args);
                        return true;
                    }
                    if (commandName != "Go")
                    {
                        return flag;
                    }
                    this.CurrentPage = this.currentPage.Text.Trim();
                    this.ListCount = Convert.ToString((int)(int.Parse(this.CurrentPage) - 1));
                    this.OnPageClick(args);
                    return true;
                }
            }
            else
            {
                this.CurrentPage = "1";
                this.ListCount = Convert.ToString((int)(int.Parse(this.CurrentPage) - 1));
                this.OnFirstClick(args);
                this.OnPageClick(args);
                return true;
            }
            if (int.Parse(this.CurrentPage) > 1)
            {
                this.CurrentPage = (int.Parse(this.CurrentPage) - 1).ToString();
            }
            this.ListCount = Convert.ToString((int)(int.Parse(this.CurrentPage) - 1));
            this.OnUpClick(args);
            this.OnPageClick(args);
            return true;
        }

        public virtual void OnDownClick(EventArgs e)
        {
            if (this.DownClick != null)
            {
                this.DownClick(this, e);
            }
        }

        public virtual void OnFirstClick(EventArgs e)
        {
            if (this.FirstClick != null)
            {
                this.FirstClick(this, e);
            }
        }

        public virtual void OnLastClick(EventArgs e)
        {
            if (this.LastClick != null)
            {
                this.LastClick(this, e);
            }
        }

        public virtual void OnPageClick(EventArgs e)
        {
            if (this.PageClick != null)
            {
                this.PageClick(this, e);
            }
        }

        public virtual void OnUpClick(EventArgs e)
        {
            if (this.UpClick != null)
            {
                this.UpClick(this, e);
            }
        }

        // Properties
        public string CurrentPage
        {
            get
            {
                this.EnsureChildControls();
                return this.currentPage.Text.Trim();
            }
            set
            {
                this.EnsureChildControls();
                string s = value;
                try
                {
                    int num = int.Parse(s);
                    int num2 = int.Parse(this.lblPage.Text.Split(new char[] { '/' })[1]);
                    if (num < 1)
                    {
                        if (this.totalSize.Equals(string.Empty) && (num2 == 0))
                        {
                            s = "0";
                        }
                        else
                        {
                            s = "1";
                        }
                        this.currentPage.Text = s;
                    }
                    else if (this.totalSize.Equals(string.Empty) && (num2 == 0))
                    {
                        this.currentPage.Text = s;
                    }
                    else if ((num > num2) && (num2 > 0))
                    {
                        s = num2.ToString();
                        this.currentPage.Text = s;
                    }
                    else
                    {
                        this.currentPage.Text = s;
                    }
                }
                catch
                {
                    s = "1";
                    this.currentPage.Text = s;
                }
                this.ListCount = Convert.ToString((int)(int.Parse(s) - 1));
            }
        }

        public string DownLinkText
        {
            get
            {
                return this.downLinkText;
            }
            set
            {
                this.downLinkText = value;
                if (this.downLink != null)
                {
                    this.downLink.Text = value;
                }
            }
        }

        public string FirstLinkText
        {
            get
            {
                return this.firstLinkText;
            }
            set
            {
                this.firstLinkText = value;
                if (this.firstLink != null)
                {
                    this.firstLink.Text = value;
                }
            }
        }

        public string LastLinkText
        {
            get
            {
                return this.lastLinkText;
            }
            set
            {
                this.lastLinkText = value;
                if (this.lastLink != null)
                {
                    this.lastLink.Text = value;
                }
            }
        }

        public string ListCount
        {
            get
            {
                this.listCount++;
                return this.listCount.ToString();
            }
            set
            {
                this.listCount = this.pageSize * Convert.ToInt32(value);
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public int StartRecord
        {
            get
            {
                int num = (int.Parse(this.CurrentPage) - 1) * this.pageSize;
                if (num < 0)
                {
                    num = 0;
                }
                return num;
            }
        }

        public string TotalSize
        {
            get
            {
                return this.totalSize;
            }
            set
            {
                this.totalSize = value;
                this.ViewState["TotalSize"] = this.totalSize;
            }
        }

        public string UpLinkText
        {
            get
            {
                return this.upLinkText;
            }
            set
            {
                this.upLinkText = value;
                if (this.upLink != null)
                {
                    this.upLink.Text = value;
                }
            }
        }
    }
}