namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCarIdToReservationModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Reservations", new[] { "Car_Id" });
            AddColumn("dbo.Reservations", "CarId", c => c.String(nullable: false));
            AlterColumn("dbo.Reservations", "Car_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "Car_Id");
            AddForeignKey("dbo.Reservations", "Car_Id", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Reservations", new[] { "Car_Id" });
            AlterColumn("dbo.Reservations", "Car_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "CarId");
            CreateIndex("dbo.Reservations", "Car_Id");
            AddForeignKey("dbo.Reservations", "Car_Id", "dbo.Cars", "Id", cascadeDelete: true);
        }
    }
}
