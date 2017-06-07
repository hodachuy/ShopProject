namespace ShopProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationRoles", "Discriminator", c => c.String(maxLength: 128));

            AddColumn("dbo.ApplicationRoles", "Discriminator", c => c.String(maxLength: 128));
        }
    }
}
