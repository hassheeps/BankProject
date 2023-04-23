<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTransaction.aspx.cs" Inherits="OnlineBanking.CreateTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
        <asp:Label ID="lblAccountNumber" runat="server" Text="Label" Font-Bold="False"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblBalance" runat="server" Text="Label" Width="80px" Font-Bold="False"></asp:Label>
        <asp:Label ID="lblBalanceAmount" runat="server" Text="Label" Font-Bold="False"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblTransactionType" runat="server" Text="Transaction Type" Font-Bold="False"></asp:Label>
        <asp:DropDownList ID="ddlTransactionType" runat="server" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged" Font-Bold="False">
        </asp:DropDownList>
    </p>
    <p>
        <asp:Label ID="lblAmount" runat="server" Text="Amount" Font-Bold="False"></asp:Label>
        <asp:TextBox ID="txtAmount" runat="server" Font-Bold="False"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvAmount" ControlToValidate = "txtAmount" ErrorMessage = "* An amount is required" runat="server" Display="Dynamic" ForeColor="Red" Font-Bold="False" Font-Italic="False"></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rfvRange" ControlToValidate = "txtAmount" MinimumValue ="0.01" MaximumValue = "10000.00" Type="Double" ErrorMessage = "* Invalid Amount" runat="server" Display="Dynamic" ForeColor="Red" Font-Bold="False"></asp:RangeValidator>
            
    </p>
    <p>
        <asp:Label ID="lblTo" runat="server" Text="To:" Font-Bold="False"></asp:Label>
        <asp:DropDownList ID="ddlPayee" runat="server" Font-Bold="False">
        </asp:DropDownList>
    </p>
    <p>
        <asp:LinkButton ID="lbtnCompleteTransaction" runat="server" OnClick="lbtnCompleteTransaction_Click" Width="200px" Font-Bold="False">Complete Transaction</asp:LinkButton>
        <asp:LinkButton ID="lbtnReturnToAccountListing" runat="server" OnClick="lbtnReturnToAccountListing_Click" Font-Bold="False">Return to Account Listing</asp:LinkButton>
    </p>
    <p>
        <asp:Label ID="lblErrorMessage" runat="server" Text="Label" Font-Bold="False"></asp:Label>
    </p>
</asp:Content>
