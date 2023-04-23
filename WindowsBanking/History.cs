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
    public partial class History : Form
    {
        ConstructorData constructorData;
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// Form can only be opened with a Constructor Data object
        /// containing client and account details.
        /// </summary>
        /// <param name="constructorData">Populated Constructor data object.</param>
        public History(ConstructorData constructorData)
        {
            //Given, more code to be added.
            InitializeComponent();
            this.constructorData = constructorData;

            Client client = constructorData.client;
            BankAccount bankAccount = constructorData.bankAccount;

            try
            {
                bankAccountBindingSource.DataSource = (from results in db.BankAccounts
                                                       where results.AccountNumber == bankAccount.AccountNumber
                                                       select results).ToList();


                clientBindingSource.DataSource = (from clientListing in db.Clients
                                                  where clientListing.ClientNumber == client.ClientNumber
                                                  select clientListing).ToList();

                var joinQuery = (from transactions in db.Transactions
                                 join types in db.TransactionTypes
                                 on transactions.TransactionTypeId equals types.TransactionTypeId
                                 where transactions.BankAccountId == bankAccount.BankAccountId
                                 select new { TypeDescription = types.Description }).ToList();

                transactionBindingSource.DataSource = (from transactions in db.Transactions
                                                      where transactions.BankAccountId == bankAccount.BankAccountId
                                                      select transactions).ToList();
                
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                string title = "Error";

                MessageBox.Show(message, title);

            }
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
        private void History_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            BankAccount bankAccount = constructorData.bankAccount;

            mlblaccountNumber.Mask = Utility.BusinessRules.AccountFormat(bankAccount.Description);

        }

        private void transactionDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
