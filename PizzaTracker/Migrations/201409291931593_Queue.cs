namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Queue : DbMigration
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
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OrderedById = c.Int(nullable: false),
                        Show = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OrderedById, cascadeDelete: true)
                .Index(t => t.OrderedById);
            
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
                        RoleId = c.Int(nullable: false),
                        LoginToken = c.String(),
                        LoginExpiration = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        EventName = c.String(),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
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
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Crusts", t => t.CrustId, cascadeDelete: true)
                .ForeignKey("dbo.Sauces", t => t.SauceId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.SizeId)
                .Index(t => t.CrustId)
                .Index(t => t.SauceId)
                .Index(t => t.Order_Id);
            
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
                        ToppingId = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Pizza_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Toppings", t => t.ToppingId, cascadeDelete: true)
                .ForeignKey("dbo.Pizzas", t => t.Pizza_Id)
                .Index(t => t.ToppingId)
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
                "dbo.Queues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Active = c.Boolean(nullable: false),
                        OrderId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        AssignedToId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedToId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.StatusId)
                .Index(t => t.AssignedToId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Queues", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Queues", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Queues", "AssignedToId", "dbo.Users");
            DropForeignKey("dbo.Pizzas", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.ToppingOptions", "Pizza_Id", "dbo.Pizzas");
            DropForeignKey("dbo.ToppingOptions", "ToppingId", "dbo.Toppings");
            DropForeignKey("dbo.Pizzas", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.Pizzas", "SauceId", "dbo.Sauces");
            DropForeignKey("dbo.Pizzas", "CrustId", "dbo.Crusts");
            DropForeignKey("dbo.OrderEvents", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OrderedById", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.Queues", new[] { "AssignedToId" });
            DropIndex("dbo.Queues", new[] { "StatusId" });
            DropIndex("dbo.Queues", new[] { "OrderId" });
            DropIndex("dbo.ToppingOptions", new[] { "Pizza_Id" });
            DropIndex("dbo.ToppingOptions", new[] { "ToppingId" });
            DropIndex("dbo.Pizzas", new[] { "Order_Id" });
            DropIndex("dbo.Pizzas", new[] { "SauceId" });
            DropIndex("dbo.Pizzas", new[] { "CrustId" });
            DropIndex("dbo.Pizzas", new[] { "SizeId" });
            DropIndex("dbo.OrderEvents", new[] { "Order_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Orders", new[] { "OrderedById" });
            DropTable("dbo.Status");
            DropTable("dbo.Queues");
            DropTable("dbo.Toppings");
            DropTable("dbo.ToppingOptions");
            DropTable("dbo.Sizes");
            DropTable("dbo.Sauces");
            DropTable("dbo.Pizzas");
            DropTable("dbo.OrderEvents");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Crusts");
        }
    }
}
