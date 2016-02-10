using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ex5p63 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void category_Click(object sender, EventArgs e)
    {
        try
        {
            DataService data = new DataService();
            string description = data.GetKind(int.Parse(Check.Text));

            if (description == "")
                error.Text = "was not found";
            else error.Text = description;
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }
    }
    protected void sexybutton_Click(object sender, EventArgs e)
    {
        try
        {
            DataService adding = new DataService();
            if (adding.AddKind(Add.Text))
                addLable.Text = "הצלחת";
            else
                addLable.Text = "הקטגוריה כבר קיימת";
        }
        catch (Exception ex)
        {
            addLable.Text = ex.Message;
        }
    }

    protected void start_Click(object sender, EventArgs e)
    {
        string st;
        DataService data = new DataService();
        try
        {
            st = data.Getallcat(); 
            subjects.Text = st;
        }
        catch (Exception ex)
        {
            addLable.Text = ex.Message;
        }
    }

    private void ListBoxKins()
    {
        DataService data = new DataService();

        OleDbDataReader datareader = data.getallsubject();

        this.listSubjects.DataSource = datareader;
        this.listSubjects.DataTextField = "NameKind";
        this.listSubjects.DataBind();

        datareader.Close();
    }
    protected void Send_Click(object sender, EventArgs e)
    {
        ListBoxKins();
    }
    protected void GridView_Click(object sender, EventArgs e)
    {
        DataService data = new DataService();
        OleDbDataReader dr = data.getallsubject();

        try
        {
            this.ihs.DataSource = dr;
            this.ihs.DataBind();
        }
        finally
        {
            dr.Close();
        }

    }
    protected void Deletebut_Click(object sender, EventArgs e)
    {
        try
        {
            DataService delete = new DataService();
            if (delete.DeleteKind(int.Parse(Delete.Text)))
                addLable.Text = "הצלחת";
            else addLable.Text = "לא הצליח";
        }
        catch (Exception ex)
        {
            addLable.Text = ex.Message;
        }
    }
    protected void subjects_TextChanged(object sender, EventArgs e)
    {

    }
    protected void NewWork_Click(object sender, EventArgs e)
    {
        try
        {
            DataService data = new DataService();
            addLable.Text = data.GetCatById(int.Parse(Check.Text));
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }
    }
}