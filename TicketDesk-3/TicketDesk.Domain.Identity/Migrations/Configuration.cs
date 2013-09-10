using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.Identity;

namespace TicketDesk.Domain.Identity.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketDesk.Domain.Identity.TicketDeskIdentityContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskIdentity";
        }


        //private string CreateToken(string message, string secret)
        //{
        //    secret = secret ?? "";
        //    var encoding = new System.Text.UnicodeEncoding();
        //    byte[] keyByte = encoding.GetBytes(secret);
        //    byte[] messageBytes = encoding.GetBytes(message);
        //    using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
        //    {
        //        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
        //        return Convert.ToBase64String(hashmessage);
        //    }
        //}

        protected override void Seed(TicketDesk.Domain.Identity.TicketDeskIdentityContext context)
        {
            SetupUsersAsync(context);


            //context.Users.AddOrUpdate(
            //    t => t.Id,
            //    new TdUser()
            //    {
            //        Id = "d0569823-1be7-482b-947b-0269b9011dc6",
            //        HomeTown = "Anytown, USA",
            //        UserName = "admin",
            //        Logins = new[]{ 
            //            new UserLogin()
            //            {
            //                LoginProvider = "local", 
            //                ProviderKey = "admin", 
            //                UserId = "d0569823-1be7-482b-947b-0269b9011dc6"
            //            }
            //        },
            //        Management = new UserManagement()
            //        {
            //            DisableSignIn = false,
            //            LastSignInTimeUtc = DateTime.Now,
            //            UserId = "d0569823-1be7-482b-947b-0269b9011dc6"
            //        }
            //    }
            //);
            //context.UserSecrets.AddOrUpdate(
            //    s => s.UserName,
            //    new UserSecret()
            //    {
            //        Secret = CreateToken("admin", null),
            //        UserName = "admin"
            //    }
            //);
        }

        private static void SetupUsersAsync(TicketDeskIdentityContext context)
        {
            var roles = new[]
            {
                new Role("TdAdmin"),
                new Role("TdStaff"),
                new Role("TdSubmitter")
            };
            var users = new[]
            {
                new TdUser()
                {
                    HomeTown = "AnyTown USA",
                    UserName = "admin"
                },
                new TdUser()
                {
                    HomeTown = "AnyTown USA",
                    UserName = "staff"
                },
                new TdUser()
                {
                    HomeTown = "AnyTown USA",
                    UserName = "user"
                }
            };
            var userRoles = new List<Tuple<string,string>>
            {
                Tuple.Create("admin", "TdAdmin"),
                Tuple.Create("admin", "TdStaff"),
                Tuple.Create("admin", "TdSubmitter"),
                Tuple.Create("staff", "TdStaff"),
                Tuple.Create("staff", "TdSubmitter"),
                Tuple.Create("user", "TdSubmitter")
            };


            var idMgr = new Microsoft.AspNet.Identity.IdentityManager(new IdentityStore(context));
            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role.Name))
                {
                    var task = idMgr.Roles.CreateRoleAsync(role);
                    task.Wait();
                }
            }
            foreach (var tdUser in users)
            {
                if (!context.Users.Any(u => u.UserName == tdUser.UserName))
                {
                    idMgr.Users.CreateLocalUser(tdUser, "password");
                }
            }
            foreach (var userRole in userRoles)
            {
                var rlTask = idMgr.Roles.FindRoleByNameAsync(userRole.Item2);
                rlTask.Wait();
                var rl = rlTask.Result;
                var usrTask = idMgr.Store.Users.FindByNameAsync(userRole.Item1, CancellationToken.None);
                usrTask.Wait();
                var usr = usrTask.Result;
                var inRoleTask = idMgr.Roles.IsUserInRoleAsync(usr.Id, rl.Id, CancellationToken.None);
                inRoleTask.Wait();
                var inRole = inRoleTask.Result;
                if (!inRole)
                {
                    var rrTask = idMgr.Roles.AddUserToRoleAsync(usr.Id, rl.Id);
                    rrTask.Wait();
                }
            }



        }
    }
}
