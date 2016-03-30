<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFriend.aspx.cs" Inherits="AddFriend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<center>
    <asp:TextBox ID="TextBoxSearchFriend" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="ButtonFindFriends" runat="server" 
        onclick="ButtonFindFriends_Click" Text="Find Friend" />
    <br />
    <br />
    <asp:GridView ID="GridViewFriends" runat="server" 
        onrowcommand="GridViewFriends_RowCommand">
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="chck" Text="Add The Friend" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Table ID="Table1" runat="server" Height="289px" Width="789px">
    </asp:Table>
    </center>
</asp:Content>

