// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.



namespace TicketDesk.Web.Client
{

    /*
     * You must add a reference to Microsoft.ComponentModel.Composition.Disgnostics to use the method
     *      A copy of this is in the library folder of the TD 2 branch: It has been compiled to work
     *      with .NET 4 and the release version of MEF from the System.ComponentModel.Composition assembly.
     */
      
    public class MefTestHelper
    {
        /// <summary>
        /// Tests composition of everything and outputs the diagnostic
        /// </summary>
        /// <returns></returns>
        //public string Test()
        //{
        //    var ci = new Microsoft.ComponentModel.Composition.Diagnostics.CompositionInfo(MefHttpApplication.RootCatalog, MefHttpApplication.ApplicationContainer);
        //    var sb = new System.Text.StringBuilder();
        //    var w = new System.IO.StringWriter(sb);
        //    Microsoft.ComponentModel.Composition.Diagnostics.CompositionInfoTextFormatter.Write(ci, w);
        //    return sb.ToString();
        //}
    }
}