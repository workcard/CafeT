namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WordModels", "Model_Freq", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WordModels", "Model_Freq");
        }
    }
}
