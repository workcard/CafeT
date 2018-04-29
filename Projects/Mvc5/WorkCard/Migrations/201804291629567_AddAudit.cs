namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAudit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        AuditID = c.Guid(nullable: false),
                        SessionID = c.String(),
                        IPAddress = c.String(),
                        UserName = c.String(),
                        URLAccessed = c.String(),
                        TimeAccessed = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.AuditID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Audits");
        }
    }
}
