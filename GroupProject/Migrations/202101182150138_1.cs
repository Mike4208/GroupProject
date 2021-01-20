namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "Id" });
            RenameColumn(table: "dbo.Orders", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_ApplicationUserID", newName: "IX_ApplicationUser_Id");
            AddColumn("dbo.Ratings", "UserName", c => c.String());
            AddColumn("dbo.Orders", "UserName", c => c.String());
            AlterColumn("dbo.Ratings", "ReviewCreated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Ratings", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ratings", "ReviewCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Orders", "UserName");
            DropColumn("dbo.Ratings", "UserName");
            RenameIndex(table: "dbo.Orders", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserID");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            CreateIndex("dbo.Ratings", "Id");
            AddForeignKey("dbo.Ratings", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
