namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAliasToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Alias");
        }
    }
}
