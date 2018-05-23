using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class ISDbContext : DbContext
    {
        public ISDbContext()
            : base("SQLSERVER")
        {
        }

        static ISDbContext()
        {
            // Don't run migrations, ever!
            Database.SetInitializer(new CreateDatabaseIfNotExists<ISDbContext>());
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        //public DbSet<ChatClient> ChatClients { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<AgentScore> AgentScores { get; set; }

        //public List<ChatMessage> GetData(DateTime afterDate, string userId)
        //{
        //    return ChatMessages.Where(a => a.UserId == userId && a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();            
        //}

        public ChatUser GetChatUser(string agentId)
        {
            var user = ChatUsers.SingleOrDefault(u => u.Id == agentId);
            return user;
        }
    }
}