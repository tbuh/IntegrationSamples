using IntegrationSamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Infrastructure
{
    public class ChatUserService
    {
        private ISDbContext iSDbContext;
        public ChatUserService()
        {
            iSDbContext = new ISDbContext();
        }

        public void Create(string id, string name)
        {
            var dbUser = iSDbContext.GetChatUser(id);
            if (dbUser == null)
            {
                var user = new ChatUser()
                {
                    Id = id,
                    Name = name
                };
                iSDbContext.Entry(user).State = System.Data.Entity.EntityState.Added;
                iSDbContext.SaveChanges();
            }
        }
    }
}