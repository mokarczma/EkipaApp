namespace Ekipa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Image", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Image", "Description");
        }
    }
}
