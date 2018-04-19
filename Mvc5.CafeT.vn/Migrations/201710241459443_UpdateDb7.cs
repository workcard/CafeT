namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuestionModels", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionModels", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
