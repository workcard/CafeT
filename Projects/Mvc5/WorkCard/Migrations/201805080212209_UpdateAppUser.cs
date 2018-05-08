namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAppUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "LastUpatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastUpatedDate", c => c.DateTime(nullable: false));
        }
    }
}
