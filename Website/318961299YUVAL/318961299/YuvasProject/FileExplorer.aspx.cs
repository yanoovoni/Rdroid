﻿using System;
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
        this.Folder = (string)Session["Folder"];
        this.FilesInFolder = (string[])Session["FilesInFolder"];
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        Session["Folder"] = this.Folder;
        Session["FilesInFolder"] = this.FilesInFolder;
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void PopulateGridView()
    {
        this.FilesInFolder = Task.GetFilesInFolder("yuval5898@walla.co.il", Folder);
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
            string FileName = this.FilesInFolder[index];
            if (FileName.EndsWith(":folder"))
            {
                int FolderNotifiyerIndex = FileName.LastIndexOf(":");
                if (FolderNotifiyerIndex > 0)
                {
                    FileName = FileName.Substring(0, FolderNotifiyerIndex);
                }
                if (this.Folder != "")
                {
                    this.Folder += "/";
                }
                this.Folder += FileName;
                PopulateGridView();
            }
            else
            {
                Task.GetFile("yuval5898@walla.co.il", this.Folder += "/" + FileName);
                //todo send the output of this task back to the client.
            }
        }
    }

    protected void ToPreviousFolder()
    {
        int index = Folder.LastIndexOf("/");
        if (index > 0)
        {
            Folder = Folder.Substring(0, index);
            PopulateGridView();
        }
    }
}