namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssueAndProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "ParentId", c => c.Guid());
            CreateIndex("dbo.Contacts", "ProjectId");
            AddForeignKey("dbo.Contacts", "ProjectId", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Contacts", new[] { "ProjectId" });
            DropColumn("dbo.WorkIssues", "ParentId");
        }
    }
}
