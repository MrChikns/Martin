namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStateOfReservationToModelAndPopulateItsStates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StateOfReservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Reservations", "StateOfReservation_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "StateOfReservation_Id");
            AddForeignKey("dbo.Reservations", "StateOfReservation_Id", "dbo.StateOfReservations", "Id");
            DropColumn("dbo.Reservations", "Discriminator");

            Sql("INSERT INTO StateOfReservations (State) VALUES ('Arrival')");
            Sql("INSERT INTO StateOfReservations (State) VALUES ('Inhouse')");
            Sql("INSERT INTO StateOfReservations (State) VALUES ('Departed')");
            Sql("INSERT INTO StateOfReservations (State) VALUES ('Cancelled')");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Reservations", "StateOfReservation_Id", "dbo.StateOfReservations");
            DropIndex("dbo.Reservations", new[] { "StateOfReservation_Id" });
            DropColumn("dbo.Reservations", "StateOfReservation_Id");
            DropTable("dbo.StateOfReservations");
        }
    }
}
