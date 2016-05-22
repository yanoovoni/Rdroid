using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class Targil9 : System.Web.UI.Page
{
    DataSet dataset;
    protected void Page_Load(object sender, EventArgs e)
    {
       if (!Page.IsPostBack )
       {
           Load_Data();

           //PopulateGridView();
           //ShowTextBox();
       }
       else
          dataset=(DataSet) Page.Session["dataset"];
     
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        ShowTextBox();
    }

    private void ShowTextBox()
    {
       
        string output = "";
      

        for (int i = 0; i < dataset.Tables["KindsTBL"].Rows.Count; i++)
        {
            output += dataset.Tables["KindsTBL"].Rows[i]["kodItem"] + "\t" + dataset.Tables["KindsTBL"].Rows[i]["typeName"] + "\n";
        }
        TextBox1.Text = output;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        PopulateGridView();
    }

    private void PopulateGridView()
    {
      
        GridView1.DataSource = dataset.Tables["KindsTBL"];
        GridView1.DataBind();
    }

    private void Load_Data()
    {
        DataService dataservice = new DataService();
        dataset = dataservice.GetSubjects2();
        Page.Session["dataset"] = dataset;
    }
}