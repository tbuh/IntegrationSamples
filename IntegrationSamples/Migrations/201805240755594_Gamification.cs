namespace IntegrationSamples.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgentScores", "Name", c => c.String());
            AddColumn("dbo.ChatUsers", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatUsers", "Score");
            DropColumn("dbo.AgentScores", "Name");
        }
    }
}
