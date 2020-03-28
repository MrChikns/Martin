namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEnumPt3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ParkingPlaces", "Label");

            Sql("UPDATE ParkingPlaces SET State = '1'");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParkingPlaces", "Label", c => c.String());
        }
    }
}
