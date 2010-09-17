using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain.Services
{
    public abstract class ApplicationSettingsServiceBase : IApplicationSettingsService
    {
        #region ISettingService Members

        public EditorModes DefaultEditorMode
        {
            get { throw new NotImplementedException(); }
        }

        public EditorModes[] GetAllowedEditorModesForRole(string roleName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets a collection of available priorities.
        /// </summary>
        /// <returns></returns>
        public abstract string[] AvailablePriorities { get; }

        /// <summary>
        /// Gets a collection of available categories.
        /// </summary>
        /// <returns></returns>
        public abstract string[] AvailableCategories { get; }

        /// <summary>
        /// Gets the available ticket types.
        /// </summary>
        /// <returns></returns>
        public abstract string[] AvailableTicketTypes { get; }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public IApplicationSettingsRepository Repository { get; set; }


        #endregion
    }
}
