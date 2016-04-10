using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
public partial class ShareContactsWithFriends : System.Web.UI.Page
{  UserDetails user;
ArrayList phoneFriendList;
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
            //Populate1();

            //dataSet = userService.GetContacts(user);
            //Session["DataSet"] = dataSet;
            PopulateFriends();
            PopulateConacts();
        }
    }
    else Response.Redirect("");

}
    private void Load_Friends_And_Contacts()
    {
        UserService userService = new UserService();
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

    private void PopulateConacts()
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
        phoneFriendList = new ArrayList();
        foreach (ListItem friend in CheckBoxListFriends.Items)
        {
            // רשימה של החברים שאיתם 
            // מעוניינים לשתף חברים
            if (friend.Selected)
            {
               phoneFriendList.Add(friend.Value);
            }
        }
        
        foreach (ListItem contactItem in CheckBoxListContacts.Items)
        { // עבור כל איש קשר ברשימה לשיתוף מעתיק לחבר
            //יצירת רשימה של אנשי הקשר
            if (contactItem.Selected)
            {
                contact = userservice.GetContactsByID(int.Parse(contactItem.Value));

                for (int i = 0; i < phoneFriendList.Count; i++ )
                {

                    moveContactsToAFriend(contact, phoneFriendList[i].ToString());

                }
            }
        }
    }
   
    public void moveContactsToAFriend(ContactDetails contact, string  friendPhone)//כאשר לוחצים על הכפור יעבור המידע של אנשי הקשר שסומנו אל החברים שסומנו 
    {
        UserService userService = new UserService();
        contact.userPhoneBelong = friendPhone;
        contact.status = "לאישור";
        try
        {
          if(!userService.IfContactExist(contact))  // מוסיפים איש קשר רק אם לא קיים
          {
              userService.InsertContact(contact);
          }
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