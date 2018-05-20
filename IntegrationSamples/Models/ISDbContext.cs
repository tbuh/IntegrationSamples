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

        public DbSet<Message> Messages { get; set; }
    }
}