using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;


public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            populateGV();
        }
    }
    public void populateGV()
    {
        localhostYuvalWebService.YuvalWebService product = new localhostYuvalWebService.YuvalWebService();
        DataSet ds = product.GetAllProducts();
        this.GridView1.DataSource = ds.Tables[0];
        this.GridView1.DataBind();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        populateGV();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        populateGV();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        populateGV();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        localhostYuvalWebService.YuvalWebService service = new localhostYuvalWebService.YuvalWebService();
        localhostYuvalWebService.ProductInBag Product = new localhostYuvalWebService.ProductInBag();
        GridViewRow row = GridView1.Rows[e.RowIndex];

        Product.ProdID = int.Parse(row.Cells[0].Text);
        Product.Price = decimal.Parse(((TextBox)row.Cells[2].Controls[0]).Text);

        service.updateProductPrice(Product);

        GridView1.EditIndex = -1;
        populateGV();
    }
}