namespace londoncoffeeService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCafeIdToReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("londoncoffee.Reviews", "CafeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("londoncoffee.Reviews", "CafeId");
        }
    }
}
