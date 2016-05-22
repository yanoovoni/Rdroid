using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class Targil16 : System.Web.UI.Page
{
    DataSet dataset;
    protected void Page_Load(object sender, EventArgs e)
    {
        PopulateGridView();
    }

    private void PopulateGridView()
    {

        GridView1.DataSource = dataset.Tables["wordsTBL"];
        GridView1.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.DataValueField == "1")
        {
            string rowFilterField = TextBox1.Text;
            DataView view = new DataView(dataset.Tables["wordsTBL"]);
            view.RowFilter = "kodItem='" + rowFilterField + "'";
            GridView1.DataSource = view;
            GridView1.DataBind();
        }
        else
        {

        }

    }
}