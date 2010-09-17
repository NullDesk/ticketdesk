using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketDesk.Domain.Services;

namespace TicketDesk.Domain.Repositories
{
    public interface IApplicationSettingsRepository
    {
        /// <summary>
        /// Gets a collection of all configured priorities.
        /// </summary>
        /// <returns></returns>
        string[] GetPriorities();


        /// <summary>
        /// Gets a collection configured categories.
        /// </summary>
        /// <returns></returns>
        string[] GetCategories();

        /// <summary>
        /// Gets the ticket types.
        /// </summary>
        /// <returns></returns>
        string[] GetTicketTypes();

        /// <summary>
        /// Gets the default editor mode.
        /// </summary>
        /// <returns></returns>
        EditorModes GetDefaultEditorMode();

        /// <summary>
        /// Gets the allowed editor modes for role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        EditorModes[] GetAllowedEditorModesForRole(string roleName);

    }
}
