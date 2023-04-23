using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility;
using BankOfBIT_BC.Models;
using BankOfBIT_BC.Data;
using System.Data.SqlClient;
using System.Data;

namespace BankOfBIT_BC.Models
{
    /// <summary>
    /// Represents a client.
    /// </summary>
    public class Client
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Display(Name = "Client\nNumber")]
        public long ClientNumber { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First\nName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last\nName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string Address { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string City { get; set; }

        [Required]
        [RegularExpression("^(N[BLSTU]|[AMN]B|[BQ]C|ON|PE|SK|YT)", ErrorMessage = "Valid province code required")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateTime { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}",
                FirstName, LastName);
            }
        }

        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return String.Format("{0} {1} {2}",
                Address, City, Province);
            }
        }

        /// <summary>
        /// Sets the next client number
        /// </summary>
        public void SetNextClientNumber()
        {
            this.ClientNumber = (long)StoredProcedure.NextNumber("NextClient");
        }


        /// <summary>
        /// Navigation property
        /// </summary>
        public virtual ICollection<BankAccount> BankAccount { get; set; }
    }

    /// <summary>
    /// Represents the state of the account.
    /// </summary>
    public abstract class AccountState
    {
        /// <summary>
        /// Instantiates the db object used for account state instances
        /// </summary>
        protected static BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AccountStateId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Display(Name = "Lower\nLimit")]
        public double LowerLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Display(Name = "Upper\nLimit")]
        public double UpperLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public double Rate { get; set; }

        [Display(Name = "Account\nState")]
        public string Description
        {
            get
            {
                return BusinessRules.ParseName(GetType().Name, "State");
            }
        }

        // Navigation Property
        public virtual ICollection<BankAccount> BankAccount { get; set; }

        // Abstract functions
        public abstract double RateAdjustment(BankAccount bankAccount);

        public abstract void StateChangeCheck(BankAccount bankAccount);


    }

    /// <summary>
    /// Represents the bronze state.
    /// </summary>
    public class BronzeState : AccountState
    {
        private static BronzeState bronzeState;
        private const double LOWER_LIMIT = 0;
        private const double UPPER_LIMIT = 5000;
        private const double BRONZE_RATE = 0.01;

        /// <summary>
        /// Creates an instance of the Bronze Account State
        /// </summary>
        private BronzeState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = BRONZE_RATE;
        }

        /// <summary>
        /// Returns the current instance of the Bronze Account State
        /// </summary>
        /// <returns>Bronze State</returns>
        public static BronzeState GetInstance()
        {
            if (bronzeState == null)
            {
                bronzeState = db.BronzeStates.SingleOrDefault();

                if (bronzeState == null)
                {
                    bronzeState = new BronzeState();
                    db.BronzeStates.Add(bronzeState);
                    db.SaveChanges();
                }
            }

            return bronzeState;
        }

        /// <summary>
        /// Checks whether the given bank account is still within the boundaries of bronze state.
        /// </summary>
        /// <param name="bankAccount">Bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance > this.UpperLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Adjusts the rate of a bronze state bank account
        /// </summary>
        /// <param name="bankAccount">bank account</param>
        /// <returns>Rate</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            if (bankAccount.Balance <= 0)
            {
                this.Rate = BRONZE_RATE;
            }
            else
            {
                this.Rate = 0.055;
            }

            return this.Rate;
        }
    }

    /// <summary>
    /// Represents the silver state.
    /// </summary>
    public class SilverState : AccountState
    {
        private static SilverState silverState;
        private const double LOWER_LIMIT = 5000;
        private const double UPPER_LIMIT = 10000;
        private const double SILVER_RATE = 0.0125;

        /// <summary>
        /// Creates an instance of the Silver Account State
        /// </summary>
        private SilverState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = SILVER_RATE;
        }

        /// <summary>
        /// Returns the current instance of the Silver Account State
        /// </summary>
        /// <returns>Silver State</returns>
        public static SilverState GetInstance()
        {
            if (silverState == null)
            {
                silverState = db.SilverStates.SingleOrDefault();

                if (silverState == null)
                {
                    silverState = new SilverState();
                    db.SilverStates.Add(silverState);
                    db.SaveChanges();
                }
            }

            return silverState;
        }

        /// <summary>
        /// Checks whether the given bank account is still within the boundaries of silver state.
        /// </summary>
        /// <param name="bankAccount">Bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance > this.UpperLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }
            if (bankAccount.Balance < this.LowerLimit)
            {
                bankAccount.AccountStateId = BronzeState.GetInstance().AccountStateId;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Adjusts the rate of a silver state bank account
        /// </summary>
        /// <param name="bankAccount">bank account</param>
        /// <returns>Rate</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            this.Rate = SILVER_RATE;

            return this.Rate;
        }

    }

    /// <summary>
    /// Represents the gold state.
    /// </summary>
    public class GoldState : AccountState
    {
        private static GoldState goldState;
        private const double LOWER_LIMIT = 10000;
        private const double UPPER_LIMIT = 20000;
        private const double GOLD_RATE = 0.02;

        /// <summary>
        /// Creates an instance of the Gold Account State
        /// </summary>
        private GoldState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = GOLD_RATE;
        }

        /// <summary>
        /// Returns the current instance of the Gold Account State
        /// </summary>
        /// <returns>Gold State</returns>
        public static GoldState GetInstance()
        {
            if (goldState == null)
            {
                goldState = db.GoldStates.SingleOrDefault();

                if (goldState == null)
                {
                    goldState = new GoldState();
                    db.GoldStates.Add(goldState);
                    db.SaveChanges();
                }
            }

            return goldState;
        }

        /// <summary>
        /// Checks to see if the bank account balance is within the boundaries of the gold state.
        /// </summary>
        /// <param name="bankAccount">bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance > this.UpperLimit)
            {
                bankAccount.AccountStateId = PlatinumState.GetInstance().AccountStateId;
            }
            if (bankAccount.Balance < this.LowerLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Adjusts the rate of a gold state bank account
        /// </summary>
        /// <param name="bankAccount">bank account</param>
        /// <returns>Rate</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            if (DateTime.Now.Year - bankAccount.DateCreated.Year >= 10)
            {
                this.Rate = GOLD_RATE + 0.01;
            }
            else
            {
                this.Rate = GOLD_RATE;
            }

            return this.Rate;
        }

    }

    /// <summary>
    /// Represents the platinum state.
    /// </summary>
    public class PlatinumState : AccountState
    {
        private static PlatinumState platinumState;
        private const double LOWER_LIMIT = 20000;
        private const double UPPER_LIMIT = 0;
        private const double PLATINUM_RATE = 0.025;

        /// <summary>
        /// Creates an instance of the Platinum Account State
        /// </summary>
        private PlatinumState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = PLATINUM_RATE;
        }

        /// <summary>
        /// Returns the current instance of the Platinum Account State
        /// </summary>
        /// <returns>Platinum State</returns>
        public static PlatinumState GetInstance()
        {
            if (platinumState == null)
            {
                platinumState = db.PlatinumStates.SingleOrDefault();

                if (platinumState == null)
                {
                    platinumState = new PlatinumState();
                    db.PlatinumStates.Add(platinumState);
                    db.SaveChanges();
                }
            }

            return platinumState;
        }

        /// <summary>
        /// Checks to see if the bank account balance is within the boundaries of the platinum state.
        /// </summary>
        /// <param name="bankAccount">bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < this.LowerLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Adjusts the rate of a platinum state bank account
        /// </summary>
        /// <param name="bankAccount">the bank account</param>
        /// <returns>the rate</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {

            //local variable instead of this.
            if (DateTime.Now.Year - bankAccount.DateCreated.Year >= 10)
            {
                this.Rate = PLATINUM_RATE + 0.01;
            }
            if (bankAccount.Balance > (LOWER_LIMIT * 2))
            {
                this.Rate += 0.005;
            }
            else
            {
                this.Rate = PLATINUM_RATE;
            }

            return this.Rate;
        }
    }

    /// <summary>
    /// Represents a bank account.
    /// </summary>
    public abstract class BankAccount
    {
        private BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BankAccountId { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [ForeignKey("AccountState")]
        public int AccountStateId { get; set; }

        [Display(Name = "Account\nNumber")]
        public long AccountNumber { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double Balance { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Date\nCreated")]
        public DateTime DateCreated { get; set; }

        public string Notes { get; set; }

        public string Description
        {
            get
            {
                return BusinessRules.ParseName(GetType().Name, "Account");
            }
        }

        /// <summary>
        /// Changes the state of a particular account based on upper and lower boundaries
        /// </summary>
        public void ChangeState()
        {
            AccountState record = (from Results in db.AccountStates where Results.AccountStateId == this.AccountStateId select Results).SingleOrDefault();

            int prevID = 0;

            while (prevID != record.AccountStateId)
            {
                record.StateChangeCheck(this);
                prevID = record.AccountStateId;
                record = (from Results in db.AccountStates where Results.AccountStateId == this.AccountStateId select Results).SingleOrDefault();
            }


        }

        public abstract void SetNextAccountNumber();

        // Navigation
        public virtual ICollection<Transaction> Transaction { get; set; }

        // Foreign Key Connections
        public virtual AccountState AccountState { get; set; }
        public virtual Client Client { get; set; }

    }

    /// <summary>
    /// Represents a savings account.
    /// </summary>
    public class SavingsAccount : BankAccount
    {

        [Required]
        [Display(Name = "Savings\nService\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double SavingsServiceCharges { get; set; }


        /// <summary>
        /// Sets the next account number for this bank account type.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("NextSavingsAccount");
        }

    }

    /// <summary>
    /// Represents a mortgage account.
    /// </summary>
    public class MortgageAccount : BankAccount
    {
        [Required]
        [Display(Name = "Mortgage\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        public double MortgageRate { get; set; }

        [Required]
        public int Amortization { get; set; }

        /// <summary>
        /// Sets the next account number for this bank account type.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("NextMortgageAccount");
        }
    }

    /// <summary>
    /// Represents an investment account.
    /// </summary>
    public class InvestmentAccount : BankAccount
    {
        [Required]
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        public double InterestRate { get; set; }

        /// <summary>
        /// Sets the next account number for this bank account type.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("NextInvestmentAccount");
        }
    }

    /// <summary>
    /// Represents a chequing account.
    /// </summary>
    public class ChequingAccount : BankAccount
    {
        [Required]
        [Display(Name = "Chequing\nService\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double ChequingServiceCharge { get; set; }

        /// <summary>
        /// Sets the next account number for this bank account type.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("NextChequingAccount");
        }
    }

    /// <summary>
    /// Represents a payee.
    /// </summary>
    public class Payee
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PayeeId { get; set; }

        [Required]
        [Display(Name = "Payee")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents an institution.
    /// </summary>
    public class Institution
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int InstitutionId { get; set; }

        [Required]
        [Display(Name = "Number")]
        public int InstitutionNumber { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public string Description { get; set; }

    }

    /// <summary>
    /// Represents a transaction type.
    /// </summary>
    public class TransactionType
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Description { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }


    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [ForeignKey("BankAccount")]
        public int BankAccountId { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        [Display(Name = "Number")]
        public long TransactionNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double? Deposit { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double? Withdrawal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Date")]
        public DateTime DateCreated { get; set; }

        public string Notes { get; set; }

        public void SetNextTransactionNumber()
        {
            this.TransactionNumber = (long)StoredProcedure.NextNumber("NextTransaction");
        }

        // Foreign Keys

        public virtual BankAccount BankAccount { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }

    /// <summary>
    /// An abstract class representing the next unique number.
    /// </summary>
    public abstract class NextUniqueNumber
    {
        protected static BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NextUniqueNumberId { get; set; }

        [Required]
        public long NextAvailableNumber { get; set; }

        
    }

    /// <summary>
    /// Represents the next available savings account
    /// </summary>
    public class NextSavingsAccount : NextUniqueNumber
    {
        private static NextSavingsAccount nextSavingsAccount;

        /// <summary>
        /// Creates instance of NextSavingsAccount
        /// </summary>
        private NextSavingsAccount()
        {
            this.NextAvailableNumber = 20000;
        }

        /// <summary>
        /// Returns an instance of the next savings account
        /// </summary>
        /// <returns>The next savings account</returns>
        public static NextSavingsAccount GetInstance()
        {
            if(nextSavingsAccount == null)
            {
                nextSavingsAccount = db.NextSavingsAccounts.SingleOrDefault();

                if(nextSavingsAccount == null)
                {
                    nextSavingsAccount = new NextSavingsAccount();
                    db.NextSavingsAccounts.Add(nextSavingsAccount);
                    db.SaveChanges();
                }
            }

            return nextSavingsAccount;
        }
    }

    /// <summary>
    /// Represents the next available savings account
    /// </summary>
    public class NextMortgageAccount : NextUniqueNumber
    {
        private static NextMortgageAccount nextMortgageAccount;

        /// <summary>
        /// Creates instance of NextMortgageAccount
        /// </summary>
        private NextMortgageAccount()
        {
            this.NextAvailableNumber = 200000;
        }

        /// <summary>
        /// Returns an instance of the next mortgage account
        /// </summary>
        /// <returns>The next mortgage account</returns>
        public static NextMortgageAccount GetInstance()
        {
            if(nextMortgageAccount == null)
            {
                nextMortgageAccount = db.NextMortgageAccounts.SingleOrDefault();

                if(nextMortgageAccount == null)
                {
                    nextMortgageAccount = new NextMortgageAccount();
                    db.NextMortgageAccounts.Add(nextMortgageAccount);
                    db.SaveChanges();
                }
            }

            return nextMortgageAccount;
        }
    }

    /// <summary>
    /// Reprsents the next available investment account
    /// </summary>
    public class NextInvestmentAccount : NextUniqueNumber
    {
        private static NextInvestmentAccount nextInvestmentAccount;

        /// <summary>
        /// Creates instance of NextInvestmentAccount
        /// </summary>
        private NextInvestmentAccount()
        {
            this.NextAvailableNumber = 2000000;
        }

        /// <summary>
        /// Returns an instance of the next investment account
        /// </summary>
        /// <returns>The next investment account</returns>
        public static NextInvestmentAccount GetInstance()
        {
            if(nextInvestmentAccount == null)
            {
                nextInvestmentAccount = db.NextInvestmentAccounts.SingleOrDefault();

                if(nextInvestmentAccount == null)
                {
                    nextInvestmentAccount = new NextInvestmentAccount();
                    db.NextInvestmentAccounts.Add(nextInvestmentAccount);
                    db.SaveChanges();
                }
            }

            return nextInvestmentAccount;

        }
    }

    public class NextChequingAccount : NextUniqueNumber
    {
        private static NextChequingAccount nextChequingAccount;

        /// <summary>
        /// Creates instance of NextChequingAccount
        /// </summary>
        private NextChequingAccount()
        {
            this.NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Returns an instance of the next chequing account
        /// </summary>
        /// <returns>The next chequing account</returns>
        public static NextChequingAccount GetInstance()
        {
            if(nextChequingAccount == null)
            {
                nextChequingAccount = db.NextChequingAccounts.SingleOrDefault();

                if(nextChequingAccount == null)
                {
                    nextChequingAccount = new NextChequingAccount();
                    db.NextChequingAccounts.Add(nextChequingAccount);
                    db.SaveChanges();
                }
            }

            return nextChequingAccount;
        }
    }

    /// <summary>
    /// Represents the next client.
    /// </summary>
    public class NextClient : NextUniqueNumber
    {
        private static NextClient nextClient;

        /// <summary>
        /// Creates instance of NextClient
        /// </summary>
        private NextClient()
        {
            this.NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Returns an instance of the next client
        /// </summary>
        /// <returns>The next client</returns>
        public static NextClient GetInstance()
        {
            if(nextClient == null)
            {
                nextClient = db.NextClients.SingleOrDefault();

                if(nextClient == null)
                {
                    nextClient = new NextClient();
                    db.NextClients.Add(nextClient);
                    db.SaveChanges();

                }
            }

            return nextClient;
        }
    }

    /// <summary>
    /// Represents the next transaction.
    /// </summary>
    public class NextTransaction : NextUniqueNumber
    {
        private static NextTransaction nextTransaction;

        /// <summary>
        /// Creates instance of NextTransaction
        /// </summary>
        private NextTransaction()
        {
            this.NextAvailableNumber = 700;
        }

        public static NextTransaction GetInstance()
        {
            if(nextTransaction == null)
            {
                nextTransaction = db.NextTransactions.SingleOrDefault();

                if(nextTransaction == null)
                {
                    nextTransaction = new NextTransaction();
                    db.NextTransactions.Add(nextTransaction);
                    db.SaveChanges();
                }
            }

            return nextTransaction;
        }
    }

    public static class StoredProcedure
    {
        /// <summary>
        /// Runs the next_number procedure stored in the BankOfBIT_BCContext database.
        /// </summary>
        /// <param name="discriminator"></param>
        /// <returns>The result of the next_number procedure</returns>
        public static long? NextNumber(string discriminator)
        {
            try
            {
                // Creates new SQL connection using my local machine as the data source host, specifically this project context. 
                SqlConnection connection = new SqlConnection("Data Source=localhost; " + "Initial Catalog=BankOfBIT_BCContext;Integrated Security=True");

                // This procedure will return a long? value which is initially set to 0.
                long? returnValue = 0;

                // Creates new SQL command to run the stored procedure "next_number" in the previously established SQL connection.
                SqlCommand storedProcedure = new SqlCommand("next_number", connection);

                // Assigns the command type to the storedProcedure SQL command.
                storedProcedure.CommandType = CommandType.StoredProcedure;

                // Provides the input parameters for the SQL command.
                storedProcedure.Parameters.AddWithValue("@Discriminator", discriminator);

                // Creates the output parameter for the SQL command.
                SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };

                // Connects the output parameter to the storedProcedure command
                storedProcedure.Parameters.Add(outputParameter);

                // Opens the previously defined connection so that the procedure may be executed.
                connection.Open();

                // Executes the procedure with the established parameters
                storedProcedure.ExecuteNonQuery();

                // Closes the connection once the procedure has run.
                connection.Close();

                // Assigns the output parameter value of the procedure to the return value of this method.
                returnValue = (long?)outputParameter.Value;

                // Returns the value in long? form
                return returnValue;
            }
            catch (Exception)
            {
                long? returnValue = null;

                return returnValue;
            }
        }
    }

}