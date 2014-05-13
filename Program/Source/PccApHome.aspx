<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PccApHome.aspx.cs" Inherits="PccApHome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EntryHome</title>
    <script language=javascript>
        function BeforeUnLoad()
        {
            if (self.frames[0].document.all[14].value != "Home")
			{
				window.open("<%=System.Configuration.ConfigurationManager.AppSettings["myServer"]%>" + "<%=System.Configuration.ConfigurationManager.AppSettings["vpath"]%>" + "/default.aspx?Type=Logout&Type2=Close","new","width=1,height=1,top=1350,left=1350,toolbar=no,location=no,status=no"); 
			}
        }
    </script>
</head>
<frameset onbeforeunload="BeforeUnLoad();"  border="0" frameSpacing="0" rows="60,*,30" frameBorder="NO">
    <frame name="top" marginWidth="0" marginHeight="0" src="Pub\Module\LoginTop.aspx" frameBorder="NO" noResize scrolling="no" />
    <frame name="Medium" marginWidth="0" marginHeight="0" src="Pub\Module\Medium.aspx?ApID=<%=Request.QueryString["ApID"]%>&Params=<%=Request.QueryString["Params"]%>" frameBorder="NO" noResize>
	<frame name="bottom" marginWidth="0" marginHeight="0" src="Pub\Module\LoginBottom.aspx" frameBorder="NO" noResize>
</frameset>
</html>
