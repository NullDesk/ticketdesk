using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain.Services
{
    public interface IApplicationSettingsService
    {
        IApplicationSettingsRepository Repository { get; }

        //TODO: When adding the update side for the editor modes settings, the set side of allowed editor modes must ensure that
        //          it throws a rules exception if the default application editor is not included in the list.
        //      On the flip side, the set for default editor needs to throw an exception if ANY of the roles lack the new value 
        //          in their list of allowed modes OR it needs to automatically add the new default to any roles that are missing
        //          that value.



        /// <summary>
        /// Gets the default editor mode.
        /// </summary>
        /// <value>The default editor mode.</value>
        EditorModes DefaultEditorMode { get; }

        /// <summary>
        /// Gets the allowed custom editor options that a role is allowed to choose from.
        /// </summary>
        /// <param name="roleName">Name of the role whose allowed modes you wish to fetch.</param>
        /// <returns></returns>
        /// <value>The allowed user editor modes for the specified role.</value>
        EditorModes[] GetAllowedEditorModesForRole(string roleName);


        /// <summary>
        /// Gets a collection of available priorities.
        /// </summary>
        /// <returns></returns>
        string[] AvailablePriorities { get; }
        /// <summary>
        /// Gets a collection of available categories.
        /// </summary>
        /// <returns></returns>
        string[] AvailableCategories { get; }

        /// <summary>
        /// Gets the available ticket types.
        /// </summary>
        /// <returns></returns>
        string[] AvailableTicketTypes { get; }

       
    }
}
