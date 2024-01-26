namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimgtooffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Title", c => c.String());
            AddColumn("dbo.Offers", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "ImagePath");
            DropColumn("dbo.Offers", "Title");
        }
    }
}
