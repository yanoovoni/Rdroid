using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class SelectProducts : System.Web.UI.Page
{
    protected ShoppingBag mShoppingBag;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mShoppingBag"] == null)
        {
            mShoppingBag = new ShoppingBag();
            Button1.Visible = false;
        }
        else
        {
            mShoppingBag = (ShoppingBag)Session["mShoppingBag"];
            Button1.Visible = true;
        }
    }

    void populate_GridView2()
    {
        GridView2.DataSource = mShoppingBag.Products;
        GridView2.DataBind();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddProduct")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridView1.Rows[index];

            string productID = row.Cells[0].Text;
            string prodactName = row.Cells[1].Text;
            string price = row.Cells[2].Text;
            ProductInBag newProduct = new ProductInBag(int.Parse(productID), prodactName, decimal.Parse(price), 1);
            mShoppingBag.AddProduct(newProduct);
            Button1.Visible = true;
            Page.Session["mShoppingBag"] = mShoppingBag;
            populate_GridView2();
        }
    }
    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        populate_GridView2();
    }
    protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView2.EditIndex = -1;
        populate_GridView2();
    }

    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        mShoppingBag = (ShoppingBag)Session["mShoppingBag"];
        ProductInBag product = new ProductInBag();
        GridViewRow row = GridView2.Rows[e.RowIndex];
        product.ProdID = int.Parse(row.Cells[0].Text);
        product.Quantity = short.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
        mShoppingBag.UpdateProduct(product);
        Session["mShoppingBag"] = mShoppingBag;
        GridView2.EditIndex = -1;
        populate_GridView2();
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        mShoppingBag = (ShoppingBag)Session["mShoppingBag"];
        ProductInBag product = new ProductInBag();
        GridViewRow row = GridView2.Rows[e.RowIndex];
        product.ProdID = int.Parse(row.Cells[0].Text);
        mShoppingBag.DeleteProduct(product);
        Session["mShoppingBag"] = mShoppingBag;
        GridView2.EditIndex = -1;
        populate_GridView2();
    }
    

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header &&
           e.Row.RowType != DataControlRowType.Footer &&
           e.Row.RowType != DataControlRowType.Pager)
        {
            short Quantity = 0;
            if (e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Alternate) &&
               e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Normal))
            {
                Quantity = Convert.ToInt16(e.Row.Cells[2].Text);
            }
            else
            {
                Quantity = Convert.ToInt16(((TextBox)e.Row.Cells[2].Controls[0]).Text);
            }
            Label labelSum = (Label)e.Row.Cells[5].FindControl("Label1");
            decimal price = Convert.ToDecimal(e.Row.Cells[3].Text);
            decimal sum = (decimal)(price * Quantity);
            labelSum.Text = sum.ToString();
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label labelFooter = (Label)e.Row.Cells[5].FindControl("Label2");
            mShoppingBag = (ShoppingBag)Session["mShoppingBag"];
            decimal sum = (decimal)mShoppingBag.GetFinalPrice();
            labelFooter.Text = sum.ToString();

        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("BuyForm.aspx");
    }
}