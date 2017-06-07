namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePropertyProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductNewFlag", c => c.Boolean());
            AddColumn("dbo.Products", "DiscountFlag", c => c.Boolean());
            AddColumn("dbo.Products", "GroupProduct", c => c.String());
            AlterColumn("dbo.Products", "Warranty", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Warranty", c => c.Int());
            DropColumn("dbo.Products", "GroupProduct");
            DropColumn("dbo.Products", "DiscountFlag");
            DropColumn("dbo.Products", "ProductNewFlag");
        }
    }
}
