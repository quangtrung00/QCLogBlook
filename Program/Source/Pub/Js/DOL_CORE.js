/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	常用函數和基類
 */
 
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//[瀏覽器檢查]//本類庫需要IE6.0以上版本才可運行
function TsCheckBrowser(){
	var ua=navigator.userAgent;
	this._ie= /msie/i.test(ua);
	this._moz=navigator.product=="Gecko";
	if(this._moz){
		/rv\:([^\);]+)(\)|;)/.test(ua);
		this._version=RegExp.$1;
		this._ie55=false;
		this._ie6=false;
		this._ie7=false;
	}
	else {
		/MSIE([^\);]+)(\)|;)/.test(ua);
		this._version=RegExp.$1;
		this._ie55= /msie 5\.5/i.test(ua);
		this._ie6= /msie 6/i.test(ua);
		this._ie7= /msie 7/i.test(ua);
	}
}
function chkIE(){
    //window.attachEvent("onerror",function(){alert('發生了錯誤');});
    try{
		var checker = new TsCheckBrowser();
		if(checker._ie55){
			alert("請使用IE6.0以上版本瀏覽");
			location.href = "about:blank";
			document.close();
		}
	}
	catch(e){
			alert("請使用IE6.0以上版本瀏覽");
			location.href = "about:blank";
			document.close();
	}
}
chkIE();

////共用函數////////////////////////////////////////////////////////////////////////////////////////////

//請求url
function RequestData(URL) {
	var ret="";
	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.Open("GET",URL, false);
	try { 
		xmlhttp.Send(); 
		var result = xmlhttp.status;
	}
	catch(ex) {
		ret ="0" + ex.description + "|" + ex.number;
	}
	if(ret==""){
		if(result==200) { 
			ret = "1" + xmlhttp.responseText; 
		}
		else{
			ret = "0" + "狀態:" + result;
		}
	}
	xmlhttp = null;
	return ret;
}

//post帶參數請求
function PostRequestData(URL,data){
	var ret="";
	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.Open("POST",URL, false);
	try { 
		xmlhttp.Send(data); 
		var result = xmlhttp.status;
	}
	catch(ex) {
		ret ="0" + ex.description + "|" + ex.number;
	}
	if(ret==""){
		if(result==200) { 
			ret = "1" + xmlhttp.responseText; 
		}
		else{
			ret = "0" + "狀態:" + result;
		}
	}
	xmlhttp = null;
	return ret;
}

//取得COOKIE值
function GetCookie(sName)
{
	var aCookie = window.document.cookie.split("; ");
	for (var i=0; i < aCookie.length; i++)
	{
		var aCrumb = aCookie[i].split("=");
		if (sName == aCrumb[0]){
			var sValue =  unescape(aCrumb[1]);
			if(sValue=="undefined")
				return null;
			else
				return sValue;
		}
	}
	return null;
}

//設置COOKIE值
function SetCookie(sName, sValue)
{
	date = new Date();
	window.document.cookie = sName + "=" + escape(sValue) + "; expires=Fri, 31 Dec 3003 23:59:59 GMT;";
}

//刪除COOKIE
function DeleteCookie(sName){
	if(GetCookie(sName)!=null){
		window.document.cookie = sName + "=; expires=Fri, 31 Dec 1003 23:59:59 GMT;";
	}
}

////Array的功能擴展////////////////////////////////////////////////////////////////////////////////////////
//從第一個元素開始搜索某個物件是否在數組中存在,并傳回序號Array.prototype.indexOf=function(o){
	for(var i=0;i<this.length;i++){
		if(this[i]==o)
			return i;
	}
	return-1;
};

//從最后一個元素開始搜索某個物件是否在數組中存在,并傳回序號Array.prototype.lastIndexOf=function(o){
	for(var i=this.length-1;i>=0;i--){
		if(this[i]==o)
			return i;
	}
	return-1;
};

//返回某數組中是否包含某個元素Array.prototype.contains=function(o){
	return this.indexOf(o)!= -1;
};

//復制一個數組(如果數組中的元素是對象﹐那么只是復制指針)
Array.prototype.copy=function(o){
	return this.concat();
};

//在數組中某個位置插入一個元素Array.prototype.insertAt=function(o,i){
	this.splice(i,0,o);
};

//在o2的前面插入o
Array.prototype.insertBefore=function(o,o2){
	var i=this.indexOf(o2);
	if(i== -1)
		this.push(o);
	else 
		this.splice(i,0,o);
};

//移除第i個數組元素Array.prototype.removeAt=function(i){
	this.splice(i,1);
};

//移除數組元素o
Array.prototype.remove=function(o){
	var i=this.indexOf(o);
	if(i!= -1)
		this.splice(i,1);
};

////String的功能擴展////////////////////////////////////////////////////////////////////////////////////////

//去除兩邊space
String.prototype.trim=function(){
	return this.replace(/(^\s+)|\s+$/g,"");
};

//將兩個或多個space合為一個space
String.prototype.clearSpace = function(){
	return this.replace(/(\s+)/g," ");
}

//查詢包含几個字符String.prototype.searchNum = function(v){
	var ret =0;
	var re = new RegExp( v, "gi" );
	var arr = re.exec(this);
	while(arr != null)
	{  
		ret++;
		arr = re.exec(this) ; 
	}
	return ret ;
}

////////////////////////////////////////////////////////////////////////////////
//基本的數據結構
/*----------------雙鏈表-------------------*/
 function DLinkList(){
	var _head;
	_head = new DListNode();
	_head.data = "Ks.Tsou 2004/4/5 ver 1.0";
	_head.prior = null;
	_head.next = null;
	this.oHead = _head;			//頭結點(不計算長度)
	this.iLength = 0;			//鏈表長度
 }

 _p = DLinkList.prototype;
 _p._className="DLinkList";
 
 //取得鏈表長度
 _p.getLength = function(){
	return this.iLength;
 };
 
 //取得第i個節點(i為序號﹐不是索引)(一般為私有變量,外部不用)
 _p._getNode = function(i){
	if (i<0) return null;
	var j = -1;
	var curNode = this.oHead;
	while(curNode !=null){
		j++;
		if(i==j)
			return curNode;
		curNode = curNode.next;
	}
	return null;
 };
 
  //取得第i個節點的值(即data,同樣i為序號﹐不是索引)
 _p.getData = function(i){
	if (i<0) return null;
	var j = -1;
	var curNode = this.oHead;
	while(curNode !=null){
		j++;
		if(i==j)
			return curNode.data;
		curNode = curNode.next;
	}
	return null;
 };

 //查找數據為d的節點位置(返回值為序號﹐而不是索引)
 _p.locateNode = function(d){
	var j = -1;
	var curNode = this.oHead;
	while(curNode!=null){
		j++;
		if(curNode.data == d)
			return j;
		curNode = curNode.next;
	}
	return -1;
 }
 
 //在位置i之后插入節點(i為序號﹐非索引)
 _p.insertList = function(i,d){
	if(i<0) i=0;								//只能在第1個位置開始插入

	if(i>this.getLength()) i = this.getLength();
	var curNode = this._getNode(i);
	if(curNode!=null){
		var insNode = new DListNode();
		insNode.data = d;
		
		insNode.prior = curNode;
		insNode.next = curNode.next;
		
		curNode.next = insNode;
		if(insNode.next!=null)
			insNode.next.prior = insNode;
		this.iLength++;
		return true;
	}
	throw new Error("insert error!");		//至少一定會抓到oHead
 }
 
 //刪除第i個節點(i為序號﹐非索引)
 _p.deleteList = function(i){
	if(i==0)throw new Error("頭結點不能刪除!");
	if(i<0)throw new Error("參數為序號﹐必須大于0!");
	var curNode = this._getNode(i);
	if(curNode !=null){
		curNode.prior.next = curNode.next;		//前一個結點一定有
		if(curNode.next!=null)
			curNode.next.prior = curNode.prior;
		curNode = null;
		this.iLength--;
		return true;
	}
	return false;
 };
 
 //結點定義
 function DListNode(){
	this.data = null;			//數據
	this.prior = null;			//上一個節點
	this.next = null;			//下一個節點
 }
 /*----------------雙鏈表<end>-------------------*/
 
 /*--------------- 隊列-------------------------*/
 function Queue(){
	this._datas = [];       //初始化數組
	this._point = 0;        //預設定指針
}

_p=Queue.prototype;

//入隊
_p.enqueue = function(o){
	this._datas[this._datas.length]=o;
}

//出隊
_p.dequeue = function(){
	var o = this._datas[this._point];
	delete this._datas[this._point];	
	this._point++;
	if(typeof(o)=="undefined")
		return null;
	else
		return o;
}


//傳回待出物件
_p.peek=function(){
	var o = this._datas[this._point];
	if(typeof(o)=="undefined")
		return null;
	else
		return o;
}

//清空陣列
_p.clear=function(){
	this._datas = [];       //初始化數組
	this._point = 0;        //預設定指針
}

//判斷元素是否在陣列中
_p.contains=function(o){
    for(var i=this._point;i<=this._datas.length;i++)
       if(this._datas[i]==o)
          return true;
    return false; 
}

_p.count = function(){
	return this._datas.length-this._point;
}

/*-----------------隊列<end>-------------------*/

/*-----------------堆棧-----------------------*/
function Stack(){
	this._datas = [];       //初始化數組
	this._point = -1;        //預設定指針
}

_p=Stack.prototype;

//入棧
_p.push = function(o){
	this._datas[++this._point]=o;
}

//出棧
_p.pop = function(){
	if(this._point==-1)return null;
	var o = this._datas[this._point];
	delete this._datas[this._point];	
	this._point--;
	return o;
}

//傳回待出物件
_p.peek=function(){
	if(this._point==-1)return null;
	var o = this._datas[this._point];
	return o;
}

//清空陣列
_p.clear=function(){
	this._datas = [];       //初始化數組
	this._point = -1;        //預設定指針
}

//判斷元素是否在陣列中
_p.contains=function(o){
    for(var i=0;i<=this._point;i++)
       if(this._datas[i]==o)
          return true;
    return false; 
}

_p.count = function(){
	return this._point+1;
}
/*-----------------堆棧<end>------------------*/

////TsObject的定義//////////////////////////////////////////////////////////////////////////////////////////

//所有DOL對象的基類
function TsObject(){
	this._hashCode=TsObject._hashCodePrefix+Math.round(Math.random()*1000)+TsObject._hashCodePrefix+TsObject._hashCodeCounter++;
	this._customProperty = {};		//對象的自定義屬性
}

//靜態屬性(所有的TsObject的全局屬性放在這里)
TsObject._hashCodeCounter=1;
TsObject._hashCodePrefix="ts";

//靜態方法
TsObject.toHashCode=function(o){
	if(o._hashCode!=null)
		return o._hashCode;
	return o._hashCode=TsObject._hashCodePrefix+Math.round(Math.random()*1000)+TsObject._hashCodePrefix+TsObject._hashCodeCounter++;
};

_p=TsObject.prototype;

_p._className="TsObject";		//類名(這些是不是私有屬性﹐不能被外部直接調用?)
_p._disposed=false;				//是否被銷毀
_p._id=null;					//對象的id

//取得HashCode
_p.toHashCode=function(){
	return TsObject.toHashCode(this);		//這里有實例(this)
};

//銷毀對象
_p.dispose=function(){
	if(this._disposed)return
	this._disposed=true;
	for(xx in this._customProperty){
		if(xx._hashCode)					//如果是DOL對象
			xx.dispose();
		xx=null;
	}
	this._customProperty = null;			
};

//將對象轉換為字符(類名)
_p.toString=function(){
	if(this._className)
		return"[object "+this._className+"]";
	return"[object Object]";
};

//設置自定義屬性
_p.setCProperty = function(sName,sValue){
	this._customProperty[sName]=sValue;
};

//取得自定義屬性
_p.getCProperty = function(sName){
	return this._customProperty[sName];
};

////TsUICom的定義//////////////////////////////////////////////////////////////
//所有帶界面的UI元件
function TsUICom(nWidth,nHeight){
	TsObject.call(this);
	//重新設置圖片目錄
	this.setImgDir();
	this._width = nWidth;
	this._height = nHeight;
	this._body = null;			//本體對象
	this._uiID = "";			//this._body在html頁面上的id值
	
}

TsUICom.ImgDir = "../../Images/MenuArea";		//UI圖片目錄

_p=TsUICom.prototype = new TsObject;

_p._className="TsUICom";		//類名(這些是不是私有屬性﹐不能被外部直接調用?)

//子類必須提供setBody方法(對this._body賦值)
_p.setBody = function(){
	this._body = null;
};

_p.getBody = function(){
	this.setBody();
	return this._body;
};

_p.setUIID = function(v){
	this._uiID = v;
};

_p.getUIID = function(){
	return this._uiID;
};

_p.getWidth = function(){
	return this._width;
};

_p.getHeight = function(){
	return this._height;
};

_p.setAttribute = function(sname,svalue){
	if(this._body!=null)
		this._body.setAttribute(sname,svalue);
}

_p.getAttribute = function(sname){
	if(this._body!=null)
		return this._body.getAttribute(sname);
}

//重新設置圖片目錄
_p.setImgDir = function(){
	;
}

_p.dispose = function(){
	TsObject.prototype.dispose.call(this);
	if(this._body!=null){
		if(this._body.parentElement!=null)
			this._body.parentElement.removeChild(this._body);
		this._body = null;
	}
}

////TsBusCom的定義//////////////////////////////////////////////////////////////
//所有無界面的元件function TsBusCom(){
	TsObject.call(this);
}

_p=TsBusCom.prototype = new TsObject;

_p._className="TsBusCom";		//類名(這些是不是私有屬性﹐不能被外部直接調用?)
