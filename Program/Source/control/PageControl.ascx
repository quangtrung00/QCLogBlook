<%@ Control Language="c#" AutoEventWireup="True" CodeFile="PageControl.ascx.cs" Inherits="WebApply.Control.PageControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<span style="HEIGHT:20px"><input type="hidden" Value="0" id="txtPageIndex" runat="server" NAME="txtPageIndex">
	<input type="hidden" Value="0" id="txtPageCount" runat="server" NAME="txtPageCount">
	<table cellSpacing="0" cellPadding="0" border="0">
		<tr>
			<td nowrap>
				<asp:Label runat="server" id="lblIntro" />
				<input type="text" Value="12" id="txtPageSize" class="cssTextBox" name="txtPageSize" Size="3"
					runat="server" onfocus="this.select();" onkeypress="if(window.event.keyCode==13){event.returnValue = false; setPageSize();}"
					style="FONT-SIZE:9pt;VERTICAL-ALIGN:middle;WIDTH:30px;BORDER-TOP-STYLE:groove;BORDER-RIGHT-STYLE:groove;BORDER-LEFT-STYLE:groove;HEIGHT:18px;TEXT-ALIGN:center;BORDER-BOTTOM-STYLE:groove"
					title="How many rows on page?"/>
			</td>
			<td nowrap>&nbsp;
				<asp:Label runat="server" id="lblIntroPage" />
				<asp:Button id="FirstPage" runat="server" OnClick="cmdMove_Click" Text="|<" width="30" Height="20"
					CssClass="cssButton" CausesValidation="False"></asp:Button>&nbsp;
			</td>
			<td nowrap>
				<asp:Button id="PreviousPage" runat="server" onClick="cmdMove_Click" Text="<<" width="30" Height="20"
					CssClass="cssButton" CausesValidation="False"></asp:Button>&nbsp;
			</td>
			<td nowrap>
				<input id="txtPageCurrentIndex" type="text" class="cssTextBox" style="FONT-SIZE:9pt;VERTICAL-ALIGN:middle;WIDTH:30px;BORDER-TOP-STYLE:groove;BORDER-RIGHT-STYLE:groove;BORDER-LEFT-STYLE:groove;HEIGHT:18px;TEXT-ALIGN:center;BORDER-BOTTOM-STYLE:groove"
					size="1" runat="server" onkeypress="if(event.keyCode==13) { event.returnValue = false; setPageIdx();}"
					NAME="txtPageCurrentIndex"/>&nbsp;
			</td>
			<td nowrap>
				<asp:Button id="NextPage" runat="server" onClick="cmdMove_Click" Text=">>" width="30" Height="20"
					CssClass="cssButton" CausesValidation="False"></asp:Button>&nbsp;
			</td>
			<td nowrap>
				<asp:Button id="LastPage" runat="server" onClick="cmdMove_Click" Text=">|" width="30" Height="20"
					CssClass="cssButton" CausesValidation="False"></asp:Button>&nbsp;
			</td>
			<td nowrap>
				<asp:Label runat="server" id="lblIntroTotal" />
				<asp:Label id="lblPage" runat="server"></asp:Label>
			</td>
				<td nowrap>
				<asp:Button Runat="server" ID="btnGetNewValue_PageSize" Width="0" OnClick="btnNewValue_OnClick"
					CommandName="PageSize" CausesValidation="False"></asp:Button>
				<asp:Button Runat="server" ID="btnGetNewValue_PageIndex" Width="0" OnClick="btnNewValue_OnClick"
					CommandName="PageIndex" CausesValidation="False"></asp:Button>
			</td>
		</tr>
	</table>
</span>
<script Language="javascript">
    function setPageSize() {
        var btnNewPageSize = document.getElementById('<%=btnGetNewValue_PageSize.ClientID%>');
        btnNewPageSize.click();
    }

    function setPageIdx() {
        var btnNewPageIndex = document.getElementById('<%=btnGetNewValue_PageIndex.ClientID%>');
        btnNewPageIndex.click();
    }
</script>
