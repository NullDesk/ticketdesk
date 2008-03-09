// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AjaxControlToolkit")]
[assembly: AssemblyDescription("Ajax Control Toolkit")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("AjaxControlToolkit")]
[assembly: AssemblyCopyright("Copyright © Microsoft Corporation 2006-2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers()]
[assembly: SecurityTransparent()]
[assembly: NeutralResourcesLanguage("en")]
[module: SuppressMessage("Microsoft.Usage", "CA2209:AssembliesShouldDeclareMinimumSecurity", Justification = "Permissions are flowed through")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "AjaxControlToolkit.MaskedEditValidatorCompatibility", Justification = "Keeping compatibility items distinct")]
[assembly: SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member", Target = "AjaxControlToolkit.AccordionPaneCollection.#System.Collections.ICollection.SyncRoot", Justification = "Interface method implementation required but unwanted")]
// The following SuppressMessages suppress warnings from localization
// resources
[assembly: SuppressMessage("Microsoft.Naming", "CA1701:ResourceStringCompoundWordsShouldBeCasedCorrectly", MessageId = "NonExistent", Scope = "resource", Target = "AjaxControlToolkit.ScriptResources.ScriptResources.resources")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "setanimation", Scope = "resource", Target = "AjaxControlToolkit.ScriptResources.ScriptResources.resources")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Outdent", Scope = "resource", Target = "AjaxControlToolkit.ScriptResources.ScriptResources.resources")]
// The following SuppressMessages address the warnings that appear because
// Microsoft.Web.Extensions.Design.dll is no longer directly referenced
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.AutoCompleteDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.CascadingDropDownDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.CalendarDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.CollapsiblePanelDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ConfirmButtonDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.DragPanelDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.DropDownDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.DropShadowDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.DynamicPopulateDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.FilteredTextBoxDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.HoverExtenderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.HoverMenuDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ListSearchDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.MaskedEditDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ModalPopupDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.MutuallyExclusiveCheckBoxDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.PagingBulletedListDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.PasswordStrengthExtenderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.PopupControlDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.PopupExtenderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ResizableControlDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.RoundedCornersDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.SliderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.SlideShowDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.TextBoxWatermarkExtenderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ToggleButtonExtenderDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "AjaxControlToolkit.ValidatorCalloutDesigner")]
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "AjaxControlToolkit.NumericUpDownDesigner.get_TextBoxList():System.Collections.Generic.List`1<System.String>")]
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "AjaxControlToolkit.ReorderListDesigner.set_CurrentView(System.Int32):System.Void")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("bab84a2c-0861-4cc9-a76d-7dbd9c04b055")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("3.0.11119.*")]
[assembly: AssemblyFileVersion("3.0.11119.0")]

// Enable script combining for all Toolkit scripts
// SliderBehavior uses unsupported WebResource substitution internally, so it's excluded from combining
[assembly: AjaxControlToolkit.ScriptCombine(ExcludeScripts = "AjaxControlToolkit.Slider.SliderBehavior.js")]
