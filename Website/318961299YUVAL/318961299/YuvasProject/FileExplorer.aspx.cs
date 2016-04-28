using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FileExplorer : System.Web.UI.Page
{
    private string Folder = "";
    private string[] FilesInFolder;
    protected void Page_Load(object sender, EventArgs e)
    {
        Folder = "";
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void PopulateGridView()
    {
        FilesInFolder = Task.GetFilesInFolder("yuval5898@walla.co.il", Folder);
        GridViewExplorer.DataSource = FilesInFolder;
        GridViewExplorer.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Folder = "";
        PopulateGridView();
    }

    protected void GridViewExplorer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewExplorer.DataSource = null;
        GridViewExplorer.DataBind();

        if (e.CommandName == "chck")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Folder += "\\" + FilesInFolder[index];
            PopulateGridView();
        }
    }

    protected void ToPreviousFolder()
    {
        int index = Folder.LastIndexOf("\\");
        if (index > 0)
        {
            Folder = Folder.Substring(0, index);
            PopulateGridView();
        }
    }
}