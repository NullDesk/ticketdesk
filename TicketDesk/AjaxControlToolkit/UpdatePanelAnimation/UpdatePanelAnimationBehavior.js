// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference path="../ExtenderBase/BaseScripts.js" />
/// <reference path="../Compat/Timer/Timer.js" />
/// <reference path="../Common/Common.js" />
/// <reference path="../Animation/Animations.js" />
/// <reference path="../Animation/AnimationBehavior.js" />


Type.registerNamespace('AjaxControlToolkit.Animation');

AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior = function(element) {
    /// <summary>
    /// Play animations just before and just after an UpdatePanel updates
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// UpdatePanel associated with the behavior
    /// </param>
    AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.initializeBase(this, [element]);
    
    // Generic animation behaviors that automatically build animations from JSON descriptions
    this._onUpdating = new AjaxControlToolkit.Animation.GenericAnimationBehavior(element);
    this._onUpdated = new AjaxControlToolkit.Animation.GenericAnimationBehavior(element);
    
    this._postBackPending = null;
    this._pageLoadedHandler = null;
}
AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.prototype = {    
    initialize : function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.callBaseMethod(this, 'initialize');
        
        // Wrap the UpdatePanel in another div (or span) that we'll do the animation with
        var element = this.get_element();
        var parentDiv = document.createElement(element.tagName);
        element.parentNode.insertBefore(parentDiv, element);
        parentDiv.appendChild(element);

        // Move the behavior from the UpdatePanel to the new parent div
        Array.remove(element._behaviors, this);
        Array.remove(element._behaviors, this._onUpdating);
        Array.remove(element._behaviors, this._onUpdated);
        if (parentDiv._behaviors) {
            Array.add(parentDiv._behaviors, this);
            Array.add(parentDiv._behaviors, this._onUpdating);
            Array.add(parentDiv._behaviors, this._onUpdated);
        } else {
            parentDiv._behaviors = [this, this._onUpdating, this._onUpdated];
        }
        this._element = this._onUpdating._element = this._onUpdated._element = parentDiv;
        
        // Initialize the generic animation behaviors
        this._onUpdating.initialize();
        this._onUpdated.initialize();

        // Attach to the beginRequest/pageLoaded events (and we'll get _pageRequestManager
        // from calling registerPartialUpdates)
        this.registerPartialUpdateEvents();
        this._pageLoadedHandler = Function.createDelegate(this, this._pageLoaded);
        this._pageRequestManager.add_pageLoaded(this._pageLoadedHandler);
    },
    
    dispose : function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        
        // Important: remove event handler before calling base dispose
        // (which will set _pageRequestManager to null)
        if (this._pageRequestManager && this._pageLoadedHandler) {
            this._pageRequestManager.remove_pageLoaded(this._pageLoadedHandler);
            this._pageLoadedHandler = null;
        }
        
        AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.callBaseMethod(this, 'dispose');
    },
    
    _partialUpdateBeginRequest : function(sender, beginRequestEventArgs) {
        /// <summary>
        /// Method that will be called when a partial update (via an UpdatePanel) begins,
        /// if registerPartialUpdateEvents() has been called.
        /// </summary>
        /// <param name="sender" type="Object">
        /// Sender
        /// </param>
        /// <param name="beginRequestEventArgs" type="Sys.WebForms.BeginRequestEventArgs">
        /// Event arguments
        /// </param>
        AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.callBaseMethod(this, '_partialUpdateBeginRequest', [sender, beginRequestEventArgs]);
        
        if (!this._postBackPending) {
            this._postBackPending = true;
            this._onUpdated.quit();
            this._onUpdating.play();
        }
    },

    _pageLoaded : function(sender, args) {
        /// <summary>
        /// Method that will be called when a partial update (via an UpdatePanel) finishes
        /// </summary>
        /// <param name="sender" type="Object">
        /// Sender
        /// </param>
        /// <param name="args" type="Sys.WebForms.PageLoadedEventArgs">
        /// Event arguments
        /// </param>
        
        if (this._postBackPending) {
            this._postBackPending = false;
            
            var element = this.get_element();
            var panels = args.get_panelsUpdated();
            for (var i = 0; i < panels.length; i++) {
                if (panels[i].parentNode == element) {
                    this._onUpdating.quit();
                    this._onUpdated.play();
                    break;
                }
            }
        }
    },

    get_OnUpdating : function() {
        /// <value type="String">
        /// Generic OnUpdating Animation's JSON definition
        /// </value>
        return this._onUpdating.get_json();
    },
    set_OnUpdating : function(value) {
        this._onUpdating.set_json(value);
        this.raisePropertyChanged('OnUpdating');
    },
    
    get_OnUpdatingBehavior : function() {
        /// <value type="AjaxControlToolkit.Animation.GenericAnimationBehavior">
        /// Generic OnUpdating Animation's behavior
        /// </value>
        return this._onUpdating;
    },
    
    
    get_OnUpdated : function() {
        /// <value type="String">
        /// Generic OnUpdated Animation's JSON definition
        /// </value>
        return this._onUpdated.get_json();
    },
    set_OnUpdated : function(value) {
        this._onUpdated.set_json(value);
        this.raisePropertyChanged('OnUpdated');
    },
    
    get_OnUpdatedBehavior : function() {
        /// <value type="AjaxControlToolkit.Animation.GenericAnimationBehavior">
        /// Generic OnUpdated Animation's behavior
        /// </value>
        return this._onUpdated;
    }
}
AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.registerClass('AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior', AjaxControlToolkit.BehaviorBase);
//    // Create a type descriptor
//    getDescriptor : function() {
//        var td = AjaxControlToolkit.Animation.UpdatePanelAnimationBehavior.callBaseMethod(this, 'getDescriptor');
//        td.addProperty('OnUpdating', String);
//        td.addProperty('OnUpdated', String);
//        return td;
//    },
