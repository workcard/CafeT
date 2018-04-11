namespace TiTiBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromId = c.String(),
                        FromName = c.String(),
                        RecipientId = c.String(),
                        RecipientName = c.String(),
                        TextFormat = c.String(),
                        TopicName = c.String(),
                        HistoryDisclosed = c.Boolean(nullable: false),
                        Local = c.String(),
                        Text = c.String(),
                        Summary = c.String(),
                        ChannelId = c.String(),
                        ServiceUrl = c.String(),
                        ReplyToId = c.String(),
                        Action = c.String(),
                        Type = c.String(),
                        Timestamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ConversationId = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Activities");
        }
    }
}
