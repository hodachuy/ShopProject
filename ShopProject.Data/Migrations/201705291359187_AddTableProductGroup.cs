namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableProductGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 256, unicode: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Image = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        GroupID = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.GroupID })
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.GroupID);
            
            AddColumn("dbo.Products", "Groups", c => c.String());
            AlterColumn("dbo.Products", "Image", c => c.String());
            AlterColumn("dbo.ProductCategories", "Image", c => c.String());
            DropColumn("dbo.Products", "GroupProduct");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "GroupProduct", c => c.String());
            DropForeignKey("dbo.ProductGroups", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductGroups", "GroupID", "dbo.Groups");
            DropIndex("dbo.ProductGroups", new[] { "GroupID" });
            DropIndex("dbo.ProductGroups", new[] { "ProductID" });
            AlterColumn("dbo.ProductCategories", "Image", c => c.String(maxLength: 256));
            AlterColumn("dbo.Products", "Image", c => c.String(maxLength: 256));
            DropColumn("dbo.Products", "Groups");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.Groups");
        }
    }
}
