using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(IApplicationSettingsService))]
    public class ApplicationSettingsService : ApplicationSettingsServiceBase
    {
        /// <summary>
        /// Unit test ctor, Initializes a new instance of the <see cref="TdSettingService"/> class.
        /// </summary>
        /// <param name="settingRepository">The setting repository.</param>
        [ImportingConstructor]
        public ApplicationSettingsService(IApplicationSettingsRepository settingsRepository)
        {
            Repository = settingsRepository;
        }

        /// <summary>
        /// Gets a collection of available priorities.
        /// </summary>
        /// <returns></returns>
        public override string[] AvailablePriorities
        {
            get
            {
                return Repository.GetPriorities();
            }
        }

        /// <summary>
        /// Gets a collection of available categories.
        /// </summary>
        /// <returns></returns>
        public override string[] AvailableCategories
        {
            get
            {
                return Repository.GetCategories();
            }
        }

        public override string[] AvailableTicketTypes
        {
            get
            {
                return Repository.GetTicketTypes();
            }
        }



    }
}
