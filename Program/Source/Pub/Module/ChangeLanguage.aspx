<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeLanguage.aspx.cs" Inherits="Pub_Module_ChangeLanguage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ChangeLanguage</title>
    <script language="JavaScript" src="../js/common.js"></script>
	<LINK href="../css/PccStyles.css" rel="stylesheet">
</head>
<body leftMargin="0" topMargin="0" bgcolor="#dcf1f1">
		<table class="BodyBg" style="PADDING-TOP: 0pt" height="100%" width="100%" border="0">
			<tr>
				<td valign="top">
					<form id="Body" method="post" runat="server">
						<FONT face="新細明體">
							<P>
								<asp:Table id="tblBody" runat="server"></asp:Table></P>
							<P>
								<asp:LinkButton id="LinkButton1" runat="server" Visible="False" onclick="LinkButton1_Click">LinkButton</asp:LinkButton></P>
						</FONT>
					</form>
				</td>
			</tr>
		</table>
	</body>
</html>
