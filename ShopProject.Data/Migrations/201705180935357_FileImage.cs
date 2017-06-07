namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileImage : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProdutImageID, cascadeDelete: true)
                .Index(t => t.ProdutImageID);
            
            DropColumn("dbo.Products", "MoreImages");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "MoreImages", c => c.String(storeType: "xml"));
            DropForeignKey("dbo.FileImages", "ProdutImageID", "dbo.Products");
            DropIndex("dbo.FileImages", new[] { "ProdutImageID" });
            DropTable("dbo.FileImages");
        }
    }
}
