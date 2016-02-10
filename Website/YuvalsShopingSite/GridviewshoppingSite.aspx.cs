using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GridviewshoppingSite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Load_Data();
    }

    private void Load_Data()
    {
        localhost.YuvalWebService service = new localhost.YuvalWebService();
        DataSet ds = service.GetAllProducts();
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridView1.PageIndex = e.NewPageIndex;
        Load_Data();

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        Load_Data();

    }
}