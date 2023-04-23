using BankOfBIT_BC.Data;
using BankOfBIT_BC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineBanking
{
    public partial class CreateTransaction : System.Web.UI.Page
    {
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// Clears all data bindings for the Payee drop down list.
        /// </summary>
        protected void ClearBindings()
        {
            ddlPayee.DataSource = null;
            ddlPayee.DataTextField = null;
            ddlPayee.DataValueField = null;
        }

        /// <summary>
        /// Creates the initial bindings for the Payee drop down list
        /// </summary>
        protected void CreatePayeeBindings()
        {
            IQueryable<Payee> payees = from results in db.Payees select results;

            ddlPayee.DataSource = payees.ToList();
            ddlPayee.DataTextField = "Description";
            ddlPayee.DataValueField = "PayeeId";
            ddlPayee.DataBind();
        }
      
        /// <summary>
        /// Handles the page load event of the create transaction web form
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            BankAccount bankAccount;
            bankAccount = (BankAccount)(Session["SessionBankAccount"]);

            lblBalance.Text = "Balance: ";
            lblBalanceAmount.Text = bankAccount.Balance.ToString("C");

            if (!IsPostBack)
            {
                try
                {
                    if (this.Page.User.Identity.IsAuthenticated)
                    {
                        lblAccountNumber.Text = "Account Number: " + bankAccount.AccountNumber.ToString();
                        
                        lblErrorMessage.Text = null;

                        IQueryable<TransactionType> transactionTypes = from results in db.TransactionTypes where results.Description == "Bill Payment" || results.Description == "Transfer" select results;

                        ddlTransactionType.DataSource = transactionTypes.ToList();
                        ddlTransactionType.DataTextField = "Description";
                        ddlTransactionType.DataValueField = "TransactionTypeId";
                        ddlTransactionType.DataBind();

                        ddlTransactionType.AutoPostBack = true;
                        ddlPayee.AutoPostBack = true;
                        
                        txtAmount.Style.Add("text-align", "right");

                        rfvAmount.Enabled = false;
                        rfvRange.Enabled = false;

                        CreatePayeeBindings();
                    }
                    else
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
                catch(Exception ex)
                {
                    lblErrorMessage.Text = ex.Message;
                }
            }
        }

        /// <summary>
        /// Handles the click event of the return to account listing link button.
        /// </summary>
        protected void lbtnReturnToAccountListing_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountListing.aspx");
            lbtnCompleteTransaction.CausesValidation = false;
        }

        /// <summary>
        /// Handles the selected index changed event of the transaction type drop down list box.
        /// </summary>
        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransactionType.SelectedIndex == 0)
            {
                ClearBindings();
                CreatePayeeBindings();
            }
            
            if(ddlTransactionType.SelectedIndex == 1)
            {
                ClearBindings();

                Client client;
                client = (Client)(Session["SessionClient"]);

                BankAccount bankAccount;
                bankAccount = (BankAccount)(Session["SessionbankAccount"]);

                IQueryable<BankAccount> bankAccounts = from results in db.BankAccounts
                                                       where results.ClientId == client.ClientId && results.BankAccountId != bankAccount.BankAccountId
                                                       select results;

                ddlPayee.DataSource = bankAccounts.ToList();
                ddlPayee.DataTextField = "AccountNumber";
                ddlPayee.DataValueField = "BankAccountId";
                ddlPayee.DataBind();
            }
        }

        /// <summary>
        /// Handles the click event of the complete transaction link button.
        /// </summary>
        protected void lbtnCompleteTransaction_Click(object sender, EventArgs e)
        {
            rfvAmount.Enabled = true;
            rfvRange.Enabled = true;

            BankAccount bankAccount;
            bankAccount = (BankAccount)(Session["SessionBankAccount"]);

            double balance = bankAccount.Balance;
            double amount = double.Parse(txtAmount.Text);
            
            Page.Validate();

            if(Page.IsValid)
            {
                try
                { 
                    if (balance < amount)
                    {
                        throw new ArgumentException("Insufficient funds available.");
                    }

                    try
                    {
                        if (ddlTransactionType.SelectedIndex == 0)
                        {
                            int payeeId = int.Parse(ddlPayee.Text);
                            Payee payee = (from results in db.Payees where results.PayeeId == payeeId select results).SingleOrDefault();
                           
                            string notes = ("Online Banking Payment to: " + payee.Description);

                            ServiceReference.TransactionManagerClient service = new ServiceReference.TransactionManagerClient();

                            service.BillPayment(bankAccount.BankAccountId, amount, notes);
                        }

                        if (ddlTransactionType.SelectedIndex == 1)
                        {
                            int fromAccountId = bankAccount.BankAccountId;
                            int toAccountId = int.Parse(ddlPayee.Text);

                            BankAccount toAccount = (from results in db.BankAccounts where results.BankAccountId == toAccountId select results).SingleOrDefault();

                            string transactionNotes = ("Online Banking Transfer From: " + bankAccount.AccountNumber + " To: " + toAccount.AccountNumber);

                            ServiceReference.TransactionManagerClient service = new ServiceReference.TransactionManagerClient();
                            service.Transfer(fromAccountId, toAccountId, amount, transactionNotes);
                        }
                    }
                    catch(Exception ex)
                    {
                        lblErrorMessage.Text = ex.Message;
                    }
                }
                catch(ArgumentException)
                {
                    lblErrorMessage.Text = "Insufficient funds available";
                }
            }
        }
    }
}