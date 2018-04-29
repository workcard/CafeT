namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "GDriveId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "GDriveId");
        }
    }
}
