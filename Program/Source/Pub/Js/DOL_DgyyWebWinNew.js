/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	界面對象
 */
 
 function DgyyWebWinNew(menusArray,nWidth,nHeight){
 	TsUICom.call(this,nWidth,nHeight);
 	this._menusArray = menusArray;		//頁面左邊菜單的配置數組
 	this._toolbar = [];					//工具欄
 	this._toolCell = null;				//左邊菜單欄td對象
 	this._arrowCell = null;				//箭頭td對象
 	this._arrowImg = null;				//箭頭圖片
 	this._winCell = null;				//包含窗口的td對象 	
 	this._TabBar = null;				//窗口管理器
 	var oThis = this;
 	
 	//單擊箭頭按鈕
 	this.arrowClick = function(e){
 		var aimg = e.srcElement;
 		if(oThis._toolCell.style.display == "none"){
 		//if(aimg.src.indexOf(DgyyWebWinNew.ArrowLeftImg)>0){		//隱藏
 			aimg.src = DgyyWebWinNew.ArrowLeftImg;
 			aimg.title = "單擊這里隱藏菜單欄";
 			oThis._toolCell.style.display = "block";	//顯示
 			//alert(oThis._toolCell.style.display);
 		}
 		else{
 			aimg.src = DgyyWebWinNew.ArrowRightImg;
 			aimg.title = "單擊這里顯示菜單欄";
 			oThis._toolCell.style.display = "none";
 			//alert(oThis._toolCell.style.display);
 		}
 	};
 	
 	
	//設置工具欄item單擊時的動作處理
	this.settoolAction = function(toolItem,cmdParam,cmdKind){
		if(cmdKind == "1"){			//1為直接打開鏈接窗口
			toolItem._onItemClick = function(){
				oThis._TabBar.openWin(toolItem._text,cmdParam);
			};
		}
		else if(cmdKind=="2"){		//執行命令
			toolItem._onItemClick = function(){
				eval(cmdParam);
			};
		}
	};
 
	//在工具欄上的鼠標
	this.arrowCellOver = function(e){
		if((oThis._arrowCell.isAutoHide)&&(oThis._toolCell.style.display == "none")){
	 		oThis._toolCell.style.display = "block";
	 	}
	}
 	
	//在工具欄外的鼠標
	this.arrowCellOut = function(e){
		if((oThis._arrowCell.isAutoHide)&&(e.toElement!=oThis._toolCell.all[0])){
	 		oThis._toolCell.style.display = "none";
	 	}
	}
	
	this.fixupClick = function(e){
		oThis._arrowImg.src = oThis._toolCell.style.display=="none"?DgyyWebWinNew.ArrowRightImg:DgyyWebWinNew.ArrowLeftImg;
		if(oThis._arrowCell.isAutoHide){
			oThis._arrowCell.isAutoHide =false;
			e.srcElement.title = "單擊這里開啟自動隱藏功能";
			e.srcElement.src = DgyyWebWinNew.FixUpImg;
			oThis._arrowImg.style.visibility = "visible";
	 		SetCookie(DgyyWebWinNew.ToolBarCookieRecord,"0");
		}
		else{
			oThis._arrowCell.isAutoHide =true;
			e.srcElement.src = DgyyWebWinNew.NoFixUpImg;
			e.srcElement.title = "單擊這里關閉自動隱藏功能";
			oThis._arrowImg.style.visibility = "hidden";
	 		SetCookie(DgyyWebWinNew.ToolBarCookieRecord,"1");
		}
	}
	
	this.setBody();
	
 }
 
 _p = DgyyWebWinNew.prototype = new TsUICom;
 _p._className="DgyyWebWinNew";
 
 ////////常數設定區(更改時只能改這里)////////////////////////////////////////
 
 DgyyWebWinNew.LeftCellWidth = "20%";					//左邊菜單欄單元格所占的寬度(占25%)
 DgyyWebWinNew.LeftCellFilter = "progid:DXImageTransform.Microsoft.gradient(startColorStr=#FF7ba6e7,endColorStr=#ff6379d6)";		//左邊單元格的漸變背景(藍色漸變)
 DgyyWebWinNew.RightCellWidth = "75%";					//右邊菜單欄單元格所占的寬度(占75%)
 DgyyWebWinNew.ToolBarCookieRecord = "DgyyWebWingToolBarTsouSet";				//記錄工具欄是否自動隱藏的cookie名稱
 DgyyWebWinNew.ToolBarInterval = 10;			//工具欄之間的間隔

_p.setImgDir = function(){
	DgyyWebWinNew.ImgDir  = TsUICom.ImgDir + "/DgyyWebWinNew";
	DgyyWebWinNew.ArrowLeftImg = DgyyWebWinNew.ImgDir + "/arrow_left.gif";			//控制工具欄顯示的向左箭頭圖片
	DgyyWebWinNew.ArrowRightImg = DgyyWebWinNew.ImgDir + "/arrow_right.gif";		//控制工具欄顯示的向右箭頭圖片
	DgyyWebWinNew.FixUpImg = DgyyWebWinNew.ImgDir + "/fixup.gif";			//控制工具欄顯示的向左箭頭圖片
	DgyyWebWinNew.NoFixUpImg = DgyyWebWinNew.ImgDir + "/nofixup.gif";			//控制工具欄顯示的向右箭頭圖片
	DgyyWebWinNew.ArrowCellBackImg = DgyyWebWinNew.ImgDir + "/cs.jpg";									//箭頭cell的背景圖片
}


 /////////常數設定區(結束)///////////////////////////////////////////////////

 //設置主體對象(覆寫基類)
_p.setBody = function(){
	if(this._body !=null)
		return;
		
	var oTable = document.createElement("table");
	this._body = oTable;
	oTable.style.tableLayout = "fixed";
	oTable.cellSpacing = "0";
	oTable.cellPadding = "0";
	oTable.style.width = this._width;
	oTable.style.height = this._height;
	var oTbody = document.createElement("tbody");
	oTable.appendChild(oTbody);
	var fRow = document.createElement("tr");
	oTbody.appendChild(fRow);

	/////////////////////////////////////////////////////////////
	//創建左邊的單元格
	var toolCell = document.createElement("TD");
	fRow.appendChild(toolCell);
	this._toolCell = toolCell;
	toolCell.style.width = DgyyWebWinNew.LeftCellWidth;
	toolCell.style.filter = DgyyWebWinNew.LeftCellFilter;
	var conDiv = document.createElement("SPAN");
	toolCell.appendChild(conDiv);
	conDiv.style.padding="10 10 0 10";
	conDiv.style.width =  "98%";
	conDiv.style.height = "100%";
	conDiv.style.overflowX = "hidden";
	conDiv.style.overflowY = "auto";

	//創建工具欄
	if(this._menusArray.length>0){
		var menus = this._menusArray;			
		var menunum = menus[0];			//大項數目
		var kk=1;						//開始遍歷數組(從1開始,0是大項數目)
		for(var jj=0;jj<menunum;jj++){
			var toolbar = new TsXpBar("100%",menus[kk++]);
			this._toolbar.insertAt(toolbar,jj);
			
			var itemNum = menus[kk++];
			for(var ll=0;ll<itemNum;ll++){
				var item1 = new TsXpBarItem(menus[kk++],menus[kk++]);
				var theurl = menus[kk++];
				var commandkind = menus[kk++];
				this.settoolAction(item1,theurl,commandkind);
				toolbar.AddItem(item1);
			}
			var toolbody=toolbar.getBody();
			if(jj>0)		//第1個不用
				toolbody.style.marginTop =  DgyyWebWinNew.ToolBarInterval;
			conDiv.appendChild(toolbody);
		}
	}
	
	//讀取cookie值﹐取得用戶的設置(自動隱藏還是不隱藏)
	var isauto = GetCookie(DgyyWebWinNew.ToolBarCookieRecord);
	isauto = isauto=="1"?true:false;
	//創建箭頭控制Cell
	var arrowCell = document.createElement("TD");
 	this._arrowCell = arrowCell;				//箭頭td對象
	fRow.appendChild(arrowCell);
	arrowCell.isAutoHide = isauto;		//是否自動隱藏
	arrowCell.style.verticalAlign = "top";
	arrowCell.style.width = 8;
	arrowCell.style.margin = 0;
	arrowCell.style.padding = "0";
	arrowCell.rowSpan = "2";
	arrowCell.style.backgroundImage = "url(" +  DgyyWebWinNew.ArrowCellBackImg + ")";
	
	var arrowDiv = document.createElement("DIV");
	arrowCell.appendChild(arrowDiv);
	arrowDiv.style.height = this._height;
	arrowDiv.style.width = 8;
	arrowDiv.attachEvent("onmouseover",this.arrowCellOver);
	arrowDiv.attachEvent("onmouseout",this.arrowCellOut);
	
	var arrowImg = document.createElement("IMG");
	arrowDiv.appendChild(arrowImg);
	arrowImg.title = isauto?"單擊這里取消菜單的自動隱藏":"單擊這里開啟菜單的自動隱藏";
	arrowImg.style.cursor="pointer";
	arrowImg.style.marginTop="8";
	arrowImg.style.marginBottom="200";
	arrowImg.attachEvent("onclick",this.fixupClick);
	arrowImg.src = isauto?DgyyWebWinNew.NoFixUpImg:DgyyWebWinNew.FixUpImg;
	
	var arrowImg = document.createElement("IMG");
	this._arrowImg = arrowImg;				//箭頭圖片	
	arrowDiv.appendChild(arrowImg);
	arrowImg.title = "單擊這里隱藏菜單";
	arrowImg.style.cursor="default";
	arrowImg.style.visibility=isauto?"hidden":"visible";
	arrowImg.attachEvent("onclick",this.arrowClick);
	arrowImg.src = DgyyWebWinNew.ArrowLeftImg;
	
				
	/////////////////////////////////////////////////////////////
	//創建右邊的單元格
	var winCell = document.createElement("TD");
	fRow.appendChild(winCell);
	this._winCell = winCell;
	winCell.attachEvent("onmouseover",this.arrowCellOut);
	//winCell.style.width = DgyyWebWinNew.RightCellWidth;
	winCell.style.height = this._height;
	winCell.style.padding = "0";
	winCell.style.verticalAlign = "Top";
	
	//設置鏈接窗口高度
	XpTabBar.WinHeight = "100%";				

	var	tabBar = new XpTabBar("100%","100%");
	winCell.appendChild(tabBar.getBody());
	this._TabBar = tabBar;
	
	oTable = null;
}

_p.Show = function(){
	document.body.appendChild(this.getBody());
};

//顯示某個子toolbar
_p.ShowToolBar = function(iIndex){
	if(this._toolbar[iIndex])
		this._toolbar[iIndex].ShowBody();
}

//隱藏某個子toolbar
_p.HideToolBar = function(iIndex){
	if(this._toolbar[iIndex])
		this._toolbar[iIndex].HideBody();

}

//打開一個新頁面
_p.OpenPage = function(title,url){
	if(this._TabBar!=null){
		this._TabBar.openWin(title,url);
	}
	else{
		alert("[Warning]必須在Show方法后調用才有效﹗");
	}
}

_p.dispose = function(){
	if(this._disposed)return;
 	this._body = null;
 	if(this._menusArray!=null){
 		for(var i=0;i<this._menusArray.length;i++)
 			this._menusArray[i]=null;
 		this._menusArray = null;
 	}
 	for(var i=0;i<this._toolbar.length;i++){
 		this._toolbar[i].dispose();
 		this._toolbar[i] = null;
 	}
 	this._toolbar = null;
 	this._TabBar.dispose();				//任務欄
 	this._TabBar = null;				//任務欄
 	this._toolCell = null;				//左邊菜單欄td對象
 	this._arrowCell = null;				//箭頭td對象
 	this._arrowImg = null;				//箭頭圖片
 	this._winCell = null;				//包含窗口的td對象 	

 	this.arrowClick = null;
	this.settoolAction = null;
	this.arrowCellOver = null;
	this.arrowCellOut =  null;
	this.fixupClick =  null;
	TsUICom.prototype.dispose.call(this);
};