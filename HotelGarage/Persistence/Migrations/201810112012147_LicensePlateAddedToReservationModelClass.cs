namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LicensePlateAddedToReservationModelClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "LicensePlate", c => c.String(nullable: false));
            DropColumn("dbo.Reservations", "CarId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "CarId", c => c.String(nullable: false));
            DropColumn("dbo.Reservations", "LicensePlate");
        }
    }
}
