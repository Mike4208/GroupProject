namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discountregex2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Discount", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Discount", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
