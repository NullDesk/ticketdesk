namespace TicketDesk.Domain.Legacy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Class Configuration. This class cannot be inherited.
    /// </summary>
    public sealed class Configuration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskCore";
        }

        
    }
}
