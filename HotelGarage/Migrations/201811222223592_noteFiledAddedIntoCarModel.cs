namespace HotelGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noteFiledAddedIntoCarModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Note", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Note");
        }
    }
}
