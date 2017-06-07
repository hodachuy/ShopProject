namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addItemGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "MenuFlag", c => c.Boolean());
            AddColumn("dbo.Groups", "LeftFlag", c => c.Boolean());
            AddColumn("dbo.Groups", "HomeFlag", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "HomeFlag");
            DropColumn("dbo.Groups", "LeftFlag");
            DropColumn("dbo.Groups", "MenuFlag");
        }
    }
}
