<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ralstoat_polleyaw_assignment06.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    Pick a store
                    <asp:DropDownList ID="ddStores" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Pick Loyalty Number
                <asp:DropDownList ID="ddLoyaltyNumber" runat="server"></asp:DropDownList>
                    </td>
                <td>
                    Pick an Employee
                    <asp:DropDownList ID="ddEmployee" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Pick a transaction type
                    <asp:DropDownList ID="ddTransactionType" runat="server"></asp:DropDownList>
                </td>
                <td>
                    Pick a product
                    <asp:DropDownList ID="ddProducts" runat="server"></asp:DropDownList>
                </td>
                <td>
                    How many of the product?
                    <asp:TextBox ID="tbNumberOfProduct" runat="server"></asp:TextBox>
                </td>

                <td>
                    Add a comment
                    <asp:TextBox ID="tbComment" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Sumbit" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tbTransactionDetails" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
