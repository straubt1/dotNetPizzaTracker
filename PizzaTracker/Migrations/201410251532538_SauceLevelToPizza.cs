namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SauceLevelToPizza : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pizzas", "SauceId", "dbo.Sauces");
            DropIndex("dbo.Pizzas", new[] { "SauceId" });
            RenameColumn(table: "dbo.Pizzas", name: "SauceId", newName: "Sauce_Id");
            CreateTable(
                "dbo.SauceOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SauceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sauces", t => t.SauceId, cascadeDelete: true)
                .Index(t => t.SauceId);
            
            AlterColumn("dbo.Pizzas", "Sauce_Id", c => c.Int());
            CreateIndex("dbo.Pizzas", "Sauce_Id");
            AddForeignKey("dbo.Pizzas", "Sauce_Id", "dbo.SauceOptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pizzas", "Sauce_Id", "dbo.SauceOptions");
            DropForeignKey("dbo.SauceOptions", "SauceId", "dbo.Sauces");
            DropIndex("dbo.SauceOptions", new[] { "SauceId" });
            DropIndex("dbo.Pizzas", new[] { "Sauce_Id" });
            AlterColumn("dbo.Pizzas", "Sauce_Id", c => c.Int(nullable: false));
            DropTable("dbo.SauceOptions");
            RenameColumn(table: "dbo.Pizzas", name: "Sauce_Id", newName: "SauceId");
            CreateIndex("dbo.Pizzas", "SauceId");
            AddForeignKey("dbo.Pizzas", "SauceId", "dbo.Sauces", "Id", cascadeDelete: true);
        }
    }
}
