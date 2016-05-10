<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FileExplorer.aspx.cs" Inherits="FileExplorer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

    <center>
    <p>
        &nbsp;</p>
        <p>
        <br />
        <asp:GridView ID="GridViewExplorer" runat="server" 
                OnRowCommand="GridViewExplorer_RowCommand" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="ReturnItem" Text="Open Folder / Download File" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
              <asp:Button ID="ToLastFolder" runat="server" Text="to last folder" 
                OnClick="ToLastFolder_Click" Visible="False" />
            <br />
    </p>
        <p>
            &nbsp;</p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />
        <br />
        <asp:FileUpload ID="FileUploadIntoPhone" runat="server" />
        <br />
        <p>
            <asp:Button ID="Upload" runat="server" OnClick="Upload_Click" Text="Click here to uploud the file" />
        </p>
        <p>
            &nbsp;</p>
        <p>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </p>
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

