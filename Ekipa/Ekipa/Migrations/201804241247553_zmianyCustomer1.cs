namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyCustomer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservation", "Customer_ID", c => c.Int());
            CreateIndex("dbo.Reservation", "Customer_ID");
            AddForeignKey("dbo.Reservation", "Customer_ID", "dbo.Customer", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservation", "Customer_ID", "dbo.Customer");
            DropIndex("dbo.Reservation", new[] { "Customer_ID" });
            DropColumn("dbo.Reservation", "Customer_ID");
        }
    }
}
