namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addstatustodate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LegalCases", "ToDoDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TrackHistories", "NodeStatus", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrackHistories", "NodeStatus");
            DropColumn("dbo.LegalCases", "ToDoDate");
        }
    }
}
