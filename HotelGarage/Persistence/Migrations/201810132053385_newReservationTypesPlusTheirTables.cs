namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newReservationTypesPlusTheirTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Discriminator");
        }
    }
}
