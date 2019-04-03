namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameStateOfReservationStates : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE StateOfReservations SET State = 'Rezervovaná' WHERE Id = 1");
            Sql("UPDATE StateOfReservations SET State = 'Ubytovaná' WHERE Id = 2");
            Sql("UPDATE StateOfReservations SET State = 'Odjetá' WHERE Id = 3");
            Sql("UPDATE StateOfReservations SET State = 'Zrušená' WHERE Id = 4");
            Sql("UPDATE StateOfReservations SET State = 'Doèasný Výjezd' WHERE Id = 5");
        }
        
        public override void Down()
        {
            Sql("UPDATE StateOfReservations SET State = 'Reserved' WHERE Id = 1");
            Sql("UPDATE StateOfReservations SET State = 'Inhouse' WHERE Id = 2");
            Sql("UPDATE StateOfReservations SET State = 'Departed' WHERE Id = 3");
            Sql("UPDATE StateOfReservations SET State = 'Cancelled' WHERE Id = 4");
            Sql("UPDATE StateOfReservations SET State = 'TemporaryLeave' WHERE Id = 5");
        }
    }
}
