<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FileExplorer.aspx.cs" Inherits="FileExplorer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

    <center>
    <p>
        &nbsp;</p>
        <p>
        <br />
        <asp:GridView ID="GridViewExplorer" runat="server" OnRowCommand="GridViewExplorer_RowCommand">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="ReturnItem" Text="Open Folder / Download File" />
            </Columns>
        </asp:GridView>
    </p>
        <p>
            &nbsp;</p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <p>
            &nbsp;</p>
    <p>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
    </p>
    <p>
    </p>
    
    </center>

</asp:Content>

