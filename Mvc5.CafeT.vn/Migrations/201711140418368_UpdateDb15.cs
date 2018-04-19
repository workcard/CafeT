namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb15 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WordModels", "Model_Freq");
            DropColumn("dbo.WordModels", "Lang");
            DropColumn("dbo.WordModels", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WordModels", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.WordModels", "Lang", c => c.Int(nullable: false));
            AddColumn("dbo.WordModels", "Model_Freq", c => c.Int(nullable: false));
        }
    }
}
