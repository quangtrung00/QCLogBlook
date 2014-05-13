/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	窗口Tab管理的對象 */

//合成對象(包括上面的TsScrollTab和下面的IEToolBar對象)
function XpTabBar(barWidth,barHeight){
 	TsUICom.call(this,barWidth,barHeight);
	
	this._titleBar = null;			//上面的TsScrollTab對象
	this._toolBar = null;			//下面的工具欄對象
	this._curFrame = null;			//當前的窗口	this._isBusy = false;			//當前是否有頁面正在載入(如果有﹐則不允許再有頁面載入)
	this._frameContainer = null;	//框架容器(td對象)
	
	var oThis = this;
	
	this._onPrevious = function(e){
		//if(oThis._curFrame!=null){
			history.back();
		//}
	}
	
	this._onNext = function(e){
		//if(oThis._curFrame!=null){
			history.forward();
		//}
	
	}
	
	this._onStop = function(e){
		if(oThis._curFrame!=null){
			oThis._curFrame.document.execCommand('stop');
			if(oThis._curFrame._processBar){
				oThis._curFrame._processBar.Stop();
				oThis._curFrame._processBar.dispose();
				oThis._curFrame._processBar = null;
			}
		}
	}
	
	this._onHome = function(e){
		//if(oThis._curFrame!=null){
		//	oThis._curFrame.src = XpTabBar.HomePage;
		//}
		if(top.window)
			top.window.location.href = XpTabBar.HomePage;
		else
			window.location.href = XpTabBar.HomePage;
	}
	
	this._onRefresh = function(e){
		if(oThis._curFrame!=null){
			var oldsrc = oThis._curFrame.src;
			var rindex = oldsrc.indexOf("refresh_id=-1");
			if(rindex>0){
				oThis._curFrame.src = oldsrc.substring(0,rindex-1) + oldsrc.substr(rindex + 13);
			}
			else if(oldsrc.indexOf("?")>0)
				oThis._curFrame.src = oldsrc + "&refresh_id=-1";
			else
				oThis._curFrame.src = oldsrc + "?refresh_id=-1";
		}
	}
	
	//當frame中的網頁已載入
	this._frameLoad = function(e){
		//alert('load');
		oThis._isBusy = false;		
		var theframe = e.srcElement;
		if(theframe._ctrlitem)
			theframe._ctrlitem.Busy = false;
		//theframe.style.visibility = "visible";
		theframe.style.display = "block";
		if(theframe._processBar){
			theframe._processBar.Stop();
			theframe._processBar.dispose();
			theframe._processBar = null;
		}
		try
		{
			//document.frames[theframe.name].attachEvent("onbeforeunload",oThis._frameUnLoad);	
			document.frames[theframe.name].attachEvent("onunload",oThis._frameUnLoad);
		}
		catch (e)
		{
			;
		}	
		//theframe.detachEvent("onload",oThis._frameLoad);	
	}
	
	//當frame中的網頁unload時
	this._frameUnLoad = function(e){
		//alert('unload');
		//debug.debug();
		var theframe = oThis._curFrame;
		if(theframe){
			oThis._isBusy = true;		
			//alert(theframe);
			if(theframe._ctrlitem)
				theframe._ctrlitem.Busy = true;
			//theframe.style.visibility = "visible";
			theframe.style.display = "none";
			if(theframe._processBar){
				theframe._processBar.Stop();
				theframe._processBar.dispose();
				theframe._processBar = null;
			}
			var p = new XpProcessBar("30%",10,"請稍候,頁面載入中...");
			theframe._processBar = p;
			oThis._frameContainer.appendChild(p.getBody());			//進度條
			p.Start(100,"50%");
		}
	}
}

XpTabBar.idCount = 0;
XpTabBar.HomePage = "http://172.16.1.25";
XpTabBar.WinHeight = "500px";		//窗口高度
_p = XpTabBar.prototype = new TsUICom;
_p._className="XpTabBar";

//打開一個新窗口(其它方法暫時不提供)
_p.openWin = function(title,src){
	var closecmd = "";					//關閉時要執行的函數名稱
	if(src.substring(0,1)=="1"){		//第1碼為1表示第2碼到|的為一個函數名稱
		closecmds = src.split("|");
		closecmd  = closecmds[0].substr(1);
		src = closecmds[1];
	}
	if(this._body==null)
		throw new Error("請先使用getBody方法加入對象體");
		
	//控制一個菜單項只能打開一個窗口(zkw 11.10)----
	//先判斷這個src是否已經打開過
	for(var i=1;i<=this._titleBar.GetItemLength();i++){
		var oItem = this._titleBar.GetItem(i);
		var oFrame = oItem.getCProperty("ctrlwin");
		if(oFrame){
			
			var oldsrc = oFrame.src;
			var rindex = oldsrc.indexOf("refresh_id=-1");
			if(rindex>0){
				oldsrc = oldsrc.substring(0,rindex-1) + oldsrc.substr(rindex + 13);
			}
			if(oldsrc==src){
				oItem.ClickIt();
				return;
			}
		
		}
	}
	//---------------------------------------------
			
		
		
	if(this._isBusy){
		alert("正在載入另一個頁面﹐請稍候...");
		return false;	
	}
	if(this._curFrame!=null){
		this._curFrame.style.display = "none";
	}
	this._isBusy = true;	
	var newItem = new TsScrollTabItem(title);
	this._titleBar.AddItem(newItem);
	var newFrame = document.createElement("iframe");
	this._curFrame = newFrame;
	newItem.setCProperty("ctrlwin",newFrame);
	newFrame.closecmd = closecmd;
	newFrame.style.border = "1px solid #cccccc";
	newFrame.frameBorder = "0";
	newFrame.style.height = XpTabBar.WinHeight;
	newFrame.style.width  = "100%";
	

	try{
		newFrame.name = "XpTabBarIFrameName" + (XpTabBar.idCount++);
		newFrame.id = newFrame.name;
	
		newFrame.src = src;
		this._frameContainer.appendChild(newFrame);
		
		//載入設定
		newItem.Busy = true;									//loading過程中
		newFrame._ctrlitem = newItem;
		//newFrame.style.visibility = "hidden";
		newFrame.style.display = "none";
		newFrame.attachEvent("onload",this._frameLoad);			//當載入完成后才進行顯示
		//alert(newFrame.name);
		//alert(document.frames[newFrame.name]);
		//document.frames(newFrame.name).attachEvent("onbeforeunload",this._frameUnLoad);			//當載入完成后才進行顯示		
		var p = new XpProcessBar("30%",10,"請稍候,頁面載入中...");
		newFrame._processBar = p;
		this._frameContainer.appendChild(p.getBody());			//進度條
		p.Start(100,"50%");
	}
	catch(ex){		//防止js出錯
		alert("頁面載入出錯,請與系統管理員聯系!"   + ex.description);
	}
	newFrame = null;
}

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
	//標題欄
	var fTr = document.createElement("tr");
	oTbody.appendChild(fTr);
	var fTd = document.createElement("Td");
	fTr.appendChild(fTd);
	//fTd.style.border = "1px solid red";
	//fTd.style.padding = "0px";
	var scrollTab = new TsScrollTab("100%");
	this._titleBar = scrollTab;
	var oThis = this;
	this._titleBar.OnSelect = function(){				//選擇事件
		var curWin = scrollTab._selectedItem.getCProperty("ctrlwin");		//當前選擇的窗口
		if((oThis._curFrame!=null)&&(oThis._curFrame!=curWin)){
			oThis._curFrame.style.display = "none";
			if(oThis._curFrame._processBar)
				oThis._curFrame._processBar._body.style.display = "none";
		}
		curWin.style.display = "block";
		if(curWin._processBar)
			curWin._processBar._body.style.display = "block";
		oThis._curFrame = curWin;
	}
	
	this._titleBar.OnClose = function(lastitem){		//關閉事件
		if(lastitem.Busy)
			oThis._isBusy = false;
		var lastWin = lastitem.getCProperty("ctrlwin");
		if(lastWin.closecmd!=""){
			eval(lastWin.closecmd);
		}
		lastWin.style.display = "none";
		if(lastWin._processBar)
			lastWin._processBar.dispose();
		oThis._frameContainer.removeChild(lastWin);
		if(oThis._curFrame==lastWin)oThis._curFrame = null;
		lastitem.setCProperty("ctrlwin",null);
		//debug.debug();
		if(scrollTab._selectedItem!=null){
			var curWin = scrollTab._selectedItem.getCProperty("ctrlwin");		//當前選擇的窗口
			curWin.style.display = "block";
			if(curWin._processBar)
				curWin._processBar.dispose();
			curWin._processBar = null;
			curWin._ctrlitem = null;
			oThis._curFrame = curWin;
		}
	}
	
	fTr.style.height = scrollTab.getHeight();
	fTd.appendChild(scrollTab.getBody());
	
	//工具欄
	var sTr = document.createElement("tr");
	oTbody.appendChild(sTr);
	var sTd = document.createElement("Td");
	sTr.appendChild(sTd);
	var tabbar = new IEToolBar("100%");
	tabbar.OnPrevious = this._onPrevious;
	tabbar.OnNext = this._onNext;
	tabbar.OnRefresh = this._onRefresh;
	tabbar.OnStop = this._onStop;
	tabbar.OnHome = this._onHome;
	this._toolBar = tabbar;
	fTr.height = tabbar.getHeight();
	sTd.appendChild(tabbar.getBody());
	
	//主體框架區
	var tTr = document.createElement("tr");
	oTbody.appendChild(tTr);
	var tTd = document.createElement("<td style='height:100%;vertical-align:top'>");
	tTr.appendChild(tTd);
	this._frameContainer = tTd;
	tTd.style.padding = "2px";
	tTd.style.backgroundColor = "#ffffff";
	
	oTable = null;
	oTbody = null;
	fTr = null;
	fTd = null;
	sTr = null;
	sTd = null;
	tTr = null;
	tTd = null;
}

_p.dispose = function(){
	if(this._disposed)return;
	//首先釋放項次的控制窗口
	for(var i=0;i<this._titleBar.GetItemLength();i++){
		var curItem =  this._titleBar.GetItem(i+1);
		var curWin = curItem.getCProperty("ctrlwin");
		curWin.document.close();
		curWin.parentElement.removeChild(curWin);
		curWin = null;
		curItem.setCProperty("ctrlwin",null);
	}
	this._titleBar.dispose();
	this._titleBar = null;			//上面的TsScrollTab對象
	this._toolBar.dispose();
	this._toolBar = null;			//下面的工具欄對象
	this._curFrame = null;			//當前的窗口
	this._frameContainer = null;	//框架容器(td對象)
	if((this._body!=null)&&(this._body.parentElement!=null))
		this._body.parentElement.removeChild(this._body);
	this._body = null;
	
	TsUICom.prototype.dispose.call(this);		
}


//模仿IE工具欄的對象(上一頁﹐下一頁﹐停止﹐刷新﹐主頁五個按鈕)
function IEToolBar(barWidth){
 	TsUICom.call(this,barWidth,IEToolBar.BarHeight);
 	
 	this.OnPrevious  = null;		//上一頁事件
 	this.OnNext = null;				//下一頁事件
 	this.OnStop = null;				//停止事件
 	this.OnRefresh = null;			//刷新事件
 	this.OnHome = null;				//主頁按鈕事件 
 	
 	var oThis = this;
 	
 	//鼠標over
 	this._aOver = function(e){
 		var src = e.srcElement;
 		if(src.tagName.toLowerCase()!="div"){
 			src = src.parentElement;		//單擊到圖片時﹐導入到td
 		}
 		src.style.border = IEToolBar.Img_OverBorder;
 		src.style.backgroundColor = IEToolBar.Img_OverBack;
 	}
 	
 	//鼠標out
 	this._aOut = function(e){
 		var src = e.srcElement;
 		if(src.tagName.toLowerCase()!="div"){
 			src = src.parentElement;		//單擊到圖片時﹐導入到td
 		}
 		src.style.border = "0px";
 		src.style.backgroundColor = "transparent";
 	
 	}
 	
 	//鼠標單擊
 	this._aClick = function(e){
 		var src = e.srcElement;
 		if(src.tagName.toLowerCase()!="td"){
 			src = src.parentElement;		//單擊到圖片時﹐導入到td
 		}
 		if((src.eType=="previous")&&(oThis.OnPrevious!=null)){
 			oThis.OnPrevious();
 		}
 		else if((src.eType=="next")&&(oThis.OnNext!=null)){
 			oThis.OnNext();
 		
 		}
 		else if((src.eType=="stop")&&(oThis.OnStop!=null)){
 			oThis.OnStop();
 		
 		}
 		else if((src.eType=="refresh")&&(oThis.OnRefresh!=null)){
  			oThis.OnRefresh();
		
 		}
 		else if((src.eType=="home")&&(oThis.OnHome!=null)){
 			oThis.OnHome();
 		}
 	
 	}
}


IEToolBar.BarHeight = "30";
IEToolBar.Style_Td = "color:gray;cursor: pointer;padding:0px;font-family:verdana;font-size:8pt;cursor:default;vertical-align:middle";						//工具欄樣式
IEToolBar.Img_OverBorder = "1px solid #A38D68";
IEToolBar.Img_OverBack = "#FFFFFF";
IEToolBar.Img_DownBack = "#FFFFcc";

 _p = IEToolBar.prototype = new TsUICom;
 _p._className="IEToolBar";
 
_p.setImgDir = function(){
	IEToolBar.ImgDir  = TsUICom.ImgDir + "/XpTabBar";
	IEToolBar.Img_BackGg = IEToolBar.ImgDir + "/IEBarBg.jpg";					//背景圖片
	IEToolBar.Img_Prefix = IEToolBar.ImgDir + "/IEBaPrefix.gif";					//左邊圖片
	IEToolBar.Img_PreviousOver = IEToolBar.ImgDir + "/PreviousOver.gif";
	IEToolBar.Img_NextOver = IEToolBar.ImgDir + "/NextOver.gif";
	IEToolBar.Img_HomeOver = IEToolBar.ImgDir + "/HomeOver.gif";
	IEToolBar.Img_RefreshOver = IEToolBar.ImgDir + "/RefreshOver.gif";
	IEToolBar.Img_StopOver = IEToolBar.ImgDir + "/StopOver.gif";
}

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
	var oTr = document.createElement("tr");
	oTbody.appendChild(oTr);
	oTr.style.backgroundImage = 'url(' + IEToolBar.Img_BackGg + ')';
	
	//第一個單元格
	var prefixTd = document.createElement("td");
	oTr.appendChild(prefixTd);
	prefixTd.style.cssText = IEToolBar.Style_Td;
	prefixTd.style.width = "12";
	prefixTd.style.backgroundImage = 'url(' + IEToolBar.Img_Prefix + ')';
	
	var aspan;
	
	var fTd = document.createElement("td");
	oTr.appendChild(fTd);
	fTd.style.width = "100";
	fTd.style.textAlign = "center";
	fTd.style.cursor = "pointer";
	aspan = document.createElement("div");
	fTd.appendChild(aspan);
	aspan.eType = "previous";
	//fTd.attachEvent("onmouseover",this._aOver);
	//fTd.attachEvent("onmouseout",this._aOut);
	fTd.attachEvent("onclick",this._aClick);
	fTd.title = "單擊這里回到上一頁";
	aspan.style.cssText = IEToolBar.Style_Td;
	aspan.innerHTML = '<img style="cursor: pointer;clear:right;border:0px solid red;vertical-align:middle" src = ' + IEToolBar.Img_PreviousOver + '><span style="cursor: pointer;border:0px solid green;clear:left;vertical-align:left">上一頁</span>';
	
	var sTd = document.createElement("td");
	oTr.appendChild(sTd);
	sTd.style.width = "100";
	sTd.style.textAlign = "center";
	sTd.style.cursor = "pointer";
	aspan = document.createElement("div");
	sTd.appendChild(aspan);
	aspan.eType = "next";
	//sTd.attachEvent("onmouseover",this._aOver);
	//sTd.attachEvent("onmouseout",this._aOut);
	sTd.attachEvent("onclick",this._aClick);
	sTd.title = "單擊這里進入下一頁";
	aspan.style.cssText = IEToolBar.Style_Td;
	aspan.innerHTML = '<img style="cursor: pointer;clear:right;border:0px solid red;vertical-align:middle" src = ' + IEToolBar.Img_NextOver + '><span style="cursor: pointer;border:0px solid green;clear:left;vertical-align:left">下一頁</span>';
	
	var tTd = document.createElement("td");
	oTr.appendChild(tTd);
	tTd.style.width = "100";
	tTd.style.textAlign = "center";
	tTd.style.cursor = "pointer";
	aspan = document.createElement("div");
	tTd.appendChild(aspan);
	aspan.eType = "stop";
	//tTd.attachEvent("onmouseover",this._aOver);
	//tTd.attachEvent("onmouseout",this._aOut);
	tTd.attachEvent("onclick",this._aClick);
	tTd.title = "單擊這里停止載入";
	aspan.style.cssText = IEToolBar.Style_Td;
	aspan.innerHTML = '<img style="cursor: pointer;clear:right;border:0px solid red;vertical-align:middle" src = ' + IEToolBar.Img_StopOver + '><span style="cursor: pointer;border:0px solid green;clear:left;vertical-align:left">停&nbsp;止</span>';
	
	var foTd = document.createElement("td");
	oTr.appendChild(foTd);
	foTd.style.width = "100";
	foTd.style.textAlign = "center";
	foTd.style.cursor = "pointer";
	aspan = document.createElement("div");
	foTd.appendChild(aspan);
	aspan.eType = "refresh";
	//foTd.attachEvent("onmouseover",this._aOver);
	//foTd.attachEvent("onmouseout",this._aOut);
	foTd.attachEvent("onclick",this._aClick);
	foTd.title = "單擊這里重新整理頁面";
	aspan.style.cssText = IEToolBar.Style_Td;
	aspan.innerHTML = '<img style="cursor: pointer;clear:right;border:0px solid red;vertical-align:middle" src = ' + IEToolBar.Img_RefreshOver + '><span style="cursor: pointer;border:0px solid green;clear:left;vertical-align:left">重新整理</span>';
	
	var fiTd = document.createElement("td");
	oTr.appendChild(fiTd);
	fiTd.style.width = "100";
	fiTd.style.textAlign = "center";
	fiTd.style.cursor = "pointer";
	aspan = document.createElement("div");
	fiTd.appendChild(aspan);
	aspan.eType = "home";
	//fiTd.attachEvent("onmouseover",this._aOver);
	//fiTd.attachEvent("onmouseout",this._aOut);
	fiTd.title = "單擊這里回到開始畫面";
	fiTd.attachEvent("onclick",this._aClick);
	aspan.style.cssText = IEToolBar.Style_Td;
	aspan.innerHTML = '<img style="cursor: pointer;clear:right;border:0px solid red;vertical-align:middle" src = ' + IEToolBar.Img_HomeOver + '><span style="cursor: pointer;border:0px solid green;clear:left;vertical-align:left">系統首頁</span>';
	
	//空白td
	var blankTd = document.createElement("td");
	oTr.appendChild(blankTd);
	blankTd.style.cssText = IEToolBar.Style_Td;
	blankTd.innerHTML = '&nbsp;';
	
	oTable = null;
	oTbody = null;
	oTr = null;
	prefixTd = null;
	fTd = null;
	sTd = null;
	tTd = null;
	foTd = null;
	fiTd = null;
	blankTd = null;
	aspan = null;
}

_p.dispose = function(){
	if(this._disposed)return;
	if((this._body!=null)&&(this._body.parentElement!=null))
		this._body.parentElement.removeChild(this._body);
	this._body = null;

 	this.OnPrevious  = null;		//上一頁事件
 	this.OnNext = null;				//下一頁事件
 	this.OnStop = null;				//停止事件
 	this.OnRefresh = null;			//刷新事件
 	this.OnHome = null;				//主頁按鈕事件 
 	this._aOver = null;
 	this._aOut = null;
 	this._aClick = null;
 	this._aMousedown = null;
 	this._aMouseup = null;
	TsUICom.prototype.dispose.call(this);	
}

///////////////////////////////////////////////////////////////////////////
 //滾動Tab對象
 function TsScrollTab(nWidth){
	TsUICom.call(this,nWidth,TsScrollTab.BodyHeight);
	this._items =  new DLinkList();		//用雙鏈表表示任務欄項次
	this._selectedItem = null;			//當前選中的項次
	this._itemCon = null;				//包含所有子項的span
	this._centerSpan = null;			//中間這個span
	this._leftID = 0;					//向左滾動的函數
	this._rightID = 0;					//向右滾動的函數
	
	this._lastSeperatorImg = null;		//上一個隱藏的分隔圖片
	
	this.OnSelect = null;				//當有tab選擇時的事件
	this.OnClose = null;				//tab關閉時的事件
	
	var oThis =this;
	
	this._aover = function(e){
		//debug.debug();
		var cW = oThis._itemCon.offsetWidth;					//已有總寬度
		var mW = oThis._centerSpan.clientWidth;					//可顯現寬度
		if((cW>mW)||((cW<mW)&&((oThis._itemCon.style.posLeft<=0)||(oThis._itemCon.style.posLeft>=mW-cW)))){
			var aDiv = e.srcElement;
			if(aDiv.scrollType == "right"){
				var minLeft = mW - cW;
				if(oThis._itemCon.style.posLeft <= minLeft){
					e.srcElement.title = "已到盡頭了";
					e.srcElement.style.cursor = "default";
					return;
				}
				else{
					e.srcElement.title = "單擊這里向右滾動顯示被隱藏的項次";
					e.srcElement.style.cursor = "pointer";
				}
			}
			else if(aDiv.scrollType == "left"){
				if(oThis._itemCon.style.posLeft >=0){
					e.srcElement.title = "已到盡頭了";
					e.srcElement.style.cursor = "default";
					return;
				}
				else{
					e.srcElement.title = "單擊這里向左滾動顯示被隱藏的項次";
					e.srcElement.style.cursor = "pointer";
				}
			}
			
			//aDiv.style.borderTop = aDiv.style.borderLeft = "1px solid #FFFFFF";
			//aDiv.style.borderRight = aDiv.style.borderBottom = "1px solid #736D63";
		}
		else{
			e.srcElement.title = "沒有項次被隱藏";
			e.srcElement.style.cursor = "default";
		}
	};
	
	this._aout = function(e){
		var cW = oThis._itemCon.offsetWidth;						//已有總寬度
		var mW = oThis._centerSpan.clientWidth;						//可顯現寬度
		if(cW>mW){
			var aDiv = e.srcElement;
			//aDiv.style.border = "0px solid #FFF7E7";
		}
	};
	
	this._adown = function(e){
		var cW = oThis._itemCon.offsetWidth;						//已有總寬度
		var mW = oThis._centerSpan.clientWidth;						//可顯現寬度
		if((cW>mW)||((cW<mW)&&((oThis._itemCon.style.posLeft<=0)||(oThis._itemCon.style.posLeft>=mW-cW)))){
			var aDiv = e.srcElement;
			if(aDiv.scrollType == "right"){
				var minLeft = mW - cW;
				if(oThis._itemCon.style.posLeft <= minLeft){
					//e.srcElement.title = "已到盡頭了";
					return;
				}
			}
			else if(aDiv.scrollType == "left"){
				if(oThis._itemCon.style.posLeft >=0){
					//e.srcElement.title = "已到盡頭了";
					return;
				}
			}
			//aDiv.style.borderRight = aDiv.style.borderBottom = "1px solid #FFFFFF";
			//aDiv.style.borderTop = aDiv.style.borderLeft = "1px solid #736D63";
			
			if(aDiv.scrollType == "right"){
				if(oThis._rightID != 0) window.clearInterval(oThis._rightID);
				oThis._leftID = window.setInterval(oThis._leftMove,50);
			}
			else if(aDiv.scrollType == "left"){
				if(oThis._leftID != 0) window.clearInterval(oThis._leftID);
				oThis._rightID = window.setInterval(oThis._rightMove,50);
			}
		}
	};
	
	this._leftMove = function(e){
		var minLeft = oThis._centerSpan.clientWidth - (oThis._itemCon.offsetWidth);
		oThis._itemCon.style.posLeft -= 20;
		if(oThis._itemCon.style.posLeft < minLeft){
			oThis._itemCon.style.posLeft = minLeft;
			window.clearInterval(oThis._leftID); 
		}
	};
	
	this._rightMove = function(e){
		oThis._itemCon.style.posLeft += 20;
		if(oThis._itemCon.style.posLeft >=0){
			oThis._itemCon.style.posLeft = 0;
			window.clearInterval(oThis._rightID); 
		}
	};
	
	this._aup = function(e){
		var cW = oThis._itemCon.offsetWidth;						//已有總寬度
		var mW = oThis._centerSpan.clientWidth;						//可顯現寬度
		if((cW>mW)||((cW<mW)&&((oThis._itemCon.style.posLeft<=0)||(oThis._itemCon.style.posLeft>=mW-cW)))){
			var aDiv = e.srcElement;
			if(aDiv.scrollType == "right"){
				var minLeft = mW - cW;
				if(oThis._itemCon.style.posLeft <= minLeft){
					//e.srcElement.title = "已到盡頭了";
					return;
				}
			}
			else if(aDiv.scrollType == "left"){
				if(oThis._itemCon.style.posLeft >=0){
					//e.srcElement.title = "已到盡頭了";
					return;
				}
			}
			//aDiv.style.border = "0px solid #FFF7E7";
			if(oThis._rightID != 0) window.clearInterval(oThis._rightID);
			if(oThis._leftID != 0) window.clearInterval(oThis._leftID);
		}
	};
	
	//關閉按鈕單擊
	this._closeClick = function(e){
		if(oThis._selectedItem!=null){
			var aitem = oThis._selectedItem;
			oThis.RemoveItem(oThis._selectedItem);
			var closeWidth = aitem._body.offsetWidth;		//應該減去的寬度
			if(oThis.OnClose!=null)oThis.OnClose(aitem);
			aitem.Close();
			oThis._itemCon.style.posWidth -= closeWidth;
		}
	}
	
	//鼠標在關閉按鈕上滑動
	this._closeOver = function(e){
		var aimg = e.srcElement;
		if(oThis._selectedItem!=null){
			aimg.style.cursor = "pointer";
			aimg.title = "單擊這里關閉顯示按鈕";
		}
		else{
			aimg.style.cursor = "default";
			aimg.title = "";
		}	
	}
 }

_p = TsScrollTab.prototype = new TsUICom;
_p.className = "TsScrollTab";

////////常數設定區(更改時只能改這里)////////////////////////////////////////

TsScrollTab.BodyHeight = 23;														//高度
TsScrollTab.LeftImgWidth = 5;														//左邊圖片寬度
TsScrollTab.RightImgWidth = 3;														//右邊圖片寬度
TsScrollTab.ArrowImgWidth = 22;														//箭頭td寬度


_p.setImgDir = function(){
	TsScrollTab.ImgDir  = TsUICom.ImgDir + "/XpTabBar";
	TsScrollTab.LeftImg = TsScrollTab.ImgDir + "/1_1.jpg";					//左邊的圖片
	TsScrollTab.RightImg = TsScrollTab.ImgDir + "/TabScrollRightBg.gif";	//右邊的圖片
	TsScrollTab.ArrowLeftImg = TsScrollTab.ImgDir + "/ArrowLeft.gif";		//左箭頭
	TsScrollTab.ArrowRightImg = TsScrollTab.ImgDir + "/ArrowRight.gif";	//右箭頭
	TsScrollTab.ArrowCloseImg = TsScrollTab.ImgDir + "/ArrowClose.gif";	//關閉箭頭
}

////////常數設定區(結束)////////////////////////////////////////////////////

//取得主體對象
_p.setBody = function(){
	if(this._body!=null)
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
	var oTr = document.createElement("tr");
	oTbody.appendChild(oTr);
	
	//加入左角的圖片
	var fTd = document.createElement("td");
	oTr.appendChild(fTd);
	fTd.style.cssText = "overflow:hidden;text-overflow:ellipsis;white-space:nowrap";
	fTd.style.width = TsScrollTab.LeftImgWidth;
	fTd.style.backgroundImage = 'url(' + TsScrollTab.LeftImg + ')';		
		
	//主體容器td
	var tdStr = "<td style='overflow:hidden;text-overflow:ellipsis;white-space:nowrap;text-align:left'>";
	var mainTd = document.createElement(tdStr);
	oTr.appendChild(mainTd);
	mainTd.style.backgroundColor = "#7a8485";
	mainTd.style.height = this._height;
	
	var oDivCenter = document.createElement("<SPAN style='width:expression(eval(this.parentElement.offsetWidth))'>");
	this._centerSpan = oDivCenter;
	//oDivCenter.style.width = 500;
	oDivCenter.style.position = "relative";
	oDivCenter.style.overflow = "hidden";
	oDivCenter.style.height = this._height;
	mainTd.appendChild(oDivCenter);
	
	var oDivContainer = document.createElement("SPAN");
	this._itemCon = oDivContainer;
	oDivContainer.style.position = "relative";
	oDivContainer.style.overflowY = "hidden";
	oDivContainer.style.whiteSpace = "nowrap";
	oDivContainer.style.verticalAlign = "middle";
	oDivContainer.style.height = this._height;
	oDivCenter.appendChild(oDivContainer);

	//加入右角的圖片
	var sTd = document.createElement("td");
	oTr.appendChild(sTd);
	sTd.style.cssText = "overflow:hidden;text-overflow:ellipsis;white-space:nowrap";
	sTd.style.width = TsScrollTab.RightImgWidth;
	sTd.style.backgroundImage = 'url(' + TsScrollTab.RightImg + ')';		

	//加入左滾按鈕
	var tTd = document.createElement("td");
	oTr.appendChild(tTd);
	tTd.style.cssText = "overflow:hidden;text-overflow:ellipsis;white-space:nowrap";
	tTd.style.width = TsScrollTab.ArrowImgWidth;
	tTd.style.backgroundColor = "#ffa500";
	var timg = document.createElement("img");
	timg.style.cursor = "pointer";
	timg.scrollType = "left";			//向左滾動
	timg.attachEvent("onmouseover",this._aover);
	timg.attachEvent("onmouseout",this._aout);
	timg.attachEvent("onmousedown",this._adown);
	timg.attachEvent("onmouseup",this._aup);
	timg.src = TsScrollTab.ArrowLeftImg;
	tTd.appendChild(timg);

	//加入右滾按鈕
	var foTd = document.createElement("td");
	oTr.appendChild(foTd);
	foTd.style.cssText = "overflow:hidden;text-overflow:ellipsis;white-space:nowrap";
	foTd.style.width = TsScrollTab.ArrowImgWidth;
	foTd.style.backgroundColor = "#ffa500";
	var foimg = document.createElement("img");
	foimg.style.cursor = "pointer";
	foimg.scrollType = "right";			//向右滾動
	foimg.attachEvent("onmouseover",this._aover);
	foimg.attachEvent("onmouseout",this._aout);
	foimg.attachEvent("onmousedown",this._adown);
	foimg.attachEvent("onmouseup",this._aup);
	foimg.src = TsScrollTab.ArrowRightImg;
	foTd.appendChild(foimg);

	//加入關閉按鈕
	var fiTd = document.createElement("td");
	oTr.appendChild(fiTd);
	fiTd.style.cssText = "overflow:hidden;text-overflow:ellipsis;white-space:nowrap;";
	fiTd.style.width = TsScrollTab.ArrowImgWidth;
	fiTd.style.backgroundColor = "#ffa500";
	var fiimg = document.createElement("img");
	fiimg.style.cursor = "pointer";
	fiimg.attachEvent("onclick",this._closeClick);
	fiimg.attachEvent("onmouseover",this._closeOver);
	fiimg.title = "單擊這里關閉當前顯示窗口";
	fiimg.src = TsScrollTab.ArrowCloseImg;
	fiTd.appendChild(fiimg);
	
	this.CreateItems();
	this._body = oTable;
	
	oTable = null;
	oTBody = null;
	oTr = null;
	fTd = null;
	mainTd = null;
	oDivCenter = null;
	oDivContainer = null;
	sTd = null;
	tTd = null;
	
	timg = null;
	foTd = null;
	foimg = null;
	fiTd = null;
	fiimg = null;
};

//創建所有子項
_p.CreateItems = function(){
	for(var i=1;i<=this._items.getLength();i++){
		this.AddItem(this._items.getData[i]);
	}
};

//增加子項
_p.AddItem = function(oItem){
	//debug.debug();
	var oThis = this;
	if(this._itemCon == null)
		this.setBody();
	var thelast =  this._items.getData(1);				//上一個不選中
	if((thelast!=null)&&(thelast.GetSelected())){
		thelast.Blur();
	}
	this._items.insertList(0,oItem);					//當前增加的總為第一個
	var oThisItem = oItem;
	
	oItem.AddSelectEvent(function(){oThis.SelectItem(oThisItem);if(oThis.OnSelect!=null)oThis.OnSelect();});	//加入選取事件(讓原來選中的不選中)
	oItem.AddCloseEvent(function(){oThis.RemoveItem(oThisItem);if(oThis.OnClose!=null)oThis.OnClose(oThisItem);});	//加入關閉事件(出列和讓下一個被選擇)
	
	this._itemCon.appendChild(oItem.getBody());
	this._itemCon.appendChild(oItem.GetSeperatorImg());
	
	this._itemCon.style.posWidth += oItem.getBody().offsetWidth;
	oItem.setCProperty("curItemNo",this._items.getLength());		//設置當前這個選項的絕對索引(即在任務欄上的顯示次序﹐用于setvisible)(用于SetSelectedVisible)
	oItem.Select();
	this.SelectItem(oItem);
	this.SetSelectedVisible();
};

//選擇某項
_p.SelectItem = function(oThisItem){
	if((this._selectedItem!=null)&&(this._selectedItem!=oThisItem))
		this._selectedItem.Blur();
	this._selectedItem = oThisItem;
	
	var itemNo = this._items.locateNode(oThisItem);
	this._items.deleteList(itemNo);
	this._items.insertList(0,oThisItem);
	
	//先顯示上一個隱藏的分隔線
	if(this._lastSeperatorImg!=null)//&&(this._lastSeperatorImg!=oThisItem.GetSeperatorImg()))
		this._lastSeperatorImg.style.visibility = "visible";
	oThisItem.GetSeperatorImg().style.visibility = "hidden";
	
	//前一個item不要分隔線	
	var itemIndex = oThisItem.getCProperty("curItemNo");
	itemIndex--;
	if(itemIndex>0){
		lastItem = this.__getItemByItemNo(itemIndex);		//最后一個不要分隔線
		this._lastSeperatorImg=lastItem.GetSeperatorImg();
		this._lastSeperatorImg.style.visibility = "hidden";
	}
	else
		//debug.debug();
	this.SetSelectedVisible();
};

//根據絕對索引取得item
_p.__getItemByItemNo = function(curItemNo){
	for(var i=0;i<this._items.getLength();i++){
		var curItem = this._items.getData(i+1);
		if((curItem!=null)&&(curItem.getCProperty("curItemNo")==curItemNo)){
			return curItem;
		}
	}
	return null;
}

//移除子項
_p.RemoveItem = function(oItem){
	
	if(oItem.GetSelected()){						//如果當前關閉的項是選擇的﹐那么要把下一個顯示出來
		//debug.debug();						
		var thenext = this._items.getData(2);		//下一個
		if(thenext!=null){
			thenext.Select();
			this.SelectItem(thenext);
		}
		else{
			this._selectedItem=null;				//如果是最后一個﹐記得清空selectedItem
		}
	}
	
	//如果當前的分隔線是隱藏的﹐那么當其刪除后﹐應該也要讓它的上一個隱藏其分隔線
	if(oItem.GetSeperatorImg().style.visibility=='hidden'){
		var itemIndex = oItem.getCProperty("curItemNo");
		itemIndex--;
		if(itemIndex>0){
			lastItem = this.__getItemByItemNo(itemIndex);		//最后一個不要分隔線
			this._lastSeperatorImg=lastItem.GetSeperatorImg();
			this._lastSeperatorImg.style.visibility = "hidden";
		}
	}


	var iIndex = this._items.locateNode(oItem);
	var removeItemNo = oItem.getCProperty("curItemNo");
	for(var i=1;i<=this._items.getLength();i++){
		var curItem = this._items.getData(i);
		var curItemNo = curItem.getCProperty("curItemNo");
		if(curItemNo>removeItemNo){
			curItem.setCProperty("curItemNo",curItemNo-1);
		}
	}
	this._items.deleteList(iIndex);
	//oItem.Close();
	this.SetSelectedVisible();
}

//把當前已選中的項次呈現在中間
_p.SetSelectedVisible = function(){
	if(this._selectedItem!=null){
		var oItem = this._selectedItem;
		var curIndex = oItem.getCProperty("curItemNo")-1;
		var conLeft = this._itemCon.style.posLeft;						//當前的左邊距
		var conWidth = this._itemCon.offsetWidth;						//當前的總寬度
		var showWidth = this._centerSpan.clientWidth;					//能顯示的寬度
		var itemLeft = curIndex * TsScrollTabItem.BodyWidth;			//項次的相對左邊距
		//讓它在可視范圍內
		if(conLeft + itemLeft<0){
			var cha = 0-(conLeft+itemLeft);
			this._itemCon.style.posLeft += cha;
		}
		else if(conLeft + itemLeft + oItem.getBody().style.posWidth >= showWidth){
			var cha = (conLeft+itemLeft) - showWidth + oItem.getBody().style.posWidth;
			this._itemCon.style.posLeft -= cha;
		}
	}
}

//按索引取得項次
_p.GetItem = function(index){
	var curItem = this._items.getData(index);		//下一個
	return curItem;
}

//取得項次數目
_p.GetItemLength = function(){
	return this._items.getLength();
}

_p.dispose = function(){
	TsUICom.prototype.dispose.call(this);
	if(this._disposed)return;
	this._body = null;
	for(var i=1;i<this._items.getLength()+1;i++){
		this._items.getData(i).dispose();
		//this._items.deleteList(i);
	}
	this._items =  null;				//用雙鏈表表示任務欄項次
	this._selectedIndex = null;			//選中的項次ID
	this._itemCon = null;				//包含所有子項的span
	this._centerSpan = null;			//中間這個span
	this._leftID = null;				//向左滾動的函數
	this._rightID = null;				//向右滾動的函數
	this._lastSeperatorImg = null;
	this.OnSelect = null;				//當有tab選擇時的事件
	this.OnClose = null;				//tab關閉時的事件	
	this._aover = null;
	this._aout = null;
	this._adown = null;
	this._leftMove = null;
	this._rightMove = null;
	this._aup = null;
};