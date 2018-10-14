namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParkingPlaceIdToReservationModelClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "ParkingPlaceId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "ParkingPlaceId");
        }
    }
}
