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


public partial class Pub_Module_Calendar : System.Web.UI.UserControl
{
    protected bool bReadOnly = true;
    protected string sButtonImage = "button";
    protected bool bshowButtonPanel = false;
    protected bool bchangeMonthAndYear = true;
    protected bool bDisabledButton = false;
    protected string sFormatDate = "yymmdd";
    protected string sOffSetX = "0";
    protected string sOffSetY = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
   
    public string Text
    {
        get
        {
            return UC_Calendar.Text.Replace("/", "");
        }
        set
        {
            UC_Calendar.Text = value;
        }
    }     

    // 'focus' for popup on focus,    // 'button' for trigger button // or 'both' for either  
    public string ButtonImage
    {
        get
        {
            return sButtonImage;
        }
        set
        {
            sButtonImage = value;
        }
    }
    //FormatDate
    public string FormatDate
    {
        get
        {
            return sFormatDate;
        }
        set
        {
            sFormatDate = value;
        }
    }

    //show panel: today&close button
    public bool ShowButtonPanel
    {
        get
        {
            return bshowButtonPanel;
        }
        set
        {
            bshowButtonPanel = value;
        }
    }
    //Alow show dropdownlist
    public bool changeMonthAndYear
    {
        get
        {
            return bchangeMonthAndYear;
        }
        set
        {
            bchangeMonthAndYear = value;
        }
    }
    // Summary: 
    //DisabledButton
    public bool DisabledButton
    {
        get
        {
            return bDisabledButton;
        }
        set
        {
            bDisabledButton = value;
        }
    }
    // Summary: 
    //Enabled
    public bool Enabled
    {
        get
        {
            return !bDisabledButton;
        }
        set
        {
            bDisabledButton = !value;
        }
    }
    // Summary: 
    //ReadOnly
    public bool ReadOnly
    {
        get
        {
            return bReadOnly;
        }
        set
        {
            bReadOnly = value;
        }
    }
    // Summary: 
    //Offset position X
    public string OffSetX
    {
        get
        {
            return sOffSetX;
        }
        set
        {
            sOffSetX = value;
        }
    }
    /// <summary>
    /// Offset position Y   
    /// </summary>
    public string OffSetY
    {
        get
        {
            return sOffSetY;
        }
        set
        {
            sOffSetY = value;
        }
    }

    public string TextBoxClientID
    {
        get { return UC_Calendar.ClientID; }
    }
    
}
