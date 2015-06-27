using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Identity;

namespace TicketDesk.Domain.Model
{
    public static class ProjectExtensions
    {

        public static async Task<int> GetUserSelectedProjectId(this UserSettingsManager userSettingsManager, TdDomainContext context)
        {
            var projects = context.Projects;
            var settings = await userSettingsManager.GetSettingsForUserAsync(context.SecurityProvider.CurrentUserId);
            var projectId = settings.SelectedProjectId ?? 0;

            //if user's selected project points to a project that no longer exists, reset
            //  normally this wouldn't happen since the dbcontext will update user settings when projects are deleted 
            if (projectId != 0 && projects.All(p => p.ProjectId != projectId))
            {
                projectId = 0;
                await UpdateUserSelectedProject(userSettingsManager, projectId, context.SecurityProvider.CurrentUserId);
                context.SaveChanges();
            }
            return projectId;

        }

        public static async Task UpdateUserSelectedProject(this UserSettingsManager userSettingsManager, int projectId, string userId)
        {
            var settings = await userSettingsManager.GetSettingsForUserAsync(userId);
            settings.SelectedProjectId = projectId;
        }
    }
}