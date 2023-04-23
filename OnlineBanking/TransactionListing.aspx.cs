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
    public partial class TransactionListing : System.Web.UI.Page
    {
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// Handles the page load event of the transaction listing page.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.User.Identity.IsAuthenticated)
                {
                    Client nextPageClient;
                    nextPageClient = (Client)Session["SessionClient"];

                    long selectedAccount = long.Parse((string)(Session["SessionSelectedAccount"]));

                    lblClientName.Text = nextPageClient.FullName;
                    lblAccountNumber.Text = "Account Number: " + selectedAccount.ToString();

                    BankAccount bankAccount = (from results in db.BankAccounts where results.AccountNumber == selectedAccount select results).SingleOrDefault();
                    Session["SessionBankAccount"] = bankAccount;

                    lblBalance.Text = "Balance: " + bankAccount.Balance.ToString("C");
                    lblErrorMessage.Text = null;

                    IQueryable<Transaction> transactions = from results in db.Transactions where results.BankAccountId == bankAccount.BankAccountId select results;

                    gvTransactions.DataSource = transactions.ToList();
                    gvTransactions.DataBind();
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
               
        }

        /// <summary>
        /// Handles the selected index changed event of the transaction listing page.
        /// </summary>
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event of the link button to create a new transaction.
        /// </summary>
        protected void lbtnTransactionWebForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateTransaction.aspx");
        }

        /// <summary>
        /// Handles the click event of the link button that directs the user to the account listing page.
        /// </summary>
        protected void lbtnAccountListing_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountListing.aspx");
        }


    }
}