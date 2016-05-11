using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    Proxy proxy = Proxy.Get_Instance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreateSiteMenu();
        }
    }

     public void CreateSiteMenu()
        {
            MenuItem Item;
            this.SiteMenu.Items.Clear();

            Item = new MenuItem("    |    העברת אנשי קשר אל חברים", "ShareFilesWithFriends.aspx");
            this.SiteMenu.Items.Add(Item);

            Item = new MenuItem("  |  סייר קבצים", "FileExplorer.aspx");
            this.SiteMenu.Items.Add(Item);

        }
     protected void SiteMenu_MenuItemClick(object sender, MenuEventArgs e)
     {
         Menu item = (Menu)sender;

         Page.Response.Redirect(item.SelectedValue);


     }
}
