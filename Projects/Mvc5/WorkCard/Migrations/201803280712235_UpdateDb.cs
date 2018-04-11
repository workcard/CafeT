namespace Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "Salary", c => c.Double(nullable: false));
            AlterColumn("dbo.WorkIssues", "IssueEstimation", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkIssues", "IssueEstimation", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.WorkIssues", "Salary");
        }
    }
}
