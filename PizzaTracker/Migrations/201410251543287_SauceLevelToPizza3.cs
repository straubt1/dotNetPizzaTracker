namespace PizzaTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SauceLevelToPizza3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SauceOptions", "SauceLevel_Id", "dbo.SauceLevels");
            DropIndex("dbo.SauceOptions", new[] { "SauceLevel_Id" });
            RenameColumn(table: "dbo.SauceOptions", name: "SauceLevel_Id", newName: "SauceLevelId");
            AlterColumn("dbo.SauceOptions", "SauceLevelId", c => c.Int(nullable: false));
            CreateIndex("dbo.SauceOptions", "SauceLevelId");
            AddForeignKey("dbo.SauceOptions", "SauceLevelId", "dbo.SauceLevels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SauceOptions", "SauceLevelId", "dbo.SauceLevels");
            DropIndex("dbo.SauceOptions", new[] { "SauceLevelId" });
            AlterColumn("dbo.SauceOptions", "SauceLevelId", c => c.Int());
            RenameColumn(table: "dbo.SauceOptions", name: "SauceLevelId", newName: "SauceLevel_Id");
            CreateIndex("dbo.SauceOptions", "SauceLevel_Id");
            AddForeignKey("dbo.SauceOptions", "SauceLevel_Id", "dbo.SauceLevels", "Id");
        }
    }
}
