// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using AjaxControlToolkit;

#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.NoBot.NoBotBehavior.js", "text/javascript")]
#endregion

namespace AjaxControlToolkit
{
    [ToolboxItem(false)]
    [ClientScriptResource("AjaxControlToolkit.NoBotBehavior", "AjaxControlToolkit.NoBot.NoBotBehavior.js")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bot", Justification = "Bot is a commonly used term")]
    [TargetControlType(typeof(Label))]
    public class NoBotExtender : ExtenderControlBase
    {
        public NoBotExtender()
        {
            EnableClientState = true;
        }

        [ExtenderControlProperty()]
        [DefaultValue("")]
        public string ChallengeScript
        {
            get
            {
                return GetPropertyValue("ChallengeScript", "");
            }
            set
            {
                SetPropertyValue("ChallengeScript", value);
            }
        }
    }
}
