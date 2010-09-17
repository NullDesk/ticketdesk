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

TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior = function (element) {
    TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior.initializeBase(this, [element]);

    //Properties    
    this._timerID = null;
    
    //Handlers
    this._pageRequestManager = null;
    this._showDelegate = null;
    this._pageBeginRequestHandler = null;
    this._pageEndRequestHandler = null;
    this._windowResizeHandler = null;
    this._isActive = false;
    
}

TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior.prototype = {
    initialize: function() {
        TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior.callBaseMethod(this, 'initialize');
        var elt = this.get_element();
        if (elt) {
            elt.style.position = 'absolute';
        }
        this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
        this._pageBeginRequestHandler = Function.createDelegate(this, this._onBeginRequest);
        this._pageEndRequestHandler = Function.createDelegate(this, this._onEndRequest);
        this._showDelegate = Function.createDelegate(this, this._onShow);
        if (this._pageRequestManager != null) {
            this._pageRequestManager.add_beginRequest(this._pageBeginRequestHandler);
            this._pageRequestManager.add_endRequest(this._pageEndRequestHandler);
        }
        this._windowResizeHandler = Function.createDelegate(this, this._onWindowResize);
        $addHandler(window, 'resize', this._windowResizeHandler);

    },
    dispose: function() {
        if (this._pageRequestManager) {
            if (this._pageBeginRequestHandler) {
                this._pageRequestManager.remove_beginRequest(this._onBeginRequest);
                this._pageEndRequestHandler = null;
            }
            if (this._pageEndRequestHandler) {
                this._pageRequestManager.remove_endRequest(this._onEndRequest);
                this._pageEndRequestHandler = null;
            }
            this._pageRequestManager = null;
        }

        if (this._windowResizeHandler) {
            $removeHandler(window, 'resize', this._windowResizeHandler);
            this._windowResizeHandler = null;
        }

        TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior.callBaseMethod(this, 'dispose');
    },
    //
    // Handlers
    //
    _onBeginRequest: function(sender, arg) {
        var curElt = arg.get_postBackElement();
        var startTimer = !(this._associatedUpdatePanelID);
        while (!startTimer && curElt) {
            if (curElt.id && this._associatedUpdatePanelID === curElt.id) {
                startTimer = true;
            }
            curElt = curElt.parentNode;
        }
        if (startTimer) {
            this._timerID = window.setTimeout(this._showDelegate, this._displayAfter);
        }
    },
    _onEndRequest: function(sender, arg) {
        var elt = this.get_element();
        if (!elt) {
            return;
        }

        elt.style.display = 'none';
        Sys.UI.DomElement.setLocation(elt, -50000, -50000);

        var iframe = elt.iframeOverlay;
        if (iframe) {
            iframe.style.display = 'none';
            Sys.UI.DomElement.setLocation(iframe, -50000, -50000);
        }
        if (this._timerID) {
            window.clearTimeout(this._timerID);
            this._timerID = null;
        }
        this._isActive = false;
    },
    _onWindowResize: function() {
        if (this._isActive) {
            this._onShow();
        }
    },
    //
    // Private methods
    //
    _onShow: function() {
        var controlToOverlay;
        var elt;

        if (this._controlToOverlayID) {
            controlToOverlay = $get(this._controlToOverlayID);
            if (!controlToOverlay) {
                return;
            }
        }

        elt = this.get_element();
        if (!elt) {
            return;
        }

        this._isActive = true;
        var iframe = (this._iframeRequired()) ? this._initializeIframe() : null;

        if (this._targetCssClass) {
            Sys.UI.DomElement.addCssClass(elt, this._targetCssClass);
        }

        elt.style.zIndex = 99999;
        elt.style.position = 'absolute';

        var bounds = (this._controlToOverlayID) ?
            this._getElementBounds(controlToOverlay) : this._getBrowserInnerBounds();

        elt.style.width = bounds.width + 'px';
        elt.style.height = bounds.height + 'px';
        Sys.UI.DomElement.setLocation(elt, bounds.x, bounds.y);

        if (iframe) {
            iframe.style.width = bounds.width + 'px';
            iframe.style.height = bounds.height + 'px';
            iframe.style.display = 'block';
            Sys.UI.DomElement.setLocation(iframe, bounds.x, bounds.y);
        }

        this._adjustDiv(controlToOverlay);
    },
    _adjustDiv: function(container) {
        if (this._centerOnContainer && this._elementToCenterID) {

            var elementToCenter = $get(this._elementToCenterID);
       
            var containerBounds = (container) ?
            this._getElementBounds(container) : this._getBrowserInnerBounds();
       
            var dfBounds = this._getElementBounds(elementToCenter);

            dfs = elementToCenter.style;

            dfs.position = 'absolute';
            dfs.zIndex = 100000

            var x = (containerBounds.width / 2) - (dfBounds.width / 2);
            var y = (containerBounds.height / 2) - (dfBounds.height / 2);
            Sys.UI.DomElement.setLocation(elementToCenter, parseInt(x), parseInt(y));
           
        }
    },
    _iframeRequired: function() {
        return ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7));
    },
    _initializeIframe: function() {
        var elt = this.get_element();
        if (!elt.iframeOverlay) {
            var iframeOverlay = document.createElement('iframe');
            iframeOverlay.style.zIndex = 99999;
            iframeOverlay.src = 'javascript:false';
            iframeOverlay.style.position = 'absolute';
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
    _getElementBounds: function(targetElement) {
        var bounds = Sys.UI.DomElement.getBounds(targetElement);
        var delta = this._getDeltaLocation();
        bounds.x = bounds.x - delta.x;
        bounds.y = bounds.y - delta.y;
        return bounds;
    },
    _getDeltaLocation: function() {  //TODO: Move to common class
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
    _getBrowserInnerBounds: function() {
        var height;
        var width;
        var compatMode = document.compatMode;
        var delta = this._getDeltaLocation();

        if (compatMode == 'CSS1Compat') {
            width = Math.max(document.documentElement.clientWidth, document.documentElement.scrollWidth);
            height = Math.max(document.documentElement.clientHeight, document.documentElement.scrollHeight);
        }
        else {
            width = (window.innerWidth) ? window.innerWidth : Math.max(document.body.clientWidth, document.body.scrollWidth);
            height = (window.innerHeight) ? window.innerHeight : Math.max(document.body.clientHeight, document.body.scrollHeight);
        }
        return new Sys.UI.Bounds(0, 0, width, height);
    },
    //
    // Properties
    //
    get_controlToOverlayID: function() {
        return this._controlToOverlayID;
    },
    set_controlToOverlayID: function(value) {
        this._controlToOverlayID = value;
    },

    get_targetCssClass: function() {
        return this._targetCssClass;
    },
    set_targetCssClass: function(value) {
        this._targetCssClass = value;
    },

    get_displayAfter: function() {
        return this._displayAfter;
    },
    set_displayAfter: function(value) {
        this._displayAfter = value;
    },

    get_associatedUpdatePanelID: function() {
        return this._associatedUpdatePanelID;
    },
    set_associatedUpdatePanelID: function(value) {
        this._associatedUpdatePanelID = value;
    },
    get_centerOnContainer: function() {
        return this._centerOnContainer;
    },
    set_centerOnContainer: function(value) {
        this._centerOnContainer = value;
    },
    get_elementToCenterID: function() {
        return this._elementToCenterID;
    },
    set_elementToCenterID: function(value) {
        this._elementToCenterID = value;
    }


}

TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior.registerClass('TicketDesk.Engine.Ajax.Extenders.UpdateProgressOverlayBehavior', Sys.UI.Behavior);

