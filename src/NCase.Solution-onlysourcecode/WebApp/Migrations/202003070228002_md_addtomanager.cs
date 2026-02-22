namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addtomanager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LegalCases", "ToManager", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LegalCases", "ToManager");
        }
    }
}
