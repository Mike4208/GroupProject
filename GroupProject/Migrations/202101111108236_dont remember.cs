namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dontremember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Address");
        }
    }
}
