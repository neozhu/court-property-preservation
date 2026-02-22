namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_mfmax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LegalCases", "CloseType", c => c.String(maxLength: 50));
            AlterColumn("dbo.LegalCases", "Org", c => c.String(maxLength: 50));
            AlterColumn("dbo.LegalCases", "Accuser", c => c.String());
            AlterColumn("dbo.LegalCases", "AccuserAddress", c => c.String());
            AlterColumn("dbo.LegalCases", "Defendant", c => c.String());
            AlterColumn("dbo.LegalCases", "DefendantAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LegalCases", "DefendantAddress", c => c.String(maxLength: 250));
            AlterColumn("dbo.LegalCases", "Defendant", c => c.String(maxLength: 250));
            AlterColumn("dbo.LegalCases", "AccuserAddress", c => c.String(maxLength: 250));
            AlterColumn("dbo.LegalCases", "Accuser", c => c.String(maxLength: 250));
            AlterColumn("dbo.LegalCases", "Org", c => c.String());
            AlterColumn("dbo.LegalCases", "CloseType", c => c.String());
        }
    }
}
