namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WordModels", "Length", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WordModels", "Length");
        }
    }
}
