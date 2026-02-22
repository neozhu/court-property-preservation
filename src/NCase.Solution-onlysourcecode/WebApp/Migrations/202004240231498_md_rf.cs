namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_rf : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TrackHistories", "Owner", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TrackHistories", "Owner", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
