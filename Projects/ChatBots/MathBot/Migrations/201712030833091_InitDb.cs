namespace MathBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(),
                        QuestionId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BotCommands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        CsharpCode = c.String(),
                        IsUnknow = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CodeFunctions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        FullBody = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        MobilePhone = c.String(),
                        HomeAddress = c.String(),
                        HomePhone = c.String(),
                        OfficeName = c.String(),
                        OfficePhone = c.String(),
                        About = c.String(),
                        ProjectId = c.Guid(),
                        IssueId = c.Guid(),
                        UserName = c.String(),
                        IsRegistered = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WordDictionaries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        English = c.String(),
                        GoogleVn = c.String(),
                        MicrosoftVn = c.String(),
                        IsRemembered = c.Boolean(nullable: false),
                        CountViews = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        CorrectAnswer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.CorrectAnswer_Id)
                .Index(t => t.CorrectAnswer_Id);
            
            CreateTable(
                "dbo.Urls",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Domain = c.String(),
                        Address = c.String(),
                        Title = c.String(),
                        HtmlContent = c.String(),
                        LastestCheck = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisplayName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        MobilePhone = c.String(),
                        About = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "CorrectAnswer_Id", "dbo.Answers");
            DropIndex("dbo.Questions", new[] { "CorrectAnswer_Id" });
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Urls");
            DropTable("dbo.Questions");
            DropTable("dbo.WordDictionaries");
            DropTable("dbo.Contacts");
            DropTable("dbo.CodeFunctions");
            DropTable("dbo.BotCommands");
            DropTable("dbo.Answers");
        }
    }
}
