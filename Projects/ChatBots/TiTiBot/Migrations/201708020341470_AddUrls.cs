namespace TiTiBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Urls",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Domain = c.String(),
                        Address = c.String(),
                        Title = c.String(),
                        HtmlContent = c.String(),
                        LastestCheck = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Urls");
        }
    }
}
