namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TermDelete1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Term", "IsDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Term", "IsDelete", c => c.Int(nullable: false));
        }
    }
}
