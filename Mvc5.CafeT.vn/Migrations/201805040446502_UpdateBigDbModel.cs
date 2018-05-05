namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBigDbModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArticleModels", "Avatar_Id", "dbo.ImageObjects");
            DropForeignKey("dbo.Courses", "Image_Id", "dbo.ImageObjects");
            DropForeignKey("dbo.Articles", "Learner_Id", "dbo.Learners");
            DropForeignKey("dbo.CourseLearners", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.CourseLearners", "Learner_Id", "dbo.Learners");
            DropForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.Trainers");
            DropForeignKey("dbo.TrainerCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.FileModels", "Image_Id", "dbo.ImageObjects");
            DropForeignKey("dbo.JobModels", "CompanyId", "dbo.CompanyModels");
            DropForeignKey("dbo.ProjectModels", "Image_Id", "dbo.ImageObjects");
            DropIndex("dbo.ArticleModels", new[] { "Avatar_Id" });
            DropIndex("dbo.Courses", new[] { "Image_Id" });
            DropIndex("dbo.Articles", new[] { "Learner_Id" });
            DropIndex("dbo.FileModels", new[] { "Image_Id" });
            DropIndex("dbo.JobModels", new[] { "CompanyId" });
            DropIndex("dbo.ProjectModels", new[] { "Image_Id" });
            DropIndex("dbo.CourseLearners", new[] { "Course_Id" });
            DropIndex("dbo.CourseLearners", new[] { "Learner_Id" });
            DropIndex("dbo.TrainerCourses", new[] { "Trainer_Id" });
            DropIndex("dbo.TrainerCourses", new[] { "Course_Id" });
            AddColumn("dbo.AnswerReviewModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.AnswerReviewModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.AnswerModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.AnswerModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.ArticleCategories", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ArticleCategories", "UpdatedBy", c => c.String());
            AddColumn("dbo.ArticleModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ArticleModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.CommentModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.CommentModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.FileModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.FileModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.ProjectModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ProjectModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.QuestionModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.QuestionModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.UrlModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.UrlModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.WordModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.WordModels", "UpdatedBy", c => c.String());
            AddColumn("dbo.WorkModels", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.WorkModels", "UpdatedBy", c => c.String());
            AlterColumn("dbo.AnswerReviewModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.AnswerModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ArticleCategories", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ArticleModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.CommentModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.FileModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.ProjectModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.QuestionModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.UrlModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.WordModels", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkModels", "CreatedDate", c => c.DateTime());
            DropColumn("dbo.AnswerReviewModels", "LastUpdatedDate");
            DropColumn("dbo.AnswerReviewModels", "LastUpdatedBy");
            DropColumn("dbo.AnswerReviewModels", "CountViews");
            DropColumn("dbo.AnswerReviewModels", "Viewers");
            DropColumn("dbo.AnswerReviewModels", "LastViewAt");
            DropColumn("dbo.AnswerReviewModels", "LastViewBy");
            DropColumn("dbo.AnswerModels", "LastUpdatedDate");
            DropColumn("dbo.AnswerModels", "LastUpdatedBy");
            DropColumn("dbo.AnswerModels", "CountViews");
            DropColumn("dbo.AnswerModels", "Viewers");
            DropColumn("dbo.AnswerModels", "LastViewAt");
            DropColumn("dbo.AnswerModels", "LastViewBy");
            DropColumn("dbo.ArticleCategories", "LastUpdatedDate");
            DropColumn("dbo.ArticleCategories", "LastUpdatedBy");
            DropColumn("dbo.ArticleCategories", "CountViews");
            DropColumn("dbo.ArticleCategories", "Viewers");
            DropColumn("dbo.ArticleCategories", "LastViewAt");
            DropColumn("dbo.ArticleCategories", "LastViewBy");
            DropColumn("dbo.ArticleModels", "LastUpdatedDate");
            DropColumn("dbo.ArticleModels", "LastUpdatedBy");
            DropColumn("dbo.ArticleModels", "Viewers");
            DropColumn("dbo.ArticleModels", "LastViewAt");
            DropColumn("dbo.ArticleModels", "LastViewBy");
            DropColumn("dbo.ArticleModels", "Avatar_Id");
            DropColumn("dbo.CommentModels", "LastUpdatedDate");
            DropColumn("dbo.CommentModels", "LastUpdatedBy");
            DropColumn("dbo.CommentModels", "Viewers");
            DropColumn("dbo.CommentModels", "LastViewAt");
            DropColumn("dbo.CommentModels", "LastViewBy");
            DropColumn("dbo.FileModels", "LastUpdatedDate");
            DropColumn("dbo.FileModels", "LastUpdatedBy");
            DropColumn("dbo.FileModels", "Viewers");
            DropColumn("dbo.FileModels", "LastViewAt");
            DropColumn("dbo.FileModels", "LastViewBy");
            DropColumn("dbo.FileModels", "Image_Id");
            DropColumn("dbo.ProjectModels", "LastUpdatedDate");
            DropColumn("dbo.ProjectModels", "LastUpdatedBy");
            DropColumn("dbo.ProjectModels", "CountViews");
            DropColumn("dbo.ProjectModels", "Viewers");
            DropColumn("dbo.ProjectModels", "LastViewAt");
            DropColumn("dbo.ProjectModels", "LastViewBy");
            DropColumn("dbo.ProjectModels", "Image_Id");
            DropColumn("dbo.QuestionModels", "LastUpdatedDate");
            DropColumn("dbo.QuestionModels", "LastUpdatedBy");
            DropColumn("dbo.QuestionModels", "CountViews");
            DropColumn("dbo.QuestionModels", "Viewers");
            DropColumn("dbo.QuestionModels", "LastViewAt");
            DropColumn("dbo.QuestionModels", "LastViewBy");
            DropColumn("dbo.UrlModels", "LastUpdatedDate");
            DropColumn("dbo.UrlModels", "LastUpdatedBy");
            DropColumn("dbo.UrlModels", "CountViews");
            DropColumn("dbo.UrlModels", "Viewers");
            DropColumn("dbo.UrlModels", "LastViewAt");
            DropColumn("dbo.UrlModels", "LastViewBy");
            DropColumn("dbo.WordModels", "LastUpdatedDate");
            DropColumn("dbo.WordModels", "LastUpdatedBy");
            DropColumn("dbo.WordModels", "CountViews");
            DropColumn("dbo.WordModels", "Viewers");
            DropColumn("dbo.WordModels", "LastViewAt");
            DropColumn("dbo.WordModels", "LastViewBy");
            DropColumn("dbo.WorkModels", "LastUpdatedDate");
            DropColumn("dbo.WorkModels", "LastUpdatedBy");
            DropColumn("dbo.WorkModels", "CountViews");
            DropColumn("dbo.WorkModels", "Viewers");
            DropColumn("dbo.WorkModels", "LastViewAt");
            DropColumn("dbo.WorkModels", "LastViewBy");
            DropTable("dbo.ImageObjects");
            DropTable("dbo.CompanyModels");
            DropTable("dbo.ComplainModels");
            DropTable("dbo.Courses");
            DropTable("dbo.Learners");
            DropTable("dbo.Articles");
            DropTable("dbo.Trainers");
            DropTable("dbo.CuriculumModels");
            DropTable("dbo.EventModels");
            DropTable("dbo.ExamModels");
            DropTable("dbo.InterviewModels");
            DropTable("dbo.IssueModels");
            DropTable("dbo.JobModels");
            DropTable("dbo.ProductModels");
            DropTable("dbo.WebPageModels");
            DropTable("dbo.CourseLearners");
            DropTable("dbo.TrainerCourses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrainerCourses",
                c => new
                    {
                        Trainer_Id = c.Guid(nullable: false),
                        Course_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainer_Id, t.Course_Id });
            
            CreateTable(
                "dbo.CourseLearners",
                c => new
                    {
                        Course_Id = c.Guid(nullable: false),
                        Learner_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.Learner_Id });
            
            CreateTable(
                "dbo.WebPageModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Url = c.String(),
                        Title = c.String(),
                        Meta = c.String(),
                        HtmlContent = c.String(),
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
                "dbo.ProductModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
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
                "dbo.JobModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AvatarPath = c.String(),
                        CompanyId = c.Guid(),
                        Title = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        Tags = c.String(),
                        VerifiedBy = c.String(),
                        VerifiedDate = c.DateTime(),
                        IsPublished = c.Boolean(),
                        IsDrafted = c.Boolean(),
                        IsPublic = c.Boolean(),
                        IsProtect = c.Boolean(),
                        IsPrivate = c.Boolean(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        Quantity = c.Int(),
                        SalaryInMoth = c.Decimal(precision: 18, scale: 2),
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
                "dbo.IssueModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Clock_AutoReset = c.Boolean(nullable: false),
                        Clock_Enabled = c.Boolean(nullable: false),
                        Clock_Interval = c.Double(nullable: false),
                        IsDaily = c.Boolean(),
                        IsWeekly = c.Boolean(),
                        IsMonthly = c.Boolean(),
                        IsYearly = c.Boolean(),
                        ReminderBefore = c.Int(),
                        MaxCountReminder = c.Int(),
                        ProjectId = c.Guid(),
                        Title = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        ExcuteBy = c.String(),
                        VerifyBy = c.String(),
                        IsFinished = c.Boolean(nullable: false),
                        IsDoing = c.Boolean(nullable: false),
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
                "dbo.InterviewModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        InterviewDate = c.DateTime(nullable: false),
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
                "dbo.ExamModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Start = c.DateTime(),
                        End = c.DateTime(),
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
                "dbo.EventModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
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
                "dbo.CuriculumModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Tags = c.String(),
                        Companies = c.String(),
                        Skills = c.String(),
                        Content = c.String(),
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
                "dbo.Trainers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Summary = c.String(),
                        Content = c.String(nullable: false),
                        Tags = c.String(),
                        Status = c.Int(nullable: false),
                        Scope = c.Int(nullable: false),
                        AvatarPath = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        Learner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Learners",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        Tags = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Image_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplainModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 250),
                        Content = c.String(),
                        AnswerId = c.Guid(),
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
                "dbo.CompanyModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumberOfStaffs = c.Int(),
                        Name = c.String(),
                        Introduction = c.String(),
                        Website = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
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
                "dbo.ImageObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        Description = c.String(),
                        FullPath = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        ArticleId = c.Guid(),
                        FileId = c.Guid(),
                        CourseId = c.Guid(),
                        ProjectId = c.Guid(),
                        Width = c.Int(),
                        Height = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WorkModels", "LastViewBy", c => c.String());
            AddColumn("dbo.WorkModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.WorkModels", "Viewers", c => c.String());
            AddColumn("dbo.WorkModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.WorkModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.WorkModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.WordModels", "LastViewBy", c => c.String());
            AddColumn("dbo.WordModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.WordModels", "Viewers", c => c.String());
            AddColumn("dbo.WordModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.WordModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.WordModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.UrlModels", "LastViewBy", c => c.String());
            AddColumn("dbo.UrlModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.UrlModels", "Viewers", c => c.String());
            AddColumn("dbo.UrlModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.UrlModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.UrlModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.QuestionModels", "LastViewBy", c => c.String());
            AddColumn("dbo.QuestionModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.QuestionModels", "Viewers", c => c.String());
            AddColumn("dbo.QuestionModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.QuestionModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.ProjectModels", "Image_Id", c => c.Guid());
            AddColumn("dbo.ProjectModels", "LastViewBy", c => c.String());
            AddColumn("dbo.ProjectModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.ProjectModels", "Viewers", c => c.String());
            AddColumn("dbo.ProjectModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.ProjectModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.FileModels", "Image_Id", c => c.Guid());
            AddColumn("dbo.FileModels", "LastViewBy", c => c.String());
            AddColumn("dbo.FileModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.FileModels", "Viewers", c => c.String());
            AddColumn("dbo.FileModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.FileModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.CommentModels", "LastViewBy", c => c.String());
            AddColumn("dbo.CommentModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.CommentModels", "Viewers", c => c.String());
            AddColumn("dbo.CommentModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.CommentModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.ArticleModels", "Avatar_Id", c => c.Guid());
            AddColumn("dbo.ArticleModels", "LastViewBy", c => c.String());
            AddColumn("dbo.ArticleModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.ArticleModels", "Viewers", c => c.String());
            AddColumn("dbo.ArticleModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.ArticleModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.ArticleCategories", "LastViewBy", c => c.String());
            AddColumn("dbo.ArticleCategories", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.ArticleCategories", "Viewers", c => c.String());
            AddColumn("dbo.ArticleCategories", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.ArticleCategories", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.ArticleCategories", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.AnswerModels", "LastViewBy", c => c.String());
            AddColumn("dbo.AnswerModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.AnswerModels", "Viewers", c => c.String());
            AddColumn("dbo.AnswerModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.AnswerModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.AnswerModels", "LastUpdatedDate", c => c.DateTime());
            AddColumn("dbo.AnswerReviewModels", "LastViewBy", c => c.String());
            AddColumn("dbo.AnswerReviewModels", "LastViewAt", c => c.DateTime());
            AddColumn("dbo.AnswerReviewModels", "Viewers", c => c.String());
            AddColumn("dbo.AnswerReviewModels", "CountViews", c => c.Int(nullable: false));
            AddColumn("dbo.AnswerReviewModels", "LastUpdatedBy", c => c.String());
            AddColumn("dbo.AnswerReviewModels", "LastUpdatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WordModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UrlModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QuestionModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProjectModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FileModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CommentModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ArticleModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ArticleCategories", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AnswerModels", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AnswerReviewModels", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WorkModels", "UpdatedBy");
            DropColumn("dbo.WorkModels", "UpdatedDate");
            DropColumn("dbo.WordModels", "UpdatedBy");
            DropColumn("dbo.WordModels", "UpdatedDate");
            DropColumn("dbo.UrlModels", "UpdatedBy");
            DropColumn("dbo.UrlModels", "UpdatedDate");
            DropColumn("dbo.QuestionModels", "UpdatedBy");
            DropColumn("dbo.QuestionModels", "UpdatedDate");
            DropColumn("dbo.ProjectModels", "UpdatedBy");
            DropColumn("dbo.ProjectModels", "UpdatedDate");
            DropColumn("dbo.FileModels", "UpdatedBy");
            DropColumn("dbo.FileModels", "UpdatedDate");
            DropColumn("dbo.CommentModels", "UpdatedBy");
            DropColumn("dbo.CommentModels", "UpdatedDate");
            DropColumn("dbo.ArticleModels", "UpdatedBy");
            DropColumn("dbo.ArticleModels", "UpdatedDate");
            DropColumn("dbo.ArticleCategories", "UpdatedBy");
            DropColumn("dbo.ArticleCategories", "UpdatedDate");
            DropColumn("dbo.AnswerModels", "UpdatedBy");
            DropColumn("dbo.AnswerModels", "UpdatedDate");
            DropColumn("dbo.AnswerReviewModels", "UpdatedBy");
            DropColumn("dbo.AnswerReviewModels", "UpdatedDate");
            CreateIndex("dbo.TrainerCourses", "Course_Id");
            CreateIndex("dbo.TrainerCourses", "Trainer_Id");
            CreateIndex("dbo.CourseLearners", "Learner_Id");
            CreateIndex("dbo.CourseLearners", "Course_Id");
            CreateIndex("dbo.ProjectModels", "Image_Id");
            CreateIndex("dbo.JobModels", "CompanyId");
            CreateIndex("dbo.FileModels", "Image_Id");
            CreateIndex("dbo.Articles", "Learner_Id");
            CreateIndex("dbo.Courses", "Image_Id");
            CreateIndex("dbo.ArticleModels", "Avatar_Id");
            AddForeignKey("dbo.ProjectModels", "Image_Id", "dbo.ImageObjects", "Id");
            AddForeignKey("dbo.JobModels", "CompanyId", "dbo.CompanyModels", "Id");
            AddForeignKey("dbo.FileModels", "Image_Id", "dbo.ImageObjects", "Id");
            AddForeignKey("dbo.TrainerCourses", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.Trainers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CourseLearners", "Learner_Id", "dbo.Learners", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CourseLearners", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Articles", "Learner_Id", "dbo.Learners", "Id");
            AddForeignKey("dbo.Courses", "Image_Id", "dbo.ImageObjects", "Id");
            AddForeignKey("dbo.ArticleModels", "Avatar_Id", "dbo.ImageObjects", "Id");
        }
    }
}
