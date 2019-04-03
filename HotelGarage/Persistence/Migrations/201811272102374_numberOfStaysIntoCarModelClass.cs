namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numberOfStaysIntoCarModelClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "NumberOfStays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "NumberOfStays");
        }
    }
}
