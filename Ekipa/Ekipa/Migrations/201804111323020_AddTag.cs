namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Company", "RoleId");
            CreateIndex("dbo.Customer", "RoleId");
            AddForeignKey("dbo.Company", "RoleId", "dbo.Role", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Customer", "RoleId", "dbo.Role", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Company", "RoleId", "dbo.Role");
            DropIndex("dbo.Customer", new[] { "RoleId" });
            DropIndex("dbo.Company", new[] { "RoleId" });
            DropTable("dbo.Role");
        }
    }
}
