namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWordModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WordModels", "Lang", c => c.Int(nullable: false));
            AddColumn("dbo.WordModels", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.WordModels", "Translation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WordModels", "Translation");
            DropColumn("dbo.WordModels", "Type");
            DropColumn("dbo.WordModels", "Lang");
        }
    }
}
