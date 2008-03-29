// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;


#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.PagingBulletedList.PagingBulletedListBehavior.js", "text/javascript")]
#endregion


namespace AjaxControlToolkit
{
    [Designer("AjaxControlToolkit.PagingBulletedListDesigner, AjaxControlToolkit")]
    [ClientScriptResource("AjaxControlToolkit.PagingBulletedListBehavior", "AjaxControlToolkit.PagingBulletedList.PagingBulletedListBehavior.js")]
    [TargetControlType(typeof(System.Web.UI.WebControls.BulletedList))]
    [System.Drawing.ToolboxBitmap(typeof(PagingBulletedListExtender), "PagingBulletedList.PagingBulletedList.ico")]
    public class PagingBulletedListExtender : ExtenderControlBase
    {
        public PagingBulletedListExtender()
        {
            EnableClientState = true;
        }

        [ExtenderControlProperty()]
        [DefaultValue(1)]
        public int IndexSize
        {
            get
            {
                return GetPropertyValue<int>("IndexSize", 1);
            }
            set
            {
                SetPropertyValue<int>("IndexSize", value);
            }
        }

        [ExtenderControlProperty()]
        public int? Height
        {
            get
            {
                return GetPropertyValue<int?>("Height", null);
            }
            set
            {
                SetPropertyValue<int?>("Height", value);
            }
        }

        [ExtenderControlProperty()]
        [DefaultValue(" - ")]
        public string Separator
        {
            get
            {
                return GetPropertyValue<string>("Separator", " - ");
            }
            set
            {
                SetPropertyValue<string>("Separator", value);
            }
        }

        //When use MaxItemPerPage IndexSize is ignored, index are automaticaly generated
        [ExtenderControlProperty()]
        public int? MaxItemPerPage
        {
            get
            {
                return GetPropertyValue<int?>("MaxItemPerPage", null);
            }
            set
            {
                SetPropertyValue<int?>("MaxItemPerPage", value);
            }
        }

        [ExtenderControlProperty()]
        [DefaultValue(false)]
        public bool ClientSort
        {
            get
            {
                return GetPropertyValue<bool>("ClientSort", false);
            }
            set
            {
                SetPropertyValue<bool>("ClientSort", value);
            }
        }

        [ExtenderControlProperty()]
        public string SelectIndexCssClass
        {
            get
            {
                return GetPropertyValue<string>("SelectIndexCssClass", string.Empty);
            }
            set
            {
                SetPropertyValue<string>("SelectIndexCssClass", value);
            }
        }

        [ExtenderControlProperty()]
        public string UnselectIndexCssClass
        {
            get
            {
                return GetPropertyValue<string>("UnselectIndexCssClass", string.Empty);
            }
            set
            {
                SetPropertyValue<string>("UnselectIndexCssClass", value);
            }
        }
    }
}


