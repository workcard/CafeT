namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfile2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastUpatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastUpatedDate");
            DropColumn("dbo.AspNetUsers", "CreatedDate");
        }
    }
}
