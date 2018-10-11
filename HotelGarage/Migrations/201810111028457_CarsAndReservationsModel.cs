namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsAndReservationsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicensePlate = c.String(nullable: false),
                        CarModel = c.String(maxLength: 50),
                        GuestsName = c.String(maxLength: 50),
                        GuestRoomNumber = c.Int(nullable: false),
                        PricePerNight = c.Int(nullable: false),
                        IsEmployee = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Arrival = c.DateTime(nullable: false),
                        Departure = c.DateTime(nullable: false),
                        IsRegistered = c.Boolean(nullable: false),
                        Car_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id, cascadeDelete: true)
                .Index(t => t.Car_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Reservations", new[] { "Car_Id" });
            DropTable("dbo.Reservations");
            DropTable("dbo.Cars");
        }
    }
}
