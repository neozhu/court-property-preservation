namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addrecorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackHistories", "Recorder", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrackHistories", "Recorder");
        }
    }
}
