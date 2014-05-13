/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	菜單欄對象
 */

 //菜單欄對象
 function TsXpMenu(nWidth,nHeight){
	if(!nHeight)nHeight = 50;		//預設為50
	TsUICom.call(this,nWidth,nHeight);
	this._items = [];				//子項
	
	this._leftSpan = null;			//菜單左部分
	this._centerSpan = null;		//菜單中間部分
	this._rightSpan = null;			//菜單右部分
	this._itemCon = null;			//包含所有子項的span
	this._leftImg = null;
	this._rightImg = null;
	
	this._leftID = 0;				//向左滾動的函數
	this._rightID = 0;				//向右滾動的函數
	this._selectedIndex = -1;		//選中的項次ID
	this._lastSelectedIndex = -1;	//上次選中的ID
	
	var oThis =this;
	this._aover = function(e){
		var aDiv = e.srcElement;
		if(aDiv.canArrow){
			//aDiv.style.borderTop = aDiv.style.borderLeft = "1px solid #FFFFFF";
			//aDiv.style.borderRight = aDiv.style.borderBottom = "1px solid #736D63";
		}
	};
	
	this._aout = function(e){
		var aDiv = e.srcElement;
		//aDiv.style.border = "0px solid #FFF7E7";
		oThis.__setImgSrc();			 
	};
	
	this._adown = function(e){
		var aDiv = e.srcElement;
		if(aDiv.canArrow){
			//aDiv.style.borderRight = aDiv.style.borderBottom = "1px solid #FFFFFF";
			//aDiv.style.borderTop = aDiv.style.borderLeft = "1px solid #736D63";
			
			if(aDiv.arrow == "right"){
				if(oThis._rightID != 0) window.clearInterval(oThis._rightID);
				oThis._leftID = window.setInterval(oThis._leftMove,50);
			}
			else if(aDiv.arrow == "left"){
				if(oThis._leftID != 0) window.clearInterval(oThis._leftID);
				oThis._rightID = window.setInterval(oThis._rightMove,50);
			}
		}
	};
	
	this._leftMove = function(e){
		var cW = oThis._itemCon.offsetWidth;						//已有總寬度
		var mW = oThis._centerSpan.parentElement.offsetWidth;		//可顯現寬度

		var minLeft = mW-cW;
		oThis._itemCon.style.posLeft -= 20;
		if(oThis._itemCon.style.posLeft < minLeft){
			oThis._itemCon.style.posLeft = minLeft;
			window.clearInterval(oThis._leftID); 
			oThis.__setImgSrc();
		}
	};
	
	this._rightMove = function(e){
		oThis._itemCon.style.posLeft += 20;
		if(oThis._itemCon.style.posLeft >=0){
			 oThis._itemCon.style.posLeft = 0;
			 window.clearInterval(oThis._rightID); 
			oThis.__setImgSrc();			 
		}
	};
	
	this._aup = function(e){
		var aDiv = e.srcElement;
		if(aDiv.canArrow){
			aDiv.style.border = "0px solid #FFF7E7";
			if(oThis._rightID != 0) window.clearInterval(oThis._rightID);
			if(oThis._leftID != 0) window.clearInterval(oThis._leftID);
			oThis.__setImgSrc();			 
			
		}
	};
	
	//重畫事件
	this.__resize = function(e){
		oThis._itemCon.style.posLeft = 0;	
		oThis.__setImgSrc();
	}
	
	//設置圖標的是否可動
	this.__setImgSrc = function(){
	    var cW = oThis._itemCon.offsetWidth;						//已有總寬度
		var mW = oThis._centerSpan.parentElement.offsetWidth;		//可顯現寬度
		//alert("已有寬度:"+ cW + "\n可現寬度:"+mW);
		if(cW>mW){
			var minLeft = mW-cW;		
			if(oThis._itemCon.style.posLeft <= minLeft){
				oThis._rightImg.src = TsXpMenu.ArrowRightDisabled;	
				oThis._rightImg.title = "沒有項次被隱藏";
				oThis._rightImg.canArrow = false;
				oThis._rightImg.style.cursor = "default";
			}
			else{
				oThis._rightImg.src = TsXpMenu.ArrowRightAbled;
				oThis._rightImg.title = "單擊這里向右滾動";
				oThis._rightImg.canArrow = true;
				oThis._rightImg.style.cursor = "pointer";
			}
			if(oThis._itemCon.style.posLeft >=0){
				oThis._leftImg.src = TsXpMenu.ArrowLeftDisabled;
				oThis._leftImg.title = "沒有項次被隱藏";
				oThis._leftImg.canArrow = false;
				oThis._leftImg.style.cursor = "default";
			}
			else{
				oThis._leftImg.src = TsXpMenu.ArrowLeftAbled;
				oThis._leftImg.title = "單擊這里向左滾動";
				oThis._leftImg.canArrow = true;
				oThis._leftImg.style.cursor = "pointer";
			}
		}
		else{
			oThis._leftImg.src = TsXpMenu.ArrowLeftDisabled;
			oThis._rightImg.src = TsXpMenu.ArrowRightDisabled;
			oThis._leftImg.title = "沒有項次被隱藏";
			oThis._rightImg.title = "沒有項次被隱藏";
			oThis._rightImg.canArrow = false;
			oThis._leftImg.canArrow = false;
			oThis._rightImg.style.cursor = "default";
			oThis._leftImg.style.cursor = "default";
		}
	}
	
	this.__initArrow = function(){
		if(oThis._body.parentElement!=null){
			oThis.__setImgSrc();
			window.clearInterval(oThis._initID); 
		}
	}
	
	this.__dispose = function(){
		if((!oThis)||(oThis._disposed))return;	
		oThis.dispose(oThis);
		window.detachEvent("onbeforeunload",oThis.dispose);
		oThis._disposed = true;
		oThis.__dispose = null;
		oThis = null;
	}
	
	this.setBody();
 }
 
 
_p = TsXpMenu.prototype = new TsUICom;
_p.className = "TsXpMenu";
	
//系統預設值
TsXpMenu.style = "filter:progid:DXImageTransform.Microsoft.gradient(startColorStr=#FFf7f7ff,endColorStr=#ffefefe7);padding:0px;margin:0px;border-width:1px;border-style:solid;border-color:white black black white ";		//菜單樣式
TsXpMenu.LogoWidth = "40px";					//logo區寬度
TsXpMenu.MenuWidth = "80%";						//菜單區寬度
TsXpMenu.ArrowWidth = "";						//箭頭區寬度

//設置菜單寬度
_p.setWidth = function(nWidth){
	this._width = nWidth;
	this._body.style.width = nWidth;
}

//設置菜單高度
_p.setHeight = function(nHeight){
	this._height = nHeight;
	this._body.style.height = nHeight;
}

//給logo區設置html
_p.setLogoHTML = function(shtml){
	this._leftSpan.innerHTML = shtml;
}

//取得logo區html
_p.getLogoHTML = function(){
	return this._leftSpan.innerHTML;
}

//設置logo區樣式
_p.setLogoStyle = function(stext){
	this._leftSpan.style.cssText = stext;	
	this._leftSpan.style.width = TsXpMenu.LogoWidth;	//在這里不允許改變寬度
}

//取得logo區樣式
_p.getLogoStyle = function(){
	return this._leftSpan.style.cssText;
}

//設置菜單區樣式
_p.setMenuStyle = function(stext){
	this._centerSpan.style.cssText = stext;	
	this._centerSpan.style.width = TsXpMenu.MenuWidth;
}

//取得菜單區樣式
_p.getMenuStyle = function(){
	return this._centerSpan.style.cssText;
}

//設置箭頭區樣式
_p.setArrowStyle = function(stext){
	this._rightSpan.style.cssText = stext;	
	this._rightSpan.style.width = "100%";				//寬,高,overflow,padding,margin不可更改
	this._rightSpan.style.height = "100%";
	this._rightSpan.style.overflow = "hidden";
	this._rightSpan.style.padding = this._rightSpan.style.margin = "0px";
}

//取得箭頭區樣式
_p.getArrowStyle = function(){
	return this._rightSpan.style.cssText;
}

_p.setImgDir = function(){
	TsXpMenu.ImgDir  = TsUICom.ImgDir + "/XpMenuBar";
	TsXpMenu.ArrowBgImg = TsXpMenu.ImgDir + "/menubg.gif";		//箭頭區背景圖片
	TsXpMenu.ArrowLeftDisabled = TsXpMenu.ImgDir + "/ld.gif";	//不可向左滾動按鈕
	TsXpMenu.ArrowLeftAbled = TsXpMenu.ImgDir + "/la.gif";		//可向左滾動按鈕
	TsXpMenu.ArrowRightDisabled = TsXpMenu.ImgDir + "/rd.gif";	//不可向右滾動按鈕
	TsXpMenu.ArrowRightAbled = TsXpMenu.ImgDir + "/ra.gif";		//可向右滾動按鈕
}

//取得主體對象
_p.setBody = function(){
	if(this._body!=null)
		return;
	var oTblCon = document.createElement("DIV");
	this._body = oTblCon;
	oTblCon.style.cssText = TsXpMenu.style;
	oTblCon.style.verticalAlign="top";
	oTblCon.style.width = this._width;
	oTblCon.style.height = this._height;
	
	var oTable = document.createElement("table");
	this._body.appendChild(oTable);
	oTable.style.width = "100%";
	oTable.style.height = "100%";
	oTable.style.padding=oTable.style.margin = "0px";
	oTable.cellSpacing = oTable.cellPadding = "0px";
	//oTable.style.border = "1px solid gold";
	oTable.style.tableLayout = "fixed";
	var oTbody = document.createElement("tbody");
	oTable.appendChild(oTbody);
	var oTr = document.createElement("tr");
	oTbody.appendChild(oTr);
	
	var leftTd = document.createElement("Td");
	this._leftSpan = leftTd;
	//leftTd.style.border="1px solid red";
	leftTd.style.width = TsXpMenu.LogoWidth;
	leftTd.innerHTML = "";
	oTr.appendChild(this._leftSpan);
	
	var centerTd = document.createElement("Td");
	//centerTd.style.border="1px solid blue";
	centerTd.style.width = TsXpMenu.MenuWidth;
	oTr.appendChild(centerTd);
	
	var oDivCenter = document.createElement("SPAN");
	this._centerSpan = oDivCenter;
	centerTd.appendChild(this._centerSpan);
	oDivCenter.style.height = "100%";
	oDivCenter.style.overflow = "hidden";
	oDivCenter.style.padding = oDivCenter.style.margin = "0px";
	//oDivCenter.style.border = "1px solid green";
	
	var oDivContainer = document.createElement("SPAN");
	this._itemCon = oDivContainer;
	this._centerSpan.appendChild(this._itemCon);
	oDivContainer.style.position = "relative";
	oDivContainer.style.posWidth = 0;
	oDivContainer.style.whiteSpace = "nowrap";
	oDivContainer.style.height = "100%";
	oDivContainer.style.padding = oDivContainer.style.margin = "0px";
	
	var rightTd = document.createElement("Td");
	//rightTd.style.border="1px solid green";
	rightTd.style.width = TsXpMenu.ArrowWidth;
	oTr.appendChild(rightTd);
	
	var oDivArrow =  document.createElement("SPAN");
	this._rightSpan = oDivArrow;
	rightTd.appendChild(this._rightSpan);
	oDivArrow.style.width = "100%";
	oDivArrow.style.height = "100%";
	oDivArrow.style.overflow = "hidden";
	oDivArrow.style.padding = oDivContainer.style.margin = "0px";
	//oDivArrow.style.border = "1px solid red";
	
	oDivArrow.style.backgroundImage = 'url(' +  TsXpMenu.ArrowBgImg + ')';
	oDivArrow.style.backgroundPosition = 'top right';
	oDivArrow.style.backgroundRepeat = "no-repeat";
	
	var oDivLeft = document.createElement("<img style='left:expression(eval(this.parentElement.clientWidth/2-20));top:expression(eval(this.parentElement.clientHeight/2-10))'>");
	this._leftImg = oDivLeft;
	this._rightSpan.appendChild(this._leftImg);
	oDivLeft.src = TsXpMenu.ArrowLeftDisabled;
	oDivLeft.arrow = "left";
	oDivLeft.style.position = "relative";
	oDivLeft.style.cursor = "default";
	oDivLeft.attachEvent("onmouseover",this._aover);
	oDivLeft.attachEvent("onmouseout",this._aout);
	oDivLeft.attachEvent("onmousedown",this._adown);
	oDivLeft.attachEvent("onmouseup",this._aup);
	
	var oDivRight = document.createElement("<img style='left:expression(eval(this.previousSibling.offsetLeft + 10));top:expression(eval(this.previousSibling.offsetTop + 10))'>");
	this._rightImg = oDivRight;
	this._rightSpan.appendChild(this._rightImg);
	oDivRight.src = TsXpMenu.ArrowRightDisabled;
	oDivRight.arrow = "right";
	oDivRight.style.position = "relative";
	oDivRight.style.cursor = "default";
	oDivRight.attachEvent("onmouseover",this._aover);
	oDivRight.attachEvent("onmouseout",this._aout);
	oDivRight.attachEvent("onmousedown",this._adown);
	oDivRight.attachEvent("onmouseup",this._aup);

	this._body.attachEvent("onresize",this.__resize);
	window.attachEvent("onbeforeunload",this.__dispose);
	oTblCon = null;
	oDivCenter = null;
	oDivContainer = null;
	oDivArrow = null;
	oDivLeft = null;
	oDivRight = null;
	oTable = null;
	oTbody = null;
	oTr = null;
	leftTd = null;
	centerTd = null;
	rightTd = null;
};

_p.GetSelectedIndex =function(){
	return this._selectedIndex;
};

_p.GetLastSelectedIndex = function(){
	return this._lastSelectedIndex;
};

//增加子項
_p.AddItem = function(oItem){
	this._items[this._items.length] = oItem;
	var oThis = this;
	var itemBody = oItem.getBody();
	this._itemCon.appendChild(itemBody);
	oItem.AddSelectEvent(function(){oThis.SelectItem(oThis._items.indexOf(oItem))});
	this.__setImgSrc();			//重新設置箭頭是否可用
};

//移除子項
_p.RemoveItem = function(iIndex){
	if(iIndex<0||iIndex>=this._items.length)	//不選取任何項
		return;					
	var oItem = this._items[iIndex];
	if(this._selectedIndex==iIndex)
		this._selectedIndex=-1;
	if(this._lastSelectedIndex==iIndex)
		this._lastSelectedIndex=-1;
	this._itemCon.removeChild(oItem.getBody());
	this._items.removeAt(iIndex);
	this.__setImgSrc();			//重新設置箭頭是否可用
	if(this._selectedIndex!=-1&&this._selectedIndex>iIndex){	//如果有選擇項﹐那移除后﹐如果是以下的﹐就要變更索引
		this._selectedIndex--;
	}
}

//移除子項
_p.RemoveItemByObject = function(oItem){
	iIndex = this._items.indexOf(oItem);
	if(iIndex!=-1){
		this.RemoveItem(iIndex);
	}
}

//選擇某項
_p.SelectItem = function(iIndex){
	if(iIndex<0||iIndex>=this._items.length)	//不選取任何項
		iIndex=-1;							
	if(this._selectedIndex==iIndex)				//如果要選取的和當前選取的一樣﹐則不做動作
		return;
	else{
		if(this._selectedIndex!=-1){							//先取消上次的
			this._lastSelectedIndex = this._selectedIndex;		//記錄上次選中的﹐可讓客戶端取消選取
			this._items[this._selectedIndex].SetSelected(false);
		}
		this._selectedIndex =iIndex;
		if(iIndex!=-1){							//如果這次有選取
			this._items[this._selectedIndex].SetSelected(true);
		}
	}
	this.SetSelectedVisible();
}

//選擇某項
_p.SelectItemByObject = function(oItem){
	iIndex = this._items.indexOf(oItem);
	if(iIndex!=-1){
		this.SelectItem(iIndex);
	}
}

//把當前已選中的項次呈現在中間
_p.SetSelectedVisible = function(){
	if(this._selectedIndex!=-1){

		var conLeft = this._itemCon.style.posLeft;		//當前的左邊距
		var showWidth = this._centerSpan.parentElement.offsetWidth;	//當前的總寬度
		var conWidth = this._itemCon.offsetWidth;				//能顯示的寬度
		var itemLeft = this._selectedIndex * this._items[0]._width;		//項次的相對左邊距
		//讓它在可視范圍內
		if(conLeft + itemLeft<0){
			var cha = 0-(conLeft+itemLeft);
			this._itemCon.style.posLeft += cha;
		}
		else if(conLeft + itemLeft + this._items[0]._width >= showWidth){
			var cha = (conLeft+itemLeft) - showWidth + this._items[0]._width;
			this._itemCon.style.posLeft -= cha;
		}
	}
}

_p.dispose = function(oThis){
	TsUICom.prototype.dispose.call(oThis);
	for(var i=0;i<this._itemNum;i++){
		oThis._items[i].dispose();
		oThis._items[i] = null;
	}
	oThis._items = null;
	oThis._itemNum = 0;
	oThis._itemCon = null;			//包含所有子項的span
	if(oThis._initID != 0) window.clearInterval(oThis._initID);
	if(oThis._leftID != 0) window.clearInterval(oThis._leftID);
	if(oThis._rightID != 0) window.clearInterval(oThis._rightID);
	oThis._initID = 0;				//初始化設置滾動圖標的按鈕函數
	oThis._leftID = 0;				//向左滾動的函數
	oThis._rightID = 0;				//向右滾動的函數
	oThis._leftImg = null;
	oThis._rightImg = null;
	oThis._selectedIndex = -1;		//選中的項次ID
	oThis._lastSelectedIndex = -1;	//上次選中的ID
	oThis._aover = null;
	oThis._aout = null;
	oThis._adown = null;
	oThis._leftMove =null;
	oThis._rightMove =null;
	oThis._aup = null;
	oThis.__resize = null;
	oThis.__setImgSrc = null;
	oThis.__initArrow = null;
};