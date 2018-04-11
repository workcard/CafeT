namespace Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateDb1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "WorkIssue_Id", "dbo.WorkIssues");
            DropIndex("dbo.Questions", new[] { "WorkIssue_Id" });
            DropColumn("dbo.Questions", "WorkIssue_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "WorkIssue_Id", c => c.Guid());
            CreateIndex("dbo.Questions", "WorkIssue_Id");
            AddForeignKey("dbo.Questions", "WorkIssue_Id", "dbo.WorkIssues", "Id");
        }
    }
}
