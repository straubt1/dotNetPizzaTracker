namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pizza : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crusts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pizzas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Double(nullable: false),
                        SizeId = c.Int(nullable: false),
                        CrustId = c.Int(nullable: false),
                        SauceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Crusts", t => t.CrustId, cascadeDelete: true)
                .ForeignKey("dbo.Sauces", t => t.SauceId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.SizeId)
                .Index(t => t.CrustId)
                .Index(t => t.SauceId);
            
            CreateTable(
                "dbo.Sauces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Width = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToppingOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Side = c.Int(nullable: false),
                        Topping_Id = c.Int(),
                        Pizza_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Toppings", t => t.Topping_Id)
                .ForeignKey("dbo.Pizzas", t => t.Pizza_Id)
                .Index(t => t.Topping_Id)
                .Index(t => t.Pizza_Id);
            
            CreateTable(
                "dbo.Toppings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Double(nullable: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        PasswordSalt = c.String(),
                        PasswordResetToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToppingOptions", "Pizza_Id", "dbo.Pizzas");
            DropForeignKey("dbo.ToppingOptions", "Topping_Id", "dbo.Toppings");
            DropForeignKey("dbo.Pizzas", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.Pizzas", "SauceId", "dbo.Sauces");
            DropForeignKey("dbo.Pizzas", "CrustId", "dbo.Crusts");
            DropIndex("dbo.ToppingOptions", new[] { "Pizza_Id" });
            DropIndex("dbo.ToppingOptions", new[] { "Topping_Id" });
            DropIndex("dbo.Pizzas", new[] { "SauceId" });
            DropIndex("dbo.Pizzas", new[] { "CrustId" });
            DropIndex("dbo.Pizzas", new[] { "SizeId" });
            DropTable("dbo.Users");
            DropTable("dbo.Toppings");
            DropTable("dbo.ToppingOptions");
            DropTable("dbo.Sizes");
            DropTable("dbo.Sauces");
            DropTable("dbo.Pizzas");
            DropTable("dbo.Crusts");
        }
    }
}
