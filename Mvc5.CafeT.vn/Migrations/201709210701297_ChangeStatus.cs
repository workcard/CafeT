namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Articles", "Scope", c => c.Int(nullable: false));
            DropColumn("dbo.Articles", "IsPublished");
            DropColumn("dbo.Articles", "IsDrafted");
            DropColumn("dbo.Articles", "IsProtect");
            DropColumn("dbo.Articles", "IsPublic");
            DropColumn("dbo.Articles", "IsPrivate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsPublic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsProtect", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsDrafted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsPublished", c => c.Boolean(nullable: false));
            DropColumn("dbo.Articles", "Scope");
            DropColumn("dbo.Articles", "Status");
        }
    }
}
