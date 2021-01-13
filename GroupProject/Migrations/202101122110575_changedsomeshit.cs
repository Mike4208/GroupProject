namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedsomeshit : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Orders", "ApplicationUserID");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.OrderDetails", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ApplicationUserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingCarts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ShoppingCarts", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.ShoppingCarts", new[] { "ProductId" });
            DropIndex("dbo.ShoppingCarts", new[] { "ApplicationUserID" });
            DropIndex("dbo.Orders", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Orders", "ApplicationUserID", c => c.Int(nullable: false));
            DropColumn("dbo.OrderDetails", "Price");
            DropTable("dbo.ShoppingCarts");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            AddColumn("dbo.Orders", "ApplicationUserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
        }
    }
}
