using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TicketDesk.Domain.Identity
{
    public class TdUser : User
    {
      public string HomeTown { get; set; }
    }
    public class TicketDeskIdentityContext: IdentityDbContext<TdUser, UserClaim, UserSecret, UserLogin, Role, UserRole, Token, UserManagement>
    {
        public TicketDeskIdentityContext() : base("TicketDesk") { }
        public TicketDeskIdentityContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public IdentityManager IdentityManager
        {
            get
            {
                return new IdentityManager(new IdentityStore(this));
            }
        }

        public async Task<List<TdUser>> GetTdSubmitters()
        {
            //TODO: cache this
            return await GetTdUsersInRole("TdSubmitter");
        }

        public async Task<IEnumerable<TdUser>> GetTdStaff()
        {
            //TODO: cache this
            return await GetTdUsersInRole("TdStaff");
        }

        private async Task<List<TdUser>> GetTdUsersInRole(string roleName)
        {
            var role = await IdentityManager.Roles.FindRoleByNameAsync(roleName, CancellationToken.None);
            var users = await IdentityManager.Roles.GetUsersInRoleAsync(role.Id);
            var accounts = new List<TdUser>();
            foreach (var user in users)
            {
                var u = await IdentityManager.Store.Users.FindAsync(user, CancellationToken.None);
                accounts.Add((TdUser) u);
            }
            return accounts;
        }

       
    }
}