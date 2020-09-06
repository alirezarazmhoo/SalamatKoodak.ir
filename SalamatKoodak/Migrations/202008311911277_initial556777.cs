namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial556777 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Personels", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Personels", "ApplicationUserId");
            RenameColumn(table: "dbo.Personels", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Personels", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Personels", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Personels", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Personels", "ApplicationUserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Personels", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Personels", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Personels", "ApplicationUser_Id");
        }
    }
}
