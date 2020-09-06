namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DecriptPass", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DecriptPass");
        }
    }
}
