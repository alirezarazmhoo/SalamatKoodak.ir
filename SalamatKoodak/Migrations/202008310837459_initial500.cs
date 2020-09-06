namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial500 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AspNetUsers", "CityId");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
        }
    }
}
