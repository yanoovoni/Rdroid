<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ex10.aspx.cs" Inherits="Ex10" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 810px">
    
        <asp:GridView ID="CityTable" runat="server">
        </asp:GridView>
        <br />

        <asp:GridView ID="People" runat="server" Height="118px" Width="217px">
        </asp:GridView>
<br />
        <asp:Label ID="error" runat="server" Text="Label"></asp:Label>
    
        <br />
        <br />
        <br />
    



        <asp:TextBox ID="ShowData" runat="server" Height="140px" TextMode="MultiLine" 
            Width="485px"></asp:TextBox>
    



        <asp:Button ID="TextBoxShow" runat="server" onclick="TextBoxShow_Click" 
            Text="Show Data in TextBox" style="height: 26px" />
        <br />
        <br />
        <br />
        <asp:GridView ID="CityPersonGriedview" runat="server">
        </asp:GridView>
        <br />
        <asp:Button ID="cityPersonGV" runat="server" onclick="cityPersonGV_Click" 
            Text="show Data in GV" />
        <br />
        <br />
        <br />
    



        <asp:DropDownList ID="DropDownListCity" runat="server" Height="16px" 
            onselectedindexchanged="DropDownListCity_SelectedIndexChanged" Width="252px">
        </asp:DropDownList>
&nbsp;<asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            style="height: 26px" Text="Button" />
    



    </div>
    </form>
</body>
</html>
