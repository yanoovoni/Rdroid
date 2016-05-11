<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShareFilesWithFriends.aspx.cs" Inherits="ShareFilesWithFriends" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">



    <title></title>
</head>
<body>
    
    <div>
    <center style="height: 380px">
    
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Friends"></asp:Label>
        <asp:CheckBoxList ID="CheckBoxListFriends"  runat="server" DataTextField="FriendNAME" 
            DataValueField="UserID">
        </asp:CheckBoxList>

       
    
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="Contact"></asp:Label>
        <br />
        <asp:CheckBoxList ID="CheckBoxListContacts" runat="server" 
            >
        </asp:CheckBoxList>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    
    </center>
    </div>
    
</body>

</html>
</asp:Content>

