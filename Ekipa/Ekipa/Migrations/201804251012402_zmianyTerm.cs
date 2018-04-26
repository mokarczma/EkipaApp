namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianyTerm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Term", "DateStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Term", "DateStop", c => c.DateTime(nullable: false));
            DropColumn("dbo.Term", "DateFrom");
            DropColumn("dbo.Term", "DateTo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Term", "DateTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.Term", "DateFrom", c => c.DateTime(nullable: false));
            DropColumn("dbo.Term", "DateStop");
            DropColumn("dbo.Term", "DateStart");
        }
    }
}
