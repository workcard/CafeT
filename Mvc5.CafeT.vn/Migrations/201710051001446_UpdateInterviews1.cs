namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInterviews1 : DbMigration
    {
        public override void Up()
        {
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
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.QuestionModels", "InterviewId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionModels", "InterviewId");
            DropTable("dbo.InterviewModels");
        }
    }
}
