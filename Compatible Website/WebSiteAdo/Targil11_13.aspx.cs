using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class Targil11_13 : System.Web.UI.Page
{
    DataSet dataSet;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Load_Data();
            dropListBind();
        }
        else
        {
            dataSet = (DataSet)Session["DataSet"];
        }
    }
    private void Load_Data()
    {
        DataService dataservice = new DataService();
        dataSet = dataservice.GetSubjectAndWords();
        Page.Session["DataSet"] = dataSet;
        
       
    }

    public void dropListBind()
    {
        DropDownList1.DataSource = dataSet;
        DropDownList1.DataTextField = "typeName";
        DropDownList1.DataValueField = "kodItem";
        DropDownList1.DataBind();
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataRow kind = dataSet.Tables["KindsTBL"].Rows.Find(DropDownList1.SelectedValue);
            TextBox1.Text = kind["typeName"].ToString();
            Label1.Text = kind["kodItem"].ToString();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }
   
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        DataService dataService = new DataService();
        dataService.DeleteKindDS(Label1.Text, dataSet);
        dropListBind();
    }
    protected void AddButton_Click(object sender, EventArgs e)
    {
        try
        {
            DataService dataService = new DataService();
            dataService.ADDKindDS(this.TextBox1.Text, dataSet);
            dropListBind();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }


    protected void ButtonUpdate_Click(object sender, EventArgs e)
    {
        DataService data = new DataService();
        data.UpdateKindsDS(Label1.Text, TextBox1.Text, dataSet);
        dropListBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataService data = new DataService();
        data.updateDbfromDS(dataSet);
        dropListBind();
    }
}