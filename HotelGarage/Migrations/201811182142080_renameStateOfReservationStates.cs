namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameStateOfReservationStates : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE StateOfReservations SET State = 'Rezervovan�' WHERE Id = 1");
            Sql("UPDATE StateOfReservations SET State = 'Ubytovan�' WHERE Id = 2");
            Sql("UPDATE StateOfReservations SET State = 'Odjet�' WHERE Id = 3");
            Sql("UPDATE StateOfReservations SET State = 'Zru�en�' WHERE Id = 4");
            Sql("UPDATE StateOfReservations SET State = 'Do�asn� V�jezd' WHERE Id = 5");
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
