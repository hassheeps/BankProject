using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBIT_BC;
using BankOfBIT_BC.Data;
using BankOfBIT_BC.Models;
using Utility;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionManager" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransactionManager.svc or TransactionManager.svc.cs at the Solution Explorer and start debugging.
    public class TransactionManager : ITransactionManager
    {
        private BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// Implementation will create a bill payment transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountId">Represents the account ID</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance.</returns>
        public double? BillPayment(int accountId, double amount, string notes)
        {
            double? balance = UpdateBalance(accountId, -amount);
            CreateTransaction(accountId, -amount, (int)TransactionTypeValues.BILL_PAYMENT, notes);

            try
            {
                return balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will calculate the interest of an account based on the argument values.
        /// </summary>
        /// <param name="accountId">Represents the account ID.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns></returns>
        public double? CalculateInterest(int accountId, string notes)
        {
            BankAccount bankAccount = (from results in db.BankAccounts where results.BankAccountId == accountId select results).SingleOrDefault();
            AccountState accountState = bankAccount.AccountState;
            
            double rate = accountState.RateAdjustment(bankAccount);
            double amount = (rate * bankAccount.Balance * 1) / 12;

            double? balance = UpdateBalance(accountId, amount);
            CreateTransaction(accountId, amount, (int)TransactionTypeValues.INTEREST, notes);

            try
            {
                return balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will create a deposit transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountID">Represents the account ID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance</returns>
        public double? Deposit(int accountId, double amount, string notes)
        {
            double? balance = UpdateBalance(accountId, amount);
            CreateTransaction(accountId, amount, (int)TransactionTypeValues.DEPOSIT, notes);

            try
            {
                return balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will create a transfer transaction based on the argument values and update the account balances.
        /// </summary>
        /// <param name="fromAccountId">Represents the initial account ID</param>
        /// <param name="toAccountId">Represents the secondary account ID</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns></returns>
        public double? Transfer(int fromAccountId, int toAccountId, double amount, string notes)
        {
            double? balance = UpdateBalance(fromAccountId, -amount);
            CreateTransaction(fromAccountId, -amount, (int)TransactionTypeValues.TRANSFER, notes);

            UpdateBalance(toAccountId, amount);
            CreateTransaction(toAccountId, amount, (int)TransactionTypeValues.TRANSFER_RECIPIENT, notes);

            try
            {
                return balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will create a withdrawal transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountId">Represents the account ID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance.</returns>
        public double? Withdrawal(int accountId, double amount, string notes)
        {
            double? balance = UpdateBalance(accountId, -amount);
            CreateTransaction(accountId, -amount, (int)TransactionTypeValues.WITHDRAWAL, notes);

            try
            {
                return balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will update the balance of a bank account based on the argument values.
        /// </summary>
        /// <param name="accountId">Represents the account ID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <returns>The bank account balance.</returns>
        private double? UpdateBalance(int accountId, double amount)
        {
            BankAccount bankAccount = (from results in db.BankAccounts where results.BankAccountId == accountId select results).SingleOrDefault();
            bankAccount.Balance += amount;
                       
            for (int i = 0; i < 4; i++)
            {
                bankAccount.AccountState.StateChangeCheck(bankAccount);
                db.SaveChanges();
            }
            
            try
            {
                return bankAccount.Balance;
            }
            catch (Exception)
            {
                double? returnValue = null;

                return returnValue;
            }
        }

        /// <summary>
        /// Implementation will create a transaction based on the argument values.
        /// </summary>
        /// <param name="accountId">Represents the accountID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="transactionTypeId">Represents the type of transaction.</param>
        /// <param name="notes">Represents the notes.</param>
        private void CreateTransaction(int accountId, double amount, int transactionTypeId, string notes)
        {
            Transaction transaction = new Transaction();
            transaction.BankAccountId = accountId;
            transaction.TransactionTypeId = transactionTypeId;
            transaction.Notes = notes;

            if (amount < 0)
            {
                transaction.Deposit = null;
                transaction.Withdrawal = amount;
            }
            else
            {
                transaction.Deposit = amount;
                transaction.Withdrawal = null;
            }

            transaction.DateCreated = DateTime.Now;
            transaction.SetNextTransactionNumber();

            db.Transactions.Add(transaction);
            db.SaveChanges();
        }
    }
}
