<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManagment13.aspx.cs" Inherits="UserManagment13" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
            BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" 
            onrowcancelingedit="GridView1_RowCancelingEdit" 
            onrowcommand="GridView1_RowCommand" onrowediting="GridView1_RowEditing" 
            onselectedindexchanged="GridView1_SelectedIndexChanged" 
            onsorting="GridView1_Sorting" 
            onrowdeleting="GridView1_RowDeleting" 
            onrowdatabound="GridView1_RowDataBound" ShowFooter="True" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5">
            <AlternatingRowStyle BackColor="Gainsboro" />
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="UserID" ReadOnly="True" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" 
                    SortExpression="FirstName" ReadOnly="True" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" 
                    SortExpression="LastName" ReadOnly="True" />
                <asp:BoundField DataField="CityID" HeaderText="CityID" ReadOnly="True" />
                <asp:TemplateField HeaderText="cityname">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("cityName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Phone" HeaderText="Phone" ReadOnly="True" />
                <asp:ButtonField CommandName="ShowRowNumber" Text="לחץ" />
                <asp:TemplateField HeaderText="כתובת">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("State") %>' 
                            Width="128px"></asp:TextBox>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Addres") %>'></asp:TextBox>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Zip") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                        <br />
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Addres") %>'></asp:Label>
                        <br />
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Zip") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ButtonType="Button" ShowEditButton="True" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>

        <br />
        <br />
&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
