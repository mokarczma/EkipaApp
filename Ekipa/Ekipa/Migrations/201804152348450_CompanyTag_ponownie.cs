namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyTag_ponownie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.CompanyTag", "CompanyId", "dbo.Company");
            DropIndex("dbo.CompanyTag", new[] { "TagId" });
            DropIndex("dbo.CompanyTag", new[] { "CompanyId" });
            DropTable("dbo.CompanyTag");
        }
    }
}
