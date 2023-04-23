using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankOfBIT_BC.Data;


namespace WindowsBanking
{
    public partial class BatchProcess : Form
    {
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();


        public BatchProcess()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void BatchProcess_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);

            institutionBindingSource.DataSource = (from results in db.Institutions select results).ToList();
            cboDescription.Enabled = false;
        }



        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //given:  Ensure key has been entered.  Note: for use with Assignment 9
            //if(txtKey.Text.Length == 0)
            //{
            //    MessageBox.Show("Please enter a key to decrypt the input file(s).", "Key Required");
            //}
            Batch batch = new Batch();

            if (radSelect.Checked)
            {
                batch.ProcessTransmission((cboDescription.SelectedValue).ToString(), "");
                rtxtLog.Text += batch.WriteLogData() + "\n";

            }
            else if(radAll.Checked)
            {

                for(int i = 0; i < cboDescription.Items.Count; i++)
                {
                    cboDescription.SelectedIndex = i;
                    string value = (cboDescription.SelectedValue).ToString();
                    batch.ProcessTransmission(value, "");
                    rtxtLog.Text += batch.WriteLogData() + "\n";
                }
            }
        }

        private void radAll_CheckedChanged(object sender, EventArgs e)
        {
            if(radAll.Checked)
            {
                cboDescription.Enabled = false;
            }
            else
            {
                cboDescription.Enabled = true;
            }
        }
    }
}
