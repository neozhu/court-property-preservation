namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addtempcaseid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempCaseIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.String(maxLength: 50),
                        Category = c.String(maxLength: 20),
                        SerialNumber = c.String(maxLength: 5),
                        Flag = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempCaseIds");
        }
    }
}
