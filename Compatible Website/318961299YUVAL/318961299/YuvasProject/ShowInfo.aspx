<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowInfo.aspx.cs" Inherits="ShowInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
    <asp:Menu ID="MenuInfo" runat="server">
    </asp:Menu>
    <br />
    <center>
    <asp:GridView ID="GridViewInfo" runat="server">
    </asp:GridView></center>
    <br />
    <br />
    <asp:Table ID="Table1" runat="server" Height="310px" Width="865px">
    </asp:Table>
</asp:Content>


