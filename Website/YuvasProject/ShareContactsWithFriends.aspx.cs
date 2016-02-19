using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ShareContactsWithFriends : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            UserDetails user = new UserDetails();
            user.userID = 1;
            UserService userService = new UserService();
            DataSet dataSet = new DataSet();

            dataSet = userService.GetFriends(user);
            Session["DataSet"] = dataSet;
            Populate();
        }



    }
    private void Populate()
    {
        DataSet dataset = (DataSet)Session["DataSet"];
        CheckBoxList1.DataSource = dataset.Tables[0];
        CheckBoxList1.DataTextField = "FriendNAME";
        CheckBoxList1.DataValueField = "UserID";
        CheckBoxList1.DataBind();
    }
}