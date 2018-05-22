namespace IntegrationSamples.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatRooms", "LastAgentReply", c => c.DateTime());
            AddColumn("dbo.ChatRooms", "LastUserReply", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatRooms", "LastUserReply");
            DropColumn("dbo.ChatRooms", "LastAgentReply");
        }
    }
}
