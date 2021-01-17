namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
