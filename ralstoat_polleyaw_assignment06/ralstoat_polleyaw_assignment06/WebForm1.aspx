<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ralstoat_polleyaw_assignment06.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 375px;
        }
        .auto-style6 {
            width: 323px;
        }
        .auto-style7 {
            width: 324px;
            height: 92px;
        }
        .auto-style8 {
            width: 323px;
            height: 51px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <div>
<!-- Hello from Bill -->        
        <table style="background-color:dodgerblue; border-color:crimson; max-width:605px" border="1">
            <tr>
                <td colspan="2" style="border-color:darkblue; text-align:center">
                    <asp:Label ID="lblHeader" runat="server" Font-Size="30" ForeColor="White" Font-Bold="true" Text="Transaction Creator"></asp:Label>
                </td>
            </tr>
            <tr >
                <td style="border-color:darkblue" class="auto-style6">
                    Store&nbsp;
                    <asp:DropDownList ID="ddStores" runat="server" Height="25px" Width="300px" OnSelectedIndexChanged="ddStores_SelectedIndexChanged"></asp:DropDownList>
                </td>
                
            
                <td style="border-color:darkblue" class="auto-style2">
                    Loyalty Number
                <asp:DropDownList ID="ddLoyaltyNumber" runat="server" Height="25px" Width="300px" OnSelectedIndexChanged="ddLoyaltyNumber_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
            <tr>
                <td style="border-color:darkblue" class="auto-style6">
                    Employee
                    <asp:DropDownList ID="ddEmployee" runat="server" Height="25px" Width="300px" OnSelectedIndexChanged="ddEmployee_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="border-color:darkblue" class="auto-style2">
                    Transaction Type
                    <asp:DropDownList ID="ddTransactionType" runat="server" Height="25px" Width="300px" OnSelectedIndexChanged="ddTransactionType_SelectedIndexChanged"></asp:DropDownList>
                </td>
                </tr>
            <tr>
                <td style="border-color:darkblue" class="auto-style6">
                    Product <asp:DropDownList ID="ddProducts" runat="server" Height="25px" Width="300px" OnSelectedIndexChanged="ddProducts_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="border-color:darkblue" class="auto-style2">
                    Product Quantity
                    <asp:TextBox ID="tbNumberOfProduct" runat="server" Width="300px" Height="25px"></asp:TextBox>
                </td>
                </tr>
            <tr>
                <td style="border-color:darkblue" colspan="2" class="auto-style7">
                    Add a comment
                    <asp:TextBox ID="tbComment" runat="server" Height="69px" Width="604px"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td style="border-color:darkblue" colspan="2" class="auto-style8">
                    <asp:Button ID="btnSubmit" runat="server" Font-Size="Large" Font-Bold="false" ForeColor="DarkBlue" Text="Submit Transaction" Height="45px" Width="604px" OnClick="btnSubmit_Click" />
                </td>                
                                   
            </tr>
            <tr>
                <td style="border-color:darkblue" colspan="2" >
                    Transaction Details
                    <asp:TextBox ID="tbTransactionDetails" runat="server" Height="198px" Width="604px" ReadOnly="True" TextMode="MultiLine"></asp:TextBox> 
                </td>
            </tr>
        </table>
        
           
                    
           
        
        
    </div>
    </form>
</body>
</html>
