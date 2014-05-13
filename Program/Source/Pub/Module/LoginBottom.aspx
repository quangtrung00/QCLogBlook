<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginBottom.aspx.cs" Inherits="Pub_Module_LoginBottom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BottomPage</title>
    <script language="JavaScript" src="../js/common.js"></script>
    <link href="../css/PccStyles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">  
	function OnLineUserClick()
		{
			window.open("<%=System.Configuration.ConfigurationManager.AppSettings["myServer"]%>" + "<%=System.Configuration.ConfigurationManager.AppSettings["vpath"]%>" + "/OnlineUser.aspx?Type=Logout&Type2=Close","new","width=600,height=600,top=100,left=100,toolbar=no,location=no,status=no"); 
		}
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
			<FONT face="新細明體">線上人數：<%=Application["OnlineCount"]%>&nbsp;&nbsp;
	<% if (System.Configuration.ConfigurationManager.AppSettings["superAdminEmail"].ToString().Trim().ToLower() == Session["UserAccount"].ToString().Trim().ToLower())
      { %>
          <asp:LinkButton ID="LinkButton1" OnClientClick="OnLineUserClick()" runat="server">OnlineUser</asp:LinkButton>
     <% }
	 %>
	 </FONT>&nbsp; 
			<A id="lnkContact" href="mailto:<%=System.Configuration.ConfigurationManager.AppSettings["System-Email"]%>?subject=系統問題反應" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image4','','../../Images/email1.gif',1)" class="A1">
			<img src="../../Images/email2.gif" border="0" name="Image4" alt="連絡我們">有問題，請反應，Thanks！</A>
	</form>
</body>
</html>
