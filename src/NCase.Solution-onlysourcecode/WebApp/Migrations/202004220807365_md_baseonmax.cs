namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_baseonmax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LegalCases", "BasedOn", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LegalCases", "BasedOn", c => c.String(maxLength: 100));
        }
    }
}
