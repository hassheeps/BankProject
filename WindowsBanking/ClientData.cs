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
    public partial class ClientData : Form
    {
        ConstructorData constructorData = new ConstructorData();
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// This constructor will execute when the form is opened
        /// from the MDI Frame.
        /// </summary>
        public ClientData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This constructor will execute when the form is opened by
        /// returning from the History or Transaction forms.
        /// </summary>
        /// <param name="constructorData">Populated ConstructorData object.</param>
        public ClientData(ConstructorData constructorData)
        {
            //Given:
            InitializeComponent();
            this.constructorData = constructorData;

            //More code to be added:
            BankAccount bankAccount = constructorData.bankAccount;

            mtxtclientNumber.Mask = Utility.BusinessRules.AccountFormat(bankAccount.Description);

            mtxtclientNumber_Leave(null, null);
        }

        /// <summary>
        /// Open the Transaction form passing ConstructorData object.
        /// </summary>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            populateConstructorData();
            //Given, more code to be added.
            ProcessTransaction transaction = new ProcessTransaction(constructorData);
            transaction.MdiParent = this.MdiParent;
            transaction.Show();
            this.Close();
        }

        /// <summary>
        /// Open the History form passing ConstructorData object.
        /// </summary>
        private void lnkDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            populateConstructorData();

            //Given, more code to be added.
            History history = new History(constructorData);
            history.MdiParent = this.MdiParent;
            history.Show();
            this.Close();
        }

        /// <summary>
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void ClientData_Load(object sender, EventArgs e)
        {

            this.Location = new Point(0,0);
            
        }

        /// <summary>
        /// Handles the leave event of the client number masked textbox event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtclientNumber_Leave(object sender, EventArgs e)
        {
            BankAccount bankAccount = constructorData.bankAccount;
            Client client = new Client();

            if (constructorData.client != null)
            {
                client = constructorData.client;
                clientBindingSource.DataSource = client;
                mtxtclientNumber.Text = client.ClientNumber.ToString();
            }
            else
            {
                long clientNumberInput = (long)(Convert.ToDouble(mtxtclientNumber.Text));
                client = (from clientListing in db.Clients where clientListing.ClientNumber == clientNumberInput select clientListing).SingleOrDefault();

                if (client == null)
                {
                    string title = "Invalid Client Number";
                    string message = "Client Number: " + mtxtclientNumber.Text + " does not exist.";

                    MessageBox.Show(message, title);

                    mtxtclientNumber.Focus();

                    lnkDetails.Enabled = false;
                    lnkProcess.Enabled = false;

                    clientBindingSource.DataSource = typeof(Client);
                    bankAccountBindingSource.DataSource = typeof(BankAccount);
                }
                else
                {
                    clientBindingSource.DataSource = client;
                }
            }        
                
            IQueryable<BankAccount> bankAccountResults = from results in db.BankAccounts where results.ClientId == client.ClientId select results;

            bankAccountBindingSource.DataSource = bankAccountResults.ToList();

            if (bankAccount != null)
            {
                cboAccountNumber.Text = bankAccount.AccountNumber.ToString();
            }

            lnkDetails.Enabled = true;
            lnkProcess.Enabled = true;

            if (string.IsNullOrEmpty(cboAccountNumber.Text))
            {
                lnkDetails.Enabled = false;
                lnkProcess.Enabled = false;

                bankAccountBindingSource.DataSource = typeof(BankAccount);
            }            
        } 
        
        /// <summary>
        /// Handles the event that populates constructor data.
        /// </summary>
        /// <returns>Populated Constructor Data</returns>
        public ConstructorData populateConstructorData()
        {
            long clientNumberInput = (long)(Convert.ToDouble(mtxtclientNumber.Text));
            Client client = (from clientListing in db.Clients where clientListing.ClientNumber == clientNumberInput select clientListing).SingleOrDefault();

            long accountNumberInput = (long)(Convert.ToDouble(cboAccountNumber.Text));
            BankAccount bankAccount = (from results in db.BankAccounts where results.AccountNumber == accountNumberInput select results).SingleOrDefault();

            constructorData.client = client;
            constructorData.bankAccount = bankAccount;

            return constructorData;

        }

        private void grpAccount_Enter(object sender, EventArgs e)
        {

        }
    }
}
