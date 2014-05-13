using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Pub_Module_Calendar_Old : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get
        {
            return UC_Calendar.Text;
        }
        set
        {
            UC_Calendar.Text = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return UC_Calendar.Enabled;
        }
        set
        {
            UC_Calendar.Enabled = value;
        }
    }
}
