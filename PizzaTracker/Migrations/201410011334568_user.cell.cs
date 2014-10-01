namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usercell : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CellPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CellPhone");
        }
    }
}
