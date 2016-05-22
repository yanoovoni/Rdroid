using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BuyForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mShoppingBag"] == null)
            Page.Response.Redirect("SelectProducts.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Order NewOrder = new Order();
        //פרטי ההזמנה
        NewOrder.CustomerID = TextBox1.Text;
        NewOrder.RequiredDate = DateTime.Now;
        NewOrder.OrderProductds = (ShoppingBag)Session["mShoppingBag"];

        try
        {
            OrderService service = new OrderService();
            Label1.Text = service.CreateOrder(NewOrder).ToString();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }
}