namespace SalamatKoodak.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial66674 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personels", "NetWorkType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Personels", "NetWorkType");
        }
    }
}
