namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOfCarProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "GuestRoomNumber", c => c.Int());
            AlterColumn("dbo.Cars", "PricePerNight", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "PricePerNight", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "GuestRoomNumber", c => c.Int(nullable: false));
        }
    }
}
