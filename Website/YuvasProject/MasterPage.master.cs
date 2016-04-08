using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Proxy.Get_Instance();
        if (!Page.IsPostBack)
        {
            CreateSiteMenu();
        }
    }

     public void CreateSiteMenu()
        {
            MenuItem Item;
            this.SiteMenu.Items.Clear();

            Item = new MenuItem("העברת אנשי קשר אל חברים", "ShareContactsWithFriends.aspx");
            this.SiteMenu.Items.Add(Item);
        }
     protected void SiteMenu_MenuItemClick(object sender, MenuEventArgs e)
     {
         Menu item = (Menu)sender;

         Page.Response.Redirect(item.SelectedValue);


     }
}
