using System.Threading.Tasks;
using System.Data.Entity;

namespace TicketDesk.Domain.Model
{
    public static class ProjectExtensions
    {
      

        public static async Task<int> GetUserSelectedProjectIdAsync(this UserSettingsManager userSettingsManager, TdDomainContext context)
        {
            //TODO: We have to take the source context as a param because we have sync callers (child action in navigation).
            //      Trying to use dependency resolver with a sync caller results in the context being invoked without a security provider.
            //      The entire concept of child actions are heavily refactored in MVC 6, so this should not be an issue in future versions.

            var projects = context.Projects;
            var settings = await userSettingsManager.GetSettingsForUserAsync(context.SecurityProvider.CurrentUserId);
            var projectId = settings.SelectedProjectId ?? 0;

            //if user's selected project points to a project that no longer exists, reset
            //  normally this wouldn't happen since the dbcontext will update user settings when projects are deleted 
            if (projectId != 0 && await projects.AllAsync(p => p.ProjectId != projectId))
            {
                projectId = 0;
                await UpdateUserSelectedProjectAsync(userSettingsManager, projectId, context.SecurityProvider.CurrentUserId);
                await context.SaveChangesAsync();
            }
            return projectId;

        }

        public static async Task UpdateUserSelectedProjectAsync(this UserSettingsManager userSettingsManager, int projectId, string userId)
        {
            var settings = await userSettingsManager.GetSettingsForUserAsync(userId);
            settings.SelectedProjectId = projectId;
        }
    }
}