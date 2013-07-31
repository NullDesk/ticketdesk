/**
 * jquery.qtip. The jQuery tooltip plugin
 * 
 * Copyright (c) 2009 Craig Thompson
 * http://craigsworks.com
 *
 * Licensed under MIT
 * http://www.opensource.org/licenses/mit-license.php
 *
 * Launch  : February 2009
 * Version : 1.0.0-beta3
 * Released: Thursday 19th March, 2009 - 17:15
 */


(function($)
{$.fn.qtip=function(options)
{if(typeof options=='object')
{if(typeof options.content!=='object')options.content={text:options.content};if(typeof options.content.title!=='object')options.content.title={text:options.content.title};if(typeof options.position!=='object')options.position={};if(typeof options.position.corner!=='object')options.position.corner={target:options.position.corner,tooltip:options.position.corner};if(typeof options.show!=='object')options.show={when:options.show};if(typeof options.show.when!=='object')options.show.when={event:options.show.when};if(typeof options.show.effect!=='object')options.show.effect={type:options.show.effect};if(typeof options.hide!=='object')options.hide={when:options.hide};if(typeof options.hide.when!=='object')options.hide.when={event:options.hide.when};if(typeof options.hide.effect!=='object')options.hide.effect={type:options.hide.effect};if(typeof options.style!=='object')options.style={name:options.style};if(typeof options.style.tip!=='object')options.style.tip={corner:options.style.tip};if(typeof options.style.tip.size!=='object')options.style.tip.size={x:options.style.tip.size,y:options.style.tip.size};if(typeof options.style.border!=='object')options.style.border={width:options.style.border};var opts=$.extend(true,{},$.fn.qtip.defaults,options);var styleExtend=[opts.style];while(typeof styleExtend[0].name=='string')
{styleExtend.unshift($.fn.qtip.styles[styleExtend[0].name]);}
styleExtend.unshift(true,{classes:{tooltip:'qtip-'+opts.style.name}},$.fn.qtip.styles.defaults);opts.style=$.extend.apply($,styleExtend);}
else if(typeof options=='string'&&options=='api')
return $(this).eq(0).data("qtip");return $(this).each(function()
{if(typeof options=='string')
{switch(options)
{case'disable':$(this).data("qtip").disable(true);break;case'enable':$(this).data("qtip").disable(false);break;case'destroy':$(this).data("qtip").destroy();break;}}
else
{var config=$.extend(true,{},opts);config.hide.effect.length=opts.hide.effect.length;config.show.effect.length=opts.show.effect.length;if(config.position.container===false)config.position.container=$(document.body);if(config.position.target===false)config.position.target=$(this);if(config.show.when.target===false)config.show.when.target=$(this);if(config.hide.when.target===false)config.hide.when.target=$(this);var obj=new qTip($(this),config);$(this).data("qtip",obj);}});}
function qTip(target,options)
{var self=this;self.options=options;self.elements={target:target.addClass(self.options.style.classes.target),tooltip:null,content:null,title:null,tip:null};self.timers={};$.extend(self,{construct:function()
{if(self.options.content.prerender===false&&self.options.show.when.event!==false)
{var showTarget=self.options.show.when.target;var showEvent=self.options.show.when.event;showTarget.bind(showEvent+".qtip-create",function()
{showTarget.unbind(showEvent+".qtip-create");self.create();showTarget.trigger(showEvent);});}
else self.create();},create:function()
{self.elements.tooltip=$(document.createElement('div')).addClass('qtip').addClass(self.options.style.classes.tooltip||self.options.style).css('-moz-border-radius','').css('-webkit-border-radius','').css({position:self.options.position.type,zIndex:6000+$('.qtip').length,width:self.options.style.width}).appendTo(self.options.position.container).data("qtip",self);self.elements.content=$(document.createElement('div')).addClass(self.options.style.classes.content).css({background:self.options.style.background,color:self.options.style.color,padding:self.options.style.padding}).html(self.options.content.text).appendTo(self.elements.tooltip);if(self.options.content.title.text!==false)createTitle.call(self);var padding=parseInt($.trim(String(self.options.style.padding)).charAt(0));self.elements.tooltip.css({width:self.elements.tooltip.outerWidth()+padding+6});createBorder.call(self);createTip.call(self);assignEvents.call(self);if(self.options.content.url!==false)
{var url=self.options.content.url;var data=self.options.content.data;self.loadContent(url,data);}
if(self.options.show.ready===true)
{self.updatePosition();self.elements.tooltip.show();self.elements.tooltip.addClass(self.options.style.classes.active);}
else self.elements.tooltip.hide();},show:function(event)
{if($(self.elements.tooltip).is(':visible')!==false)return;self.beforeShow.call(self);function afterShow(){self.onShow.call(self);}
if(typeof self.options.show.solo=='object')var solo=$(self.options.show.solo);else if(self.options.show.solo===true)var solo=$('div.qtip').not(self.elements.tooltip);if(solo)solo.each(function(){$(this).qtip("api").hide();});self.updatePosition(event);if(typeof self.options.show.effect.type=='function')
{self.options.show.effect.type.call(self.elements.tooltip,self.options.show.effect.length);self.elements.tooltip.queue(function(){afterShow();$(this).dequeue();});}
else
{switch(self.options.show.effect.type)
{case'fade':self.elements.tooltip.fadeIn(self.options.show.effect.length,afterShow);break;case'slide':self.elements.tooltip.slideDown(self.options.show.effect.length,function(){afterShow();self.updatePosition();});break;case'grow':self.elements.tooltip.show(self.options.show.effect.length,afterShow);break;default:self.elements.tooltip.show(null,afterShow);break;}
self.elements.tooltip.addClass(self.options.style.classes.active);}
if(self.options.position.type.search(/(fixed|absolute)/)!==-1)self.focus();self.onShow.call(self);return self;},hide:function()
{clearTimeout(self.timers.show);if($(self.elements.tooltip).is(':visible')===false)return;self.beforeHide.call(self);function afterHide(){self.onHide.call(self);}
if(typeof self.options.hide.effect.type=='function')
{self.options.hide.effect.type.call(self.elements.tooltip,self.options.hide.effect.length);self.elements.tooltip.queue(function(){afterHide();$(this).dequeue();});}
else
{switch(self.options.hide.effect.type)
{case'fade':self.elements.tooltip.fadeOut(self.options.hide.effect.length,afterHide);break;case'slide':self.elements.tooltip.slideUp(self.options.hide.effect.length,function(){afterHide();self.updatePosition();});break;case'grow':self.elements.tooltip.hide(self.options.hide.effect.length,afterHide);break;default:self.elements.tooltip.hide(null,afterHide);break;}
self.elements.tooltip.removeClass(self.options.style.classes.active);}
self.onHide.call(self);return self;},focus:function()
{var baseIndex=6000;var curIndex=parseInt(self.elements.tooltip.css('z-index'));var newIndex=baseIndex+$('.qtip').length-1;if(curIndex!==newIndex)
{$('.qtip').not(self.elements.tooltip).each(function()
{$(this).css({zIndex:parseInt($(this).css('z-index'))-1});})
self.elements.tooltip.css({zIndex:newIndex});}},disable:function(state)
{return assignEvents.call(self,!state);},updatePosition:function(event)
{self.beforePositionUpdate.call(self);if(self.options.position.target=='mouse')
{var posX=event.pageX;var posY=event.pageY;}
else
{var corner=self.options.position.corner.target;var elemPos=self.options.position.target.offset();var posX=elemPos.left;var posY=elemPos.top;if(corner.search(/bottom/i)!==-1)posY+=self.options.position.target.outerHeight();if(corner.search(/right/i)!==-1)posX+=self.options.position.target.outerWidth();if(corner.search(/(left|right)Middle/)!==-1)posY+=self.options.position.target.outerHeight()/2;else if(corner.search(/(top|bottom)Middle/)!==-1)posX+=self.options.position.target.outerWidth()/2;}
var tooltipLink=self.options.position.corner.tooltip;if(tooltipLink.search(/bottom/i)!==-1)posY-=self.elements.tooltip.outerHeight();if(tooltipLink.search(/right/i)!==-1)posX-=self.elements.tooltip.outerWidth();if(tooltipLink.search(/(left|right)Middle/)!==-1)posY-=self.elements.tooltip.outerHeight()/2;else if(tooltipLink.search(/(top|bottom)Middle/)!==-1)posX-=self.elements.tooltip.outerWidth()/2;posX+=self.options.position.adjust.x;posY+=self.options.position.adjust.y;if(self.options.position.adjust.screen)
{var newPos=screenAdjust(posX,posY,event,options);posX=newPos.left;posY=newPos.top;}
if(self.options.position.target=='mouse')
{var adjust=(tooltipLink.search(/top/)!==-1)?3:-3;posX+=adjust;posY+=adjust;}
self.elements.tooltip.css({left:posX,top:posY});self.onPositionUpdate.call(self);return self;},updateContent:function(html)
{if(self.options.content.title.text!==false)
self.elements.content.find('.'+self.options.style.classes.title).eq(0).after(html);else
self.elements.content.html(html);return self;},loadContent:function(url,data)
{if(typeof data!=='object')data=null;self.beforeContentLoad.call(self);self.elements.content.load(url,data,function()
{self.onContentLoad.call(self);if(self.options.content.title.text!==false)createTitle.call(self);self.updatePosition();});return self;},destroy:function()
{assignEvents.call(self,false);self.elements.tooltip.remove();self.elements.target.removeData("qtip");return true;},getVersion:function(){return[1,0,0,'beta2'];},getPos:function(){return self.elements.tooltip.offset();},beforePositionUpdate:self.options.api.beforePositionUpdate,onPositionUpdate:self.options.api.onPositionUpdate,beforeShow:self.options.api.beforeShow,onShow:self.options.api.onShow,beforeHide:self.options.api.beforeHide,onHide:self.options.api.onHide,beforeContentLoad:self.options.api.beforeContentLoad,onContentLoad:self.options.api.onContentLoad});self.construct();};function calculateTip(corner,width,height)
{var tips={bottomRight:[[0,0],[width,height],[width,0]],bottomLeft:[[0,0],[width,0],[0,height]],topRight:[[0,height],[width,0],[width,height]],topLeft:[[0,0],[0,height],[width,height]],topMiddle:[[0,height],[width/2,0],[width,height]],bottomMiddle:[[0,0],[width,0],[width/2,height]],rightMiddle:[[0,0],[width,height/2],[0,height]],leftMiddle:[[width,0],[width,height],[0,height/2]]}
tips.leftTop=tips.bottomRight;tips.rightTop=tips.bottomLeft;tips.leftBottom=tips.topRight;tips.rightBottom=tips.topLeft;return tips[corner];};function createTitle()
{var self=this;self.elements.title=$(document.createElement('div')).addClass(self.options.style.classes.title).html(self.options.content.title.text).prependTo(self.elements.content);if(self.options.content.title.button!==false)
{var button=$(document.createElement('a')).attr('href','#').css('float','right').addClass(self.options.style.classes.button).html(self.options.content.title.button).prependTo(self.elements.title).click(self.hide)}};function createBorder()
{var self=this;var width=self.options.style.border.width;var radius=self.options.style.border.radius;var color=self.options.style.border.color||self.options.style.tip.color;if(radius===0)
self.elements.content.css({border:width+'px solid '+color})
else
{var wrapper=$(document.createElement('div')).addClass('qtip-contentWrapper').append(self.elements.content).appendTo(self.elements.tooltip);var borders={topLeft:[radius,radius],topRight:[0,radius],bottomLeft:[radius,0],bottomRight:[0,0]}
var shapes={};for(var i in borders)
{shapes[i]=$(document.createElement('div')).css({height:radius,width:radius,overflow:'hidden',float:(i.search(/Left/)!==-1)?'left':'right',lineHeight:0.1})}
if(document.createElement('canvas').getContext)
{for(var i in borders)
{var canvas=$(document.createElement('canvas')).attr('height',radius).attr('width',radius).css({verticalAlign:'top'}).appendTo(shapes[i]);var context=canvas.get(0).getContext('2d');context.fillStyle=color;context.beginPath();context.arc(borders[i][0],borders[i][1],radius,0,Math.PI*2,false);context.fill();}}
else if($.browser.msie||document.namespaces)
{var borders={topLeft:[-90,90,0],topRight:[-90,90,-radius],bottomLeft:[90,270,0],bottomRight:[90,270,-radius]}
for(var i in borders)
{$(document.createElement('v:arc')).attr('fill','true').attr('fillcolor',color).attr('stroked','false').attr('startangle',borders[i][0]).attr('endangle',borders[i][1]).css({width:radius*2+3,height:radius*2+3,marginLeft:(i.search(/Right/)!==-1)?borders[i][2]-3.5:-1,marginTop:-1,verticalAlign:'top'}).appendTo(shapes[i]);if(($.support&&$.support.objectAll)||($.browser.msie&&parseInt($.browser.version.charAt(0))<7))
{if(i.search(/Left/)!==-1)
shapes[i].css({marginRight:-3})
else if(i.search(/Right/)!==-1)
shapes[i].css({marginLeft:-3})}}}
var betweenCorners=$(document.createElement('div')).addClass('qtip-betweenCorners').css({height:radius,overflow:'hidden',backgroundColor:color,lineHeight:0.1})
var borderTop=$(document.createElement('div')).addClass('qtip-borderTop').css({height:radius}).append(shapes['topLeft']).append(shapes['topRight']).append(betweenCorners).prependTo(wrapper);var borderBottom=$(document.createElement('div')).addClass('qtip-borderBottom').css({height:radius,clear:'both'}).append(shapes['bottomLeft']).append(shapes['bottomRight']).append(betweenCorners.clone()).appendTo(wrapper);var sideWidth=Math.max(radius,(radius+(width-radius)))
var vertWidth=Math.max(width-radius,0);self.elements.content.css({margin:0,border:'0px solid '+color,borderWidth:vertWidth+'px '+sideWidth+'px',position:'relative',clear:'both'})}};function createTip(corner)
{var self=this;if(self.options.style.tip.corner===false)return;else if(!corner)corner=self.options.style.tip.corner;var color=self.options.style.tip.color||self.options.style.border.color;$(self.elements.tooltip).find('.'+self.options.style.classes.tip).remove();self.elements.tip=$(document.createElement('div')).addClass(self.options.style.classes.tip).css({width:12,margin:'0 auto',lineHeight:0.1,textAlign:'left'}).attr('rel',corner)
var coordinates=calculateTip(corner,self.options.style.tip.size.x,self.options.style.tip.size.y);if(document.createElement('canvas').getContext)
{var canvas=$(document.createElement('canvas')).attr('width',self.options.style.tip.size.x).attr('height',self.options.style.tip.size.y).appendTo(self.elements.tip);var context=canvas.get(0).getContext('2d');context.fillStyle=color;context.beginPath();context.moveTo(coordinates[0][0],coordinates[0][1]);context.lineTo(coordinates[1][0],coordinates[1][1]);context.lineTo(coordinates[2][0],coordinates[2][1]);context.fill();}
else if($.browser.msie||document.namespaces)
{var path='m'+coordinates[0][0]+','+coordinates[0][1];path+=' l'+coordinates[1][0]+','+coordinates[1][1];path+=' '+coordinates[2][0]+','+coordinates[2][1];path+=' xe';$(document.createElement('v:shape')).attr('fillcolor',color).attr('stroked','false').attr('coordsize',self.options.style.tip.size.x+','+self.options.style.tip.size.y).attr('path',path).css({width:self.options.style.tip.size.x,height:self.options.style.tip.size.y,marginTop:-1}).appendTo(self.elements.tip)}
else return;var radiusAdjust=self.options.style.border.radius;var sideAdjust=(self.options.style.border.radius==0)?0:radiusAdjust;var pixelAdjust=($.browser.msie||document.namespaces)?1:0;if(corner.search(/top|bottom/)!==-1)
{if(corner.search(/Left/)!==-1)
{self.elements.tip.css({marginLeft:radiusAdjust-pixelAdjust});self.elements.tooltip.css({marginLeft:-radiusAdjust})}
else if(corner.search(/Right/)!==-1)
{self.elements.tip.css({marginLeft:Math.floor(self.elements.tooltip.outerWidth()-self.options.style.tip.size.x-sideAdjust-pixelAdjust)});self.elements.tooltip.css({marginLeft:sideAdjust})}
if(corner.search(/top/)!==-1)self.elements.tip.css({marginTop:-self.options.style.tip.size.y+1});}
else if(corner.search(/left|right/)!==-1)
{if(corner.search(/Middle/)!==-1)
self.elements.tip.css({position:'absolute',marginTop:Math.floor((self.elements.tooltip.outerHeight()/2)-(self.options.style.tip.size.y/2))});else if(corner.search(/Top/)!==-1)
{self.elements.tip.css({position:'relative',top:radiusAdjust+self.options.style.tip.size.y});self.elements.tooltip.css({marginTop:-radiusAdjust-self.options.style.tip.size.y})}
else if(corner.search(/Bottom/)!==-1)
{self.elements.tip.css({position:'relative',top:Math.floor(self.elements.tooltip.outerHeight()-radiusAdjust)});self.elements.tooltip.css({marginTop:radiusAdjust})}
if(corner.search(/left/)!==-1)
self.elements.tip.css({marginLeft:-self.options.style.tip.size.y});else
self.elements.tip.css({marginLeft:self.elements.tooltip.outerWidth()-1-pixelAdjust});}
if(corner.search(/left|top|right/)!==-1)
self.elements.tip.prependTo(self.elements.tooltip);else
self.elements.tip.appendTo(self.elements.tooltip);var paddingCorner='padding-'+corner.match(/left|right|top|bottom/)[0];var paddingSize=(paddingCorner.search(/left/)!==-1)?self.options.style.tip.size.x:self.options.style.tip.size.y
if(paddingCorner!=='padding-bottom')self.elements.tooltip.css(paddingCorner,paddingSize-1);};function assignEvents(state)
{var self=this;var showTarget=self.options.show.when.target;var hideTarget=self.options.hide.when.target;if(state!==false)
{if(self.options.position.type.search(/(fixed|absolute)/)!==-1)
self.elements.tooltip.bind("mouseover.qtip",self.focus);if(self.options.position.target=='mouse')
{showTarget.bind("mousemove.qtip",self.updatePosition);showTarget.bind("mouseout.qtip",self.hide);}
var inactiveEvents=['click','dblclick','mousedown','mouseup','mousemove','mouseout','mouseenter','mouseleave','mouseover'];function inactiveTimer()
{clearTimeout(self.timers.inactive);self.timers.inactive=setTimeout(function()
{$(inactiveEvents).each(function()
{hideTarget.unbind(this+'.qtip-inactive');self.elements.content.unbind(this+'.qtip-inactive');});self.hide();},self.options.hide.delay);}
function showMethod(event)
{event.stopPropagation();clearTimeout(self.timers.hide);if(self.options.hide.when.event=='inactive')
{$(inactiveEvents).each(function()
{hideTarget.bind(this+'.qtip-inactive',inactiveTimer);self.elements.content.bind(this+'.qtip-inactive',inactiveTimer);});inactiveTimer();}
clearTimeout(self.timers.show);self.timers.show=setTimeout(function(event){self.show(event)},self.options.show.delay);}
if(self.options.hide.when.event!=='inactive')
{function hideMethod(event)
{if(self.options.hide.fixed===true&&self.options.hide.when.event.search(/mouse(move|over|out|enter|leave)/i)!==-1&&$(event.relatedTarget).parents('.qtip').length>0)
{event.stopPropagation();event.preventDefault();return false;}
event.stopPropagation();clearTimeout(self.timers.show);clearTimeout(self.timers.hide);self.timers.hide=setTimeout(function(){self.hide()},self.options.hide.delay);}}
if(self.options.show.when.target.add(self.options.show.when.target).length===1&&self.options.show.when.event==self.options.hide.when.event&&self.options.show.when.event=='click')
showTarget.toggle(showMethod,hideMethod);else
{if($.isFunction(showMethod))showTarget.bind(self.options.show.when.event+'.qtip',showMethod);if($.isFunction(hideMethod))hideTarget.bind(self.options.hide.when.event+'.qtip',hideMethod);}}
else
{showTarget.unbind("mousemove.qtip",self.updatePosition);showTarget.unbind("mouseout.qtip",self.hide);showTarget.unbind(self.options.show.when.event+'.qtip');hideTarget.unbind(self.options.hide.when.event+'.qtip');self.elements.tooltip.unbind(self.options.hide.when.event+'.qtip');self.elements.tooltip.unbind("mouseover.qtip",self.focus);}
return state;};function screenAdjust(posX,posY,event)
{var self=this;var corner=self.options.style.tip.corner||self.options.position.tooltip;var newX=posX+self.elements.tooltip.outerWidth();var newY=posY+self.elements.tooltip.outerHeight();var windowWidth=$(window).width()+$(window).scrollLeft();var windowHeight=$(window).height()+$(window).scrollTop();var overflow={leftMin:(posX<0),leftMax:(newX>=windowWidth),topMin:(posY<$(window).scrollTop()),topMax:(newY>=windowHeight)};if(overflow.leftMin||overflow.leftMax)
{if(overflow.leftMin)
posX=(self.options.position.target=='mouse')?event.pageX:self.options.position.target.offset().left+self.options.position.target.outerWidth();else if(overflow.leftMax)
{if(corner.search(/(top|bottom)Middle/)!==-1)
posX=posX-(self.elements.tooltip.outerWidth()/2)-(self.options.position.adjust.x*2);else
posX=posX-self.options.position.target.outerWidth()-self.elements.tooltip.outerWidth()-(self.options.position.adjust.x*2);}
if(self.options.style.tip.corner!==false)
{if(corner.search(/(top|bottom)Middle/)!==-1)
{if(overflow.leftMin)
corner=corner.replace('Middle','Left');else if(overflow.leftMax)
corner=corner.replace('Middle','Right');}
else if(corner.search(/right/)!==-1)corner=corner.replace('right','left');else if(corner.search(/Right/)!==-1)corner=corner.replace('Right','Left');else if(corner.search(/left/)!==-1)corner=corner.replace('left','right');else if(corner.search(/Left/)!==-1)corner=corner.replace('Left','Right');}}
if(overflow.topMin||overflow.topMax)
{if(overflow.topMin)
posY=(self.options.position.target=='mouse')?event.pageY:self.options.position.target.offset().top+self.options.position.target.outerHeight();else if(overflow.topMax)
{if(corner.search(/(left|right)Middle/)!==-1)
posY=posY-(self.options.position.target.outerHeight()/2)-(self.elements.tooltip.outerHeight()/2)-(self.options.position.adjust.y*2);else
posY=posY-self.options.position.target.outerHeight()-self.elements.tooltip.outerHeight()-(self.options.position.adjust.y*2);}
if(self.options.style.tip.corner!==false)
{if(corner.search(/(left|right)Middle/)!==-1)
{if(overflow.topMin)
corner=corner.replace('Middle','Top');else if(overflow.topMax)
corner=corner.replace('Middle','Bottom');}
else if(corner.search(/top/)!==-1)corner=corner.replace('top','bottom');else if(corner.search(/Top/)!==-1)corner=corner.replace('Top','Bottom');else if(corner.search(/bottom/)!==-1)corner=corner.replace('bottom','top');else if(corner.search(/Bottom/)!==-1)corner=corner.replace('Bottom','Top');}}
if(self.options.style.tip.corner!==false)
if(corner!=self.elements.tip.attr('rel'))self.createTip(corner);return{left:posX,top:posY};};function debug(text)
{if(window.console&&window.console.log)
window.console.log('qTip: '+text);};$.fn.qtip.defaults={content:{prerender:false,text:'',url:false,data:null,title:{text:false,button:false}},position:{target:false,corner:{target:'bottomRight',tooltip:'topLeft'},adjust:{x:0,y:0,screen:false},type:'absolute',container:false},show:{when:{target:false,event:'mouseover'},effect:{type:'fade',length:100},delay:140,solo:false,ready:false},hide:{when:{target:false,event:'mouseout'},effect:{type:'fade',length:100},delay:0,fixed:false},api:{beforePositionUpdate:function(){},onPositionUpdate:function(){},beforeShow:function(){},onShow:function(){},beforeHide:function(){},onHide:function(){},beforeContentLoad:function(){},onContentLoad:function(){}}};$.fn.qtip.styles={defaults:{width:'auto',padding:5,background:'white',color:'#111',border:{width:1,radius:0,color:'#d3d3d3'},tip:{corner:false,color:false,size:{x:12,y:12}},classes:{target:'',tip:'qtip-tip',title:'qtip-title',content:'qtip-content',active:'qtip-active'}},cream:{border:{width:3,radius:0,color:'#F9E98E'},background:'#FBF7AA',color:'#a2844a',width:'auto',classes:{tooltip:'qtip-cream'}},light:{border:{width:3,radius:0,color:'#E2E2E2'},background:'white',color:'#454545',width:'auto',classes:{tooltip:'qtip-light'}},dark:{border:{width:3,radius:0,color:'#303030'},background:'#505050',color:'#f3f3f3',width:'auto',classes:{tooltip:'qtip-dark'}},red:{border:{width:3,radius:0,color:'#CE6F6F'},background:'#F79992',color:'#A94141',width:'auto',classes:{tooltip:'qtip-red'}},green:{border:{width:3,radius:0,color:'#A9DB66'},background:'#CDE6AC',color:'#58792E',width:'auto',classes:{tooltip:'qtip-green'}}}})(jQuery);if(document.namespaces&&document.namespaces["v"]==null)
{document.namespaces.add("v","urn:schemas-microsoft-com:vml");var stylesheet=document.createStyleSheet().owningElement;stylesheet.styleSheet.cssText="v\\:*{behavior:url(#default#VML); display: inline-block }";}