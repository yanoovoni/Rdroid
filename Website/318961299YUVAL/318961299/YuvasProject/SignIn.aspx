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

        birth date: day:<asp:DropDownList 
            ID="DropDownListBDay" runat="server" Height="22px" Width="55px">
        </asp:DropDownList>
&nbsp;month:<asp:DropDownList ID="DropDownListBMonth" runat="server" Height="17px" 
             Width="54px" 
            onselectedindexchanged="DropDownListBMonth_SelectedIndexChanged">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
        </asp:DropDownList>
&nbsp;year:
        <asp:DropDownList ID="DropDownListBYears" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        <br />

        sex:
        <asp:DropDownList ID="DropDownList1" runat="server" Height="17px" Width="57px">
            <asp:ListItem>male</asp:ListItem>
            <asp:ListItem>female</asp:ListItem>
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
