using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionManager" in both code and config file together.
    [ServiceContract]
    public interface ITransactionManager
    {
        /// <summary>
        /// Implementation will create a deposit transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountID">Represents the account ID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance</returns>
        [OperationContract]
        double? Deposit(int accountID, double amount, string notes);

        /// <summary>
        /// Implementation will create a withdrawal transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountId">Represents the account ID.</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance.</returns>
        [OperationContract]
        double? Withdrawal(int accountId, double amount, string notes);

        /// <summary>
        /// Implementation will create a bill payment transaction based on the argument values and update the account balance.
        /// </summary>
        /// <param name="accountId">Represents the account ID</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns>The account balance.</returns>
        [OperationContract]
        double? BillPayment(int accountId, double amount, string notes);

        /// <summary>
        /// Implementation will create a transfer transaction based on the argument values and update the account balances.
        /// </summary>
        /// <param name="fromAccountId">Represents the initial account ID</param>
        /// <param name="toAccountId">Represents the secondary account ID</param>
        /// <param name="amount">Represents the amount.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns></returns>
        [OperationContract]
        double? Transfer(int fromAccountId, int toAccountId, double amount, string notes);

        /// <summary>
        /// Implementation will calculate the interest of an account based on the argument values.
        /// </summary>
        /// <param name="accountId">Represents the account ID.</param>
        /// <param name="notes">Represents the notes.</param>
        /// <returns></returns>
        [OperationContract]
        double? CalculateInterest(int accountId, string notes);
    }
}
