namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stateOfReservationModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "StateOfReservationId", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "StateOfReservationId");
        }
    }
}
