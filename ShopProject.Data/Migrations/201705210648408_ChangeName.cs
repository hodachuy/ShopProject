namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FileImages", name: "ProdutImageID", newName: "ProductImageID");
            RenameIndex(table: "dbo.FileImages", name: "IX_ProdutImageID", newName: "IX_ProductImageID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.FileImages", name: "IX_ProductImageID", newName: "IX_ProdutImageID");
            RenameColumn(table: "dbo.FileImages", name: "ProductImageID", newName: "ProdutImageID");
        }
    }
}
