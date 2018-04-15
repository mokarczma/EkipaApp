namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
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
                        Location = c.String(),
                        RoleId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        PhoneNumer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
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
                "dbo.CompanyTerm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DateFrom = c.DateTime(nullable: false),
                        DateTime = c.DateTime(nullable: false),
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
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.CompanyTag", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Tag", "Company_Id", "dbo.Company");
            DropIndex("dbo.CompanyTag", new[] { "TagId" });
            DropIndex("dbo.CompanyTag", new[] { "CompanyId" });
            DropIndex("dbo.Tag", new[] { "Company_Id" });
            DropTable("dbo.Customer");
            DropTable("dbo.CompanyTerm");
            DropTable("dbo.CompanyTag");
            DropTable("dbo.Tag");
            DropTable("dbo.Company");
        }
    }
}
