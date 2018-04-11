namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnswerReviewModels", "Viewers", c => c.String());
            AddColumn("dbo.AnswerModels", "Viewers", c => c.String());
            AddColumn("dbo.ArticleCategories", "Viewers", c => c.String());
            AddColumn("dbo.Articles", "Viewers", c => c.String());
            AddColumn("dbo.ImageObjects", "Viewers", c => c.String());
            AddColumn("dbo.CommentModels", "Viewers", c => c.String());
            AddColumn("dbo.CompanyModels", "Viewers", c => c.String());
            AddColumn("dbo.ComplainModels", "Viewers", c => c.String());
            AddColumn("dbo.Courses", "Viewers", c => c.String());
            AddColumn("dbo.CrawlerModels", "Viewers", c => c.String());
            AddColumn("dbo.CuriculumModels", "Viewers", c => c.String());
            AddColumn("dbo.EventModels", "Viewers", c => c.String());
            AddColumn("dbo.ExamModels", "Viewers", c => c.String());
            AddColumn("dbo.FileModels", "Viewers", c => c.String());
            AddColumn("dbo.InterviewModels", "Viewers", c => c.String());
            AddColumn("dbo.IssueModels", "Viewers", c => c.String());
            AddColumn("dbo.JobModels", "Viewers", c => c.String());
            AddColumn("dbo.MenuItemModels", "Viewers", c => c.String());
            AddColumn("dbo.MenuModels", "Viewers", c => c.String());
            AddColumn("dbo.ApplicationMessages", "Viewers", c => c.String());
            AddColumn("dbo.ProductModels", "Viewers", c => c.String());
            AddColumn("dbo.ProjectModels", "Viewers", c => c.String());
            AddColumn("dbo.QuestionModels", "Viewers", c => c.String());
            AddColumn("dbo.ApplicationSettings", "Viewers", c => c.String());
            AddColumn("dbo.UrlModels", "Host", c => c.String());
            AddColumn("dbo.UrlModels", "Domain", c => c.String());
            AddColumn("dbo.UrlModels", "HtmlContent", c => c.String());
            AddColumn("dbo.UrlModels", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.UrlModels", "LastRead", c => c.DateTime());
            AddColumn("dbo.UrlModels", "ArticleId", c => c.Guid());
            AddColumn("dbo.UrlModels", "QuestionId", c => c.Guid());
            AddColumn("dbo.UrlModels", "InterviewId", c => c.Guid());
            AddColumn("dbo.UrlModels", "AnswerId", c => c.Guid());
            AddColumn("dbo.UrlModels", "CourseId", c => c.Guid());
            AddColumn("dbo.UrlModels", "Viewers", c => c.String());
            AddColumn("dbo.WebPageModels", "Viewers", c => c.String());
            AddColumn("dbo.WorkModels", "Viewers", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkModels", "Viewers");
            DropColumn("dbo.WebPageModels", "Viewers");
            DropColumn("dbo.UrlModels", "Viewers");
            DropColumn("dbo.UrlModels", "CourseId");
            DropColumn("dbo.UrlModels", "AnswerId");
            DropColumn("dbo.UrlModels", "InterviewId");
            DropColumn("dbo.UrlModels", "QuestionId");
            DropColumn("dbo.UrlModels", "ArticleId");
            DropColumn("dbo.UrlModels", "LastRead");
            DropColumn("dbo.UrlModels", "Type");
            DropColumn("dbo.UrlModels", "HtmlContent");
            DropColumn("dbo.UrlModels", "Domain");
            DropColumn("dbo.UrlModels", "Host");
            DropColumn("dbo.ApplicationSettings", "Viewers");
            DropColumn("dbo.QuestionModels", "Viewers");
            DropColumn("dbo.ProjectModels", "Viewers");
            DropColumn("dbo.ProductModels", "Viewers");
            DropColumn("dbo.ApplicationMessages", "Viewers");
            DropColumn("dbo.MenuModels", "Viewers");
            DropColumn("dbo.MenuItemModels", "Viewers");
            DropColumn("dbo.JobModels", "Viewers");
            DropColumn("dbo.IssueModels", "Viewers");
            DropColumn("dbo.InterviewModels", "Viewers");
            DropColumn("dbo.FileModels", "Viewers");
            DropColumn("dbo.ExamModels", "Viewers");
            DropColumn("dbo.EventModels", "Viewers");
            DropColumn("dbo.CuriculumModels", "Viewers");
            DropColumn("dbo.CrawlerModels", "Viewers");
            DropColumn("dbo.Courses", "Viewers");
            DropColumn("dbo.ComplainModels", "Viewers");
            DropColumn("dbo.CompanyModels", "Viewers");
            DropColumn("dbo.CommentModels", "Viewers");
            DropColumn("dbo.ImageObjects", "Viewers");
            DropColumn("dbo.Articles", "Viewers");
            DropColumn("dbo.ArticleCategories", "Viewers");
            DropColumn("dbo.AnswerModels", "Viewers");
            DropColumn("dbo.AnswerReviewModels", "Viewers");
        }
    }
}
