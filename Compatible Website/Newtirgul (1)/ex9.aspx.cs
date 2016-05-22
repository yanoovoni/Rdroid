using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ex9 : System.Web.UI.Page
{
    DataSet dataset;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            Load_Data();
        }
        else
        {
            dataset = (DataSet)Session["DataSet"];
        }
    }

    private void Load_Data()
    {
        DataService data= new DataService();
        try
        {
            dataset = data.subjectsIntoDataSet();
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }
        Page.Session["DataSet"] = dataset;

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView_Click(object sender, EventArgs e)
    {
        DataService data = new DataService();
      //  DataSet dataset = null; 

        try
        {
            dataset = data.subjectsIntoDataSet(); 
            this.GData.DataSource = dataset;
            this.GData.DataBind();
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }

    }
    protected void TextboxShow_Click(object sender, EventArgs e)
    {
        DataService data = new DataService();
        string output = "";

        for (int i = 0; i < dataset.Tables["Category"].Rows.Count; i++)
        {
            output += dataset.Tables["Category"].Rows[i]["KindID"] + "\t" + dataset.Tables["Category"].Rows[i]["NameKind"] + "\n";
        }

        ShowTextBox.Text = output;
    }
}