<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransactionListing.aspx.cs" Inherits="OnlineBanking.TransactionListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <br />
    <asp:Label ID="lblClientName" runat="server" Text="Label" Font-Bold="False"></asp:Label>
</p>
<p>
    <asp:Label ID="lblAccountNumber" runat="server" Text="Label" Width="250px" Font-Bold="False"></asp:Label>
    <asp:Label ID="lblBalance" runat="server" Text="Label" Font-Bold="False"></asp:Label>
</p>
<p>
    <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Font-Bold="False">
        <Columns>
            <asp:BoundField DataField="DateCreated" DataFormatString="{0:d}" HeaderText="Date" >
            <ItemStyle Width="125px" />
            </asp:BoundField>
            <asp:BoundField DataField="TransactionType.Description" HeaderText="Transaction Type" />
            <asp:BoundField DataField="Deposit" DataFormatString="{0:c}" HeaderText="Amount In">
            <ItemStyle HorizontalAlign="Right" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Withdrawal" DataFormatString="{0:c}" HeaderText="Amount Out">
            <ItemStyle HorizontalAlign="Right" Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Notes" HeaderText="Details">
            <HeaderStyle Width="300px" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
        <RowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="Gray" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
</p>
<p>
    <asp:LinkButton ID="lbtnTransactionWebForm" runat="server" OnClick="lbtnTransactionWebForm_Click" Width="225px" Font-Bold="False">Pay Bills and Transfer Funds</asp:LinkButton>
    <asp:LinkButton ID="lbtnAccountListing" runat="server" OnClick="lbtnAccountListing_Click" Font-Bold="False">Return to Account Listing</asp:LinkButton>
</p>
    <p>
        <asp:Label ID="lblErrorMessage" runat="server" Text="Label" Font-Bold="False" ForeColor="Black"></asp:Label>
</p>
</asp:Content>
