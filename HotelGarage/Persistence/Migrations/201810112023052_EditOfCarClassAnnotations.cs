namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOfCarClassAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "GuestRoomNumber", c => c.Int(nullable: true));
            AlterColumn("dbo.Cars", "PricePerNight", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "GuestRoomNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "PricePerNight", c => c.Int(nullable: false));
        }
    }
}
