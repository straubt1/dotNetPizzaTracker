namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageQueues", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageQueues", "Date");
        }
    }
}
