<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginTop.aspx.cs" Inherits="Pub_Module_LoginTop" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TopPage</title>
    <script language="JavaScript" src="../js/common.js"></script>
	<script language="javascript" src="../js/DOL_CORE.js"></script>
	<script language="javascript" src="../js/DOL_MenuBar.js"></script>
	<script language="javascript" src="../js/DOL_XpItem.js"></script>
	<style>
	    .dataTit01 { FONT-WEIGHT: bold; letter-spacing:3pt; FONT-SIZE: 15pt; COLOR: #ffffff; FONT-FAMILY: Arial;filter: Glow(Color=#167C99, Strength=3); }
	</style>
    <script language=javascript>
            function fun1(){
				var apID = GetApLink(LoginTop.txtTopMenu.value,"APName","APID",GetApName());
				top.frames[1].location.href("Medium.aspx?ApID=" + apID);
			}
			
			function funReLogin(){        			                                             			                                             			                                                     
			   	chk = confirm("確認登出現有使用者？");                    
			      if(chk == true){window.open("../../Default.aspx","_top");}    
			}
			  
			function funExit(){				
				top.window.close();
			}     
			
			function PccLoad()
			{
				TsXpMenu.LogoWidth = 200;
					
				var arrApName = SecondLayer(LoginTop.txtTopMenu.value,"APName"); 
				var arrApID = SecondLayer(LoginTop.txtTopMenu.value,"APID");
				var oItem = new Array();
				
				var oMenu = new TsXpMenu("100%"); 
				//設定ApName Title
				var _logostr = "";
				_logostr += "<table cellspacing =0 cellpadding=0 border=0 width=100%><tr>";
				_logostr += "<td align=center class='dataTit01'>請決定系統名稱</td></td></table>";
				oMenu.setLogoHTML(_logostr); 
				
				var oBody = oMenu.getBody(); 
				
				oMenu.setHeight(60);
				
				for (i = 0 ; i < arrApName.length ; i++)
				{
					//因為系統不知道ApID為多少，所以先以系統管理的圖示來表示，若建立系統後，需把以下的Mark，並建立ApID的圖示，並使用變動之圖示即把下二行的Mark拿掉
					//oItem[i] = new TsXpMenuItem(arrApName[i],"../../Images/MenuArea/104.gif");
					oItem[i] = new TsXpMenuItem(SplitApName(arrApName[i]),"../../Images/MenuArea/104.gif");
					//oItem[i] = new TsXpMenuItem(SplitApName(arrApName[i]),"../../Images/MenuArea/" + arrApID[i] +".gif");
					oItem[i].AddSelectEvent(fun1);
					oMenu.AddItem(oItem[i]);
				}
				
				var reLoginItem = new TsXpMenuItem("重新<br>登錄","../../Images/MenuArea/menu_login.gif");
				reLoginItem.AddSelectEvent(funReLogin);
				oMenu.AddItem(reLoginItem);
				
				var ExitItem = new TsXpMenuItem("離開<br>系統","../../Images/MenuArea/menu_exit.gif");
				ExitItem.AddSelectEvent(funExit);
				oMenu.AddItem(ExitItem);
				
				
				
				document.body.appendChild(oBody);
			}
			
			function SplitApName(apName)
			{
				if ((apName.length < 3) || (apName.length > 4)) return apName;
				
				var strbegin = apName.substr(0,2);
				var strend = apName.substr(2);
				
				return strbegin + "<br>" + strend;
				
			}
			
			function GetApName()
			{
				//判斷是否是只有圖檔或是圖檔加文字
				var apName = event.srcElement.innerText;
				if (apName.length == 0)
					apName = event.srcElement.parentNode.innerText;
				
				var strtemp = "";
				
				for(i=0;i<apName.length;i++)
				{
					if (apName.charCodeAt(i) > 32)
					{
						strtemp = strtemp + apName.charAt(i);
					}
				}
				
				apName = strtemp;
				
				return apName;
				
			}
			
			
    </script>
</head>
<body onload="PccLoad();" style="BACKGROUND:#f0f0f0;MARGIN:0px">
    <form id="LoginTop"  method=post runat="server">
        <input type="hidden" id="txtTopMenu" name="txtTopMenu" runat="server">
    </form>
</body>
</html>
