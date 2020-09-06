namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.AspNetUsers", "CityId");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
    }
}
