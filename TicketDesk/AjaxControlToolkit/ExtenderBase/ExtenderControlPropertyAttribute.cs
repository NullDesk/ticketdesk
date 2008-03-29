// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxControlToolkit
{
    /// <summary>
    /// Signifies that this property is to be emitted as a client script property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExtenderControlPropertyAttribute : Attribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Exposing this for user convenience")]
        private static ExtenderControlPropertyAttribute Yes = new ExtenderControlPropertyAttribute(true);
        private static ExtenderControlPropertyAttribute No = new ExtenderControlPropertyAttribute(false);
        private static ExtenderControlPropertyAttribute Default = No;
        
        #region [ Fields ]

        private bool _isScriptProperty;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new ExtenderControlPropertyAttribute
        /// </summary>
        public ExtenderControlPropertyAttribute()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new ExtenderControlPropertyAttribute
        /// </summary>
        /// <param name="isScriptProperty"></param>
        public ExtenderControlPropertyAttribute(bool isScriptProperty)
        {
            _isScriptProperty = isScriptProperty;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Whether this property should be exposed to the client
        /// </summary>
        public bool IsScriptProperty
        {
            get { return _isScriptProperty; }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Tests for object equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) {
                return true;
            }
            ExtenderControlPropertyAttribute other = obj as ExtenderControlPropertyAttribute;
            if (other != null)
            {
                return other._isScriptProperty == _isScriptProperty;
            }
            return false;
        }

        /// <summary>
        /// Gets a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _isScriptProperty.GetHashCode();
        }

        /// <summary>
        /// Gets whether this is the default value for this attribute
        /// </summary>
        /// <returns></returns>
        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }

        #endregion
    }
}
