namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileImages", "ProdutImageID", "dbo.Products");
            DropIndex("dbo.FileImages", new[] { "ProdutImageID" });
            AddColumn("dbo.Products", "MoreImages", c => c.String(storeType: "xml"));
            DropTable("dbo.FileImages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FileImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Extension = c.String(),
                        ProdutImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Products", "MoreImages");
            CreateIndex("dbo.FileImages", "ProdutImageID");
            AddForeignKey("dbo.FileImages", "ProdutImageID", "dbo.Products", "ID", cascadeDelete: true);
        }
    }
}
