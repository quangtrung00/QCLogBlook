/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	仿Windows Xp的界面風格左邊的工具欄 */

//Xp風格左邊的工具欄      
function TsXpBar(nWidth,sCaption){
	TsUICom.call(this,nWidth,50);
	this._caption = sCaption;
	this._arrowimg = null;
	this._items = {};			//子項
	this._itemNum = 0;			//子項的數量
	this._bodyShow = true;		//主體是否顯示
	this._bodyInID = 0;			//收起來的函數
	this._bodyOutID = 0;		//展開來的函數
	this._initialHeight = 0;	//記錄最初的高度
	this._oBody = null;			//項次所在的對象
	this._oDingDiv = null;		//用來固定高度的div
	
	var oThis = this;
	
	this._eImgArrowClick = function(e){
		oThis.ArrowClick(e);
	}
	
	this.BodyContract = function(){
		if(oThis._initialHeight==0){
			 oThis._initialHeight = oThis._oBody.offsetHeight;
			 oThis._oDingDiv.style.height = oThis._initialHeight;
		}
		if(oThis._oBody.offsetHeight>5){
		//if(oThis._oBody.style.posTop>0-oThis._oBody.offsetHeight){
			oThis._oBody.style.height = oThis._oBody.offsetHeight - 5 ;
			//oThis._oBody.style.posTop -= 5;
		}
		else{
			oThis._oBody.style.display = "none";
			window.clearInterval(oThis._bodyInID);
		}
	};
 
	this.BodyExpand = function(){
		//alert(oThis._oBody.offsetHeight<oThis._initialHeight);
		if(oThis._oBody.offsetHeight<oThis._initialHeight){
			oThis._oBody.style.height = oThis._oBody.offsetHeight + 5 ;
			if(oThis._oBody.style.height >oThis._initialHeight) oThis.oBody.style.height = oThis._initialHeight;
		}
		else{
 			window.clearInterval(oThis._bodyOutID);
		}
	};

}

 _p=TsXpBar.prototype = new TsUICom;
 _p.className = "TsXpBar";
 
 
_p.setImgDir = function(){
	TsXpBar.ImgDir  = TsUICom.ImgDir + "/DgyyWebWinNew";
	TsXpBar.UpArrow = TsXpBar.ImgDir + "/arrow_up.gif";
	TsXpBar.DownArrow = TsXpBar.ImgDir + "/arrow_down.gif";
}

 
 _p._getCaption = function(){
 
 	var oDing  = document.createElement("DIV");		//用來固定整個高度	oDing.title = this._caption;
	oDing.style.width = "100%";
	oDing.style.padding = "0px";

	var oDivBody = document.createElement("DIV");		
	oDivBody.style.borderTop = oDivBody.style.borderLeft = oDivBody.style.borderRight = "1px solid white";
	oDivBody.style.overflow = "hidden";
	oDivBody.style.padding = "0px";
	oDivBody.style.width = "100%";

	var oTblCaption = document.createElement("<TABLE style='width:expression(eval(this.parentElement.clientWidth));'>");
	//oTblCaption.style.borderTop = oTblCaption.style.borderLeft = oTblCaption.style.borderRight = "1px solid white";
	oTblCaption.style.tableLayout = "fixed";
	oTblCaption.cellSpacing = "0";
	oTblCaption.cellPadding = "0";
	//oTblCaption.style.width = "100%";
	oTblCaption.style.filter = "progid:DXImageTransform.Microsoft.gradient(startColorStr=#FFffffff,endColorStr=#ffcedbff,GradientType=1)";
	
	var oTbyCaption = document.createElement("TBODY");
	
	var oTrCaption = document.createElement("TR");
	
	var oTdCaption = document.createElement("TD");
	oTdCaption.style.padding="2 0 2 5";
	
	var oDivTitle = document.createElement("DIV");	
	oDivTitle.innerHTML = this._caption;
	oDivTitle.style.fontSize = "8pt";
	oDivTitle.style.fontFamily = "verdana";
	oDivTitle.style.fontWeight = "bold";
	oDivTitle.style.color = "#215dc6";
	oDivTitle.style.whiteSpace = "nowrap";
	oDivTitle.style.textOverflow = "ellipsis";
	oDivTitle.style.overflow = "hidden";
	oDivTitle.style.width = "100%";
	//oDivTitle.style.border = "1px solid red";

	oTdCaption.appendChild(oDivTitle);
	
	var oTdImg = document.createElement("TD");
	oTdImg.style.width="20px";
	oTdImg.style.padding="2 5 2 0";
	oTdImg.style.textAlign = "right";
	
	var oImgArrow = document.createElement("IMG");
	this._arrowimg = oImgArrow;
	oImgArrow.src = TsXpBar.UpArrow;
	oImgArrow.attachEvent("onclick",this._eImgArrowClick);
	
	oTdImg.appendChild(oImgArrow);
	
	oTrCaption.appendChild(oTdCaption);
	oTrCaption.appendChild(oTdImg);
	
	oTbyCaption.appendChild(oTrCaption);
	oTblCaption.appendChild(oTbyCaption); 
	oDivBody.appendChild(oTblCaption);
	oDing.appendChild(oDivBody);
	return oDing;
};

_p._getMain = function(){
	
	var oDing  = document.createElement("DIV");		//用來固定整個高度	oDing.style.width = "100%";
	oDing.style.padding = "0px";

	var oDivBody = document.createElement("DIV");		
	oDivBody.style.borderBottom = oDivBody.style.borderLeft = oDivBody.style.borderRight = "1px solid white";
	oDivBody.style.width = "100%";
	oDivBody.style.overflow = "hidden";
	oDivBody.style.position = "relative";
	oDivBody.style.backgroundColor = "#d6dff7";
	oDivBody.style.padding = "0px";

	var oTblBody = document.createElement("<TABLE style='width:expression(eval(this.parentElement.clientWidth-2));'>");
	//oTblBody.style.width = this._width -2;
	oTblBody.style.tableLayout = "fixed";
	oTblBody.cellSpacing = "0";
	oTblBody.cellPadding = "0";
	var oTbyBody = document.createElement("TBODY");
	for(var i=0;i<this._itemNum;i++){
		var oTrItem = document.createElement("TR");
		var oTdItem = document.createElement("TD");
		oTdItem.style.padding = "1 5";
		var item = this._items[i].getBody();
		oTdItem.appendChild(item);
		oTrItem.appendChild(oTdItem);
		oTbyBody.appendChild(oTrItem);
	}
	
	oTblBody.appendChild(oTbyBody);
	oDivBody.appendChild(oTblBody);
	oDing.appendChild(oDivBody);
	this._oBody = oDivBody;
	if(this._bodyShow){
		this._oBody.style.display = "block";
		this._arrowimg.src = TsXpBar.UpArrow;
	}
	else{
		this._oBody.style.display = "none";
		this._arrowimg.src = TsXpBar.DownArrow;
	}

	//this._oDingDiv = oDing;		//下面的工具欄位置不影響
	this._oDingDiv = oDivBody;		//下面的工具欄位置受影響
	return oDing;
};
 
 //創建工具欄
 _p.setBody = function(){
	var barCon = document.createElement("DIV");		//窗口容器
	barCon.style.width = this._width;
	barCon.appendChild(this._getCaption());
	barCon.appendChild(this._getMain());
	this._body = barCon;
 };
 
 //增加子項目
 _p.AddItem = function(oItem){
	this._items[this._itemNum++] = oItem;
 };
 
 //顯示body
 _p.ShowBody = function(){
	this._bodyShow = true;
	
	if(this._body!=null){
		this._oBody.style.display = "block";
		this._arrowimg.src = TsXpBar.UpArrow;
	}
 }
 
 //隱藏body
 _p.HideBody = function(){
	this._bodyShow = false;
	if(this._body!=null){
		this._oBody.style.display = "none";
		this._arrowimg.src = TsXpBar.DownArrow;
	}
 }
 
 _p.ArrowClick = function(e){
	//alert(e);
	if(this._oBody !=null){
		if(this._bodyShow){
			//alert('收縮');
			e.srcElement.src = TsXpBar.DownArrow;
			this._bodyShow = false;
			if(this._bodyOutID!=0) window.clearInterval(this._bodyOutID);
			this._oBody.style.display = "block";
			this._bodyInID = window.setInterval(this.BodyContract,20);
		}
		else{
			//alert('展開');
			e.srcElement.src = TsXpBar.UpArrow;
			this._bodyShow = true;
			if(this._bodyInID!=0) window.clearInterval(this._bodyInID);
			this._oBody.style.display = "block";
			this._bodyOutID = window.setInterval(this.BodyExpand,20);
		}
	}
 }
 
 _p.dispose = function(){
	TsUICom.prototype.dispose.call(this);
	if(this._disposed)return;
	this._body = null;
	this._caption = null;
	
	for(var i=0;i<this._itemNum;i++){
		this._items[i].dispose();
		this._items[i] = null;
	}
	
	this._items = null;			//子項
	this._itemNum = null;			//子項的數量
	this._bodyShow = null;		//主體是否顯示
	this._bodyInID = null;			//收起來的函數
	this._bodyOutID = null;		//展開來的函數
	this._initialHeight = null;	//記錄最初的高度
	this._oBody = null;			//項次所在的對象
	this._oDingDiv = null;		//用來固定高度的div
	
	
	this._eImgArrowClick = null;
	
	this.BodyContract = null;
 
	this.BodyExpand = null;
	
 }