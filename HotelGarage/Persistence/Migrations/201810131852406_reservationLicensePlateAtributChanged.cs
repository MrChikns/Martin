namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reservationLicensePlateAtributChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "LicensePlate", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservations", "LicensePlate", c => c.String(nullable: false));
        }
    }
}
