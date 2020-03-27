namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class introduceEnums : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ParkingPlaces", "StateOfPlaceId", "dbo.StateOfPlaces");
            DropIndex("dbo.ParkingPlaces", new[] { "StateOfPlaceId" });
            AddColumn("dbo.ParkingPlaces", "Label", c => c.String());
            AddColumn("dbo.ParkingPlaces", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "State", c => c.Int(nullable: false));
            DropColumn("dbo.ParkingPlaces", "StateOfPlaceId");
            DropColumn("dbo.Reservations", "StateOfReservationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "StateOfReservationId", c => c.Byte(nullable: false));
            AddColumn("dbo.ParkingPlaces", "StateOfPlaceId", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "State");
            DropColumn("dbo.ParkingPlaces", "State");
            DropColumn("dbo.ParkingPlaces", "Label");
            CreateIndex("dbo.ParkingPlaces", "StateOfPlaceId");
            AddForeignKey("dbo.ParkingPlaces", "StateOfPlaceId", "dbo.StateOfPlaces", "Id", cascadeDelete: true);
        }
    }
}
