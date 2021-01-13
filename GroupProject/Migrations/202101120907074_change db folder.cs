namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedbfolder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrderProducts", newName: "OrderDetails");
            AddColumn("dbo.Orders", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "Price");
            DropColumn("dbo.Orders", "FirstName");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Address", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "TotalPrice");
            RenameTable(name: "dbo.OrderDetails", newName: "OrderProducts");
        }
    }
}
