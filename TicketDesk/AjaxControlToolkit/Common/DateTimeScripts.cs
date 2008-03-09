using System.Web.UI;
using System.Web;
using System.Web.Script.Services;
using AjaxControlToolkit;
using System.Globalization;

[assembly: WebResource("AjaxControlToolkit.Common.DateTime.js", "application/x-javascript")]

namespace AjaxControlToolkit
{
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource(null, "AjaxControlToolkit.Common.DateTime.js")]
    public static class DateTimeScripts
    {
    }
}
