namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyRes : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Opinion", name: "RezervationId", newName: "ReservationId");
            RenameIndex(table: "dbo.Opinion", name: "IX_RezervationId", newName: "IX_ReservationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Opinion", name: "IX_ReservationId", newName: "IX_RezervationId");
            RenameColumn(table: "dbo.Opinion", name: "ReservationId", newName: "RezervationId");
        }
    }
}
