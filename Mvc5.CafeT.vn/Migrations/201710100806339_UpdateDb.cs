namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnswerReviewModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.AnswerModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ArticleCategories", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ImageObjects", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.CommentModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.CompanyModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ComplainModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.CrawlerModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.CuriculumModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ExamModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.FileModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.InterviewModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.IssueModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuItemModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationMessages", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuestionModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationSettings", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.UrlModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.WebPageModels", "IsChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.WorkModels", "IsChanged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkModels", "IsChanged");
            DropColumn("dbo.WebPageModels", "IsChanged");
            DropColumn("dbo.UrlModels", "IsChanged");
            DropColumn("dbo.ApplicationSettings", "IsChanged");
            DropColumn("dbo.QuestionModels", "IsChanged");
            DropColumn("dbo.ProjectModels", "IsChanged");
            DropColumn("dbo.ProductModels", "IsChanged");
            DropColumn("dbo.ApplicationMessages", "IsChanged");
            DropColumn("dbo.MenuModels", "IsChanged");
            DropColumn("dbo.MenuItemModels", "IsChanged");
            DropColumn("dbo.JobModels", "IsChanged");
            DropColumn("dbo.IssueModels", "IsChanged");
            DropColumn("dbo.InterviewModels", "IsChanged");
            DropColumn("dbo.FileModels", "IsChanged");
            DropColumn("dbo.ExamModels", "IsChanged");
            DropColumn("dbo.EventModels", "IsChanged");
            DropColumn("dbo.CuriculumModels", "IsChanged");
            DropColumn("dbo.CrawlerModels", "IsChanged");
            DropColumn("dbo.Courses", "IsChanged");
            DropColumn("dbo.ComplainModels", "IsChanged");
            DropColumn("dbo.CompanyModels", "IsChanged");
            DropColumn("dbo.CommentModels", "IsChanged");
            DropColumn("dbo.ImageObjects", "IsChanged");
            DropColumn("dbo.Articles", "IsChanged");
            DropColumn("dbo.ArticleCategories", "IsChanged");
            DropColumn("dbo.AnswerModels", "IsChanged");
            DropColumn("dbo.AnswerReviewModels", "IsChanged");
        }
    }
}
