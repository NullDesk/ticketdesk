using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models
{
    public class FilterBarViewModel
    {
        private UserTicketListSetting CurrentListSetting { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        private TicketDeskUserManager UserManager { get; set; }
        public FilterBarViewModel(UserTicketListSetting currentListSetting)
        {
            CurrentListSetting = currentListSetting;
            RoleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
        }


        public SelectList ItemsPerPageSelectList
        {
            get
            {
                return new SelectList(
                    new[]
                    {
                        new {Text = "10", Value = "10"},
                        new {Text = "20", Value = "20"},
                        new {Text = "30", Value = "30"},
                        new {Text = "50", Value = "50"},
                        new {Text = "100", Value = "100"}
                    },
                    "Value",
                    "Text",
                    CurrentListSetting.ItemsPerPage
                    );
            }
        }

        public SelectList CurrentStatusSelectList
        {
            get
            {

                string selectedStatus = "any";
                var fColumn = CurrentListSetting.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "CurrentStatus");
                if (fColumn != null)
                {
                    if (fColumn.ColumnValue != "closed")
                    {
                        selectedStatus = fColumn.ColumnValue.ToString();
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


        public Dictionary<string, object> OwnerFilterHtmlAttributes
        {
            get
            {
                var isOwnerFilterDisabled = CurrentListSetting.DisabledFilterColumnNames.Contains("Owner");
                var cVal = String.Format("postback{0}", (isOwnerFilterDisabled) ? " disabled" : string.Empty);
                var ownerHtmlAttributes = new Dictionary<string, object> { { "Class", cVal } };
                if (isOwnerFilterDisabled)
                {
                    ownerHtmlAttributes.Add("Disabled", true);
                }
                return ownerHtmlAttributes;
            }
        }

       

        public SelectList SubmittersSelectList
        {
            get
            {
                string selectedUserName = "anyone";
                var fColumn = CurrentListSetting.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "Owner");
                if (fColumn != null)
                {
                    selectedUserName = fColumn.ColumnValue.ToString();
                }
               
                var lusers = GetUsersInRole("TdInternalUsers");
                lusers.Insert(0, new UserItem { Name = "anyone", DisplayName = "-- anyone --" });

                return new SelectList(lusers, "Name", "DisplayName", selectedUserName);
            }
        }

       

        public SelectList AssignedToSelectList
        {
            get
            {
                string selectedUserName = "anyone";
                var fColumn = CurrentListSetting.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "AssignedTo");
                if (fColumn != null)
                {
                    //when filter for column exists, but the value is null it means the selection was unassigned
                    selectedUserName = (string.IsNullOrEmpty(fColumn.ColumnValue.ToString())) ? "unassigned" : fColumn.ColumnValue.ToString();
                }

                var lusers = GetUsersInRole("TdHelpDeskUsers");
                lusers.Insert(0, new UserItem { Name = "anyone", DisplayName = "-- anyone --" });
                lusers.Insert(1, new UserItem { Name = "unassigned", DisplayName = "-- unassigned --" });

                return new SelectList(lusers, "Name", "DisplayName", selectedUserName);
            }
        }

        public Dictionary<string, object> AssignedFilterHtmlAttributes
        {
            get
            {
                var isAssignedToFilterDisabled = CurrentListSetting.DisabledFilterColumnNames.Contains("AssignedTo");
                var cVal = String.Format("postback{0}", (isAssignedToFilterDisabled) ? " disabled" : string.Empty);
                var assignedHtmlAttributes = new Dictionary<string, object> { { "Class", cVal } };
                if (isAssignedToFilterDisabled)
                {
                    assignedHtmlAttributes.Add("Disabled", true);
                }
                return assignedHtmlAttributes;
            }
        }

        public Dictionary<string, object> StatusFilterHtmlAttributes
        {
            get
            {
                var isCurrentStatusFilterDisabled = CurrentListSetting.DisabledFilterColumnNames.Contains("CurrentStatus");
                var cVal = String.Format("postback{0}", (isCurrentStatusFilterDisabled) ? " disabled" : string.Empty);
                var statusHtmlAttributes = new Dictionary<string, object> { { "Class", cVal } };
                if (isCurrentStatusFilterDisabled)
                {
                    statusHtmlAttributes.Add("Disabled", true);
                }
                
                return statusHtmlAttributes;
            }
        }

        internal class UserItem
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }

        private List<UserItem> GetUsersInRole(string roleName)
        {
            //TODO: Add caching for this, either here or deeper in the underlying framework
            return RoleManager
                .FindByName(roleName)
                .Users
                .GetUsersInRole(UserManager)
                .OrderBy(u => u.DisplayName)
                .Select(u => new UserItem { Name = u.Id, DisplayName = u.DisplayName })
                .ToList();
        }
    }
}