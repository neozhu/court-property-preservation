namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addstatus : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NodeConfigs", new[] { "Node" });
            AddColumn("dbo.NodeConfigs", "Status", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.NodeConfigs", "NextStatus", c => c.String(maxLength: 20));
            CreateIndex("dbo.NodeConfigs", new[] { "Node", "Status" }, unique: true, name: "IX_NODECONFIG");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NodeConfigs", "IX_NODECONFIG");
            DropColumn("dbo.NodeConfigs", "NextStatus");
            DropColumn("dbo.NodeConfigs", "Status");
            CreateIndex("dbo.NodeConfigs", "Node", unique: true);
        }
    }
}
