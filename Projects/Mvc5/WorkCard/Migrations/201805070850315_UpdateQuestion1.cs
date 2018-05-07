namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateQuestion1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "JobId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "JobId");
        }
    }
}
