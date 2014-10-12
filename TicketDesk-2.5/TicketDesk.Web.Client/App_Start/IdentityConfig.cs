using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class TicketDeskUserManager : UserManager<TicketDeskUser>
    {
        public TicketDeskUserManager(IUserStore<TicketDeskUser> store)
            : base(store)
        {
        }

        //public static TicketDeskUserManager Create(IdentityFactoryOptions<TicketDeskUserManager> options, IOwinContext context) 
        //{
        //    var manager = new TicketDeskUserManager(new UserStore<TicketDeskUser>(context.Get<TicketDeskIdentityContext>()));
        //    // Configure validation logic for usernames
        //    manager.UserValidator = new UserValidator<TicketDeskUser>(manager)
        //    {
        //        AllowOnlyAlphanumericUserNames = false,
        //        RequireUniqueEmail = true
        //    };

        //    // Configure validation logic for passwords
        //    manager.PasswordValidator = new PasswordValidator
        //    {
        //        RequiredLength = 5,
        //        RequireNonLetterOrDigit = false,
        //        RequireDigit = false,
        //        RequireLowercase = false,
        //        RequireUppercase = false,
        //    };

        //    // Configure user lockout defaults
        //    manager.UserLockoutEnabledByDefault = true;
        //    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        //    // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        //    // You can write your own provider and plug it in here.
        //    manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<TicketDeskUser>
        //    {
        //        MessageFormat = "Your security code is {0}"
        //    });
        //    manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<TicketDeskUser>
        //    {
        //        Subject = "Security Code",
        //        BodyFormat = "Your security code is {0}"
        //    });
        //    manager.EmailService = new EmailService();
        //    manager.SmsService = new SmsService();
        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider =
        //            new DataProtectorTokenProvider<TicketDeskUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}
    }
    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class TicketDeskRoleManager : RoleManager<IdentityRole>
    {
        public TicketDeskRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }
        ////TODO: Why are options passed here, and what are they for? This is from the Microsoft.AspNet.Identity.Samples package
        //public static TicketDeskRoleManager Create(IdentityFactoryOptions<TicketDeskRoleManager> options, IOwinContext context)
        //{
        //    return new TicketDeskRoleManager(new RoleStore<IdentityRole>(context.Get<TicketDeskIdentityContext>()));
        //}

    }
    // Configure the application sign-in manager which is used in this application.
    public class TicketDeskSignInManager : SignInManager<TicketDeskUser, string>
    {
        public TicketDeskSignInManager(TicketDeskUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(TicketDeskUser user)
        {
            return user.GenerateUserIdentityAsync((TicketDeskUserManager)UserManager);
        }

        public static TicketDeskSignInManager Create(IdentityFactoryOptions<TicketDeskSignInManager> options, IOwinContext context)
        {
            return new TicketDeskSignInManager(context.GetUserManager<TicketDeskUserManager>(), context.Authentication);
        }
    }
}
