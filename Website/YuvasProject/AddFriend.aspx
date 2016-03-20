<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFriend.aspx.cs" Inherits="AddFriend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="Button1" runat="server" 
        
        style="position: absolute; top: 200px; left: 370px; z-index: 1; right: 518px;" 
        Text="Button" />
    <asp:TextBox ID="TextBox1" runat="server" 
        style="z-index: 1; left: 321px; top: 143px; position: absolute; height: 21px" 
        Width="150px" ></asp:TextBox>

     <asp:GridView ID="GridViewUsers" runat="server"  
        style="position: absolute; top: 250px; left: 300px; z-index: 1; right: 518px;" 
        Visible="False" >
     </asp:gridView>
</asp:Content>

