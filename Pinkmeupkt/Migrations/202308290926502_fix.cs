namespace Pinkmeupkt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
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
                    EmailAddress = c.String(),
                    SendRecipientEmail = c.Boolean(nullable: false),
                    TimeOfPurchase = c.DateTime(nullable: false),
                    Message = c.String(),
                    Buyer_Id = c.String(maxLength: 128),
                    Offer_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.Giftcards", "Offer_Id");
            CreateIndex("dbo.Giftcards", "Buyer_Id");
            AddForeignKey("dbo.Giftcards", "Offer_Id", "dbo.Offers", "Id");
            AddForeignKey("dbo.Giftcards", "Buyer_Id", "dbo.AspNetUsers", "Id");

            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Giftcards", "Buyer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Giftcards", "Offer_Id", "dbo.Offers");
            DropIndex("dbo.Giftcards", new[] { "Buyer_Id" });
            DropIndex("dbo.Giftcards", new[] { "Offer_Id" });
            DropTable("dbo.Giftcards");
        }
    }
}
