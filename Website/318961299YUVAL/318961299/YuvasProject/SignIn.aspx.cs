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
    protected void Button1_Click(object sender, EventArgs e)
    {
        localhostWebService.UserDetails ud = new localhostWebService.UserDetails();
        localhostWebService.WebService service = new localhostWebService.WebService();
        UserService userService = new UserService();
        UserDetails userDetails = new UserDetails();
        ud.email = TextBox1.Text;
        ud.password = TextBox2.Text;
        localhostWebService.UserDetails user = service.EnterToSite(ud);
        if (user != null)
        {
            Session["UserDetails"] = user;
            Response.Redirect("FileExplorer.aspx");
        }
        else
        {
            Label1.Text = "Does Not Exist";
        }

    }
}