namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SauceLevels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SauceLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SauceLevels");
        }
    }
}
