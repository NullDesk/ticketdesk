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

 
using System;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace TicketDesk.Engine.Ajax.Extenders {
    public abstract class ExtenderBase : ExtenderControl {
        internal const string Control_Not_Found = "Unable to find control id '{0}' referenced by the '{1}' property of '{2}'";
        internal const string Target_Is_Null = "The target control is not defined.";

        /// <summary>
        /// Locate control by walking up the control tree
        /// ref: Ajax Control Toolkit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal Control FindControlHelper(string id) {
            Control c = base.FindControl(id);
            Control nc = NamingContainer;

            while ((null == c) && (null != nc)) {
                c = nc.FindControl(id);
                nc = nc.NamingContainer;
            }
            return c;
        }
    }
}