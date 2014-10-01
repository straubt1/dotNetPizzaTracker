namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToppingClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Toppings", "Class", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Toppings", "Class");
        }
    }
}
