 /*
 * XpItem 1.0
 * Copyright (c) 2004-2005 Kw.Tsou
 *
 * XpWin belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires
 * license.
 * Author:		Kw.Tsou
 * Date:		2004.4.2
 * Description:	項次對象
 */

////////////////////////////////////////////////////////////////////
//[項次]根類

function TsXpItem(nWidth,nHeight,sText,sICon){
	TsUICom.call(this,nWidth,nHeight);
	this._text = sText;			//文字
	this._icon = sICon;			//圖標
	this._selected = false;		//是否被選中
}

_p = TsXpItem.prototype = new TsUICom;
_p._className="TsXpItem";

_p.GetSelected = function(){
	return this._selected;
};

_p.GetText = function(){
	return this._text;
};

_p.SetText = function(v){
	this._text = v;
};

_p.GetICon = function(){
	return this._icon;
};

_p.SetICon = function(v){
	this._icon = v;
};

_p.dispose = function(){
	TsUICom.prototype.dispose.call(this);
	this._text = null;			//文字
	this._icon = null;			//圖標
	this._selected = null;		//是否被選中};



////////////////////////////////////////////////////////////////////
//[菜單欄項次]
function TsXpMenuItem(sText,sICon){
	//一個參數表示直接設定html
	this._isSetHTML = false;			//是否直接設定為HTML代碼
	if(TsXpMenuItem.arguments.length==1)
		this._isSetHTML = true;
	TsXpItem.call(this,TsXpMenuItem.Width,"100%",sText,sICon);
	
	this._onItemSelect = [];
	
	var oThis = this;
	
	this._closeover = function(e){
		var closeimg = oThis._body;
		if(!oThis._selected){
			closeimg.style.borderTop = closeimg.style.borderLeft = "1px solid #FFFFFF";
			closeimg.style.borderRight = closeimg.style.borderBottom = "1px solid #736D63";
			closeimg.style.color = "black";
		}
		else{
			closeimg.style.backgroundColor = "#ffffff";
		}
	};
	
	this._closeout = function(e){
		var closeimg = oThis._body;
		if(!oThis._selected){
			closeimg.style.color = "gray";
			closeimg.style.border = "1px solid #FFF7E7";
			closeimg.style.background = "none";
		}
		else{
			closeimg.style.background = "none";
		}
	};
	
	
	this._closedown = function(e){
		var closeimg = oThis._body;
		if(!oThis._selected){
			closeimg.style.borderRight = closeimg.style.borderBottom = "1px solid #FFFFFF";
			closeimg.style.borderTop = closeimg.style.borderLeft = "1px solid #736D63";
		}
		else{
			closeimg.style.backgroundColor = "#ffffff";
		}
	};
	
	this._select = function(e){
		oThis._selected = true;
		oThis._body.style.color = "black";
		oThis._body.style.backgroundColor = "#f3f3f3";
		oThis._body.style.borderRight = oThis._body.style.borderBottom = "1px solid #FFFFFF";
		oThis._body.style.borderTop = oThis._body.style.borderLeft = "1px solid #736D63";
		for(var i=0;i<oThis._onItemSelect.length;i++){
			var onEvent = oThis._onItemSelect[i];
			onEvent(e);
		}
	};
	
	this._imgerr = function(e){
		var img = e.srcElement;
		img.outerHTML = "<span title='" + img.src + "找不到' style='color:red'>圖片路徑錯誤。路徑﹕" + img.src + "</span>";
	}
}

_p=TsXpMenuItem.prototype=new TsXpItem;
TsXpMenuItem.Width = 78;

_p._className="TsXpMenuItem";

_p.setItemStyle = function(styleText){
	if(this._body!=null){
		this._body.style.cssText = styleText;			//以下style不可更改
		this._body.style.overflow = "hidden";
		this._body.style.padding = "0";
		this._body.style.margin = "0";
		this._body.style.textAlign = "center";
		this._body.style.cursor = "pointer";
		this._body.style.border = "1px solid #FFF7E7";
		this._body.style.background = "none";
		this._body.style.verticalAlign = "top";
		this._body.style.width = this._width;
		this._body.style.height = this._height;
	}
}

_p.getItemStyle = function(){
	if(this._body!=null)
		return this._body.style.cssText;
	return "";
}

_p.setIConStyle = function(styleText){
	if(!this._isSetHTML&&this._body!=null&&this._body.children.length==2){
		this._body.children[0].style.cssText = styleText;
	}

}

_p.getIConStyle = function(){
	if(!this._isSetHTML&&this._body!=null&&this._body.children.length==2){
		return this._body.children[0].style.cssText ;
	}
	return "";
}

_p.setTextStyle = function(stylename,styleText){
	//debug.debug();
	if(!this._isSetHTML&&this._body!=null&&this._body.children.length==2){
		this._body.children[1].style[stylename]=styleText;
	}
	else if(!this._isSetHTML&&this._body!=null&&this._body.children.length==1){
		this._body.children[0].style[stylename]=styleText;
	
	}

}

_p.getTextStyle = function(stylename){
	if(!this._isSetHTML&&this._body!=null&&this._body.children.length==2){
		return this._body.children[1].style[stylename];
	}
	else if(!this._isSetHTML&&this._body!=null&&this._body.children.length==1){
		return this._body.children[1].style[stylename];
	}
	return "";
}

_p.AddSelectEvent = function(oFun){
	//alert(typeof(oFun));
	if(typeof(oFun)=="function"){
		this._onItemSelect.insertAt(oFun,0);
	}
};

_p.SetSelected = function(flag){
	if(this._body!=null){
		if(flag){
			this._selected = true;
			this._body.style.color = "black";
			this._body.style.borderRight = this._body.style.borderBottom = "1px solid #FFFFFF";
			this._body.style.borderTop = this._body.style.borderLeft = "1px solid #736D63";
			//this._body.style.border = "1px solid #0a246a";
			//this._body.style.backgroundColor = "#8592b5";	
		}
		else{
			this._selected = false;
			this._body.style.color = "gray";
			//this._body.all[0].style.filter = "gray";
			this._body.style.border = "1px solid #FFF7E7";
			//this._body.style.border = "1px solid #FFF7E7";
			this._body.style.background = "none";

		}
	}
};

_p.setBody = function(){
	if(this._body!=null)
		return;
	var oDivBody = document.createElement("SPAN");
	this._body = oDivBody;
	oDivBody.style.overflow = "hidden";
	oDivBody.style.padding = "0";
	oDivBody.style.margin = "0";
	oDivBody.style.textAlign = "center";
	oDivBody.style.cursor = "pointer";
	oDivBody.style.border = "1px solid #FFF7E7";
	oDivBody.style.background = "none";
	oDivBody.style.verticalAlign = "top";
	oDivBody.style.width = this._width;
	oDivBody.style.height = this._height;
	oDivBody.attachEvent("onclick",this._select);
	oDivBody.attachEvent("onmouseover",this._closeover);
	oDivBody.attachEvent("onmouseout",this._closeout);
	oDivBody.attachEvent("onmousedown",this._closedown);
	
	if(this._isSetHTML){
		oDivBody.innerHTML = this._text;
		//alert(oDivBody.outerHTML);
		return;
	}
	
	if(this._icon!=""){
		var oImgICon = document.createElement("IMG");
		oImgICon.src = this._icon;
		//oImgICon.style.verticalAlign = "top";
		//oImgICon.style.width = "50";
		//oImgICon.style.height = "50";
		oImgICon.attachEvent("onerror",this._imgerr);
		oDivBody.appendChild(oImgICon);
		oImgICon = null;
	}
	
	var oSpnText = document.createElement("<SPAN>");
	//oSpnText.style.border = "1px solid red";
	oSpnText.style.height = "100%";
	oSpnText.style.paddingTop = "16";
	//oSpnText.style.width = "100%";
	oSpnText.style.textAlign="center";
	oSpnText.style.cursor = "pointer";
	oSpnText.style.fontFamily = "新細明體";
	oSpnText.style.fontSize = "9pt";
	oSpnText.style.color = "gray";
	oSpnText.innerHTML = this._text;
	oDivBody.appendChild(oSpnText);
	
	
	oDivBody = null;
	oSpnText = null;
	return;
};

_p.dispose = function(){
	TsXpItem.prototype.dispose.call(this);
	for(var i=0;i<this._onItemSelect.length;i++){
		this._onItemSelect[i] = null;
	}
	this._closeover = null;
	this._closeout = null;
	this._closedown = null;
	this._select = null;
};

////////////////////////////////////////////////////////////////////
//[工具欄項次]
function TsXpBarItem(sText,sICon){
	TsXpItem.call(this,"","",sText,sICon);
	this._onItemClick = null;
	this.Busy = false;		//是否在loading過程中
	var oThis = this;
	this._click = function(){
		if(oThis._onItemClick!=null){
			oThis._onItemClick();
		}
	};
}

 _p=TsXpBarItem.prototype = new TsXpItem();
 
//返回一個DOM物件
 _p.setBody = function(){
	var barCon = document.createElement("DIV");
	barCon.style.whiteSpace = "nowrap";
	barCon.style.textOverflow = "ellipsis";
	barCon.style.overflow = "hidden";
	barCon.style.width = "100%";
	barCon.style.height = "100%";
	barCon.style.fontFamily = "verdana";
	barCon.style.fontSize = "8pt";
	barCon.style.color = "#215dc6";
	barCon.style.cursor = "pointer";
	//barCon.style.border = "1px solid red";
	barCon.title = this._text;
	barCon.attachEvent("onclick",this._click);
	if(this._icon!=""){
		var sicon = document.createElement("IMG");
		sicon.src = this._icon;
		barCon.appendChild(sicon);
		sicon = null;
	}
	var stext = document.createElement("SPAN");
	stext.innerHTML = this._text;
	barCon.appendChild(stext);
	this._body = barCon;
	barCon = null;
	stext = null;
};

_p.dispose = function(){
	if(this._disposed)return;
	this._onItemClick = null;
	this.Busy = null;		//是否在loading過程中
	this._click = null;
	TsXpItem.prototype.dispose.call(this);
};


////////////////////////////////////////////////////////////////////
//[Tab窗口管理器項次]
function TsScrollTabItem(sText){
	TsXpItem.call(this,TsScrollTabItem.BodyWidth,TsScrollTabItem.BodyHeight,sText,"");
	this._sImgSeperator = null;	//分隔圖片(方便排版放在外面)
	this._onClose = [];				//響應關閉事件
	this._onSelect = [];			//響應選擇事件
	this._selected = false;			//狀態(是否選中)
	var oThis = this;

	//項次雙擊事件
	this._dbClick = function(e){
		oThis._close(e);
		return false;
	}
	
	//項次單擊事件
	this._click = function(e){
		oThis._select(e);
	}
	
	//關閉方法
	this._close = function(e){
		for(var i=0;i<oThis._onClose.length;i++){
			var onEvent = oThis._onClose[i];
			onEvent(e);
		}
		oThis.Close();
	};
	
	//選擇方法
	this._select = function(e){
		if(!oThis._selected){
			oThis.Select();
			for(var i=0;i<oThis._onSelect.length;i++){
				var onEvent = oThis._onSelect[i];
				onEvent(e);
			}
		}
	};
	
	//將項次的顯示設為顯示狀態
	this._setSelectedStatus = function(){		
		if(this._body!=null){
			var oDivBody = this._body;
			oDivBody.style.cssText = TsScrollTabItem.SelectStyle;		//默認不選擇
			oDivBody.style.width = this._width;
			oDivBody.style.height =  this._height;
			oThis._sImgSeperator.style.visibility = "hidden";
			oDivBody.innerHTML = "<span style='border-top:2px solid #7a8485;height:100%;background-image:url(" + TsScrollTabItem.SelectedLeftImg  + ");clear:right;width:" + TsScrollTabItem.BodyLeftImgWidth + ";background-repeat :no-repeat;'></span><span style='width:90%;height:100%;padding-top:10px;border-top:1px solid #7a8485;clear:both;background-image:url(" + TsScrollTabItem.SelectedMiddleImg + ")'> " + this._text + "</span><span style='border-top:2px solid #7a8485;height:100%;background-image:url(" + TsScrollTabItem.SelectedRightImg  + ");clear:left;width:" + TsScrollTabItem.BodyRightImgWidth + ";background-repeat :no-repeat;'></span>";
		}
	}
	
	//將項次的顯示設為背后顯示狀態
	this._setNonSelectedStatus = function(){
		if(this._body!=null){
			var oDivBody = this._body;
			oDivBody.style.cssText = TsScrollTabItem.NonSelectStyle;		//默認不選擇
			oDivBody.style.width = this._width;
			oDivBody.style.height =  this._height;
			oThis._sImgSeperator.style.visibility = "visible";
			oDivBody.innerHTML = this._text;
		}
	}
}

_p=TsScrollTabItem.prototype=new TsXpItem;
_p._className="TsScrollTabItem";

TsScrollTabItem.BodyWidth = "100";				//項次寬
TsScrollTabItem.BodyHeight = "25";				//項次高
TsScrollTabItem.BodyLeftImgWidth = "8";			//選擇時﹐有一個span用來放左邊圖片﹐寬度為8
TsScrollTabItem.BodyRightImgWidth = "10";		//選擇時﹐有一個span用來放右邊圖片﹐寬度為10
//TsScrollTabItem.NonSelectedBgImg = "/WebApClient/WebUI/images/XpTabBar/noSelectBg.gif";		//未選擇時的背景圖片
TsScrollTabItem.SelectStyle = "border-bottom:1px solid #efece5;clear:both;vertical-align:bottom;color:black;background-color:#7a8485;cursor:default;text-align:center;padding:0px 5px 0px 5px;overflow-x:visible;overflow-y:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:8pt;font-family:Verdana";				//選擇時的樣式
TsScrollTabItem.NonSelectStyle = "border-bottom:1px solid white;clear:both;vertical-align:bottom;color:white;background-color:#7a8485;cursor: pointer;text-align:center;padding:6px 5px 0px 5px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:8pt;font-family:Verdana";			//未選擇時的樣式
_p.setImgDir = function(){
	TsScrollTabItem.ImgDir  = TsUICom.ImgDir + "/XpTabBar";
	TsScrollTabItem.SeperatorImg = TsScrollTabItem.ImgDir + "/seperator.gif";					//分隔圖片
	TsScrollTabItem.SelectedLeftImg = TsScrollTabItem.ImgDir + "/ItemSelectLeft.gif";			//左邊圖片
	TsScrollTabItem.SelectedRightImg = TsScrollTabItem.ImgDir + "/ItemSelectRight.gif";		//右邊圖片
	TsScrollTabItem.SelectedMiddleImg = TsScrollTabItem.ImgDir + "/ItemSelectMiddle.gif";		//中間圖片
}


//選擇該項
_p.Select = function(){
	this._selected = true;
	this._setSelectedStatus();
}

//關閉該項
_p.Close = function(){
	this.dispose();
}

//blur該項
_p.Blur = function(){
	this._selected = false;
	this._setNonSelectedStatus();
}

//加入關閉事件
_p.AddCloseEvent = function(oFun){
	if(typeof(oFun)=="function"){
		this._onClose.insertAt(oFun,0);
	}
};

//加入選擇事件
_p.AddSelectEvent = function(oFun){
	if(typeof(oFun)=="function"){
		this._onSelect.insertAt(oFun,0);
	}
};

//是否選擇了該項
_p.GetSelected = function(){
	return this._selected;
};

//單擊項次
_p.ClickIt = function(){
	if(this._body)
		this._body.click();
}

_p.setBody = function(){
	if(this._body!=null)
		return;
	var oDivBody = document.createElement("SPAN");
	oDivBody.style.cssText = TsScrollTabItem.NonSelectStyle;		//默認不選擇
	oDivBody.style.width = this._width;
	oDivBody.style.height =  this._height;
	oDivBody.attachEvent("onclick",this._click);
	//oDivBody.attachEvent("ondblclick",this._dbClick);
	oDivBody.innerHTML = this._text;
	//oDivBody.innerHTML = "<span style='height:100%;background-image:url(" + TsScrollTabItem.SelectedLeftImg  + ");clear:right;width:" + TsScrollTabItem.BodyLeftImgWidth + ";background-repeat :no-repeat;vertical-align:bottom'></span><span style='height:100%;padding-top:10px;border-top:2px solid #7a8485;clear:both;background-image:url(" + TsScrollTabItem.SelectedMiddleImg + ")'> " + this._text + "</span><span style='height:100%;background-image:url(" + TsScrollTabItem.SelectedRightImg  + ");clear:left;width:" + TsScrollTabItem.BodyRightImgWidth + ";background-repeat :no-repeat;vertical-align:bottom;'></span>";
	
	oDivBody.title = "單擊這里顯示(" + this._text + ")的頁面";	
	this._body = oDivBody;
	//oDivBody.detachEvent("onclick",this._click);
	oDivBody = null;
};

//取得分隔圖片
_p.GetSeperatorImg = function(){
	if(this._sImgSeperator==null){
		var oImgSeparator = document.createElement("IMG");
		this._sImgSeperator = oImgSeparator;
		oImgSeparator.src = TsScrollTabItem.SeperatorImg;
		oImgSeparator.style.verticalAlign = "bottom";
		oImgSeparator = null;
	}
	return this._sImgSeperator;
}

_p.dispose = function(){
	if(this._disposed)return;
	if((this._body!=null)&&(this._body.parentElement!=null)){
		this._body.parentElement.removeChild(this._body);
		this._body.detachEvent("onclick",this._click);	
	}	
	this._body = null;
	if((this._sImgSeperator!=null)&&(this._sImgSeperator.parentElement!=null))
		this._sImgSeperator.parentElement.removeChild(this._sImgSeperator);
	this._sImgSeperator = null;			//分隔圖片(方便排版放在外面加入)
	
	for(var i=0;i<this._onClose.length;i++)
		this._onClose[i] = null;
	this._onClose = null;				//響應關閉事件
	for(var i=0;i<this._onSelect.length;i++)
		this._onSelect[i] = null;
	this._onSelect = null;				//響應關閉事件
	this._selected = null;				//狀態(是否選中)
	this._dbClick = null;
	this._click = null;
	this._close =null;
	this._select = null;
	this._setSelectedStatus = null;
	this._setNonSelectedStatus = null;
	TsXpItem.prototype.dispose.call(this);	
}