namespace IntegrationSamples.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatTables : DbMigration
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
                "dbo.ChatClients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectionId = c.String(),
                        ChatUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatUsers", t => t.ChatUser_Id)
                .Index(t => t.ChatUser_Id);
            
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentId = c.String(),
                        IsOnline = c.Boolean(nullable: false),
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
                        IsClosed = c.Boolean(nullable: false),
                        OpenDate = c.DateTime(nullable: false),
                        CloseDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Messages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        UserId = c.String(),
                        Platform = c.String(),
                        AddedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ChatMessages", "ChatRoomId", "dbo.ChatRooms");
            DropForeignKey("dbo.ChatClients", "ChatUser_Id", "dbo.ChatUsers");
            DropIndex("dbo.ChatMessages", new[] { "ChatRoomId" });
            DropIndex("dbo.ChatClients", new[] { "ChatUser_Id" });
            DropTable("dbo.ChatRooms");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.ChatUsers");
            DropTable("dbo.ChatClients");
            DropTable("dbo.AgentScores");
        }
    }
}
