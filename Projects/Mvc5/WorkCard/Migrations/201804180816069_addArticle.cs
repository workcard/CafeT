namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addArticle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Content = c.String(),
                        ProjectId = c.Guid(),
                        IssueId = c.Guid(),
                        QuestionId = c.Guid(),
                        JobId = c.Guid(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.IssueViews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IssueViews",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Content = c.String(),
                        Message = c.String(),
                        Owner = c.String(),
                        Price = c.Double(nullable: false),
                        AssignedUserName = c.String(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        IssueEstimation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        Repeat = c.Int(nullable: false),
                        ProjectId = c.Guid(),
                        IsClosed = c.Boolean(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Articles");
        }
    }
}
