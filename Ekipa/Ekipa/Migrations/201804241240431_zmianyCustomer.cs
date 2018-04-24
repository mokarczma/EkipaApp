namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "Customer_ID", "dbo.Customer");
            DropIndex("dbo.Reservation", new[] { "Customer_ID" });
            DropColumn("dbo.Reservation", "Customer_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "Customer_ID", c => c.Int());
            CreateIndex("dbo.Reservation", "Customer_ID");
            AddForeignKey("dbo.Reservation", "Customer_ID", "dbo.Customer", "ID");
        }
    }
}
