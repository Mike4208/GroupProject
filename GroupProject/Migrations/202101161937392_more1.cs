namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class more1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 5));
            AlterColumn("dbo.AspNetUsers", "PostalCode", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PostalCode", c => c.String());
            AlterColumn("dbo.AspNetUsers", "City", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
    }
}
