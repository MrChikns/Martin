namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reservationModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "StateOfReservation_Id", "dbo.StateOfReservations");
            DropIndex("dbo.Reservations", new[] { "StateOfReservation_Id" });
            DropColumn("dbo.Reservations", "StateOfReservation_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "StateOfReservation_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "StateOfReservation_Id");
            AddForeignKey("dbo.Reservations", "StateOfReservation_Id", "dbo.StateOfReservations", "Id");
        }
    }
}
