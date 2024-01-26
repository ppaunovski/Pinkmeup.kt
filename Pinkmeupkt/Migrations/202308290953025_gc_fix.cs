namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gc_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Giftcards", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Giftcards", "Email");
        }
    }
}
