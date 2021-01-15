namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tryingforcart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShoppingCarts", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShoppingCarts", "ProductId", "dbo.Products");
            DropIndex("dbo.ShoppingCarts", new[] { "ApplicationUserID" });
            DropIndex("dbo.ShoppingCarts", new[] { "ProductId" });
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CartID = c.String(),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            DropTable("dbo.ShoppingCarts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropTable("dbo.Carts");
            CreateIndex("dbo.ShoppingCarts", "ProductId");
            CreateIndex("dbo.ShoppingCarts", "ApplicationUserID");
            AddForeignKey("dbo.ShoppingCarts", "ProductId", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ShoppingCarts", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
