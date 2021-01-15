namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cartrating : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Carts", new[] { "ProductId" });
            CreateIndex("dbo.Carts", "ProductID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Carts", new[] { "ProductID" });
            CreateIndex("dbo.Carts", "ProductId");
        }
    }
}
