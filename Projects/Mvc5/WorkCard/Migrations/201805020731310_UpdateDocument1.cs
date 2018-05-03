namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDocument1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ArticleId", c => c.Guid());
            AddColumn("dbo.Documents", "IssueId", c => c.Guid());
            AddColumn("dbo.Documents", "JobId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "JobId");
            DropColumn("dbo.Documents", "IssueId");
            DropColumn("dbo.Documents", "ArticleId");
        }
    }
}
