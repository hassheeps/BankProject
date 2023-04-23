using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using BankOfBIT_BC.Data;
using BankOfBIT_BC.Models;
using Utility;

namespace WindowsBanking
{
    public class Batch
    {
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// The name of the xml input file.
        /// </summary>
        private String inputFileName;

        /// <summary>
        /// The name of the log file.
        /// </summary>
        private String logFileName;

        /// <summary>
        /// The data to be written to the log file.
        /// </summary>
        private String logData;

        private void ProcessErrors(IEnumerable<XElement> beforeQuery, IEnumerable<XElement> afterQuery, String message)
        {
            foreach(XElement element in beforeQuery)
            {
                if(!afterQuery.Contains(element))
                {
                    logData += "------ERROR------ \n" +
                               "File: " + inputFileName + "\n" +
                               "Institution: " + element.Element("institution") + "\n" +
                               "Account Number: " + element.Element("account_no") + "\n" +
                               "Transaction Type: " + element.Element("type") + "\n" +
                               "Amount: " + element.Element("amount") + "\n" +
                               "Note: " + element.Element("notes") + "\n" +
                               "Nodes: " + element.Nodes().Count() + "\n" +
                               message + "\n\n";
                }
            }
        }

        /// <summary>
        /// Verifies the attributes of the xml file's root element.
        /// </summary>
        private void ProcessHeader()
        {    
            // Gets the "account_update" element of the log being processed.

            XDocument xDocument = XDocument.Load(inputFileName);
            XElement account_update = xDocument.Element("account_update");

            // Counts the attributes in the "account_update" element.

            IEnumerable<XAttribute> attributeList = account_update.Attributes();
            int attributeCount = attributeList.Count();

            // Retrieves the "date" attribute of the "account_update" element

            XAttribute xAttribute_date = account_update.Attribute("date");
            DateTime date = ((DateTime)xAttribute_date);

            // Retrieves the "institution" attribute of the "account_update" element.

            XAttribute xAttribute_institution = account_update.Attribute("institution");
            int institutionNumber = ((int)xAttribute_institution);

            // Checks the institution table in the database for a record matching the "institution" attribute in the "account_update" element.

            Institution institution = (from results in db.Institutions where results.InstitutionNumber == institutionNumber select results).SingleOrDefault();

            // Retrieves the "checksum" attribute of the "account_update" element.

            XAttribute xAttribute_checksum = account_update.Attribute("checksum");
            int checksum = ((int)xAttribute_checksum);

            IEnumerable<XElement> transactions = xDocument.Descendants("transaction");
            IEnumerable<XElement> amounts = transactions.Elements("amount");

            double total = 0;

            // Verifies the actual sum of the "amounts" in the xml document against the "checksum" attribute.

            foreach(XElement xele in amounts)
            {
                total += double.Parse(xele.Value);
            }

            // Validation exceptions

            if(attributeCount != 3)
            {
                throw new ArgumentException("ERROR: The number of attributes in file " + inputFileName + " is invalid.\n\n");
                WriteLogData();
            }

            if (date != DateTime.Today)
            {
                throw new ArgumentException("ERROR: The date in file " + inputFileName + " is invalid.\n\n");
                WriteLogData();
            }

            if(institution == null)
            {
                throw new ArgumentException("Error: The institution number in file " + inputFileName + " is invalid.\n\n");
                WriteLogData();
            }

            if(checksum != total)
            {
                throw new ArgumentException("Error: The check sum number in file " + inputFileName + " is invalid.\n\n");
                WriteLogData();
            }
            
        }

        /// <summary>
        /// Verifies the contents of the detail records in the input file.
        /// </summary>
        private void ProcessDetails()
        {
            XDocument xDocument = XDocument.Load(inputFileName);
            XElement account_update = xDocument.Element("account_update");
            XAttribute xAttribute_institution = account_update.Attribute("institution");
            int documentInstitution = ((int)xAttribute_institution);

            // Creates collection of transactions

            IEnumerable<XElement> transactions = xDocument.Descendants("transaction");

            // Refines collection to only those transactions that have 5 descendant nodes.

            IEnumerable<XElement> filteredTransactions_node = transactions.Where(x => x.Nodes().Count() == 5);
            ProcessErrors(transactions, filteredTransactions_node, "Invalid number of nodes");

            // Refines collection to only those transactions where the institution number matches the document institution number.

            IEnumerable<XElement> filteredTransactions_institution = filteredTransactions_node.Where(x => int.Parse(x.Element("institution").Value) == documentInstitution);
            ProcessErrors(filteredTransactions_node, filteredTransactions_institution, "Invalid institution number.");

            // Refines collection to only those transactions where the type and amount nodes are numeric.

            IEnumerable<XElement> filteredTransactions_numeric = filteredTransactions_institution.Where(x => Numeric.IsNumeric((x.Element("type").Value), System.Globalization.NumberStyles.Integer))
                                                       .Where(x => Numeric.IsNumeric((x.Element("amount").Value), System.Globalization.NumberStyles.Float));
            ProcessErrors(filteredTransactions_institution, filteredTransactions_numeric, "Invalid value type.");

            // Refines collection to only those transactions where the transaction type is "withdrawal" or "interest calculation".

            IEnumerable<XElement> filteredTransactions_types = filteredTransactions_numeric.Where(x => int.Parse(x.Element("type").Value) == 2 || int.Parse(x.Element("type").Value) == 6);
            ProcessErrors(filteredTransactions_numeric, filteredTransactions_types, "Invalid transaction type");

            // Refines collection to only those transactions where the transaction amounts are valid for the transaction type.

            IEnumerable<XElement> filteredTransactions_amounts = filteredTransactions_types.Where(x => (int.Parse(x.Element("type").Value) == 2 && int.Parse(x.Element("amount").Value) > 0) ||
                                                                   (int.Parse(x.Element("type").Value) == 6 && float.Parse(x.Element("amount").Value) == 0));
            ProcessErrors(filteredTransactions_types, filteredTransactions_amounts, "Invalid transaction amount.");

            // Refines collection to only those transactions where the account number exists in the database.

            IEnumerable<long> accountNumbers = (from results in db.BankAccounts select results.AccountNumber).ToList();

            foreach (XElement transaction in transactions)
            {
                bool test = accountNumbers.Contains(long.Parse(transaction.Element("account_no").Value));
            }
            
            IEnumerable<XElement> filteredTransactions_accountNumber = filteredTransactions_amounts.Where(x => accountNumbers.Contains(long.Parse(x.Element("account_no").Value)));

            ProcessErrors(filteredTransactions_amounts, filteredTransactions_accountNumber, "Invalid account number."); 

            ProcessTransactions(filteredTransactions_amounts);
        }

        private void ProcessTransactions(IEnumerable<XElement> transactionRecords)
        {
            TransactionReference.TransactionManagerClient service = new TransactionReference.TransactionManagerClient();

            foreach (XElement transactionRecord in transactionRecords)
            {
                long accountNumber = long.Parse(transactionRecord.Element("account_no").Value);

                BankAccount bankAccount = (from results in db.BankAccounts
                                           where results.AccountNumber == accountNumber
                                           select results).SingleOrDefault();

                if (transactionRecord.Element("type").Value == "2")
                {
                    double amount = double.Parse(transactionRecord.Element("amount").Value);

                    try
                    {
                        service.Withdrawal(bankAccount.BankAccountId, amount, "Withdrawal");

                        logData += "Transaction completed successfully: Withdrawal - " +
                                    amount +
                                    " applied to account " + (bankAccount.AccountNumber).ToString() +
                                    ".\n";
                    }
                    catch(Exception ex)
                    {

                        logData += "Transaction completed unsuccessfully.\n" + ex.Message;
                    }
                }
                else if(bankAccount != null)
                {
                    try
                    {
                        service.CalculateInterest(bankAccount.BankAccountId, "Interest Calculation");

                        logData += "Transaction completed successfully: Interest - *** applied to account " + (bankAccount.AccountNumber).ToString() + ".\n";
                    }
                    catch(Exception ex)
                    {
                        logData += "Transaction completed unsuccessfully.\n";
                    }                               
                }
            }
        }

        public String WriteLogData()
        {
            string content = logData;
            StreamWriter writer = new StreamWriter(logFileName, true);
            
            writer.Write(content);
            writer.Close();

            logData = "";
            return content;
        }

        /// <summary>
        /// Processes the transmission of the transaction log.
        /// </summary>
        /// <param name="institution">The institution.</param>
        /// <param name="key"></param>
        public void ProcessTransmission(String institution, String key)
        {
            
            DateTime fullDate = DateTime.Now;
            string dayOfYear = fullDate.DayOfYear.ToString("000");

            string baseFileName = fullDate.ToString("yyyy") + "-" + dayOfYear + "-" + institution;
            
            inputFileName = baseFileName + ".xml";
            logFileName = "LOG " + baseFileName + ".txt";

            try
            {
                if (File.Exists(inputFileName))
                {
                    ProcessHeader();
                    ProcessDetails();
                }
                else
                {
                    logData += "The file " + inputFileName + " does not exist.";
                }
            }
            catch(Exception ex)
            {
                logData += ex.Message;
            }


        }
    }
}
