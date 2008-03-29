// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AjaxControlToolkit
{
    /// <summary>
    /// Describes a profile binding to an Extender property.
    /// </summary>
    public class ProfilePropertyBinding
    {
        private string _extenderPropertyName;
        private string _profilePropertyName;

        /// <summary>
        /// The property on the Extender (properties) class to bind to.  This is defined on the Extender's TargetProperties object.
        /// </summary>
        /// 
        [NotifyParentProperty(true)]
        public string ExtenderPropertyName
        {
            get
            {
                return _extenderPropertyName;
            }
            set
            {
                _extenderPropertyName = value;
            }
        }

        /// <summary>
        /// The profile property to bind to.  This is defined in the website's web.config.
        /// </summary>
        /// 
        [NotifyParentProperty(true)]
        public string ProfilePropertyName
        {
            get
            {
                return _profilePropertyName;
            }
            set
            {
                _profilePropertyName = value;
            }
        }
    }
}
