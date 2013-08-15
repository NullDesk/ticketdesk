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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Web.Client.Helpers;
using TicketDesk.Domain.Services;
using System.Configuration;
using System.Web.Mvc;
using TicketDesk.Domain.Models;

namespace TicketDesk.Web.Client.Models
{
    public class TicketCreateViewModel
    {

        public SelectListUtility ListUtility { get; private set; }
        public ISecurityService Security { get; private set; }
        public SettingsService Settings { get; private set; }
        public Ticket Ticket { get; set; }

        public TicketCreateViewModel(ISecurityService security, SettingsService settings, Ticket ticket)
        {
            ListUtility = MefHttpApplication.ApplicationContainer.GetExportedValue<SelectListUtility>();
            Security = security;
            Settings = settings;
            Ticket = ticket;
        }

        public bool DisplayOwner
        {
            get
            {
                return Security.IsTdStaff();
            }
        }

        public bool DisplayTags
        {
            get
            {
                return Security.IsTdStaff() || Settings.ApplicationSettings.AllowSubmitterRoleToEditTags;
            }
        }

        public bool DisplayPriorityList
        {
            get
            {
                return Security.IsTdStaff() || Settings.ApplicationSettings.AllowSubmitterRoleToEditPriority;
                
            }
        }

        public SelectList PriorityList
        {
            get { return ListUtility.GetPriorityList(true, string.Empty); }

        }

        public SelectList CategoryList
        {
            get { return ListUtility.GetCategoryList(true, string.Empty); }
        }

        public SelectList TicketTypeList
        {
            get { return ListUtility.GetTicketTypeList(true, string.Empty); }
        }

        public SelectList OwnersList
        {
            get { return ListUtility.GetSubmittersList(true, Security.CurrentUserName); }
        }
    }
}