using IntegrationSamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Infrastructure
{
    public class GamificationService
    {
        private ISDbContext iSDbContext;
        public GamificationService()
        {
            iSDbContext = new ISDbContext();
        }

        public void AddPoint(string agentId)
        {
            var user = iSDbContext.GetChatUser(agentId);
            user.Score++;            
            iSDbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            iSDbContext.SaveChanges();
        }

        public List<ChatUser> GetTop5Scores()
        {
            return iSDbContext.ChatUsers.OrderByDescending(u => u.Score).Take(5).ToList();
        }             
    }
}