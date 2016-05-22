using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

public partial class Targil_5 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();

        try
        {
            string sts = dataservice.GetCategoryName(int.Parse(TextBox1.Text));
            if (sts != "")
            {
                Label1.Text = sts;

            }
            else
                Label1.Text = "not exist";
        }
        catch (Exception ex)
        {
            Label2.Text = "error" + ex.Message;
        }

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();

        try
        {
            if (dataservice.addcategory(TextBox2.Text))
            {
                Label1.Text = "done";

            }
            else
                Label1.Text = "category exist";
        }
        catch (Exception ex)
        {
            Label2.Text = "error" + ex.Message;
        }
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();

        try
        {
            TextBox3.Text = dataservice.ToText();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();
        OleDbDataReader dr=dataservice.ShowAll();
        try
        {
            ListBox1.DataSource = dr;
            ListBox1.DataTextField = "typeName";
            ListBox1.DataValueField = "kodItem";
            ListBox1.DataBind();
            dr.Close();

        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();
        OleDbDataReader dr = dataservice.ShowAll();

        try
        {
            GridView1.DataSource = dr;
            GridView1.DataBind();
            dr.Close();

        }
        catch(Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();
        
        try
        {
            if (dataservice.deleteCategory(int.Parse(TextBox4.Text)))
            {
                Label1.Text = "finish";
            }
            else
            {
                Label1.Text = "category not found";
            }
        }
        catch(Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        Label3.Text = DropDownList1.DataTextField;
    }

    protected void ButtonSug_Click(object sender, EventArgs e)
    {
        DataService dataservice = new DataService();

        try
        {
            string sts = dataservice.sugMila(TextBox1.Text);
            if (sts != "")
            {
                Label1.Text = sts;

            }
            else
                Label1.Text = "not exist";
        }
        catch (Exception ex)
        {
            Label2.Text = "error" + ex.Message;
        }
    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }
}