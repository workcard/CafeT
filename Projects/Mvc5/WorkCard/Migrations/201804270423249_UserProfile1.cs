namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfile1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Profile_UserId", "dbo.UserProfiles");
            DropIndex("dbo.AspNetUsers", new[] { "Profile_UserId" });
            AddColumn("dbo.AspNetUsers", "About", c => c.String());
            DropColumn("dbo.AspNetUsers", "Profile_UserId");
            DropTable("dbo.UserProfiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        LastActivityDate = c.DateTimeOffset(nullable: false, precision: 7),
                        About = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "Profile_UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "About");
            CreateIndex("dbo.AspNetUsers", "Profile_UserId");
            AddForeignKey("dbo.AspNetUsers", "Profile_UserId", "dbo.UserProfiles", "UserId");
        }
    }
}
