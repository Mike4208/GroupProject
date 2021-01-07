namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullabledatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Created", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "LastLog", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastLog", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
