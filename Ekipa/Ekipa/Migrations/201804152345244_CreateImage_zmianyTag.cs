namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImage_zmianyTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyTag", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.CompanyTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.Tag", "Company_Id", "dbo.Company");
            DropIndex("dbo.Tag", new[] { "Company_Id" });
            DropIndex("dbo.CompanyTag", new[] { "CompanyId" });
            DropIndex("dbo.CompanyTag", new[] { "TagId" });
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        CompanyId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        MainPicture = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageCompany",
                c => new
                    {
                        Image_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_Id, t.Company_Id })
                .ForeignKey("dbo.Image", t => t.Image_Id, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Image_Id)
                .Index(t => t.Company_Id);
            
            DropColumn("dbo.Tag", "Company_Id");
            DropTable("dbo.CompanyTag");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tag", "Company_Id", c => c.Int());
            DropForeignKey("dbo.ImageCompany", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.ImageCompany", "Image_Id", "dbo.Image");
            DropIndex("dbo.ImageCompany", new[] { "Company_Id" });
            DropIndex("dbo.ImageCompany", new[] { "Image_Id" });
            DropTable("dbo.ImageCompany");
            DropTable("dbo.Image");
            CreateIndex("dbo.CompanyTag", "TagId");
            CreateIndex("dbo.CompanyTag", "CompanyId");
            CreateIndex("dbo.Tag", "Company_Id");
            AddForeignKey("dbo.Tag", "Company_Id", "dbo.Company", "Id");
            AddForeignKey("dbo.CompanyTag", "TagId", "dbo.Tag", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyTag", "CompanyId", "dbo.Company", "Id", cascadeDelete: true);
        }
    }
}
