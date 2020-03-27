namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEnumPt2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.StateOfReservations");
            DropTable("dbo.StateOfPlaces");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StateOfPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StateOfReservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
