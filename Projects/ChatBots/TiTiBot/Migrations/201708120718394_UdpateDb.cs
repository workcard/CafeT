namespace TiTiBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UdpateDb : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Activities", newName: "ActivityBoes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ActivityBoes", newName: "Activities");
        }
    }
}
