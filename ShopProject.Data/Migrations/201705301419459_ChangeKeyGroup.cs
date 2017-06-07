namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKeyGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductGroups", "GroupID", "dbo.Groups");
            DropIndex("dbo.ProductGroups", new[] { "GroupID" });
            DropPrimaryKey("dbo.Groups");
            DropPrimaryKey("dbo.ProductGroups");
            AlterColumn("dbo.Groups", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ProductGroups", "GroupID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Groups", "ID");
            AddPrimaryKey("dbo.ProductGroups", new[] { "ProductID", "GroupID" });
            CreateIndex("dbo.ProductGroups", "GroupID");
            AddForeignKey("dbo.ProductGroups", "GroupID", "dbo.Groups", "ID", cascadeDelete: true);
            DropColumn("dbo.Products", "Groups");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Groups", c => c.String());
            DropForeignKey("dbo.ProductGroups", "GroupID", "dbo.Groups");
            DropIndex("dbo.ProductGroups", new[] { "GroupID" });
            DropPrimaryKey("dbo.ProductGroups");
            DropPrimaryKey("dbo.Groups");
            AlterColumn("dbo.ProductGroups", "GroupID", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.Groups", "ID", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AddPrimaryKey("dbo.ProductGroups", new[] { "ProductID", "GroupID" });
            AddPrimaryKey("dbo.Groups", "ID");
            CreateIndex("dbo.ProductGroups", "GroupID");
            AddForeignKey("dbo.ProductGroups", "GroupID", "dbo.Groups", "ID", cascadeDelete: true);
        }
    }
}
