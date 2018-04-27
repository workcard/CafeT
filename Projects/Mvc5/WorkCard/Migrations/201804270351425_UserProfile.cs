namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfile : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.AspNetUsers", "Profile_UserId");
            AddForeignKey("dbo.AspNetUsers", "Profile_UserId", "dbo.UserProfiles", "UserId");
            DropColumn("dbo.AspNetUsers", "About");
            DropColumn("dbo.AspNetUsers", "BugNetUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BugNetUserId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "About", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "Profile_UserId", "dbo.UserProfiles");
            DropIndex("dbo.AspNetUsers", new[] { "Profile_UserId" });
            DropColumn("dbo.AspNetUsers", "Profile_UserId");
            DropTable("dbo.UserProfiles");
        }
    }
}
