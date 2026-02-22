namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addmanager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LegalCases", "Manager", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LegalCases", "Manager");
        }
    }
}
