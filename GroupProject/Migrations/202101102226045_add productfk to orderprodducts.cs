namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproductfktoorderprodducts : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderProducts", "ProductID");
            AddForeignKey("dbo.OrderProducts", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderProducts", "ProductID", "dbo.Products");
            DropIndex("dbo.OrderProducts", new[] { "ProductID" });
        }
    }
}
