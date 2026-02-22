namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addtempcaseid_f1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempCaseIds", "Year", c => c.String(maxLength: 10));
            AddColumn("dbo.TempCaseIds", "Expires", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempCaseIds", "Expires");
            DropColumn("dbo.TempCaseIds", "Year");
        }
    }
}
