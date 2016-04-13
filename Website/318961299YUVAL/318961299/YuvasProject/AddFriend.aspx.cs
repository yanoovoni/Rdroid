using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class AddFriend : System.Web.UI.Page
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

        }
    }
    }
    protected void ButtonFindFriends_Click(object sender, EventArgs e)
    {
        localhostWebService.WebService service = new localhostWebService.WebService();
        DataSet dataset = new DataSet();
        string FullName = TextBoxSearchFriend.Text;
        int x = 0;
        string LastName = "";

        for (int i = 0; i < FullName.Length; i++)//הפעולה מחלקת את הסטרינג השלם לשתי מילים: שם פרטי ושם משפחה
        {
            if (FullName[i].ToString() == " ")
            {
                x = i + 1;
            }        
        }
        LastName = FullName.Substring(x, FullName.Length-x);
        
        FullName = FullName.Substring(0, x-1);

        dataset = service.FindFriends(FullName, LastName);

        GridViewFriends.DataSource = dataset;
        Session["DataSetFriends"] = dataset;
        GridViewFriends.DataBind(); 
    }
    protected void GridViewFriends_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        localhostWebService.WebService service = new localhostWebService.WebService();
        localhostWebService.FriendDetails friend = new localhostWebService.FriendDetails();
        DataSet dataset=(DataSet)Session["DataSetFriends"];
        UserService userService = new UserService();
        if (e.CommandName == "chck")
        {
            friend.phoneIDAsking = user.phoneNumber;
            int index = Convert.ToInt32(e.CommandArgument);
            
            friend.phoneIDAccepting = dataset.Tables[0].Rows[index]["PhoneNumber"].ToString();
            friend.dateOfFriendship = DateTime.Now;
            friend.status = false;
            service.InsertFriend(friend);
            LabelMsg.Text = "הוסף בהתחלה";
        }


    }


}