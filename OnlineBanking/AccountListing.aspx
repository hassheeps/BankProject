<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountListing.aspx.cs" Inherits="OnlineBanking.AccountListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <br />
</p>
<p>
    <asp:Label ID="lblClientName" runat="server" Text="Label"></asp:Label>
</p>
<asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" OnSelectedIndexChanged="gvAccounts_SelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="AccountNumber" HeaderText="Account Number">
        <ItemStyle Width="75px" />
        </asp:BoundField>
        <asp:BoundField DataField="Notes" HeaderText="Account Notes">
        <ItemStyle Width="300px" />
        </asp:BoundField>
        <asp:BoundField DataField="Balance" DataFormatString="{0:c}" HeaderText="Balance">
        <ItemStyle HorizontalAlign="Right" Width="150px" />
        </asp:BoundField>
    </Columns>
    <FooterStyle BackColor="#CCCCCC" />
    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
    <RowStyle BackColor="White" />
    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
    <SortedAscendingCellStyle BackColor="#F1F1F1" />
    <SortedAscendingHeaderStyle BackColor="#808080" />
    <SortedDescendingCellStyle BackColor="#CAC9C9" />
    <SortedDescendingHeaderStyle BackColor="#383838" />
</asp:GridView>
<br />
<asp:Label ID="lblErrorMessage" runat="server" Text="Label"></asp:Label>
</asp:Content>
