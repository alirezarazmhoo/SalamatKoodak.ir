namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial55677 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Personels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        IdTelegram = c.String(nullable: false),
                        NationalCode = c.String(nullable: false),
                        RelationTypeId = c.Int(nullable: false),
                        PersonelStatusId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.PersonelStatus", t => t.PersonelStatusId, cascadeDelete: true)
                .ForeignKey("dbo.RelationTypes", t => t.RelationTypeId, cascadeDelete: true)
                .Index(t => t.RelationTypeId)
                .Index(t => t.PersonelStatusId)
                .Index(t => t.CityId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Personels", "RelationTypeId", "dbo.RelationTypes");
            DropForeignKey("dbo.Personels", "PersonelStatusId", "dbo.PersonelStatus");
            DropForeignKey("dbo.Personels", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Personels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Personels", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Personels", new[] { "CityId" });
            DropIndex("dbo.Personels", new[] { "PersonelStatusId" });
            DropIndex("dbo.Personels", new[] { "RelationTypeId" });
            DropTable("dbo.Personels");
        }
    }
}
