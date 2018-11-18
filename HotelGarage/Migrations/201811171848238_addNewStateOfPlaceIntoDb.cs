namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewStateOfReservationIntoDb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO StateOfReservations (State) VALUES ('TemporaryLeave')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM StateOfReservations WHERE State='TemporaryLeave'");
        }
    }
}
