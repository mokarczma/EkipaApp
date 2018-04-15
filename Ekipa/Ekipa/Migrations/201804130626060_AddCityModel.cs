namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCityModel : DbMigration
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
            
            AddColumn("dbo.Company", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Company", "CityId");
            AddForeignKey("dbo.Company", "CityId", "dbo.City", "ID", cascadeDelete: true);
            DropColumn("dbo.Company", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Company", "Location", c => c.String());
            DropForeignKey("dbo.Company", "CityId", "dbo.City");
            DropIndex("dbo.Company", new[] { "CityId" });
            DropColumn("dbo.Company", "CityId");
            DropTable("dbo.City");
        }
    }
}
