namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix0imgoffer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "Title", c => c.String());
        }
    }
}
