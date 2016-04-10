<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebUserControlFriends.ascx.cs" Inherits="WebUserControlFriends" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
    .style2
    {
        height: 20px;
    }
</style>
  <asp:Panel ID="Panel1" runat="server">
<table class="style1">
    <tr>
        <td>
            <asp:CheckBoxList ID="CheckBoxListFriends" runat="server" 
                DataTextField="FriendNAME" DataValueField="UserID">
            </asp:CheckBoxList>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
          
            
        </td>
        <td class="style2">
        </td>
    </tr>
</table>
</asp:Panel>

