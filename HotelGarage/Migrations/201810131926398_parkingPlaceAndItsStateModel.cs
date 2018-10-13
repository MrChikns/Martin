namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parkingPlaceAndItsStateModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateOfPlaceId = c.Int(nullable: false),
                        Reservation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservations", t => t.Reservation_Id)
                .ForeignKey("dbo.StateOfPlaces", t => t.StateOfPlaceId, cascadeDelete: true)
                .Index(t => t.StateOfPlaceId)
                .Index(t => t.Reservation_Id);
            
            CreateTable(
                "dbo.StateOfPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            Sql("INSERT INTO StateOfPlaces (Name) VALUES ('Free')");
            Sql("INSERT INTO StateOfPlaces (Name) VALUES ('Occupied')");
            Sql("INSERT INTO StateOfPlaces (Name) VALUES ('Reserved')");
            Sql("INSERT INTO StateOfPlaces (Name) VALUES ('OccupiedByStaff')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkingPlaces", "StateOfPlaceId", "dbo.StateOfPlaces");
            DropForeignKey("dbo.ParkingPlaces", "Reservation_Id", "dbo.Reservations");
            DropIndex("dbo.ParkingPlaces", new[] { "Reservation_Id" });
            DropIndex("dbo.ParkingPlaces", new[] { "StateOfPlaceId" });
            DropTable("dbo.StateOfPlaces");
            DropTable("dbo.ParkingPlaces");
        }
    }
}
