namespace BankOfBIT_BC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing_transactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Withdrawal", c => c.Double());
            DropColumn("dbo.Transactions", "withdrawal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "withdrawal", c => c.Double());
            DropColumn("dbo.Transactions", "Withdrawal");
        }
    }
}
