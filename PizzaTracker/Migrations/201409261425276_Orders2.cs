namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
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
            
            AddColumn("dbo.Pizzas", "Order_Id", c => c.Int());
            CreateIndex("dbo.Pizzas", "Order_Id");
            AddForeignKey("dbo.Pizzas", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pizzas", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderEvents", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Pizzas", new[] { "Order_Id" });
            DropIndex("dbo.OrderEvents", new[] { "Order_Id" });
            DropColumn("dbo.Pizzas", "Order_Id");
            DropTable("dbo.OrderEvents");
            DropTable("dbo.Orders");
        }
    }
}
