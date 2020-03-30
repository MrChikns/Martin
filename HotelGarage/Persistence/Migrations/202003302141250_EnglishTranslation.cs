namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnglishTranslation : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 1' WHERE Id = 1");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 2' WHERE Id = 2");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 3' WHERE Id = 3");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 4' WHERE Id = 4");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 5' WHERE Id = 5");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 6' WHERE Id = 6");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 7' WHERE Id = 7");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 8' WHERE Id = 8");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 9' WHERE Id = 9");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 10' WHERE Id = 10");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 11' WHERE Id = 11");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage 12' WHERE Id = 12");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 1' WHERE Id = 13");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 2' WHERE Id = 14");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 3' WHERE Id = 15");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 4' WHERE Id = 16");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 5' WHERE Id = 17");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 6' WHERE Id = 18");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside 7' WHERE Id = 19");
        }
        
        public override void Down()
        {
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 1' WHERE Id = 1");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 2' WHERE Id = 2");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 3' WHERE Id = 3");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 4' WHERE Id = 4");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 5' WHERE Id = 5");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 6' WHERE Id = 6");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 7' WHERE Id = 7");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 8' WHERE Id = 8");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 9' WHERE Id = 9");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 10' WHERE Id = 10");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 11' WHERE Id = 11");
            Sql("UPDATE ParkingPlaces SET Name = 'Garáž 12' WHERE Id = 12");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 1' WHERE Id = 13");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 2' WHERE Id = 14");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 3' WHERE Id = 15");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 4' WHERE Id = 16");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 5' WHERE Id = 17");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 6' WHERE Id = 18");
            Sql("UPDATE ParkingPlaces SET Name = 'Venku 7' WHERE Id = 19");
        }
    }
}
