namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_createtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        DocId = c.String(maxLength: 500),
                        Type = c.String(maxLength: 50),
                        Path = c.String(nullable: false),
                        Ext = c.String(maxLength: 5),
                        ExpireDate = c.DateTime(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Zone = c.String(maxLength: 50),
                        Address = c.String(maxLength: 150),
                        Contect = c.String(maxLength: 150),
                        CompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.LegalCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.String(nullable: false, maxLength: 50),
                        Project = c.String(maxLength: 200),
                        Category = c.String(nullable: false, maxLength: 20),
                        Status = c.String(nullable: false, maxLength: 10),
                        Expires = c.Int(nullable: false),
                        Cause = c.String(),
                        Feature = c.String(maxLength: 200),
                        BasedOn = c.String(maxLength: 100),
                        Subject = c.String(),
                        FromDepartment = c.String(maxLength: 20),
                        ToDepartment = c.String(maxLength: 20),
                        ToUser = c.String(maxLength: 20),
                        Recorder = c.String(maxLength: 20),
                        Examiner = c.String(maxLength: 20),
                        OriginCaseId = c.String(maxLength: 50),
                        ReceiveDate = c.DateTime(),
                        RegisterDate = c.DateTime(nullable: false),
                        PreUnderlyingAsset = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AllocateDate = c.DateTime(),
                        PreCloseDate = c.DateTime(),
                        ClosedDate = c.DateTime(),
                        CloseType = c.String(),
                        UnderlyingAsset = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Court = c.String(maxLength: 50),
                        Org = c.String(),
                        Accuser = c.String(maxLength: 250),
                        AccuserAddress = c.String(maxLength: 250),
                        Defendant = c.String(maxLength: 250),
                        DefendantAddress = c.String(maxLength: 250),
                        Receivable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Received = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Opinion1 = c.String(),
                        Opinion2 = c.String(),
                        Opinion3 = c.String(),
                        Comment = c.String(),
                        Proposer = c.String(maxLength: 20),
                        FilingDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CaseId, unique: true);
            
            CreateTable(
                "dbo.NodeConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Node = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 200),
                        Expires = c.Int(nullable: false),
                        Roles = c.String(maxLength: 20),
                        Users = c.String(maxLength: 20),
                        NextNode = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Node, unique: true);
            
            CreateTable(
                "dbo.TrackHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.String(nullable: false, maxLength: 50),
                        Status = c.String(nullable: false, maxLength: 10),
                        Node = c.String(nullable: false, maxLength: 10),
                        Owner = c.String(nullable: false, maxLength: 20),
                        ToUser = c.String(maxLength: 20),
                        BeginDate = c.DateTime(nullable: false),
                        CompletedDate = c.DateTime(),
                        Expires = c.Int(nullable: false),
                        DoDate = c.DateTime(),
                        Elapsed = c.Int(nullable: false),
                        State = c.String(maxLength: 50),
                        Comment = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courts", "CompanyId", "dbo.Companies");
            DropIndex("dbo.NodeConfigs", new[] { "Node" });
            DropIndex("dbo.LegalCases", new[] { "CaseId" });
            DropIndex("dbo.Courts", new[] { "CompanyId" });
            DropIndex("dbo.Courts", new[] { "Name" });
            DropTable("dbo.TrackHistories");
            DropTable("dbo.NodeConfigs");
            DropTable("dbo.LegalCases");
            DropTable("dbo.Courts");
            DropTable("dbo.Attachments");
        }
    }
}
