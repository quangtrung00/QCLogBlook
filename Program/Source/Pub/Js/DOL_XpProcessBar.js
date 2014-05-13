/*
 * DHTML Object Library(DOL)1.0
 * Copyright (c) 2005-2010 MB Technologies
 *
 * DOL belongs to Pouchen Company (YYDG, CHN). All rights reserved.
 * You are not allowed to copy or modify this code. Commercial use requires license.
 * Author:		Kw.Tsou
 * Date:		2005.1.7
 * Description:	進度條對象
 */

function XpProcessBar(nWidth,nHeight,sPrompt){
 	TsUICom.call(this,nWidth,nHeight);
 	
 	if(sPrompt)
 		this._prompt = sPrompt;
 	else
 		this._prompt = "處理中...";
 	this._moveFunction = null;
 	this._addSeed = 10;				//移動速度種子
 	this.Status = 0;				//0為沒在跑﹐1在跑
 	
 	var oThis = this;
 	
	this._beginProcess = function(){
		var jindu=oThis._body.all[1];
		var hudong = jindu.all[0];
		if(hudong.style.posLeft < jindu.clientWidth){
			hudong.style.posLeft += oThis._addSeed;
			var childs = hudong.all;
			for(var i=0;i<childs.length;i++){
					var oldstr =childs[i].filters.item("DXImageTransform.Microsoft.gradient").EndColorStr.substr(3,1);
					if(oldstr=='c')
						var newstr = oThis._hexSub(oldstr);
					else
						var newstr = oThis._hexAdd(oldstr);
					var newcolor = newstr + newstr + newstr + newstr + newstr + newstr;
					var endcolorstr = childs[i].filters.item("DXImageTransform.Microsoft.gradient").EndColorStr.substr(0,3)  + newcolor;
					childs[i].filters.item("DXImageTransform.Microsoft.gradient").EndColorStr=  endcolorstr;
			}
		}
		else{
			hudong.style.posLeft = -30;
			oThis._addSeed=10;
		}
		oThis.Status = 1;
		oThis._moveFunction=window.setTimeout(oThis._beginProcess,50);
	}
	
	this._endProcess = function(){
		if(oThis._moveFunction!=null)
			window.clearTimeout(oThis._moveFunction);
		if(oThis._body){
		var jindu=oThis._body.all[1];
			if(jindu){
				var hudong = jindu.all[0];
				hudong.style.posLeft = -30;
				oThis._addSeed=10;
				
			}
		}
		oThis.Status = 0;
	}

 	this._hexAdd = function(hexstr){
		hexstr = hexstr.toLowerCase();
		switch(hexstr){
			case "9":
				hexstr = "a";
				break;
			case "a":
				hexstr = "b";
				break;
			case "b":
				hexstr = "c";
				break;
			case "c":
			case "d":
			case "e":
			case "f":
				hexstr = "3";
				break;
			default:
				hexstr = (parseInt(hexstr) + 1) + "";
		}
		return hexstr;				
	}
			
	this._hexSub = function(hexstr){
		hexstr = hexstr.toLowerCase();
		switch(hexstr){
			case "0":
				hexstr = "c";
				break;
			case "a":
				hexstr = "9";
				break;
			case "b":
				hexstr = "a";
				break;
			case "c":
			case "d":
			case "e":
			case "f":
				hexstr = "b";
				break;
			default:
				hexstr = (parseInt(hexstr) - 1) + "";
		}
		return hexstr;
	}
}

XpProcessBar.GrainNum = 3;			//進度條里面的顆粒數量
XpProcessBar.GrainWidth = 8;		//顆粒寬度
XpProcessBar.GrainMargin = 1;
XpProcessBar.GrainFilter = "progid:DXImageTransform.Microsoft.gradient(startColorStr=#ffffffff,endColorStr=#ff888888)";

XpProcessBar.BodyBorderTop = "1px solid #cccccc";
XpProcessBar.BodyBorderRight = "1px solid #e5e5e5";
XpProcessBar.BodyFilter = " progid:DXImageTransform.Microsoft.gradient(startColorStr=#ffcccccc,endColorStr=#ffffffff,GradientType=0)";
XpProcessBar.GrainStyle = "overflow:hidden;";				//里面的顆粒樣式


var _p = XpProcessBar.prototype = new TsUICom;
_p._className="XpProcessBar";

_p.setBody = function(){
	if(this._body!=null)return;
	
	var adiv = document.createElement("div");
	this._body = adiv;
	adiv.style.display = "none";
	adiv.style.width = this._width;
	adiv.style.height = this._height;
	adiv.style.position = "absolute";
	
		var tspan = document.createElement("div");
		tspan.style.fontSize = "8pt";
		tspan.style.fontFamily = "verdana";
		tspan.style.color = "gray";
		tspan.style.padding = "20";
		tspan.innerHTML = this._prompt;
		adiv.appendChild(tspan);
	
	var ttdiv = document.createElement("div");
	//this._body = ttdiv;
	ttdiv.style.width = "100%";
	ttdiv.style.height = this._height;
	ttdiv.style.position = "absolute";
	ttdiv.style.borderTop = ttdiv.style.borderLeft = XpProcessBar.BodyBorderTop;
	ttdiv.style.borderRight = ttdiv.style.borderBottom = XpProcessBar.BodyBorderRight;
	ttdiv.style.filter = XpProcessBar.BodyFilter;
	adiv.appendChild(ttdiv);
	
	var tdiv = document.createElement("div");
	ttdiv.appendChild(tdiv);
	tdiv.style.position = "relative";
	tdiv.style.left = 0-XpProcessBar.GrainNum*(XpProcessBar.GrainWidth+ XpProcessBar.GrainMargin*2);
	
	for(var i=0;i<XpProcessBar.GrainNum;i++){
		var aspan = document.createElement("span");
		tdiv.appendChild(aspan);
		aspan.style.width = XpProcessBar.GrainWidth;
		aspan.style.height = this._height - 2*XpProcessBar.GrainMargin;
		aspan.style.margin = XpProcessBar.GrainMargin;
		aspan.style.overflow = "hidden";
		aspan.style.filter = XpProcessBar.GrainFilter;
	}
	
	aspan = null;
	tdiv = null;
	adiv = null;
}

_p.Start = function(top,left){
	if(this._body==null)throw new Error("沒有加入body");
	this._body.style.display = "block";
	if(top)
		this._body.style.top = top;
	if(left)
		this._body.style.left = left;
	if((this.Status!=1)&&(this._beginProcess))
		this._beginProcess();
}

_p.Stop = function(){
	if(this._body!=null){
		this._body.style.display = "none";
		if((this.Status!=0)&&(this._endProcess))
			this._endProcess();
	}
}

_p.dispose = function(){
	if(this._disposed)return;
	if((this._body!=null)&&(this._body.parentElement!=null))
		this._body.parentElement.removeChild(this._body);
 	this._body = null;
	this._endProcess();
 	this._moveFunction = null;
 	this._addSeed = null;				//移動速度種子
 	this.Status = null;					//0為沒在跑﹐1在跑
 	this._beginProcess = null;
	this._endProcess = null;
	this._hexAdd = null;
	this._hexSub = null;
	TsUICom.prototype.dispose.call(this);
}