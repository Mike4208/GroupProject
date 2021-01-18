namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _223 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "ReviewCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "ReviewCreated", c => c.DateTime(nullable: false));
        }
    }
}
