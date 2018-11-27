namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDatabseModelData : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE StateOfPlaces SET Name = 'Volno' WHERE Id = 1");
            Sql("UPDATE StateOfPlaces SET Name = 'Obsazeno' WHERE Id = 2");
            Sql("UPDATE StateOfPlaces SET Name = 'Rezervováno' WHERE Id = 3");
            Sql("UPDATE StateOfPlaces SET Name = 'Zamìstnanec' WHERE Id = 4");

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
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 1' WHERE Id = 20");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 2' WHERE Id = 21");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 3' WHERE Id = 22");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 4' WHERE Id = 23");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 5' WHERE Id = 24");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 6' WHERE Id = 25");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 7' WHERE Id = 26");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 8' WHERE Id = 27");
            Sql("UPDATE ParkingPlaces SET Name = 'Staff 9' WHERE Id = 28");
        }
        
        public override void Down()
        {
            Sql("UPDATE StateOfOlaces SET Name = 'Free' WHERE Id = 1");
            Sql("UPDATE StateOfOlaces SET Name = 'Occupied' WHERE Id = 2");
            Sql("UPDATE StateOfOlaces SET Name = 'Reserved' WHERE Id = 3");
            Sql("UPDATE StateOfOlaces SET Name = 'OccupiedByStaff' WHERE Id = 4");

            Sql("UPDATE ParkingPlaces SET Name = 'Garage1' WHERE Id = 1");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage2' WHERE Id = 2");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage3' WHERE Id = 3");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage4' WHERE Id = 4");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage5' WHERE Id = 5");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage6' WHERE Id = 6");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage7' WHERE Id = 7");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage8' WHERE Id = 8");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage9' WHERE Id = 9");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage10' WHERE Id = 10");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage11' WHERE Id = 11");
            Sql("UPDATE ParkingPlaces SET Name = 'Garage12' WHERE Id = 12");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside1' WHERE Id = 13");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside2' WHERE Id = 14");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside3' WHERE Id = 15");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside4' WHERE Id = 16");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside5' WHERE Id = 17");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside6' WHERE Id = 18");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside7' WHERE Id = 19");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside11' WHERE Id = 20");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside12' WHERE Id = 21");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside21' WHERE Id = 22");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside31' WHERE Id = 23");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside32' WHERE Id = 24");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside41' WHERE Id = 25");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside42' WHERE Id = 26");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside43' WHERE Id = 27");
            Sql("UPDATE ParkingPlaces SET Name = 'Outside44' WHERE Id = 28");
        }
    }
}
