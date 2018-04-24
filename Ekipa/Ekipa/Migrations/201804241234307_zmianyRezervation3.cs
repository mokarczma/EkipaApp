namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyRezervation3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "Company_Id", "dbo.Company");
            DropIndex("dbo.Reservation", new[] { "Company_Id" });
            DropColumn("dbo.Reservation", "Company_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "Company_Id", c => c.Int());
            CreateIndex("dbo.Reservation", "Company_Id");
            AddForeignKey("dbo.Reservation", "Company_Id", "dbo.Company", "Id");
        }
    }
}
