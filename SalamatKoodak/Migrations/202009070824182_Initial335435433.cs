namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial335435433 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personels", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Personels", "CreatedDate");
        }
    }
}
