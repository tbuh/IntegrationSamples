using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IntegrationSamples.Infrastructure
{
    public class ApplicationUser : IUser<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        internal Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store) : base(store)
        {
        }

        public override Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            return base.AddLoginAsync(userId, login);
        }

    }
}