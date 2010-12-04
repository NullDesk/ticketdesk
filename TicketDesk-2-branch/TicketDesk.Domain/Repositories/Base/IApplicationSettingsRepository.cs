using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketDesk.Domain.Services;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Repositories
{
    public interface IApplicationSettingsRepository
    {
        

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

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Setting> GetAllSettings();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settingsToSave">The settings to save.</param>
        /// <returns></returns>
        bool SaveSettings(IEnumerable<Setting> settingsToSave);

    }
}
