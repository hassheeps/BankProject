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
    public partial class AccountListing : System.Web.UI.Page
    {
        BankOfBIT_BCContext db = new BankOfBIT_BCContext();

        /// <summary>
        /// Handles the page_load event of the account listing page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (this.Page.User.Identity.IsAuthenticated)
                    {
                        int index = Page.User.Identity.Name.IndexOf('@');
                        long clientNumber = long.Parse(Page.User.Identity.Name.Substring(0, index));

                        Client client = (from results in db.Clients where results.ClientNumber == clientNumber select results).SingleOrDefault();
                        bool isValid = true;

                        Session["SessionClient"] = client;
                        Session["SessionPrimitive"] = isValid;

                        lblClientName.Text = client.FullName;
                        lblErrorMessage.Text = null;

                        IQueryable<BankAccount> accounts = from results in db.BankAccounts where results.ClientId == client.ClientId select results;

                        gvAccounts.DataSource = accounts.ToList();
                        gvAccounts.DataBind();

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
        /// Handles the event that the selected account index has changed.
        /// </summary>
        protected void gvAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SessionSelectedAccount"] = gvAccounts.Rows[gvAccounts.SelectedIndex].Cells[1].Text;

            Response.Redirect("~/TransactionListing.aspx");
        }
    }
}