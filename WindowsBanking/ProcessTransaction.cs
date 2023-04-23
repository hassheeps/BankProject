using BankOfBIT_BC.Data;
using BankOfBIT_BC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsBanking
{
    public partial class ProcessTransaction : Form
    {
        ConstructorData constructorData;
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// Form can only be opened with a Constructor Data object
        /// containing client and account details.
        /// </summary>
        /// <param name="constructorData">Populated Constructor data object.</param>
        public ProcessTransaction(ConstructorData constructorData)
        {
            //Given, more code to be added.
            InitializeComponent();
            this.constructorData = constructorData;

            Client client = constructorData.client;
            BankAccount bankAccount = constructorData.bankAccount;

            bankAccountBindingSource.DataSource = (from results in db.BankAccounts
                                                   where results.AccountNumber == bankAccount.AccountNumber
                                                   select results).ToList();


            clientBindingSource.DataSource = (from clientListing in db.Clients
                                              where clientListing.ClientNumber == client.ClientNumber
                                              select clientListing).ToList();

            transactionTypeBindingSource.DataSource = (from results in db.TransactionTypes
                                                      where results.TransactionTypeId <= 4
                                                      select results).ToList();

            cboPayeeAccount.DataSource = (from results in db.BankAccounts
                                          where results.ClientId == bankAccount.ClientId
                                          && results.AccountNumber != bankAccount.AccountNumber
                                          select results).ToList();

            payeeBindingSource.DataSource = db.Payees.ToList();

            lblPayeeAccount.Hide();
            cboPayeeAccount.Hide();
            lblNoAdditionalAccounts.Hide();
            lnkUpdate.Enabled = true;

        }

        /// <summary>
        /// Return to the Client Data form passing specific client and 
        /// account information within ConstructorData.
        /// </summary>
        private void lnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClientData client = new ClientData(constructorData);
            client.MdiParent = this.MdiParent;
            client.Show();
            this.Close();
        }
        /// <summary>
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void ProcessTransaction_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0,0);

            BankAccount bankAccount = constructorData.bankAccount;

            mlblaccountNumber.Mask = Utility.BusinessRules.AccountFormat(bankAccount.Description);
        }

        /// <summary>
        /// Handles the Selected Index Changed event of the Transaction Type combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            BankAccount bankAccount = constructorData.bankAccount;
            int selectedIndex = Int32.Parse(cboDescription.SelectedValue.ToString());

            switch(selectedIndex)
            {
                //Deposit
                case 1:
                    cboPayeeAccount.Hide();
                    lblPayeeAccount.Hide();
                    lblNoAdditionalAccounts.Hide();
                    lnkUpdate.Enabled = true;
                    break;

                //Withdrawal
                case 2:
                    cboPayeeAccount.Hide();
                    lblPayeeAccount.Hide();
                    lblNoAdditionalAccounts.Hide();
                    lnkUpdate.Enabled = true;
                    break;

                //Bill Payments
                case 3:
                    cboPayeeAccount.Show();
                    lblPayeeAccount.Show();

                    cboPayeeAccount.DataSource = db.Payees.ToList();
                    cboPayeeAccount.DisplayMember = "Description";

                    lblNoAdditionalAccounts.Hide();
                    lnkUpdate.Enabled = true;
                    break;

                //Transfers
                case 4:
                    cboPayeeAccount.Show();
                    lblPayeeAccount.Show();

                    cboPayeeAccount.DataSource = (from results in db.BankAccounts
                                                  where results.ClientId == bankAccount.ClientId
                                                  && results.AccountNumber != bankAccount.AccountNumber
                                                  select results).ToList();
                    cboPayeeAccount.DisplayMember = "AccountNumber";

                    lblNoAdditionalAccounts.Hide();
                    lnkUpdate.Enabled = true;
                    
                    if(string.IsNullOrEmpty(cboPayeeAccount.Text))
                    {
                        cboPayeeAccount.Hide();
                        lblPayeeAccount.Hide();
                        lblNoAdditionalAccounts.Show();
                        lnkUpdate.Enabled = false;
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the link clicked event of the update link button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!Utility.Numeric.IsNumeric(txtAmount.Text, System.Globalization.NumberStyles.Number))
                {
                    MessageBox.Show("Please enter a valid amount", "Amount error");
                    txtAmount.Focus();
                }
                else
                {
                    BankAccount bankAccount = constructorData.bankAccount;
                    int selectedIndex = Int32.Parse(cboDescription.SelectedValue.ToString());

                    double transactionAmount = double.Parse(txtAmount.Text.ToString());
                    double balanceAmount = bankAccount.Balance;

                    if (selectedIndex != 1)
                    {
                        if (transactionAmount > balanceAmount)
                        {
                            MessageBox.Show("Insufficient funds exist for requested transaction.", "Insufficient Funds");
                        }
                    }

                    TransactionReference.TransactionManagerClient service = new TransactionReference.TransactionManagerClient();

                    try
                    {
                        if (selectedIndex == 1)
                        {
                            service.Deposit(bankAccount.BankAccountId, transactionAmount, "Deposit");
                            double newBalance = balanceAmount + transactionAmount;
                            string stringBalance = newBalance.ToString("C");
                            lblBalanceField.Text = stringBalance;
                            txtAmount.Text = "";
                        }
                        else if (selectedIndex == 2)
                        {
                            service.Withdrawal(bankAccount.BankAccountId, transactionAmount, "Withdrawal");
                            double newBalance = balanceAmount - transactionAmount;
                            string stringBalance = newBalance.ToString("C");
                            lblBalanceField.Text = stringBalance;
                            txtAmount.Text = "";
                        }
                        else if (selectedIndex == 3)
                        {
                            service.BillPayment(bankAccount.BankAccountId, transactionAmount, "Bill Payment");
                            double newBalance = balanceAmount - transactionAmount;
                            string stringBalance = newBalance.ToString("C");
                            lblBalanceField.Text = stringBalance;
                            txtAmount.Text = "";
                        }
                        else if (selectedIndex == 4)
                        {
                            BankAccount recipient = (from results in db.BankAccounts
                                                     where results.AccountNumber == long.Parse(cboPayeeAccount.Text)
                                                     select results).SingleOrDefault();

                            int recipientNumber = Int32.Parse(recipient.AccountNumber.ToString());

                            service.Transfer(bankAccount.BankAccountId, recipientNumber, transactionAmount, "Transfer");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error completing transaction.", "Transaction Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void grpTransaction_Enter(object sender, EventArgs e)
        {

        }
    }
}
