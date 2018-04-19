namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBigModify : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuItemModels", "ParentMenu_Id", "dbo.MenuModels");
            DropIndex("dbo.MenuItemModels", new[] { "ParentMenu_Id" });
            CreateTable(
                "dbo.WordModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Model_Value = c.String(),
                        Model_Index = c.Int(nullable: false),
                        IsRemembered = c.Boolean(nullable: false),
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
            
            DropTable("dbo.CrawlerModels");
            DropTable("dbo.MenuItemModels");
            DropTable("dbo.MenuModels");
            DropTable("dbo.ApplicationMessages");
            DropTable("dbo.ApplicationSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Url = c.String(),
                        Name = c.String(),
                        PathLogo = c.String(),
                        Metas = c.String(),
                        UploadFolder = c.String(),
                        DbServerIp = c.String(),
                        DbInstanceName = c.String(),
                        DbUserName = c.String(),
                        DbPassword = c.String(),
                        EmailSmtp = c.String(),
                        EmailUser = c.String(),
                        EmailPassword = c.String(),
                        EmailPort = c.Int(nullable: false),
                        EmailEnableSSL = c.Boolean(nullable: false),
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
                "dbo.ApplicationMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Message = c.String(),
                        StartDisplay = c.DateTime(nullable: false),
                        EndDisplay = c.DateTime(nullable: false),
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
                "dbo.MenuModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ActionName = c.String(),
                        ControllerName = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
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
                "dbo.MenuItemModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ActionName = c.String(),
                        ControllerName = c.String(),
                        Url = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        Viewers = c.String(),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                        ParentMenu_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CrawlerModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Depth = c.Int(nullable: false),
                        Url = c.String(nullable: false),
                        PathConfig = c.String(),
                        PathOutput = c.String(),
                        AcceptKeywords = c.String(),
                        RejectKeywords = c.String(),
                        Enable = c.Boolean(nullable: false),
                        LastestRun = c.DateTime(),
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
            
            DropTable("dbo.WordModels");
            CreateIndex("dbo.MenuItemModels", "ParentMenu_Id");
            AddForeignKey("dbo.MenuItemModels", "ParentMenu_Id", "dbo.MenuModels", "Id");
        }
    }
}
