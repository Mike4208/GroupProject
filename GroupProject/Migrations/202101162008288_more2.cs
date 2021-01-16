namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class more2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 5));
        }
    }
}
