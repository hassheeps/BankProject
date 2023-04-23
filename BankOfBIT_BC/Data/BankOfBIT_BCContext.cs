using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankOfBIT_BC.Data
{
    public class BankOfBIT_BCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BankOfBIT_BCContext() : base("name=BankOfBIT_BCContext")
        {
        }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.AccountState> AccountStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.BronzeState> BronzeStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.SilverState> SilverStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.GoldState> GoldStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.PlatinumState> PlatinumStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.ChequingAccount> ChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.InvestmentAccount> InvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.MortgageAccount> MortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.SavingsAccount> SavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextUniqueNumber> NextUniqueNumbers { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextSavingsAccount> NextSavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextMortgageAccount> NextMortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextInvestmentAccount> NextInvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextChequingAccount> NextChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextTransaction> NextTransactions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.NextClient> NextClients { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.Payee> Payees { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.Institution> Institutions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.TransactionType> TransactionTypes { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_BC.Models.Transaction> Transactions { get; set; }
    }
}
