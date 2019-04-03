namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsAndReservationsModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "LicensePlate", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "LicensePlate", c => c.String(nullable: false));
        }
    }
}
