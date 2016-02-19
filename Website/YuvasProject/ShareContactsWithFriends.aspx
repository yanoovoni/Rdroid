<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShareContactsWithFriends.aspx.cs" Inherits="ShareContactsWithFriends" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center style="height: 380px">
    
        <br />
        <br />
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" DataTextField="FriendNAME" 
            DataValueField="UserID">
        </asp:CheckBoxList>
    
    </center>
    </div>
    </form>
</body>
</html>
