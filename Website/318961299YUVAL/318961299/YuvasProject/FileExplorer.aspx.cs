using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FileExplorer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }



    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void PopulateGridView()
    {
        GridViewExplorer.DataSource = Task.GetFilesInFolder("yuval5898@walla.co.il","");
        GridViewExplorer.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        PopulateGridView();
    }

    protected void GridViewExplorer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewExplorer.DataSource = null;
        GridViewExplorer.DataBind();

        if (e.CommandName == "chck")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewExplorer.DataSource = Task.GetFilesInFolder("yuval5898@walla.co.il", );
            GridViewExplorer.DataBind();
        }
    }
}