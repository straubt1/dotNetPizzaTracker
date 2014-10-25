namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SauceLevelToPizza2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SauceOptions", "SauceLevel_Id", c => c.Int());
            CreateIndex("dbo.SauceOptions", "SauceLevel_Id");
            AddForeignKey("dbo.SauceOptions", "SauceLevel_Id", "dbo.SauceLevels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SauceOptions", "SauceLevel_Id", "dbo.SauceLevels");
            DropIndex("dbo.SauceOptions", new[] { "SauceLevel_Id" });
            DropColumn("dbo.SauceOptions", "SauceLevel_Id");
        }
    }
}
