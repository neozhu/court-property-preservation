namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addSerialNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LegalCases", "SerialNumber", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LegalCases", "SerialNumber");
        }
    }
}
