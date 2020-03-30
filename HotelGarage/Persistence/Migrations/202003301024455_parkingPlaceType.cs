namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parkingPlaceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParkingPlaces", "Type", c => c.Int(nullable: false));

            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 1");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 2");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 3");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 4");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 5");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 6");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 7");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 8");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 9");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 10");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 11");
            Sql("UPDATE ParkingPlaces SET Type = '1' WHERE Id = 12");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 13");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 14");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 15");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 16");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 17");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 18");
            Sql("UPDATE ParkingPlaces SET Type = '2' WHERE Id = 19");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 20");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 21");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 22");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 23");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 24");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 25");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 26");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 27");
            Sql("UPDATE ParkingPlaces SET Type = '3' WHERE Id = 28");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParkingPlaces", "Type");
        }
    }
}
