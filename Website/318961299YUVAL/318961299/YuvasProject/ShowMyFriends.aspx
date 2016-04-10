<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowMyFriends.aspx.cs" Inherits="ShowMyFriends" %>

<%@ Register src="WebUserControlFriends.ascx" tagname="WebUserControlFriends" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:WebUserControlFriends ID="WebUserControlFriends2" runat="server" />
    <p>
        <br />
    </p>
    <p>
    </p>
</asp:Content>

