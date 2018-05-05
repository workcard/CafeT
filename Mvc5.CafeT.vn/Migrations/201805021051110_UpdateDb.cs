namespace Mvc5.CafeT.vn.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileModels", "Path", c => c.String());
            AddColumn("dbo.FileModels", "GDriveId", c => c.String());
            AddColumn("dbo.FileModels", "DownloadUrl", c => c.String());
            AddColumn("dbo.FileModels", "CountOfDownload", c => c.Int(nullable: false));
            AddColumn("dbo.FileModels", "Size", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileModels", "Size");
            DropColumn("dbo.FileModels", "CountOfDownload");
            DropColumn("dbo.FileModels", "DownloadUrl");
            DropColumn("dbo.FileModels", "GDriveId");
            DropColumn("dbo.FileModels", "Path");
        }
    }
}
