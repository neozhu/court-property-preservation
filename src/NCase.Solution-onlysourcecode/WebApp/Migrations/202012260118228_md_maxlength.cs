namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_maxlength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LegalCases", new[] { "CaseId" });
            AlterColumn("dbo.LegalCases", "CaseId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.LegalCases", "Project", c => c.String(maxLength: 256));
            AlterColumn("dbo.LegalCases", "Feature", c => c.String(maxLength: 256));
            AlterColumn("dbo.LegalCases", "OriginCaseId", c => c.String(maxLength: 512));
            CreateIndex("dbo.LegalCases", "CaseId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.LegalCases", new[] { "CaseId" });
            AlterColumn("dbo.LegalCases", "OriginCaseId", c => c.String(maxLength: 50));
            AlterColumn("dbo.LegalCases", "Feature", c => c.String(maxLength: 200));
            AlterColumn("dbo.LegalCases", "Project", c => c.String(maxLength: 200));
            AlterColumn("dbo.LegalCases", "CaseId", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.LegalCases", "CaseId", unique: true);
        }
    }
}
