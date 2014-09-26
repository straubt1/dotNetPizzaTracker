namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pizza2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToppingOptions", "Topping_Id", "dbo.Toppings");
            DropIndex("dbo.ToppingOptions", new[] { "Topping_Id" });
            RenameColumn(table: "dbo.ToppingOptions", name: "Topping_Id", newName: "ToppingId");
            AlterColumn("dbo.ToppingOptions", "ToppingId", c => c.Int(nullable: false));
            CreateIndex("dbo.ToppingOptions", "ToppingId");
            AddForeignKey("dbo.ToppingOptions", "ToppingId", "dbo.Toppings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToppingOptions", "ToppingId", "dbo.Toppings");
            DropIndex("dbo.ToppingOptions", new[] { "ToppingId" });
            AlterColumn("dbo.ToppingOptions", "ToppingId", c => c.Int());
            RenameColumn(table: "dbo.ToppingOptions", name: "ToppingId", newName: "Topping_Id");
            CreateIndex("dbo.ToppingOptions", "Topping_Id");
            AddForeignKey("dbo.ToppingOptions", "Topping_Id", "dbo.Toppings", "Id");
        }
    }
}
