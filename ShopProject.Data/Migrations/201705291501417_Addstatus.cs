namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Status");
        }
    }
}
