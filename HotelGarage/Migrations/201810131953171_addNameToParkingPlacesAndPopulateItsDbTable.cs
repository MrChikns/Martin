namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameToParkingPlacesAndPopulateItsDbTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParkingPlaces", "Name", c => c.String());

            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage1','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage2','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage3','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage4','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage5','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage6','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage7','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage8','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage9','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage10','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage11','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Garage12','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside1','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside2','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside3','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside4','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside5','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside6','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside7','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside11','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside12','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside21','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside31','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside32','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside41','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside42','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside43','1')");
            Sql("INSERT INTO ParkingPlaces (Name, StateOfPlaceId) VALUES ('Outside44','1')");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParkingPlaces", "Name");
        }
    }
}
