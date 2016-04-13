using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;


public partial class ShowInfo : System.Web.UI.Page
{
    localhostWebService.UserDetails user;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new localhostWebService.UserDetails();
        user.phoneNumber = "0547645029";
        Page.Session["User"] = user;
        if (!Page.IsPostBack)
        {
            CreateMenu();

        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {

    }
    public void CreateMenu()
    {
        MenuItem Item;
        this.MenuInfo.Items.Clear();

        Item = new MenuItem("לראות את כל אנשי הקשר", "PopulateGridViewWithContacts");
        this.MenuInfo.Items.Add(Item);
    }

    public void PopulateGridViewWithContacts()
    {
        localhostWebService.WebService service = new localhostWebService.WebService();
        DataSet dataset = new DataSet();
        dataset = service.GetContacts(user);
        GridViewInfo.DataSource = dataset;
        GridViewInfo.DataBind();
    }
}