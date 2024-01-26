namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gcs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Giftcards",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Surname = c.String(nullable: false),
                    Phone = c.String(),
                    SendRecipientEmail = c.Boolean(nullable: false),
                    TimeOfPurchase = c.DateTime(nullable: false),
                    Message = c.String(),
                    OfferId = c.Int(nullable: false),
                    Buyer_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Buyer_Id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.OfferId)
                .Index(t => t.Buyer_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Giftcards", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.Giftcards", "Buyer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Giftcards", new[] { "Buyer_Id" });
            DropIndex("dbo.Giftcards", new[] { "OfferId" });
            DropTable("dbo.Giftcards");
        }
    }
}
