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
            

            dataSet = userService.GetFriendsAndContacts(user);
            Session["DataSet"] = dataSet;
            //Populate1();

            //dataSet = userService.GetContacts(user);
            //Session["DataSet"] = dataSet;
            PopulateFriends();
            Populate2();
        }



    }
    private void PopulateFriends()
    {
        DataSet dataset = (DataSet)Session["DataSet"];
        CheckBoxListFriends.DataSource = dataset.Tables["Friends"];
        CheckBoxListFriends.DataTextField = "FriendNAME";
        CheckBoxListFriends.DataValueField = "UserID";
        CheckBoxListFriends.DataBind();
    }

    private void Populate2()
    {
        DataSet dataset = (DataSet)Session["DataSet"];
        CheckBoxListContacts.DataSource = dataset.Tables[1];
        CheckBoxListContacts.DataTextField = "ContactNAME";
        CheckBoxListContacts.DataValueField = "ID";
        CheckBoxListContacts.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        UserService userservice = new UserService();
        ContactDetails contact = new ContactDetails();
        foreach (ListItem item in CheckBoxListContacts.Items)
        {
            if (item.Selected)
            {
                contact = userservice.GetContactsByID(int.Parse(item.Value));
            }
            foreach (ListItem item1 in CheckBoxListFriends.Items)
            {
                if (item1.Selected)
                {
                    moveContactsToAFriend(contact, int.Parse(item1.Value));
                }
            }
        }
    }

    public void moveContactsToAFriend(ContactDetails contact, int IdFriend)//כאשר לוחצים על הכפור יעבור המידע של אנשי הקשר שסומנו אל החברים שסומנו
    {
        UserService userService = new UserService();
        contact.userIDbelong = IdFriend;
        try
        {
            userService.InsertContact(contact);
            Label1.Text = "yaaay";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void CheckBoxListContacts_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}