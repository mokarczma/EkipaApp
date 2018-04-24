namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyRezerwacja5 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Term", "CustomerId");
            AddForeignKey("dbo.Term", "CustomerId", "dbo.Customer", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Term", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Term", new[] { "CustomerId" });
        }
    }
}
