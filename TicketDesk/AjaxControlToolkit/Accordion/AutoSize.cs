// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;

namespace AjaxControlToolkit
{
    /// <summary>
    /// AutoSize provides several options for resizing an Accordion control
    /// </summary>
    public enum AutoSize
    {
        /// <summary>
        /// Allow the Accordion to grow as tall as it wants
        /// </summary>
        None = 0,

        /// <summary>
        /// Force the Accordion to be a specific height
        /// </summary>
        Fill = 1,

        /// <summary>
        /// Limit the Accordion to a specific height
        /// </summary>
        Limit = 2
    }
}