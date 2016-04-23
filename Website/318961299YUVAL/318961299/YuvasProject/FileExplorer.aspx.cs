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
}