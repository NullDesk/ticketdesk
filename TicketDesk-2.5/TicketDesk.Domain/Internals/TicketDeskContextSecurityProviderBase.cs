using System;

namespace TicketDesk.Domain
{
    /// <summary>
    /// Provides the minimal set of security services to support business logic 
    /// </summary>
    public abstract class TicketDeskContextSecurityProviderBase
    {
        public abstract Func<string> GetCurrentUserId { get; }

        public abstract Func<string, bool> GetIsTdHelpDeskUser { get; }

        public abstract Func<string, bool> GetIsTdInternalUser { get; }

        public abstract Func<string, bool> GetIsTdAdministrator { get; }

        public abstract Func<string, bool> GetIsTdPendingUser { get; }

        public bool IsTdHelpDeskUser
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdInternalUser
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdAdministrator
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }

        public bool IsTdPendingUser
        {
            get { return GetIsTdHelpDeskUser(GetCurrentUserId()); }
        }
    }
}
