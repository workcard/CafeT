namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionModels", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionModels", "Discriminator");
        }
    }
}
