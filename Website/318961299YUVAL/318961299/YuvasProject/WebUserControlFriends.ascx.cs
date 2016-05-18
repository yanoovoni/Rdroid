using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using localhostWebService;

public partial class WebUserControlFriends : System.Web.UI.UserControl
{
    UserDetails user;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new UserDetails();
        user.phoneNumber = "0547645029";
        Page.Session["User"] = user;
        if (Page.Session["User"] != null)
        {

            user = (UserDetails)Page.Session["User"];

            if (!Page.IsPostBack)
            {
                Load_Friends_And_Contacts();
                PopulateFriends();

            }
        }
    }
    private void Load_Friends_And_Contacts()
    {
        WebService userService = new WebService();
        DataSet dataSet = new DataSet();
        dataSet = userService.GetFriendsAndContacts(user);
        Session["DataSet"] = dataSet;
    }

    private void PopulateFriends()
    {
        DataSet dataset = (DataSet)Session["DataSet"];
        CheckBoxListFriends.DataSource = dataset.Tables["Friends"];
        CheckBoxListFriends.DataTextField = "FriendNAME";
        CheckBoxListFriends.DataValueField = "PhoneNumber";
        CheckBoxListFriends.DataBind();
    }
}