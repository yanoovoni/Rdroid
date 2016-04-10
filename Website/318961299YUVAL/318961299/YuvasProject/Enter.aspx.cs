using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Enter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        UserService userService = new UserService();
        UserDetails userDetails = new UserDetails();
        userDetails.email=TextBox1.Text;
        userDetails.password=TextBox2.Text;
        int ID=userService.EnterToSite(userDetails);
        if (ID != 0)
        {
            Session["UserID"] = ID;
            Label1.Text = Session["UserID"].ToString();

        }
        else
        {
            Label1.Text = "Does Not Exist";
        }

    }
}