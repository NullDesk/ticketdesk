// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


using System;

[assembly: System.Web.UI.WebResource("AjaxControlToolkit.Compat.DragDrop.DragDropScripts.js", "text/javascript")]

namespace AjaxControlToolkit
{
    [RequiredScript(typeof(TimerScript))]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource(null, "AjaxControlToolkit.Compat.DragDrop.DragDropScripts.js")]
    public static class DragDropScripts
    {
    }
}