<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Medium.aspx.cs" Inherits="Pub_Module_Medium" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>主要選單頁面</title>
    <script language="javascript" src="../js/common.js" />
    <!-- 遇到很奇怪的問題，就是若沒有事先加入一個js檔，則DOL_CORE就不動作，而且加入的這個檔案也不會做動作，
        這真的是很奇怪，連檔案不存在都可以執行 20080214 By Lemor -->
    <script language="javascript" src="../js/aaa.js"></script>
    <script language="javascript" src="../js/DOL_CORE.js"></script>
    <script language="javascript" src="../js/DOL_XpToolBar.js"></script>
	<script language="javascript" src="../js/DOL_XpTabBar.js"></script>
	<script language="javascript" src="../js/DOL_XpItem.js"></script>
	<script language="javascript" src="../js/DOL_XpProcessBar.js"></script>
	<script language="javascript" src="../js/DOL_DgyyWebWinNew.js"></script>
	
	<script language="javascript">
			function pccLoad()
			{
				var menuArray = Medium.txtLeftMenu.value;
					
				if (menuArray != "")
				{
					eval(menuArray);	
					//在new DgyyWebWinNew之前設定工具欄之間的間隔距離
					DgyyWebWinNew.ToolBarInterval =1;
					
					//實例化界面對象(字符串數組,寬度,高度)
					oWin = new DgyyWebWinNew(menus,"100%","100%");
					
					for (i = 0; i < menus.length; i++)
					{
						//表示只有一般區會被打開
						if (i != 1)
							oWin.HideToolBar(i);
					}
					//顯示在body中
					oWin.Show();  
					
					oWin.OpenPage("訊息頁","LoginBody.aspx?ApID=0");
				}
				else
				{
					window.parent.location.href = '../../Default.aspx';
				}
			}
		</script>
	</HEAD>
	<body onload="pccLoad();" scroll=no style="BACKGROUND:#f0f0f0;MARGIN:0px">
		<form id="Medium" method="post" runat="server">
			<input type="hidden" id="txtLeftMenu" name="txtLeftMenu" runat="server">
		</form>
	</body>
</html>
