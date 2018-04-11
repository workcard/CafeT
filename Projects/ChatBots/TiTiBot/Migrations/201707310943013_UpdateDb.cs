namespace TiTiBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Activities");
            AlterColumn("dbo.Activities", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Activities", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Activities");
            AlterColumn("dbo.Activities", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Activities", "Id");
        }
    }
}
