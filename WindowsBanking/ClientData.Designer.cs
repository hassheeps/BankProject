namespace WindowsBanking
{
    partial class ClientData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label fullNameLabel;
            System.Windows.Forms.Label lblDateCreated;
            System.Windows.Forms.Label lblfullAddress;
            System.Windows.Forms.Label accountNumberLabel;
            System.Windows.Forms.Label lblBlanace;
            System.Windows.Forms.Label lblAccountType;
            System.Windows.Forms.Label lblNotes;
            System.Windows.Forms.Label lblclientNumber;
            System.Windows.Forms.Label lblState;
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.mtxtclientNumber = new System.Windows.Forms.MaskedTextBox();
            this.bankAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblfullAddressLabelField = new System.Windows.Forms.Label();
            this.clientBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbldateTimeField = new System.Windows.Forms.Label();
            this.lblfullNameField = new System.Windows.Forms.Label();
            this.grpAccount = new System.Windows.Forms.GroupBox();
            this.lblStateField = new System.Windows.Forms.Label();
            this.lblNotesField = new System.Windows.Forms.Label();
            this.lblAccountTypeField = new System.Windows.Forms.Label();
            this.lblBalanceField = new System.Windows.Forms.Label();
            this.cboAccountNumber = new System.Windows.Forms.ComboBox();
            this.lnkDetails = new System.Windows.Forms.LinkLabel();
            this.lnkProcess = new System.Windows.Forms.LinkLabel();
            fullNameLabel = new System.Windows.Forms.Label();
            lblDateCreated = new System.Windows.Forms.Label();
            lblfullAddress = new System.Windows.Forms.Label();
            accountNumberLabel = new System.Windows.Forms.Label();
            lblBlanace = new System.Windows.Forms.Label();
            lblAccountType = new System.Windows.Forms.Label();
            lblNotes = new System.Windows.Forms.Label();
            lblclientNumber = new System.Windows.Forms.Label();
            lblState = new System.Windows.Forms.Label();
            this.grpClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankAccountBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientBindingSource)).BeginInit();
            this.grpAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // fullNameLabel
            // 
            fullNameLabel.AutoSize = true;
            fullNameLabel.Location = new System.Drawing.Point(76, 61);
            fullNameLabel.Name = "fullNameLabel";
            fullNameLabel.Size = new System.Drawing.Size(57, 13);
            fullNameLabel.TabIndex = 4;
            fullNameLabel.Text = "Full Name:";
            // 
            // lblDateCreated
            // 
            lblDateCreated.AutoSize = true;
            lblDateCreated.Location = new System.Drawing.Point(61, 119);
            lblDateCreated.Name = "lblDateCreated";
            lblDateCreated.Size = new System.Drawing.Size(73, 13);
            lblDateCreated.TabIndex = 5;
            lblDateCreated.Text = "Date Created:";
            // 
            // lblfullAddress
            // 
            lblfullAddress.AutoSize = true;
            lblfullAddress.Location = new System.Drawing.Point(67, 90);
            lblfullAddress.Name = "lblfullAddress";
            lblfullAddress.Size = new System.Drawing.Size(67, 13);
            lblfullAddress.TabIndex = 6;
            lblfullAddress.Text = "Full Address:";
            // 
            // accountNumberLabel
            // 
            accountNumberLabel.AutoSize = true;
            accountNumberLabel.Location = new System.Drawing.Point(51, 42);
            accountNumberLabel.Name = "accountNumberLabel";
            accountNumberLabel.Size = new System.Drawing.Size(90, 13);
            accountNumberLabel.TabIndex = 2;
            accountNumberLabel.Text = "Account Number:";
            // 
            // lblBlanace
            // 
            lblBlanace.AutoSize = true;
            lblBlanace.Location = new System.Drawing.Point(428, 42);
            lblBlanace.Name = "lblBlanace";
            lblBlanace.Size = new System.Drawing.Size(49, 13);
            lblBlanace.TabIndex = 6;
            lblBlanace.Text = "Balance:";
            // 
            // lblAccountType
            // 
            lblAccountType.AutoSize = true;
            lblAccountType.Location = new System.Drawing.Point(400, 72);
            lblAccountType.Name = "lblAccountType";
            lblAccountType.Size = new System.Drawing.Size(77, 13);
            lblAccountType.TabIndex = 8;
            lblAccountType.Text = "Account Type:";
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new System.Drawing.Point(48, 100);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new System.Drawing.Size(38, 13);
            lblNotes.TabIndex = 10;
            lblNotes.Text = "Notes:";
            // 
            // lblclientNumber
            // 
            lblclientNumber.AutoSize = true;
            lblclientNumber.Location = new System.Drawing.Point(57, 34);
            lblclientNumber.Name = "lblclientNumber";
            lblclientNumber.Size = new System.Drawing.Size(76, 13);
            lblclientNumber.TabIndex = 7;
            lblclientNumber.Text = "Client Number:";
            // 
            // lblState
            // 
            lblState.AutoSize = true;
            lblState.Location = new System.Drawing.Point(48, 72);
            lblState.Name = "lblState";
            lblState.Size = new System.Drawing.Size(35, 13);
            lblState.TabIndex = 11;
            lblState.Text = "State:";
            // 
            // grpClient
            // 
            this.grpClient.Controls.Add(lblclientNumber);
            this.grpClient.Controls.Add(this.mtxtclientNumber);
            this.grpClient.Controls.Add(lblfullAddress);
            this.grpClient.Controls.Add(this.lblfullAddressLabelField);
            this.grpClient.Controls.Add(lblDateCreated);
            this.grpClient.Controls.Add(this.lbldateTimeField);
            this.grpClient.Controls.Add(fullNameLabel);
            this.grpClient.Controls.Add(this.lblfullNameField);
            this.grpClient.Location = new System.Drawing.Point(55, 25);
            this.grpClient.Name = "grpClient";
            this.grpClient.Size = new System.Drawing.Size(677, 162);
            this.grpClient.TabIndex = 0;
            this.grpClient.TabStop = false;
            this.grpClient.Text = "Client Data";
            // 
            // mtxtclientNumber
            // 
            this.mtxtclientNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtxtclientNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "Client.ClientNumber", true));
            this.mtxtclientNumber.Location = new System.Drawing.Point(147, 29);
            this.mtxtclientNumber.Mask = "0000-9999";
            this.mtxtclientNumber.Name = "mtxtclientNumber";
            this.mtxtclientNumber.Size = new System.Drawing.Size(117, 20);
            this.mtxtclientNumber.TabIndex = 8;
            this.mtxtclientNumber.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.mtxtclientNumber.Leave += new System.EventHandler(this.mtxtclientNumber_Leave);
            // 
            // bankAccountBindingSource
            // 
            this.bankAccountBindingSource.DataSource = typeof(BankOfBIT_BC.Models.BankAccount);
            // 
            // lblfullAddressLabelField
            // 
            this.lblfullAddressLabelField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblfullAddressLabelField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.clientBindingSource, "FullAddress", true));
            this.lblfullAddressLabelField.Location = new System.Drawing.Point(147, 85);
            this.lblfullAddressLabelField.Name = "lblfullAddressLabelField";
            this.lblfullAddressLabelField.Size = new System.Drawing.Size(476, 23);
            this.lblfullAddressLabelField.TabIndex = 7;
            // 
            // clientBindingSource
            // 
            this.clientBindingSource.DataSource = typeof(BankOfBIT_BC.Models.Client);
            // 
            // lbldateTimeField
            // 
            this.lbldateTimeField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldateTimeField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.clientBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "d"));
            this.lbldateTimeField.Location = new System.Drawing.Point(147, 115);
            this.lbldateTimeField.Name = "lbldateTimeField";
            this.lbldateTimeField.Size = new System.Drawing.Size(117, 23);
            this.lbldateTimeField.TabIndex = 6;
            // 
            // lblfullNameField
            // 
            this.lblfullNameField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblfullNameField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.clientBindingSource, "FullAddress", true));
            this.lblfullNameField.Location = new System.Drawing.Point(147, 56);
            this.lblfullNameField.Name = "lblfullNameField";
            this.lblfullNameField.Size = new System.Drawing.Size(476, 23);
            this.lblfullNameField.TabIndex = 5;
            // 
            // grpAccount
            // 
            this.grpAccount.Controls.Add(lblState);
            this.grpAccount.Controls.Add(this.lblStateField);
            this.grpAccount.Controls.Add(lblNotes);
            this.grpAccount.Controls.Add(this.lblNotesField);
            this.grpAccount.Controls.Add(lblAccountType);
            this.grpAccount.Controls.Add(this.lblAccountTypeField);
            this.grpAccount.Controls.Add(lblBlanace);
            this.grpAccount.Controls.Add(this.lblBalanceField);
            this.grpAccount.Controls.Add(accountNumberLabel);
            this.grpAccount.Controls.Add(this.cboAccountNumber);
            this.grpAccount.Controls.Add(this.lnkDetails);
            this.grpAccount.Controls.Add(this.lnkProcess);
            this.grpAccount.Location = new System.Drawing.Point(55, 189);
            this.grpAccount.Name = "grpAccount";
            this.grpAccount.Size = new System.Drawing.Size(677, 251);
            this.grpAccount.TabIndex = 1;
            this.grpAccount.TabStop = false;
            this.grpAccount.Text = "Bank Account Data";
            this.grpAccount.Enter += new System.EventHandler(this.grpAccount_Enter);
            // 
            // lblStateField
            // 
            this.lblStateField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStateField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "AccountState.Description", true));
            this.lblStateField.Location = new System.Drawing.Point(147, 67);
            this.lblStateField.Name = "lblStateField";
            this.lblStateField.Size = new System.Drawing.Size(117, 23);
            this.lblStateField.TabIndex = 12;
            // 
            // lblNotesField
            // 
            this.lblNotesField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNotesField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "Notes", true));
            this.lblNotesField.Location = new System.Drawing.Point(147, 97);
            this.lblNotesField.Name = "lblNotesField";
            this.lblNotesField.Size = new System.Drawing.Size(476, 87);
            this.lblNotesField.TabIndex = 11;
            // 
            // lblAccountTypeField
            // 
            this.lblAccountTypeField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccountTypeField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "Description", true));
            this.lblAccountTypeField.Location = new System.Drawing.Point(506, 68);
            this.lblAccountTypeField.Name = "lblAccountTypeField";
            this.lblAccountTypeField.Size = new System.Drawing.Size(117, 23);
            this.lblAccountTypeField.TabIndex = 9;
            // 
            // lblBalanceField
            // 
            this.lblBalanceField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBalanceField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "C2"));
            this.lblBalanceField.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBalanceField.Location = new System.Drawing.Point(506, 38);
            this.lblBalanceField.Name = "lblBalanceField";
            this.lblBalanceField.Size = new System.Drawing.Size(117, 23);
            this.lblBalanceField.TabIndex = 7;
            // 
            // cboAccountNumber
            // 
            this.cboAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "AccountNumber", true));
            this.cboAccountNumber.DataSource = this.bankAccountBindingSource;
            this.cboAccountNumber.DisplayMember = "AccountNumber";
            this.cboAccountNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAccountNumber.FormattingEnabled = true;
            this.cboAccountNumber.Location = new System.Drawing.Point(147, 39);
            this.cboAccountNumber.Name = "cboAccountNumber";
            this.cboAccountNumber.Size = new System.Drawing.Size(117, 21);
            this.cboAccountNumber.TabIndex = 3;
            // 
            // lnkDetails
            // 
            this.lnkDetails.AutoSize = true;
            this.lnkDetails.Enabled = false;
            this.lnkDetails.Location = new System.Drawing.Point(386, 212);
            this.lnkDetails.Name = "lnkDetails";
            this.lnkDetails.Size = new System.Drawing.Size(65, 13);
            this.lnkDetails.TabIndex = 1;
            this.lnkDetails.TabStop = true;
            this.lnkDetails.Text = "View Details";
            this.lnkDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDetails_LinkClicked);
            // 
            // lnkProcess
            // 
            this.lnkProcess.AutoSize = true;
            this.lnkProcess.Enabled = false;
            this.lnkProcess.Location = new System.Drawing.Point(160, 212);
            this.lnkProcess.Name = "lnkProcess";
            this.lnkProcess.Size = new System.Drawing.Size(104, 13);
            this.lnkProcess.TabIndex = 0;
            this.lnkProcess.TabStop = true;
            this.lnkProcess.Text = "Process Transaction";
            this.lnkProcess.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkProcess_LinkClicked);
            // 
            // ClientData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 479);
            this.Controls.Add(this.grpAccount);
            this.Controls.Add(this.grpClient);
            this.Name = "ClientData";
            this.Text = "ClientData";
            this.Load += new System.EventHandler(this.ClientData_Load);
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankAccountBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientBindingSource)).EndInit();
            this.grpAccount.ResumeLayout(false);
            this.grpAccount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.GroupBox grpAccount;
        private System.Windows.Forms.LinkLabel lnkDetails;
        private System.Windows.Forms.LinkLabel lnkProcess;
        private System.Windows.Forms.BindingSource clientBindingSource;
        private System.Windows.Forms.Label lblfullAddressLabelField;
        private System.Windows.Forms.Label lbldateTimeField;
        private System.Windows.Forms.Label lblfullNameField;
        private System.Windows.Forms.ComboBox cboAccountNumber;
        private System.Windows.Forms.BindingSource bankAccountBindingSource;
        private System.Windows.Forms.Label lblNotesField;
        private System.Windows.Forms.Label lblAccountTypeField;
        private System.Windows.Forms.Label lblBalanceField;
        private System.Windows.Forms.MaskedTextBox mtxtclientNumber;
        private System.Windows.Forms.Label lblStateField;
    }
}