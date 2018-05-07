namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "JobId", c => c.Guid());
            AddColumn("dbo.Questions", "ArticleId", c => c.Guid());
            DropColumn("dbo.Questions", "StoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "StoryId", c => c.Guid());
            DropColumn("dbo.Questions", "ArticleId");
            DropColumn("dbo.Questions", "JobId");
        }
    }
}
