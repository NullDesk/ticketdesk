using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Providers
{
    /// <summary>
    /// oAuth authorization server
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<UserManager<UserProfile>> _userManagerFactory;

        /// <summary>
        /// ctor
        /// </summary>		
        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<UserProfile>> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }	

        /// <summary>
        /// oAuth Resource Password Login Flow
		/// 1. Checks the password with the Identity API
		/// 2. Create a user identity for the bearer token
		/// 3. Create a user identity for the cookie
		/// 4. Calls the context.Validated(ticket) to tell the oAuth2 server to protect the ticket as an access token and send it out in JSON payload
		/// 5. Signs the cookie identity so it can send the authentication cookie
        /// </summary>
        /// <param name="context">The authorization context</param>
		/// <returns>Task</returns>		
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserManager<UserProfile> userManager = _userManagerFactory())
            {
                UserProfile user = await userManager.FindAsync(context.UserName, context.Password);
                
                if (user == null)
                {
                    context.SetError("invalid_grant", "Invalid user or password");
                    return;
                }

                var oAuthIdentity = await userManager.CreateIdentityAsync(user,
                    context.Options.AuthenticationType);
                var cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);

                var justCreatedIdentity = await userManager.FindByNameAsync(user.UserName);
                var roles = await userManager.GetRolesAsync(justCreatedIdentity.Id);

                var properties = CreateProperties(user.UserName, roles.ToArray());
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                                
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        /// <summary>
        /// Add parameters to the response
        /// </summary>
        /// <param name="context">Endpoint context</param>
        /// <returns>Task</returns>		
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Password resource owner credentials don´t provide a client identifier
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Validate the redirect uri
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Create the authentication properties
		/// Create the requires properties that would be converted into Claims
        /// </summary>
        /// <param name="userName">The user name</param>
		/// <param name="roles">The user roles</param>
        /// <returns>The properties</returns>
        public static AuthenticationProperties CreateProperties(string userName,  string[] roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "roles", String.Join("," , roles) }
            };
            return new AuthenticationProperties(data);
        }
    }
}