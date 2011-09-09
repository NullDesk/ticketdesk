// original code by: Raj Kaimal 
//     http://weblogs.asp.net/rajbk/

//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so.

//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.

// Modified by: Stephen M. Redd
//      http://www.reddnet.net


Type.registerNamespace('TicketDesk.Engine.Ajax.Extenders');

TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior = function(element) {
TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior.initializeBase(this, [element]);

    // Properties
    this._updatePanelID = null;
    this._offsetX = null;
    this._offsetY = null;
    this._pageRequestManager = null;
    this._updatePanelzIndex = 9000;
    this._calloutWidth = 15;
    this._positionElementID = null;
    this._timerID = null;
    
    //Handlers
    this._pageEndRequestHandler = null;
    this._windowResizeHandler = null;
    this._bodyClickHandler = null;
    this._updatePanelClickHandler = null;
}

TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior.prototype = {
    initialize : function() {
        TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior.callBaseMethod(this, 'initialize');
        this._addHandlers();
        
        var elt = this.get_element();
        elt.style.position = 'absolute';
        
        //Fix for UpdatePanel not having CSS
        var parentNode = elt.parentNode;
        if (parentNode) {
            if (parentNode.style.display == 'none')
                parentNode.style.display = 'inline';
            if (parentNode.style.visibility == 'hidden')
                parentNode.style.visibility = '';
        }
        
        (this._loadVisible) ? this.show() : this.hide();
    },
    dispose : function() {
        this._clearHandlers();
        TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior.callBaseMethod(this, 'dispose');
    },
    //
    // Public methods
    //
    show : function () {
        
        if (!this._positionElementExists()) {
            return;
        }        
        
        var elt = this.get_element();
        if (!elt) {
            return;
        }
        
        var divShadow = this._initializeShadowDiv();
        var tblCallout = (this._calloutColor) ? this._initializeCalloutTable() : null;
        var bottomIframe = (this._iframeRequired()) ? this._initializeIframe() : null;
        var positionEltLocation = this._positionEltLocation();  
        
        if (!elt.divPostback) {
            var divPostback = document.createElement('div');
            divPostback.id = this.get_clientID();
            divPostback.style.display = 'none';
            elt.parentNode.insertBefore(divPostback, elt);
            elt.divPostback = divPostback;
        }
        
        elt.style.position = 'absolute';
        elt.style.zIndex = this._updatePanelzIndex;
        this._setVisible(elt, true);
        
        var deltaLocation = this._getDeltaLocation();
        var x = positionEltLocation.x - deltaLocation.x;
        var y = positionEltLocation.y - deltaLocation.y;
        
        Sys.UI.DomElement.setLocation(elt, x, y);
        
        var bounds = Sys.UI.DomElement.getBounds(elt);
        var panelWidth = bounds.width;
        var panelHeight = bounds.height;
        
        Sys.UI.DomElement.setLocation(divShadow, x + 3, y + 3);
        this._setVisible(divShadow, true);
        divShadow.style.width = panelWidth + 'px';
        divShadow.style.height = panelHeight + 'px';
        
        if (tblCallout) {
            Sys.UI.DomElement.setLocation(tblCallout, x - this._calloutWidth, y);
            this._setVisible(tblCallout, true);
        }
    
        if (bottomIframe) {
            Sys.UI.DomElement.setLocation(bottomIframe, x, y);
            this._setVisible(bottomIframe, true);
            bottomIframe.style.width = panelWidth + 3 + 'px';
            bottomIframe.style.height = panelHeight + 3 + 'px';
        }                
    },
    hide : function () {

        var elt = this.get_element();
        if (!elt) {
            return;
        }
        
        this._setVisible(elt, false);
        
        if (elt.divShadow)
            this._setVisible(elt.divShadow, false);
        
        if (elt.tblCallout)
            this._setVisible(elt.tblCallout, false);
        
        if (elt.iframeOverlay) 
            this._setVisible(elt.iframeOverlay, false);

    },
    //
    // Private methods
    //
    _addHandlers : function() {
        this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
        var elt = this.get_element();
        
        this._pageEndRequestHandler = Function.createDelegate(this, this._onEndRequest);
        this._pageRequestManager.add_pageLoaded(this._pageEndRequestHandler);
        
        this._pageInitRequestHandler = Function.createDelegate(this, this._onInitRequest);
        this._pageRequestManager.add_initializeRequest(this._pageInitRequestHandler)
        
        this._updatePanelClickHandler = Function.createDelegate(this, this._onClick);
        $addHandler(elt, 'click', this._updatePanelClickHandler);
        
        this._windowResizeHandler = Function.createDelegate(this, this._onWindowResize);
        $addHandler(window, 'resize', this._windowResizeHandler);
    
        this._bodyClickHandler = Function.createDelegate(this, this._onBodyClick);
        $addHandler(document.body, 'click', this._bodyClickHandler);
        
        this._postBackDelegate = Function.createDelegate(this, this._doPostBack);
    },
    _clearHandlers : function() {
        var elt = this.get_element();
        
        if (this._pageRequestManager) {
            if (this._pageEndRequestHandler) {
                this._pageRequestManager.remove_pageLoaded(this._onEndRequest);
                this._pageEndRequestHandler = null;
                this._pageRequestManager.remove_initializeRequest(this._onInitRequest);
                this._pageInitRequestHandler = null;
            }
            this._pageRequestManager = null;
        }

        if (this._updatePanelClickHandler) {
            $removeHandler(elt, 'click', this._updatePanelClickHandler);            
            this._updatePanelClickHandler = null;
        }
        
        if (this._windowResizeHandler) {
            $removeHandler(window, 'resize', this._windowResizeHandler);            
            this._windowResizeHandler = null;
        }
        
        if (this._bodyClickHandler) {
            $removeHandler(document.body, 'click', this._bodyClickHandler);
            this._bodyClickHandler = null;
        }
        
        this._postBackDelegate = null;
    },
    _positionEltLocation : function() {
        var positionElement = $get(this._positionElementID);
        var bounds = Sys.UI.DomElement.getBounds(positionElement);    
        var x = bounds.x + this.get_offsetX();
        
        switch(this._align) {
            case TicketDesk.Engine.Ajax.Extenders.HorizontalAlign.Left:
                break;
            case TicketDesk.Engine.Ajax.Extenders.HorizontalAlign.Center:
                x += parseInt(bounds.width /2);
                break;
            case TicketDesk.Engine.Ajax.Extenders.HorizontalAlign.Right:
                x += bounds.width;
                break;
        }
        
        if (this._calloutColor) {
            x += this._calloutWidth;
        }
        
        var y = bounds.y + this.get_offsetY();
        switch(this._valign) {
             case TicketDesk.Engine.Ajax.Extenders.VerticalAlign.Middle :
                y += parseInt(bounds.height /2);
                break;
            case TicketDesk.Engine.Ajax.Extenders.VerticalAlign.Top :
                break;
            case TicketDesk.Engine.Ajax.Extenders.VerticalAlign.Bottom :
                y += bounds.height;
                break;
        }
        
         return new Sys.UI.Point(x, y);
    },
    _positionElementExists : function() {
        if (this._positionElementID) {
            var positionElement = $get(this._positionElementID);
            if (positionElement != null) {
                return true;
            }
        }
        return false;
    },
    _isInsideUpdatePanel : function (element) {
        while (element) {
            if (element.id && element.id === this.get_element().id) {
                return true;
            }
            element = element.parentNode;
        }
        return false;
    },
    _iframeRequired : function() {
        return ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7));
    },
    _updatePanelVisible : function() {
        return this._getVisible(this.get_element());
    },
    _initializeShadowDiv : function() {
        var elt = this.get_element();
        if (!elt.divShadow) {
            var divShadow = document.createElement('div');
            divShadow.innerHTML = '&nbsp;';
            divShadow.style.zIndex = this._updatePanelzIndex - 1;
            divShadow.style.position = 'absolute';
            divShadow.style.display = 'none';
            divShadow.style.backgroundColor = '#999999';
            divShadow.style.opacity = .5;
            divShadow.style.MozOpacity = .5;
            divShadow.style.KhtmlOpacity = .5;
            divShadow.style.filter = 'alpha(opacity=50)';
        
            elt.parentNode.insertBefore(divShadow, elt);
            elt.divShadow = divShadow;
        }
        return elt.divShadow;
    },
    _initializeIframe : function() {
        var elt = this.get_element();
        if (!elt.iframeOverlay) {
            iframeOverlay = document.createElement('iframe');
            iframeOverlay.style.zIndex = this._updatePanelzIndex - 2;
            iframeOverlay.src = 'javascript:false';
            iframeOverlay.style.position = 'absolute';
            iframeOverlay.style.display = 'none';
            iframeOverlay.style.margin = '0px';
            iframeOverlay.style.padding = '0px';
            iframeOverlay.style.opacity = 0;
            iframeOverlay.style.MozOpacity = 0;
            iframeOverlay.style.KhtmlOpacity = 0;
            iframeOverlay.style.filter = 'alpha(opacity=0)';
            iframeOverlay.style.border = 'none';
            
            elt.parentNode.insertBefore(iframeOverlay, elt);
            elt.iframeOverlay = iframeOverlay;
        }
        
        return elt.iframeOverlay;
    },
    _initializeCalloutTable : function () {
         var elt = this.get_element();
        if (!elt.tblCallout) {
            var tbl = document.createElement('table');
            var tb = document.createElement('tbody');
            
            tbl.style.position = 'absolute';
            tbl.style.display = 'none';
            tbl.border = '0px';
            tbl.cellPadding = '0px';
            tbl.cellSpacing = '0px';
            
            for (var i = 0; i < this._calloutWidth; i++) {
                var tr = document.createElement('tr');

                for (var j = 0; j < this._calloutWidth; j++) {
                    var tc = document.createElement('td');
                   
                    if (j > i - 1) {
                        tc.style.backgroundColor = this._calloutColor;
                        var opacity = (1 - j/this._calloutWidth);
                        if (this._calloutType === TicketDesk.Engine.Ajax.Extenders.CalloutType.TransparentGradient) {
                            tc.style.filter = 'alpha(opacity=' + opacity * 100 + ')';
                            tc.style.opacity = opacity;
                            tc.style.MozOpacity = opacity;
                            tc.style.KhtmlOpacity = opacity;
                            tc.style.MozOpacity = opacity;
                        }
                    }
                    if ((j == i) || (i == 0)) {
                        tc.style.backgroundColor = 
                            (this._calloutBorderColor) ? this._calloutBorderColor : this._calloutColor;
                    }
                    tc.style.width = '1px';
                    tc.style.height = '1px';
                    tr.appendChild(tc);
                }
                tb.appendChild(tr);
            }
            
            tbl.appendChild(tb);
            
            elt.parentNode.insertBefore(tbl, elt);
            elt.tblCallout = tbl;
        }
        return elt.tblCallout;
    },
    _getVisible : function (elt) {
        return (elt) && (elt.style.visibility !== 'hidden') && (elt.style.display !== 'none');
    },
    _setVisible : function (elt, value) {
        if (value) {
            elt.style.display = 'block';
            elt.style.visibility = 'visible';
        }
        else {
            elt.style.display = 'none';
        }
    },
    _doPostBack : function() {
        if (this.get_autoPostBack()) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.abortPostBack();

            __doPostBack(this.get_uniqueID(),'');
        }
        this.hide();
    },
    _getDeltaLocation : function() { //TODO: Move to common class
        var elt = this.get_element();
        var eltLocation = Sys.UI.DomElement.getLocation(elt);
        var dx = eltLocation.x - elt.offsetLeft;
        var dy = eltLocation.y - elt.offsetTop;
        
        if (Sys.Browser.agent === Sys.Browser.Safari) {
            dx -= document.body.offsetLeft;
            dy -= document.body.offsetTop;
        }
        
        return new Sys.UI.Point(dx, dy);
    },
    //
    // Event delegates
    //
    _onInitRequest : function(sender, arg) {
        var element = arg.get_postBackElement();
        if (this._updatePanelVisible() && this.get_autoPostBack()) {
            if (this.get_clientID() !== element.id) {
                if (!this._isInsideUpdatePanel(element)) {
                    arg.set_cancel(true);
                }
            }
        }
    },
    _onEndRequest : function (sender, arg) {
        var elt = this.get_element(); 
        if (!elt) {
            return;
        }
        var dataItems = arg.get_dataItems()[this.get_clientID()];
        if (dataItems !== undefined) {
            var dataObject = eval('(' + dataItems + ')');
            if (typeof(dataObject) === 'object') {
                this._positionElementID = dataObject.PositionElementID;
                if (dataObject.Visible) {
                    this.show();
                }
                else {
                    this.hide();
                }
            }
        }
        else {
            if (this._updatePanelVisible()) {
                var updatedPanels = arg.get_panelsUpdated();
                for (i=0; i < updatedPanels.length; i++) { 
                    if (updatedPanels[i].id === elt.id) {
                        this.show();
                    }
                }
            }
        }
        
        if (this._timerID) {
            window.clearTimeout(this._timerID);
            this._timerID = null;
        }
    },
    _onBodyClick : function(e) {
        if (this._updatePanelVisible()) {
            var element = e.target ? e.target : e.srcElement;
            if (!this._isInsideUpdatePanel(element)) {
                if (this.get_autoPostBack()) {
                    //Delay to handle multiple requests when
                    //user clicks on postback element on page
                    //causing another postback event
                    this._timerID = window.setTimeout(this._postBackDelegate, 50);
                }
                else {
                    this.hide();
                }
            }
        }
    },
    _onWindowResize : function() {
        if (this._updatePanelVisible()) {
            this.show();      
        }      
    },
    _onClick : function(e) {
        if (this._updatePanelVisible()) {
            var target = e.target ? e.target : e.srcElement;
            //Safari fix
            if (e.target) {
                if (e.target.nodeType == 3) {
                    target = target.parentNode;
                }
            }
            
            if ((target.getAttribute('uppHide') == 'true') 
                && (target.getAttribute('uppTarget') == this.get_element().id))  {
                this._doPostBack();
                
            }
        }
    },
    //
    // Properties
    //
    get_offsetX : function() {
        return this._offsetX;
    },
    set_offsetX : function(value) {
        this._offsetX = value;
    },
    
    get_offsetY : function()  {
        return this._offsetY;
    },
    set_offsetY : function(value) {
        this._offsetY = value;
    },
    
    get_clientID : function() {
        return this._clientID;
    },
    set_clientID : function(value) {
        this._clientID = value;
    },
    
    get_uniqueID : function() {
        return this._uniqueID;
    },
    set_uniqueID : function(value) {
        this._uniqueID = value;
    },
    
    get_calloutColor : function() {
        return this._calloutColor;
    },
    set_calloutColor : function(value) {
        this._calloutColor = value;
    },
    
    get_calloutBorderColor : function() {
        return this._calloutBorderColor;
    },
    set_calloutBorderColor : function(value) {
        this._calloutBorderColor = value;
    },
    
    get_calloutType : function() {
        return this._calloutType;
    },
    set_calloutType : function(value) {
        this._calloutType = value;
    },
    
    get_align : function() {
        return this._align;
    },
    set_align : function(value) {
        this._align = value;
    },
    
    get_valign : function() {
        return this._valign;
    },
    set_valign : function(value) {
        this._valign = value;
    },
    
    get_loadVisible : function() {
        return this._loadVisible;
    },
    set_loadVisible : function(value) {
        this._loadVisible = value;
    },
    
    get_positionElementID : function() {
        return this._positionElementID;
    },
    set_positionElementID : function(value) {
        this._positionElementID = value;
    },
    
    get_autoPostBack : function() {
        return this._autoPostBack;
    },
    set_autoPostBack : function(value) {
        this._autoPostBack = value;
    }

}

TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior.registerClass('TicketDesk.Engine.Ajax.Extenders.UpdatePanelPopupBehavior', Sys.UI.Behavior);

//Enums

TicketDesk.Engine.Ajax.Extenders.HorizontalAlign = function(){
	throw Error.invalidOperation();
};
TicketDesk.Engine.Ajax.Extenders.HorizontalAlign.prototype = 
{
    Right  : 0,
    Center : 1,
    Left   : 2
}
TicketDesk.Engine.Ajax.Extenders.HorizontalAlign.registerEnum('TicketDesk.Engine.Ajax.Extenders.HorizontalAlign');


TicketDesk.Engine.Ajax.Extenders.VerticalAlign = function(){
	throw Error.invalidOperation();
};
TicketDesk.Engine.Ajax.Extenders.VerticalAlign.prototype = 
{
    Middle : 0,
    Top    : 1,
    Bottom : 2
}
TicketDesk.Engine.Ajax.Extenders.VerticalAlign.registerEnum('TicketDesk.Engine.Ajax.Extenders.VerticalAlign');

TicketDesk.Engine.Ajax.Extenders.CalloutType = function(){
	throw Error.invalidOperation();
};
TicketDesk.Engine.Ajax.Extenders.CalloutType.prototype = 
{
    TransparentGradient : 0,
    Solid    : 1
}
TicketDesk.Engine.Ajax.Extenders.CalloutType.registerEnum('TicketDesk.Engine.Ajax.Extenders.CalloutType');
