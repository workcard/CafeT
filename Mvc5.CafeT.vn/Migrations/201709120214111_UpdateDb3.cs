namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventModels", "Timer_Id", "dbo.TimerObjects");
            DropIndex("dbo.EventModels", new[] { "Timer_Id" });
            DropColumn("dbo.EventModels", "Timer_Id");
            DropTable("dbo.TimerObjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TimerObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timer_AutoReset = c.Boolean(nullable: false),
                        Timer_Enabled = c.Boolean(nullable: false),
                        Timer_Interval = c.Double(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastUpdatedDate = c.DateTime(),
                        LastUpdatedBy = c.String(),
                        CountViews = c.Int(nullable: false),
                        LastViewAt = c.DateTime(),
                        LastViewBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EventModels", "Timer_Id", c => c.Guid());
            CreateIndex("dbo.EventModels", "Timer_Id");
            AddForeignKey("dbo.EventModels", "Timer_Id", "dbo.TimerObjects", "Id");
        }
    }
}
