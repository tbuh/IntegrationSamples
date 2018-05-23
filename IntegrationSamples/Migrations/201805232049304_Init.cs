namespace IntegrationSamples.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentId = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsFromPlatform = c.Boolean(nullable: false),
                        Platform = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        ChatRoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatRooms", t => t.ChatRoomId, cascadeDelete: true)
                .Index(t => t.ChatRoomId);
            
            CreateTable(
                "dbo.ChatRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        AgentId = c.String(),
                        ConnectionId = c.String(),
                        IsClosed = c.Boolean(nullable: false),
                        OpenDate = c.DateTime(nullable: false),
                        LastAgentReply = c.DateTime(),
                        LastUserReply = c.DateTime(),
                        CloseDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "ChatRoomId", "dbo.ChatRooms");
            DropIndex("dbo.ChatMessages", new[] { "ChatRoomId" });
            DropTable("dbo.ChatUsers");
            DropTable("dbo.ChatRooms");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.AgentScores");
        }
    }
}
