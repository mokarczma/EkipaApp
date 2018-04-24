namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmiany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TermCompany", "Term_Id", "dbo.Term");
            DropForeignKey("dbo.TermCompany", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.Customer", "Term_Id", "dbo.Term");
            DropIndex("dbo.Customer", new[] { "Term_Id" });
            DropIndex("dbo.TermCompany", new[] { "Term_Id" });
            DropIndex("dbo.TermCompany", new[] { "Company_Id" });
            CreateIndex("dbo.Term", "CompanyId");
            AddForeignKey("dbo.Term", "CompanyId", "dbo.Company", "Id", cascadeDelete: true);
            DropColumn("dbo.Customer", "Term_Id");
            DropTable("dbo.TermCompany");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TermCompany",
                c => new
                    {
                        Term_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Term_Id, t.Company_Id });
            
            AddColumn("dbo.Customer", "Term_Id", c => c.Int());
            DropForeignKey("dbo.Term", "CompanyId", "dbo.Company");
            DropIndex("dbo.Term", new[] { "CompanyId" });
            CreateIndex("dbo.TermCompany", "Company_Id");
            CreateIndex("dbo.TermCompany", "Term_Id");
            CreateIndex("dbo.Customer", "Term_Id");
            AddForeignKey("dbo.Customer", "Term_Id", "dbo.Term", "Id");
            AddForeignKey("dbo.TermCompany", "Company_Id", "dbo.Company", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TermCompany", "Term_Id", "dbo.Term", "Id", cascadeDelete: true);
        }
    }
}
