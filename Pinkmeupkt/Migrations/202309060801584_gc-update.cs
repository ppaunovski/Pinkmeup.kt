namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gcupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Giftcards", "AppointmentTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Giftcards", "AppointmentTime");
        }
    }
}
