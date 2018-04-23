namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReservationOpinion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompanyTerm", newName: "Term");
            RenameTable(name: "dbo.CompanyTermCompany", newName: "TermCompany");
            RenameColumn(table: "dbo.TermCompany", name: "CompanyTerm_Id", newName: "Term_Id");
            RenameColumn(table: "dbo.Customer", name: "CompanyTerm_Id", newName: "Term_Id");
            RenameIndex(table: "dbo.Customer", name: "IX_CompanyTerm_Id", newName: "IX_Term_Id");
            RenameIndex(table: "dbo.TermCompany", name: "IX_CompanyTerm_Id", newName: "IX_Term_Id");
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescriptionCustomer = c.String(),
                        DescriptionCompany = c.String(),
                        CompanyAccept = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
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
                "dbo.CompanyImagesVM",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MainPicturePath = c.String(),
                        MainPictureID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ImageVM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 150),
                        Link = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        MainPicture = c.Boolean(nullable: false),
                        CompanyImagesVM_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyImagesVM", t => t.CompanyImagesVM_ID)
                .Index(t => t.CompanyImagesVM_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageVM", "CompanyImagesVM_ID", "dbo.CompanyImagesVM");
            DropForeignKey("dbo.Opinion", "RezervationId", "dbo.Reservation");
            DropForeignKey("dbo.Reservation", "CompanyId", "dbo.Company");
            DropIndex("dbo.ImageVM", new[] { "CompanyImagesVM_ID" });
            DropIndex("dbo.Opinion", new[] { "RezervationId" });
            DropIndex("dbo.Reservation", new[] { "CompanyId" });
            DropTable("dbo.ImageVM");
            DropTable("dbo.CompanyImagesVM");
            DropTable("dbo.Opinion");
            DropTable("dbo.Reservation");
            RenameIndex(table: "dbo.TermCompany", name: "IX_Term_Id", newName: "IX_CompanyTerm_Id");
            RenameIndex(table: "dbo.Customer", name: "IX_Term_Id", newName: "IX_CompanyTerm_Id");
            RenameColumn(table: "dbo.Customer", name: "Term_Id", newName: "CompanyTerm_Id");
            RenameColumn(table: "dbo.TermCompany", name: "Term_Id", newName: "CompanyTerm_Id");
            RenameTable(name: "dbo.TermCompany", newName: "CompanyTermCompany");
            RenameTable(name: "dbo.Term", newName: "CompanyTerm");
        }
    }
}
