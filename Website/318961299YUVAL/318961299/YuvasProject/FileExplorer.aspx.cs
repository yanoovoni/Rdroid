using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

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
        string[] TempFilesInFolder = (string[])FilesInFolder.Clone();
        for (int i = 0; i < TempFilesInFolder.Length; i++)
        {
            if (TempFilesInFolder[i].EndsWith(":folder"))
            {
                TempFilesInFolder[i] = TempFilesInFolder[i].Substring(0, TempFilesInFolder[i].Length - ":folder".Length);
            }
        }
        GridViewExplorer.DataSource = TempFilesInFolder;
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

        if (e.CommandName == "ReturnItem")
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
                string Data;
                bool GotFile = Task.GetFile("yuval5898@walla.co.il", this.Folder + "/" + FileName, out Data);
                if (GotFile)
                {
                    byte[] ByteData = Encoding.UTF8.GetBytes(Data);
                    DownloadFile(FileName, ByteData);
                }
                else
                {
                    Label1.Text = Data;
                }
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

    protected void DownloadFile(string filename, byte[] filedata)
    {
        Response.Clear();
        Response.ClearHeaders();
        Response.ClearContent();
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        Response.AddHeader("Content-Length", filedata.Length.ToString());
        Response.ContentType = "text/plain";
        Response.Flush();
        Response.BinaryWrite(filedata);
        Response.End();
    }

    static protected byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}