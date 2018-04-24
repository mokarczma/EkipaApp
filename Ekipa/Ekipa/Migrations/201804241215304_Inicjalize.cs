namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicjalize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Speciality = c.String(),
                        Services = c.String(),
                        Pricing = c.String(),
                        CityId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        PhoneNumer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Term",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        PhoneNumber = c.String(),
                        RoleId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Term_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Term", t => t.Term_Id)
                .Index(t => t.RoleId)
                .Index(t => t.Term_Id);
            
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescriptionCustomer = c.String(),
                        DescriptionCompany = c.String(),
                        CompanyAccept = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        TermId = c.Int(nullable: false),
                        Customer_ID = c.Int(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Term", t => t.TermId, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.Customer_ID)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.TermId)
                .Index(t => t.Customer_ID)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Opinion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        GradeValue = c.Int(nullable: false),
                        AdminAccept = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        RezervationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservation", t => t.RezervationId, cascadeDelete: true)
                .Index(t => t.RezervationId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        Description = c.String(),
                        CompanyId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        MainPicture = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
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
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TermCompany",
                c => new
                    {
                        Term_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Term_Id, t.Company_Id })
                .ForeignKey("dbo.Term", t => t.Term_Id, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Term_Id)
                .Index(t => t.Company_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.CompanyTag", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Reservation", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.Image", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Customer", "Term_Id", "dbo.Term");
            DropForeignKey("dbo.Customer", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Company", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Reservation", "Customer_ID", "dbo.Customer");
            DropForeignKey("dbo.Reservation", "TermId", "dbo.Term");
            DropForeignKey("dbo.Opinion", "RezervationId", "dbo.Reservation");
            DropForeignKey("dbo.TermCompany", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.TermCompany", "Term_Id", "dbo.Term");
            DropForeignKey("dbo.Company", "CityId", "dbo.City");
            DropIndex("dbo.TermCompany", new[] { "Company_Id" });
            DropIndex("dbo.TermCompany", new[] { "Term_Id" });
            DropIndex("dbo.CompanyTag", new[] { "TagId" });
            DropIndex("dbo.CompanyTag", new[] { "CompanyId" });
            DropIndex("dbo.Image", new[] { "CompanyId" });
            DropIndex("dbo.Opinion", new[] { "RezervationId" });
            DropIndex("dbo.Reservation", new[] { "Company_Id" });
            DropIndex("dbo.Reservation", new[] { "Customer_ID" });
            DropIndex("dbo.Reservation", new[] { "TermId" });
            DropIndex("dbo.Customer", new[] { "Term_Id" });
            DropIndex("dbo.Customer", new[] { "RoleId" });
            DropIndex("dbo.Company", new[] { "RoleId" });
            DropIndex("dbo.Company", new[] { "CityId" });
            DropTable("dbo.TermCompany");
            DropTable("dbo.Tag");
            DropTable("dbo.CompanyTag");
            DropTable("dbo.Image");
            DropTable("dbo.Role");
            DropTable("dbo.Opinion");
            DropTable("dbo.Reservation");
            DropTable("dbo.Customer");
            DropTable("dbo.Term");
            DropTable("dbo.Company");
            DropTable("dbo.City");
        }
    }
}
