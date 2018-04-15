namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCompanyTerm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyTerm", "CompanyId", "dbo.Company");
            DropIndex("dbo.CompanyTerm", new[] { "CompanyId" });
            CreateTable(
                "dbo.CompanyTermCompany",
                c => new
                    {
                        CompanyTerm_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyTerm_Id, t.Company_Id })
                .ForeignKey("dbo.CompanyTerm", t => t.CompanyTerm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.CompanyTerm_Id)
                .Index(t => t.Company_Id);
            
            AddColumn("dbo.Customer", "CompanyTerm_Id", c => c.Int());
            CreateIndex("dbo.Customer", "CompanyTerm_Id");
            AddForeignKey("dbo.Customer", "CompanyTerm_Id", "dbo.CompanyTerm", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "CompanyTerm_Id", "dbo.CompanyTerm");
            DropForeignKey("dbo.CompanyTermCompany", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.CompanyTermCompany", "CompanyTerm_Id", "dbo.CompanyTerm");
            DropIndex("dbo.CompanyTermCompany", new[] { "Company_Id" });
            DropIndex("dbo.CompanyTermCompany", new[] { "CompanyTerm_Id" });
            DropIndex("dbo.Customer", new[] { "CompanyTerm_Id" });
            DropColumn("dbo.Customer", "CompanyTerm_Id");
            DropTable("dbo.CompanyTermCompany");
            CreateIndex("dbo.CompanyTerm", "CompanyId");
            AddForeignKey("dbo.CompanyTerm", "CompanyId", "dbo.Company", "Id", cascadeDelete: true);
        }
    }
}
