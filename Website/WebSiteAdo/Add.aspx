<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Add.aspx.cs" Inherits="Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 82px">
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:Label ID="LabelEror" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="LabelMsg" runat="server" Text="Label"></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server" Height="22px" 
        ontextchanged="TextBox1_TextChanged"></asp:TextBox>
    </form>
</body>
</html>
