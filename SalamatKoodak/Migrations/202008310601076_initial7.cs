namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "NationalCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "CityId");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            DropColumn("dbo.AspNetUsers", "CityId");
            DropColumn("dbo.AspNetUsers", "NationalCode");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
