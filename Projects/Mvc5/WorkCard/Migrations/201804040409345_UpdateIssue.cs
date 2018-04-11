namespace Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "IsVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.IssueViews", "IsVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IssueViews", "IsVerified");
            DropColumn("dbo.WorkIssues", "IsVerified");
        }
    }
}
