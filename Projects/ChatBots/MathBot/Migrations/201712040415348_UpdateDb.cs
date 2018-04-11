namespace MathBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WordDictionaries", "Vietnameses", c => c.String());
            DropColumn("dbo.WordDictionaries", "GoogleVn");
            DropColumn("dbo.WordDictionaries", "MicrosoftVn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WordDictionaries", "MicrosoftVn", c => c.String());
            AddColumn("dbo.WordDictionaries", "GoogleVn", c => c.String());
            DropColumn("dbo.WordDictionaries", "Vietnameses");
        }
    }
}
