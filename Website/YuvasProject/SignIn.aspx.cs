using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Finnish_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            UserDetails user = new UserDetails();
            user.firstName = TextBox1.Text;
            user.lastName = TextBox2.Text;
            user.password = TextBox3.Text;
            user.email = TextBox4.Text;


            UserService userService = new UserService();
            try
            {
                userService.InsertUser(user);
            }
            catch (Exception ex)
            {
                this.LabelMassege.Text = ex.Message;
            }
        }

    }
}