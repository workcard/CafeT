namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateArticle : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Articles", newName: "ArticleModels");
            DropIndex("dbo.ArticleModels", new[] { "Learner_Id" });
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
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        Learner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Learner_Id);
            
            DropColumn("dbo.ArticleModels", "Discriminator");
            DropColumn("dbo.ArticleModels", "Learner_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArticleModels", "Learner_Id", c => c.Guid());
            AddColumn("dbo.ArticleModels", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropIndex("dbo.Articles", new[] { "Learner_Id" });
            DropTable("dbo.Articles");
            CreateIndex("dbo.ArticleModels", "Learner_Id");
            RenameTable(name: "dbo.ArticleModels", newName: "Articles");
        }
    }
}
