namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gcupdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Giftcards", "AppointmentTimeString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Giftcards", "AppointmentTimeString");
        }
    }
}
