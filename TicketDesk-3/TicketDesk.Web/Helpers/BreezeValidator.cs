using Breeze.ContextProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Validators;
using TicketDesk.Domain.Model;
using Breeze.ContextProvider.EF6;
using Microsoft.AspNet.Identity;

namespace TicketDesk.Web.Helpers
{
    public class BreezeValidator : IBreezeValidator
    {
        UserManager<UserProfile> UserManager { get; set; }

        public BreezeValidator(UserManager<UserProfile> usermanager)
        {
            this.UserManager = usermanager;
        }

        public bool BeforeSaveEntity(EntityInfo entityInfo)
        {

            

            return true;
        }

        public Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {

            // Add custom logic here in order to save entities

            List<EntityInfo> userprofiles;

            // - In order to save and manage accounts you need to use the AccountController and not Breeze

            if (saveMap.TryGetValue(typeof(UserProfile), out userprofiles))
            {
                var errors = userprofiles.Select(oi =>
                {
                    return new EFEntityError(oi, "Save Failed", "Cannot save Users using the Breeze api", "UserProfileId");
                });
                throw new EntityErrorsException(errors);
            }

            return saveMap;
        }
    }
}