// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference path="../ExtenderBase/BaseScripts.js" />


Type.registerNamespace("AjaxControlToolkit");

AjaxControlToolkit.NoBotBehavior = function(element) {
    /// <summary>
    /// The NoBotBehavior is used to evaluate JavaScript on the client
    /// to ensure they are running from within a browser
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// DOM Element to associate the behavior with
    /// </param>
    AjaxControlToolkit.NoBotBehavior.initializeBase(this, [element]);

    this._ChallengeScript = "";
}
AjaxControlToolkit.NoBotBehavior.prototype = {
    initialize : function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        AjaxControlToolkit.NoBotBehavior.callBaseMethod(this, "initialize");
        
        // Evaluate challenge script and store response in ClientState
        var response = eval(this._ChallengeScript);
        AjaxControlToolkit.NoBotBehavior.callBaseMethod(this, "set_ClientState", [response]);
    },

    dispose : function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        AjaxControlToolkit.NoBotBehavior.callBaseMethod(this, "dispose");
    },

    get_ChallengeScript : function() {
        /// <value type="String">
        /// JavaScript to be evaluated
        /// </value>
        return this._ChallengeScript;
    },
    set_ChallengeScript : function(value) {
        if (this._ChallengeScript != value) { 
            this._ChallengeScript = value;
            this.raisePropertyChanged('ChallengeScript');
        }
    }
}
AjaxControlToolkit.NoBotBehavior.registerClass("AjaxControlToolkit.NoBotBehavior", AjaxControlToolkit.BehaviorBase);
//    getDescriptor : function() {
//        var td = AjaxControlToolkit.NoBotBehavior.callBaseMethod(this, "getDescriptor");
//        td.addProperty("ChallengeScript", String);
//        return td;
//    },
