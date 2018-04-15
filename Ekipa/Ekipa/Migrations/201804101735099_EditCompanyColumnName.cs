namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCompanyColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyTerm", "DateTo", c => c.DateTime(nullable: false));
            CreateIndex("dbo.CompanyTerm", "CompanyId");
            AddForeignKey("dbo.CompanyTerm", "CompanyId", "dbo.Company", "Id", cascadeDelete: true);
            DropColumn("dbo.CompanyTerm", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyTerm", "DateTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.CompanyTerm", "CompanyId", "dbo.Company");
            DropIndex("dbo.CompanyTerm", new[] { "CompanyId" });
            DropColumn("dbo.CompanyTerm", "DateTo");
        }
    }
}
