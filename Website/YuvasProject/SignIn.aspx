<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="SignIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>
    
         
        first name:
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<br />

        <br />

        <br />

        last name:
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <br />

        <br />

        password:
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        
        <br />

        <br />

        validate password:
        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
        <br />
        <br />
        
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="TextBox3" ControlToValidate="TextBox8" 
            ErrorMessage="CompareValidator">הסיסמא אינה תואמת</asp:CompareValidator>
        <br />
        <br />
        <br />

        Email:
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        <br />
        <br />
        
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="TextBox4" ErrorMessage="RegularExpressionValidator" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">the email is invaid</asp:RegularExpressionValidator>
        <br />

        <br />

        birth date: day:
        <asp:TextBox ID="TextBox5" runat="server" Width="22px"></asp:TextBox>
 month:
        <asp:TextBox ID="TextBox6" runat="server" Width="24px"></asp:TextBox>
 year:
        <asp:TextBox ID="TextBox7" runat="server" Width="51px"></asp:TextBox>
        <br />
        <br />
        <br />

        sex:
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        <br />

        <asp:Button ID="Finnish" runat="server" Text="Finnish" 
            onclick="Finnish_Click" />

        <asp:Button ID="cancel" runat="server" Text="cancel" />
        <br />
        <br />
        <br />
        <asp:Label ID="LabelMassege" runat="server"></asp:Label>
        <br />

        <br />

    </center>
    </div>
    </form>
</body>
</html>
