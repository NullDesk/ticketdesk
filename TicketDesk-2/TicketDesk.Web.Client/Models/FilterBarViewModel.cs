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
using TicketDesk.Domain.Models;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Models
{
    public class FilterBarViewModel
    {
        public FilterBarViewModel(TicketCenterListSettings listPreferences, UserInfo[] staffUsers, UserInfo[] submitterUsers)
        {
            Preferences = listPreferences;
            StaffUsers = staffUsers;
            SubmitterUsers = submitterUsers;
        }

        private UserInfo[] SubmitterUsers { get; set; }

        private UserInfo[] StaffUsers { get; set; }

        private TicketCenterListSettings Preferences { get; set; }

        public SelectList ItemsPerPageSelectList
        {
            get
            {
                return new SelectList(
                    new[] 
                    {   
                        new { Text="10", Value="10"}, 
                        new { Text="20", Value="20"},
                        new { Text="30", Value="30"},
                        new { Text="50", Value="50"},
                        new { Text="100", Value="100"}
                    },
                "Value",
                "Text",
                Preferences.ItemsPerPage
                );
            }
        }

        public SelectList CurrentStatusSelectList
        {
            get
            {

                string selectedStatus = "any";
                var fColumn = Preferences.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "CurrentStatus");
                if (fColumn != null)
                {
                    if (fColumn.ColumnValue != "closed")
                    {
                        selectedStatus = fColumn.ColumnValue;
                    }
                    else
                    {
                        selectedStatus = (fColumn.UseEqualityComparison.HasValue && fColumn.UseEqualityComparison.Value) ? "closed" : "open";
                    }
                }

                return new SelectList(
                    new[] 
                    {   
                        new { Text="-- any --", Value="any"}, 
                        new { Text="-- open --", Value="open"},
                        new { Text="Active", Value="active"},
                        new { Text="More Info", Value="moreinfo"},
                        new { Text="Resolved", Value="resolved"},
                        new { Text="Closed", Value="closed"}
                    },
                "Value",
                "Text",
                selectedStatus
                );
            }
        }

        public SelectList SubmittersSelectList
        {
            get
            {
                string selectedUserName = "anyone";
                var fColumn = Preferences.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "Owner");
                if (fColumn != null)
                {
                    selectedUserName = fColumn.ColumnValue;
                }


                List<UserInfo> lusers = new List<UserInfo>(SubmitterUsers.OrderBy(u => u.DisplayName));
                lusers.Insert(0, new UserInfo() { Name = "anyone", DisplayName = "-- anyone --" });

                return new SelectList(lusers, "Name", "DisplayName", selectedUserName);
            }
        }

        public SelectList AssignedToSelectList
        {
            get
            {
                string selectedUserName = "anyone";
                var fColumn = Preferences.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "AssignedTo");
                if (fColumn != null)
                {
                    //when filter for column exists, but the value is null it means the selection was unassigned
                    selectedUserName = (string.IsNullOrEmpty(fColumn.ColumnValue)) ? "unassigned" : fColumn.ColumnValue;
                }


                List<UserInfo> lusers = new List<UserInfo>(StaffUsers.OrderBy(u => u.DisplayName));
                lusers.Insert(0, new UserInfo() { Name = "anyone", DisplayName = "-- anyone --" });
                lusers.Insert(1, new UserInfo(){Name = "unassigned", DisplayName = "-- unassigned --"});

                return new SelectList(lusers, "Name", "DisplayName", selectedUserName);
            }
        }
    }
}