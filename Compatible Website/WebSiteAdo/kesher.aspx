<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kesher.aspx.cs" Inherits="kesher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 157px">
    
        <asp:Label ID="LabelMsg" runat="server" Text="Label"></asp:Label>
        &nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <br />
    
    </div>
    <asp:Button ID="Connection" runat="server" onclick="Button1_Click" 
        Text="צור קשר" />
    </form>
</body>
</html>
