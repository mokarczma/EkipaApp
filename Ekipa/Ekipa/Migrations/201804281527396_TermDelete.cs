namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TermDelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Term", "IsDelete", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Term", "IsDelete");
        }
    }
}
