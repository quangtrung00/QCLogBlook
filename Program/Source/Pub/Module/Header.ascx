<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Pub_Module_Header" %>
<script SRC="<%= ResolveClientUrl("~/")%>Pub/js/common.js" LANGUAGE="JavaScript">
	</script>
	<link rel="stylesheet" href="<%= ResolveClientUrl("~/")%>Pub/css/PccStyles.css">
	<script language="javascript">
			window.onload = window_common_onload;

			function window_common_onload()
			{
				if ( top.document.all )
				{
					//alert('<%=Session["PageLayer"]%>');
					if ('<%=Session["UserID"]%>' == "")
					{ 
						window.parent.parent.location.href = '<%=Session["PageLayer"]%>' + 'Default.aspx';  
					}
				}
			}
	
	</script>
	<table id="table1" cellpadding = "0"  cellspacing = "0" border = 0 width = "100%" style="display:none"  background="<%= ResolveClientUrl("~/")%>images/LoginPage/bgTitle.png">
	<tr>
		<td height='18px' style="padding-left:5px">
			<asp:label id="PccTitle" runat="server" Width="100%" Font-Bold="True" ForeColor ="white"  Font-Size="10pt">Title</asp:label>
		</td>
	</tr>
	</table>
	