namespace MathBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UrlModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Url = c.String(),
                        Host = c.String(),
                        Domain = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        HtmlContent = c.String(),
                        Type = c.Int(nullable: false),
                        LastRead = c.DateTime(),
                        ArticleId = c.Guid(),
                        QuestionId = c.Guid(),
                        InterviewId = c.Guid(),
                        AnswerId = c.Guid(),
                        CourseId = c.Guid(),
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
            
            DropTable("dbo.Urls");
        }
        
        public override void Down()
        {
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
            
            DropTable("dbo.UrlModels");
        }
    }
}
