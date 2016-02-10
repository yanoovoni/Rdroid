using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class kesher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string connectionString = Connect.getConnectionString();
        OleDbConnection myConnection = new OleDbConnection(connectionString);

        try
        {
            myConnection.Open();
            LabelMsg.Text = "פתיחת קשר הצליחה";
        }
        catch (OleDbException ex)
        {
            LabelMsg.Text = ex.Message;
        }
        finally
        {
            myConnection.Close();
            Label1.Text = "סגירת התקשרות";
        }
    }

    }
